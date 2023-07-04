using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatSystem
{
    public Character player;
    public Character opponent;
    public bool isPlayerTurn = true;

    private EnemyAI enemyAI;
    
    public event Action<bool> OnTurnChanged;
    public event Action<string> OnVictory;

    public CombatSystem(Character player, Character opponent)
    {
        this.player = player;
        this.opponent = opponent;
        this.enemyAI = new EnemyAI(player);
    }

    private IEnumerator OpponentTurn()
    {
        // Wait for a short delay before making the opponent's move
        yield return new WaitForSeconds(1);
        var partToAttack = enemyAI.ChooseBodyPartToAttack();
        Attack(partToAttack);
    }

    public void Attack(BodyPart part)
    {
        if (isPlayerTurn)
        {
            opponent.TakeDamage(part, 20); // Arbitrary damage value
            if (opponent.IsDefeated())
            {
                OnVictory?.Invoke("Player"); // Fire victory event
                return;
            }

            isPlayerTurn = !isPlayerTurn;
            OnTurnChanged?.Invoke(isPlayerTurn); // Fire event
            if (!isPlayerTurn)
            {
                // Start opponent's turn after a delay
                CoroutineHelper.Instance.StartHelperCoroutine(OpponentTurn());
            }
        }
        else
        {
            var partToAttack = enemyAI.ChooseBodyPartToAttack();
            player.TakeDamage(partToAttack, 20); // Arbitrary damage value
            if (player.IsDefeated())
            {
                OnVictory?.Invoke("Opponent"); // Fire victory event
                return;
            }

            isPlayerTurn = !isPlayerTurn;
            OnTurnChanged?.Invoke(isPlayerTurn); // Fire event
        }
    }

    public void Defend()
    {
        if (isPlayerTurn)
        {
            player.Defend();
            isPlayerTurn = !isPlayerTurn;
            OnTurnChanged?.Invoke(isPlayerTurn);
            if (!isPlayerTurn)
            {
                CoroutineHelper.Instance.StartHelperCoroutine(OpponentTurn());
            }
        }
    }

}



