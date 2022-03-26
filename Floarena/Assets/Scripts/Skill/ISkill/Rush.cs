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

    private Vector3 direction = new Vector3();

    private float remainingDuration = 0.0f;

    public AudioSource audioSource;
    public AudioClip rushAudio;

    void Awake() {
        player = MatchManager.instance.GetPlayer();
        characterController = player.GetComponent<CharacterController>();
        playerManager = player.GetComponent<PlayerManager>();
        audioSource = player.GetComponent<AudioSource>();
    }

    void Update() {
        if (remainingDuration > 0.0f) {
            characterController.Move(direction * (speed * Time.deltaTime));
            remainingDuration -= Time.deltaTime;
        }
    }
    
    [ClientRpc]
    public void RpcPlayRushAudio() {
        if (!isLocalPlayer) {
            audioSource.PlayOneShot(rushAudio, 0.2f);
        } 
    }

    [Command(requiresAuthority = false)]
    public void CmdPlayRushAudio() {
        RpcPlayRushAudio();
    }

    public void Execute(Vector3 skillPosition) {
        CmdPlayRushAudio();
        audioSource.PlayOneShot(rushAudio, 0.2f); // Only plays audio for local player
        remainingDuration = range / speed;
        direction = skillPosition.normalized;
        playerManager.DisableMoveForDuration(remainingDuration);
    }

}
