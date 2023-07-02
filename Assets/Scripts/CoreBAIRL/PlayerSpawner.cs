using UnityEngine;

public class PlayerSpawner
{
    private GameObject playerPrefab;

    public PlayerSpawner(GameObject playerPrefab)
    {
        this.playerPrefab = playerPrefab;
    }

    public PlayerController SpawnPlayer(PointOfInterest spawnPoint)
    {
        GameObject player = UnityEngine.Object.Instantiate(playerPrefab, spawnPoint.transform.position, Quaternion.identity);
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController == null)
        {
            Debug.LogError("Player Prefab does not have a PlayerController script");
            return null;
        }

        return playerController;
    }
}


