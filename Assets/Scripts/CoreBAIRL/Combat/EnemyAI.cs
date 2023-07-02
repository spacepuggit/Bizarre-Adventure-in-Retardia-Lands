using System.Linq;
using UnityEngine;

public class EnemyAI
{
    private Character player;

    public EnemyAI(Character player)
    {
        this.player = player;
    }

    public BodyPart ChooseBodyPartToAttack()
    {
        // Get a list of all body parts that haven't been destroyed yet
        var undestroyedBodyParts = System.Enum.GetValues(typeof(BodyPart))
            .Cast<BodyPart>()
            .Where(bodyPart => !player.IsBodyPartDestroyed(bodyPart))
            .ToList();

        // If there are no undestroyed body parts, the player is defeated
        if (!undestroyedBodyParts.Any())
        {
            Debug.Log("Player has been defeated.");
            return default(BodyPart);
        }

        // Select a random undestroyed body part to attack
        var randomBodyPart = undestroyedBodyParts[UnityEngine.Random.Range(0, undestroyedBodyParts.Count)];

        return randomBodyPart;
    }
}