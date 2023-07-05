using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BodyPart
{
    Head,
    Torso,
    LeftArm,
    RightArm,
    LeftLeg,
    RightLeg
}

[Serializable]
public class BodyPartHealth
{
    public int Health;
    public SpriteRenderer Sprite;
    public bool IsDestroyed = false;
    public bool IsImportant = false;
}

[Serializable]
public class BodyPartStatusEntry
{
    public BodyPart Key;
    public BodyPartHealth Value;
}

public class Character : MonoBehaviour
{
    public float destructionThreshold = 0.5f;
    public float flickerDuration = 0.5f;
    public float flickerSpeed = 0.1f;
    public List<BodyPartStatusEntry> bodyPartStatusEntries;
    public bool isDefending = false;

    private Dictionary<BodyPart, BodyPartHealth> bodyPartsHealth;
    private int totalBodyParts;
    private int destroyedBodyParts = 0;
    
    private IEnumerator highlightCoroutine;
    

    private void Awake()
    {
        bodyPartsHealth = new Dictionary<BodyPart, BodyPartHealth>();
        totalBodyParts = bodyPartStatusEntries.Count;

        foreach (var entry in bodyPartStatusEntries)
        {
            bodyPartsHealth.Add(entry.Key, entry.Value);
        }
    }

    public void TakeDamage(BodyPart part, int damage)
    {
        if (!bodyPartsHealth.ContainsKey(part) || bodyPartsHealth[part].IsDestroyed) return;
        
        if (isDefending)
        {
            damage /= 2;
            isDefending = false;
        }
        
        bodyPartsHealth[part].Health -= damage;
        StartCoroutine(Flicker(bodyPartsHealth[part].Sprite));

        // Check if body part is destroyed
        if (bodyPartsHealth[part].Health <= 0)
        {
            bodyPartsHealth[part].IsDestroyed = true;
            bodyPartsHealth[part].Sprite.enabled = false;
            destroyedBodyParts++;

            Debug.Log($"{part} is destroyed.");

            // Check if important part is destroyed or half of the body parts are destroyed
            if (!bodyPartsHealth[part].IsImportant &&
                !(destroyedBodyParts >= totalBodyParts * destructionThreshold)) return;
            Debug.Log($"{gameObject.name} has been defeated.");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"{part} now has {bodyPartsHealth[part].Health} health.");
        }
    }

    public int GetBodyPartHealth(BodyPart part)
    {
        return bodyPartsHealth.ContainsKey(part) ? bodyPartsHealth[part].Health : 0;
    }

    public bool IsBodyPartDestroyed(BodyPart part)
    {
        return bodyPartsHealth.ContainsKey(part) && bodyPartsHealth[part].IsDestroyed;
    }

    private IEnumerator Flicker(SpriteRenderer sprite)
    {
        float flickerTime = 0;
        while (flickerTime < flickerDuration)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(flickerSpeed);
            flickerTime += flickerSpeed;
        }
        sprite.enabled = true; // Ensure the sprite is enabled after flickering
    }
    
    public bool IsDefeated()
    {
        // Check if important part is destroyed or half of the body parts are destroyed
        if (bodyPartsHealth.Values.Any(bodyPartHealth => bodyPartHealth.IsImportant && bodyPartHealth.IsDestroyed))
        {
            return true;
        }
        return destroyedBodyParts >= totalBodyParts * destructionThreshold;
    }
    
    public void HighlightBodyPart(BodyPart part, bool highlight)
    {
        if (bodyPartsHealth.ContainsKey(part))
        {
            if (highlight)
            {
                StartCoroutine(Flicker(bodyPartsHealth[part].Sprite));
            }
            else
            {
                StopCoroutine(Flicker(bodyPartsHealth[part].Sprite));
                bodyPartsHealth[part].Sprite.color = Color.white;
            }
        }
    }


    private IEnumerator HighlightCoroutine(BodyPart part)
    {
        // Alternate the visibility of the body part every few frames
        while (true)
        {
            SetBodyPartVisibility(part, false);
            yield return new WaitForSeconds(0.5f);
            SetBodyPartVisibility(part, true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void SetBodyPartVisibility(BodyPart part, bool visible)
    {
        // Check if the body part exists in the dictionary
        if (bodyPartsHealth.ContainsKey(part))
        {
            // Set the visibility of the SpriteRenderer associated with the body part
            bodyPartsHealth[part].Sprite.enabled = visible;
        }
    }

    public void Defend()
    {
        isDefending = true;
    }

}
