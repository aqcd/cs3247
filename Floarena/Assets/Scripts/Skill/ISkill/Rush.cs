using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rush : NetworkBehaviour, ISkill {
    private GameObject player;
    private GameObject opponent;
    private CharacterController characterController;
    private PlayerManager playerManager;
    private float range = SkillConstants.RUSH_RANGE;
    private float speed = SkillConstants.RUSH_SPEED;
    private float damageMagnitude = SkillConstants.RUSH_DAMAGE;

    private float damageRadius = SkillConstants.RUSH_AOE_RADIUS;

    private Vector3 direction = new Vector3();

    private float remainingDuration = 0.0f;

    private AudioManager audioManager;
    //public AudioSource audioSource;
    //public AudioClip rushAudio;

    private bool isActive = false;

    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        characterController = player.GetComponent<CharacterController>();
        playerManager = player.GetComponent<PlayerManager>();
        //audioSource = player.GetComponent<AudioSource>();
        audioManager = MatchManager.instance.GetComponent<AudioManager>();
    }

    void Update() {
        if (remainingDuration > 0.0f) {
            characterController.Move(direction * (speed * Time.deltaTime));
            remainingDuration -= Time.deltaTime;

            if (characterController.velocity.magnitude < 0.99 * speed) {
                remainingDuration = 0.0f;
            }
        } else if (isActive) {
            Damage();
            playerManager.EnableMove();
            isActive = false;
        }
    }
    /*
    [ClientRpc]
    public void RpcPlayRushAudio() {
        if (!isLocalPlayer) {
            audioSource.PlayOneShot(rushAudio, 0.2f);
        } 
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayRushAudio() {
        RpcPlayRushAudio();
    }*/

    public void Execute(Vector3 skillPosition) {
        //CmdPlayRushAudio();
        //audioSource.PlayOneShot(rushAudio, 0.2f); // Only plays audio for local player
        audioManager.PlaySound(0, player.transform.position);
        isActive = true;
        remainingDuration = range / speed;
        direction = skillPosition.normalized;
        playerManager.DisableMoveForDuration(remainingDuration);
    }

    private void Damage() {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, damageRadius);
        foreach (Collider collider in hitColliders) {
            GameObject hitObject = collider.gameObject;
            if (hitObject.tag != "Player" && hitObject.tag != "Damageable") {
                continue;
            }

            if (hitObject != player) {
                hitObject.SendMessage("TakeDamage", damageMagnitude);
            }
        }
    }
}
