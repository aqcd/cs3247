using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{    
    private bool canMove = true;
    private bool canSkill = true;

    private float disableMoveTimer = 0.0f;
    private float disableSkillTimer = 0.0f;
    private PlayerStats playerBuffs;

    //TODO: Change to private after testing
    public Dictionary<Attribute, float> buffDurations;
    void Awake() {
        playerBuffs = new PlayerStats();
        buffDurations = new Dictionary<Attribute, float>{
            { Attribute.HP, 0 },
            { Attribute.AD, 0 },
            { Attribute.AS, 0 },
            { Attribute.AR, 0 },
            { Attribute.MS, 0 },
        };
    }

    void Update() 
    {
        if (disableMoveTimer > 0.0f)
        {
            disableMoveTimer -= Time.deltaTime;
        }
        if (disableMoveTimer <= 0.0f)
        {
            canMove = true;
            disableMoveTimer = 0.0f;
        }

        if (disableSkillTimer > 0.0f)
        {
            disableSkillTimer -= Time.deltaTime;
        }
        if (disableSkillTimer <= 0.0f)
        {
            canSkill = true;
            disableSkillTimer = 0.0f;
        }

        foreach(Attribute att in Enum.GetValues(typeof(Attribute))) {
            float currTimer = buffDurations[att];
            if (currTimer > 0.0f) 
            {
                buffDurations[att] = currTimer - Time.deltaTime;
            }
            if (currTimer <= 0.0f)
            {
                playerBuffs.ResetAttribute(att);
                buffDurations[att] = 0.0f;
            }
        }
    }

    public void EnableMove() {
        canMove = true;
        disableMoveTimer = 0.0f;
    }

    public void DisableMoveForDuration(float duration)
    {
        canMove = false;
        disableMoveTimer = Mathf.Max(duration, disableMoveTimer);
    }

    public void DisableSkillForDuration(float duration)
    {
        canSkill = false;
        disableSkillTimer = Mathf.Max(duration, disableSkillTimer);
    }

    public void StunForDuration(float duration)
    {
        DisableMoveForDuration(duration);
        DisableSkillForDuration(duration);
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public bool GetCanSkill()
    {
        return canSkill;
    }

    public void BuffForDuration(Effect effect, float duration)
    {
        float currentBuffDuration = buffDurations[effect.attribute];
        if (currentBuffDuration < duration) {
            playerBuffs.ResetAttribute(effect.attribute);
            playerBuffs.ApplyEffect(effect);
            buffDurations[effect.attribute] = duration;
        }
    }

    public float GetAttributeBuff(Attribute attribute)
    {
        return playerBuffs.GetAttributeValue(attribute);
    }

    public float GetBuffDuration(Attribute attribute)
    {
        return buffDurations[attribute];
    }
}
