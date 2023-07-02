using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scene Data", order = 51)]
public class SceneData : ScriptableObject
{
    public string sceneName;
    public string description;
    public Sprite sceneImage;
}