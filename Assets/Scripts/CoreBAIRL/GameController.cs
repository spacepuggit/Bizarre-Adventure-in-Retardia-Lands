using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameObject playerPrefab;
    
    public TextMeshPro currentSceneText;
    public TextMeshPro nextScenesText;
    
    public Transform buttonContainer;
    public EventController eventController;

    // Start is called before the first frame update
    void Start()
    {
        mapGenerator.RecreateBoard();
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            // Get the first point of interest
            PointOfInterest firstPoint = mapGenerator.GetFirstPointOfInterest();

            if (firstPoint != null)
            {
                // Instantiate the player at the position of the first point of interest
                GameObject player = Instantiate(playerPrefab, firstPoint.transform.position, Quaternion.identity);
                // Set the current point in the PlayerController script
                PlayerController playerController = player.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.buttonContainer = buttonContainer;
                    playerController.currentSceneText = currentSceneText;
                    playerController.nextScenesText = nextScenesText;
                    playerController.SetCurrentPoint(firstPoint);
                    playerController.eventController = eventController;
                }
                else
                {
                    Debug.LogError("Player Prefab does not have a PlayerController script");
                }

            }
            else
            {
                Debug.LogError("No points of interest available for player spawn");
            }
        }
        else
        {
            Debug.LogError("Player Prefab is not set in GameController");
        }
    }
}
