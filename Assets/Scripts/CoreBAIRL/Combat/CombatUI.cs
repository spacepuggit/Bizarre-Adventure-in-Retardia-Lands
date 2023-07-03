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
    public Button defendButton;
    private CombatSystem combatSystem;
    private int selectedBodyPartIndex = 0;
    private bool isChoosingAction = true; // Add this
    private bool isAttacking = false; // Add this

    private void Start()
    {
        combatSystem = ServiceLocator.Instance.Get<CombatSystem>();
        PopulateBodyPartDropdown();
        attackButton.onClick.AddListener(OnAttackButton);
        defendButton.onClick.AddListener(OnDefendButton);
        combatSystem.OnTurnChanged += OnTurnChanged;
    }

    private void Update()
    {
        if (combatSystem.isPlayerTurn)
        {
            if (isChoosingAction)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    isAttacking = !isAttacking;
                    // Update the UI to reflect the new selection
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isChoosingAction = false;
                    bodyPartDropdown.gameObject.SetActive(isAttacking);
                }
            }
            else if (isAttacking)
            {
                BodyPart previousPart = (BodyPart)Enum.Parse(typeof(BodyPart), bodyPartDropdown.options[selectedBodyPartIndex].text);
                combatSystem.opponent.HighlightBodyPart(previousPart, false);

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    selectedBodyPartIndex--;
                    if (selectedBodyPartIndex < 0)
                    {
                        selectedBodyPartIndex = bodyPartDropdown.options.Count - 1;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    selectedBodyPartIndex++;
                    if (selectedBodyPartIndex >= bodyPartDropdown.options.Count)
                    {
                        selectedBodyPartIndex = 0;
                    }
                }

                BodyPart selectedPart = (BodyPart)Enum.Parse(typeof(BodyPart), bodyPartDropdown.options[selectedBodyPartIndex].text);
                combatSystem.opponent.HighlightBodyPart(selectedPart, true);

                bodyPartDropdown.value = selectedBodyPartIndex;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isAttacking)
                {
                    OnAttackButton();
                }
                else
                {
                    OnDefendButton();
                }
            }
        }
    }


    private void OnTurnChanged(bool isPlayerTurn)
    {
        attackButton.interactable = isPlayerTurn;
        defendButton.interactable = isPlayerTurn;
        if (isPlayerTurn)
        {
            isChoosingAction = true;
            bodyPartDropdown.gameObject.SetActive(false);
        }
    }

    private void PopulateBodyPartDropdown()
    {
        bodyPartDropdown.options.Clear();
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
        if (isChoosingAction)
        {
            isChoosingAction = false;
            bodyPartDropdown.gameObject.SetActive(true);
        }
        else
        {
            BodyPart selectedPart = (BodyPart)Enum.Parse(typeof(BodyPart), bodyPartDropdown.options[bodyPartDropdown.value].text);
            combatSystem.Attack(selectedPart);
            isChoosingAction = true;
            bodyPartDropdown.gameObject.SetActive(false);
        }
    }

    
    private void OnDefendButton()
    {
        if (isChoosingAction)
        {
            combatSystem.Defend();
            isChoosingAction = true;
        }
    }
}
