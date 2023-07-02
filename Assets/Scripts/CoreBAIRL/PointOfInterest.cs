using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EventType
{
    Battle,
    Chest,
    Event,
    Shop
}

public class PointOfInterest : MonoBehaviour
{
    public List<PointOfInterest> NextPointsOfInterestWithPath { get; set; } = new();
    public SceneData sceneData;
    public EventType eventType;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (PointOfInterest nextPoint in NextPointsOfInterestWithPath)
        {
            Gizmos.DrawLine(transform.position, nextPoint.transform.position);
        }
    }

    public string LoadScene()
    {
        if (sceneData != null && !string.IsNullOrEmpty(sceneData.sceneName))
        {
            SceneManager.LoadSceneAsync(sceneData.sceneName, LoadSceneMode.Additive);
            return sceneData.sceneName;
        }
        else
        {
            Debug.LogWarning("SceneData is null or sceneName is empty. Can't load the scene.");
            return null;
        }
    }

}

