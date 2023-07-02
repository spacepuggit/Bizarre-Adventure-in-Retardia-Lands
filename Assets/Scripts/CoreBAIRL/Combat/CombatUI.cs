using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI : MonoBehaviour
{
    public TMP_Dropdown bodyPartDropdown;
    public Button attackButton;
    private CombatSystem combatSystem;

    private void Start()
    {
        combatSystem = ServiceLocator.Instance.Get<CombatSystem>();

        PopulateBodyPartDropdown();

        attackButton.onClick.AddListener(OnAttackButton);

        // Subscribe to the OnTurnChanged event
        combatSystem.OnTurnChanged += OnTurnChanged;
    }

    private void OnTurnChanged(bool isPlayerTurn)
    {
        // Enable the attack button if it's the player's turn, disable it otherwise
        attackButton.interactable = isPlayerTurn;
    }

    private void PopulateBodyPartDropdown()
    {
        bodyPartDropdown.options.Clear();

        // Add body parts to dropdown
        foreach (BodyPart bodyPart in Enum.GetValues(typeof(BodyPart)))
        {
            Character characterToAttack = combatSystem.isPlayerTurn ? combatSystem.opponent : combatSystem.player;

            if (!characterToAttack.IsBodyPartDestroyed(bodyPart))
            {
                bodyPartDropdown.options.Add(new TMP_Dropdown.OptionData() { text = bodyPart.ToString() });
            }
        }
    }

    private void OnAttackButton()
    {
        BodyPart selectedPart = (BodyPart)Enum.Parse(typeof(BodyPart), bodyPartDropdown.options[bodyPartDropdown.value].text);

        // Log initial health
        Character characterToAttack = combatSystem.isPlayerTurn ? combatSystem.opponent : combatSystem.player;
        Debug.Log($"{selectedPart} initially has {characterToAttack.GetBodyPartHealth(selectedPart)} health.");

        combatSystem.Attack(selectedPart);

        // Log attack information
        string attacker = combatSystem.isPlayerTurn ? "Player" : "Opponent";
        Debug.Log($"{attacker} attacked {selectedPart}. It's now {(combatSystem.isPlayerTurn ? "Player's" : "Opponent's")} turn.");

        PopulateBodyPartDropdown();
    }
}