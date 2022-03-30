using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour, ISkill
{
    private GameObject player;
    private Health playerHealth;
    private float healMagnitude = SkillConstants.HEAL_HP_RATIO;
    private AudioManager audioManager;

    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        playerHealth = player.GetComponent<Health>();
        audioManager = player.GetComponent<AudioManager>();
    }

    public void Execute(Vector3 skillPosition) 
    {
        // Set the boolean to play casting animation to true.
        player.GetComponent<Animator>().SetBool("isHeal", true);

        int healing = Mathf.FloorToInt(healMagnitude * playerHealth.maxHealth);
        playerHealth.TakeHealing(healing);
        audioManager.PlaySound(AudioIndex.INCREASE_HEALTH_AUDIO, skillPosition);
    }
}
