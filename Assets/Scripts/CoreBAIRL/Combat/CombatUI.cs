using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatUI : MonoBehaviour
{
    private VisualElement rootElement;
    private Button attackButton;
    private Button defendButton;
    private CombatSystem combatSystem;
    private Character enemy;
    private BodyPart selectedBodyPart = BodyPart.Head;

    private void Start()
    {
        combatSystem = ServiceLocator.Instance.Get<CombatSystem>();
        
        enemy = combatSystem.opponent;
        
        var uiDocument = GetComponent<UIDocument>();
        rootElement = uiDocument.rootVisualElement;

        // Find the UI elements
        attackButton = rootElement.Q<Button>("AttackButton");
        defendButton = rootElement.Q<Button>("DefendButton");

        // Add event listeners
        attackButton.clicked += OnAttackButtonClicked;
        defendButton.clicked += OnDefendButtonClicked;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Change selectedBodyPart to the next body part
            selectedBodyPart = GetNextBodyPart(selectedBodyPart);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Change selectedBodyPart to the previous body part
            selectedBodyPart = GetPreviousBodyPart(selectedBodyPart);
        }

        // Highlight the selected body part on the enemy character
        enemy.HighlightBodyPart(selectedBodyPart, true);
    }

    private void OnAttackButtonClicked()
    {
        combatSystem.Attack(selectedBodyPart);
    }

    private void OnDefendButtonClicked()
    {
        combatSystem.Defend();
    }
    
    private BodyPart GetNextBodyPart(BodyPart currentBodyPart)
    {
        int nextBodyPartIndex = ((int)currentBodyPart + 1) % Enum.GetValues(typeof(BodyPart)).Length;
        return (BodyPart)nextBodyPartIndex;
    }

    private BodyPart GetPreviousBodyPart(BodyPart currentBodyPart)
    {
        int previousBodyPartIndex = ((int)currentBodyPart - 1 + Enum.GetValues(typeof(BodyPart)).Length) % Enum.GetValues(typeof(BodyPart)).Length;
        return (BodyPart)previousBodyPartIndex;
    }


}