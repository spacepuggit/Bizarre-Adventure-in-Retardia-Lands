using UnityEngine;
using UnityEngine.UIElements;

public class CombatUI : MonoBehaviour
{
    private VisualElement rootElement;
    private Button attackButton;
    private Button defendButton;

    private void Start()
    {
        // Get a reference to the root UI document
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        // Find the UI elements
        attackButton = rootElement.Q<Button>("AttackButton");
        defendButton = rootElement.Q<Button>("DefendButton");

        // Add event listeners
        attackButton.clicked += OnAttackButtonClicked;
        defendButton.clicked += OnDefendButtonClicked;
    }

    private void OnAttackButtonClicked()
    {
        // Handle attack button click
    }

    private void OnDefendButtonClicked()
    {
        // Handle defend button click
    }
}