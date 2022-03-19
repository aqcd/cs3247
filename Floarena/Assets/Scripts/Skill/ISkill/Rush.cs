using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Rush : NetworkBehaviour, ISkill
{
    private GameObject player;
    private CharacterController characterController;
    private PlayerManager playerManager;
    private float range = SkillConstants.RUSH_RANGE;
    private float speed = SkillConstants.RUSH_SPEED;
    private float damageMagnitude = SkillConstants.RUSH_DAMAGE;

    private Vector3 direction = new Vector3();

    private float remainingDuration = 0.0f;

    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        characterController = player.GetComponent<CharacterController>();
        playerManager = player.GetComponent<PlayerManager>();
    }

    void Update() {
        if (remainingDuration > 0.0f) {
            characterController.Move(direction * (speed * Time.deltaTime));
            remainingDuration -= Time.deltaTime;
        }
    }

    public void Execute(Vector3 skillPosition)
    {
        remainingDuration = range / speed;
        direction = skillPosition.normalized;
        playerManager.DisableMoveForDuration(remainingDuration);
    }
}
