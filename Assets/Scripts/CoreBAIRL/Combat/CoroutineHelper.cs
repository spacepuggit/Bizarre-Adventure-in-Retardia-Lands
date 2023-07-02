using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static CoroutineHelper instance;

    public static CoroutineHelper Instance
    {
        get
        {
            if (instance != null) return instance;
            // Create a new GameObject with a CoroutineHelper component
            GameObject gameObject = new GameObject("CoroutineHelper");
            instance = gameObject.AddComponent<CoroutineHelper>();

            // Make sure the CoroutineHelper isn't destroyed between scenes
            DontDestroyOnLoad(gameObject);

            return instance;
        }
    }

    public Coroutine StartHelperCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}