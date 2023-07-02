using TMPro;
using UnityEngine.UI;

public class SceneController
{
    public TextMeshPro currentSceneText;
    public TextMeshPro nextScenesText;

    public void UpdateCurrentSceneText(string text)
    {
        currentSceneText.text = text;
    }

    public void UpdateNextSceneText(string text)
    {
        nextScenesText.text = text;
    }
}
