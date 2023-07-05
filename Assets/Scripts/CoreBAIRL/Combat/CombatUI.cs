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
    private BodyPart lastHighlightedBodyPart = BodyPart.Head;

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

        // Only highlight the selected body part if it has changed
        if (selectedBodyPart != lastHighlightedBodyPart)
        {
            // Unhighlight the last highlighted body part
            enemy.HighlightBodyPart(lastHighlightedBodyPart, false);

            // Highlight the selected body part on the enemy character
            enemy.HighlightBodyPart(selectedBodyPart, true);

            lastHighlightedBodyPart = selectedBodyPart;
        }
    }

    private void OnAttackButtonClicked()
    {
        if (combatSystem.state == CombatState.Idle)
        {
            combatSystem.StartAttack();
            defendButton.visible = false;
            // Enable body part selection
        }
        else if (combatSystem.state == CombatState.SelectingAttackTarget)
        {
            combatSystem.EndAttack(selectedBodyPart);
            defendButton.visible = true;
            // Disable body part selection
        }
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