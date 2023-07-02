using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private PointOfInterest currentPoint;
    private List<PointOfInterest> nextPoints;
    private string currentSceneName; // Keep track of the currently loaded scene
    
    public TextMeshPro currentSceneText;
    public TextMeshPro nextScenesText;
    public GameObject buttonPrefab; // Assign this in the Unity editor
    public Transform buttonContainer; // Assign this in the Unity editor
    public EventController eventController;

    void Start()
    {
        if (currentPoint != null)
        {
            nextPoints = currentPoint.NextPointsOfInterestWithPath;
            UpdateButtons();
        }
        else
        {
            Debug.LogError("currentPoint is null. Make sure to set it before the first frame update.");
        }
    }

    void MoveToNextPoint(PointOfInterest nextPoint)
    {
        if (nextPoint != null)
        {
            transform.position = nextPoint.transform.position;
            currentPoint = nextPoint;
            nextPoints = currentPoint.NextPointsOfInterestWithPath;
            UpdateCurrentSceneText();
            UpdateButtons();
            
            eventController.TriggerEvent(nextPoint.eventType);

            // Unload the old scene if there is one
            if (!string.IsNullOrEmpty(currentSceneName))
            {
                SceneManager.UnloadSceneAsync(currentSceneName);
            }

            // Load the scene associated with the next point of interest additively
            currentSceneName = nextPoint.LoadScene();
        }
        else
        {
            Debug.LogError("nextPoint is null. Make sure NextPointsOfInterestWithPath does not contain null values.");
        }
    }

    void UpdateButtons()
    {
        // Clear existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new buttons
        foreach (PointOfInterest point in nextPoints)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonContainer);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = point.sceneData.sceneName;
            button.onClick.AddListener(() => MoveToNextPoint(point));
        }
    }

    public void SetCurrentPoint(PointOfInterest point)
    {
        currentPoint = point;
        nextPoints = currentPoint.NextPointsOfInterestWithPath;
        UpdateCurrentSceneText();
        UpdateButtons();
    }
    
    private void UpdateCurrentSceneText()
    {
        if (currentPoint != null && currentPoint.sceneData != null)
        {
            currentSceneText.text = "Current Scene: " + currentPoint.sceneData.sceneName;
        }
        else
        {
            currentSceneText.text = "Current Scene: Unknown";
        }
    }
}
