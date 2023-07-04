using UnityEngine;
using UnityEngine.UIElements;

public class CombatUI : MonoBehaviour
{
    private VisualElement rootElement;
    private Button attackButton;
    private Button defendButton;
    private CombatSystem combatSystem;

    private void Start()
    {
        combatSystem = ServiceLocator.Instance.Get<CombatSystem>();
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
        BodyPart chosenPart = GetChosenBodyPart();
        
        combatSystem.Attack(chosenPart);
    }

    private void OnDefendButtonClicked()
    {
        combatSystem.Defend();
    }
    
    private BodyPart GetChosenBodyPart()
    {
        return BodyPart.Head;
    }
}