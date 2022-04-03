using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rush : NetworkBehaviour, ISkill
{
    private GameObject player;
    private CharacterController characterController;
    private PlayerManager playerManager;
    private float speed = SkillConstants.RUSH_SPEED;
    private float range = SkillConstants.RUSH_RANGE;
    private float damageMagnitude = SkillConstants.RUSH_DAMAGE;

    private float damageRadius = SkillConstants.RUSH_AOE_RADIUS;

    private Vector3 direction = new Vector3();

    private float remainingDuration = 0.0f;

    private bool isActive = false;

    private AudioManager audioManager;

    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        characterController = player.GetComponent<CharacterController>();
        playerManager = player.GetComponent<PlayerManager>();
        audioManager = player.GetComponent<AudioManager>();
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

    public void Execute(Vector3 skillPosition)
    {
        isActive = true;
        remainingDuration = Mathf.Min(skillPosition.magnitude, range) / speed;
        direction = skillPosition.normalized;
        playerManager.DisableMoveForDuration(remainingDuration);
        audioManager.PlaySound(AudioIndex.RUSH_AUDIO, skillPosition);
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
