using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{    
    private bool canMove = true;
    private bool canSkill = true;

    private float disableMoveTimer = 0.0f;
    private float disableSkillTimer = 0.0f;
    // Start is called before the first frame update
    void Awake() {

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
}
