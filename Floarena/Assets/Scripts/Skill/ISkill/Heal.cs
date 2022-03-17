using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, ISkill
{
    private GameObject player;
    private Health playerHealth;
    private float healMagnitude = (float) SkillConstants.HEAL_HP_RATIO;

    void Start()
    {
        player = GameManager.instance.GetPlayer();
        playerHealth = player.GetComponent<Health>();
    }

    public void Execute(Vector3 skillPosition) 
    {   
        int healing = Mathf.FloorToInt(healMagnitude * playerHealth.maxHealth);
        playerHealth.TakeHealing(healing);
    }
}
