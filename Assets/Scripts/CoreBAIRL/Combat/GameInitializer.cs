using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    public Character playerPrefab;
    public Character enemyPrefab;
    private Character playerInstance;
    private Character enemyInstance;

    private void Awake()
    {
        playerInstance = Instantiate(playerPrefab);
        enemyInstance = Instantiate(enemyPrefab);
        CombatSystem combatSystem = new CombatSystem(playerInstance, enemyInstance);
        ServiceLocator.Instance.Register<CombatSystem>(combatSystem);
    }
}


