using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform buttonContainer;
    public EventController eventController;
    [SerializeField] private GameObject playerPrefab; // Keep this in GameController

    private MapController mapController;
    private PlayerSpawner playerSpawner;
    private SceneController sceneController;

    void Start()
    {
        mapController = new MapController();
        playerSpawner = new PlayerSpawner(playerPrefab); // Pass playerPrefab here
        sceneController = new SceneController();

        mapController.GenerateMap();

        PointOfInterest firstPoint = mapController.GetFirstPointOfInterest();
        PlayerController playerController = playerSpawner.SpawnPlayer(firstPoint);

        playerController.buttonContainer = buttonContainer;
        playerController.currentSceneText = sceneController.currentSceneText;
        playerController.nextScenesText = sceneController.nextScenesText;
        playerController.SetCurrentPoint(firstPoint);
        playerController.eventController = eventController;
    }
}
