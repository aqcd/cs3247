using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Heal : NetworkBehaviour, ISkill
{
    private GameObject player;
    private Health playerHealth;
    private float healMagnitude = SkillConstants.HEAL_HP_RATIO;
    private AudioManager audioManager;

    void Start() {
        if (!isServer) {
            player = MatchManager.instance.GetPlayer();
            playerHealth = player.GetComponent<Health>();
            audioManager = player.GetComponent<AudioManager>();
        }
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
