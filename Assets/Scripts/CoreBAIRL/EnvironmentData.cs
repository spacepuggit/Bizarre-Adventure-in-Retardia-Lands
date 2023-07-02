using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentData", menuName = "ScriptableObjects/EnvironmentData", order = 1)]
public class EnvironmentData : ScriptableObject
{
    public List<SceneData> sceneDataList;
    public SceneData startingSceneData; 
    public SceneData finishingSceneData;
}