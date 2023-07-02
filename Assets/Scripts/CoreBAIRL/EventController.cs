using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public GameObject battlePrefab;
    public GameObject chestPrefab;
    public GameObject eventPrefab;
    public GameObject shopPrefab;

    public void TriggerEvent(EventType eventType)
    {
        switch (eventType)
        {
            case EventType.Battle:
                Instantiate(battlePrefab);
                break;
            case EventType.Chest:
                Instantiate(chestPrefab);
                break;
            case EventType.Event:
                Instantiate(eventPrefab);
                break;
            case EventType.Shop:
                Instantiate(shopPrefab);
                break;
            default:
                Debug.LogError("Invalid event type: " + eventType);
                break;
        }
    }
}

