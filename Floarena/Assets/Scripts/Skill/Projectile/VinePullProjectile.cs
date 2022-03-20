using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VinePullProjectile : NetworkBehaviour
{
    private float range = SkillConstants.VINE_PULL_RANGE;
    private float projectileSpeed = SkillConstants.VINE_PULL_PROJECTILE_SPEED;
    private float damageMagnitude = SkillConstants.VINE_PULL_DAMAGE;
    private float pullSpeed = SkillConstants.VINE_PULL_PROJECTILE_SPEED * 1.3f;
    private GameObject player;
    private CharacterController playerCharacterController;
    private Rigidbody rb;

    private Vector3 direction = new Vector3();
    private float remainingDuration = 0.0f;
    private IEnumerator deathCoroutine;
    private bool hit = false;
    
    void Awake()
    {   
        deathCoroutine = DeathRoutine();
        StartCoroutine(deathCoroutine);
    }

    void Update()
    {
        if (playerCharacterController == null) {
            return;
        }
        if (remainingDuration > 0.0f) 
        {
            playerCharacterController.Move(direction * pullSpeed * Time.deltaTime);
            remainingDuration -= Time.deltaTime;
        }
        if (hit && remainingDuration <= 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
    }

    [ClientRpc]
    public void OnSpawn(Vector3 dir, int spawnPlayerNum) 
    {
        if (MatchManager.instance.GetPlayerNum() == spawnPlayerNum) {
            player = MatchManager.instance.GetPlayer();
        } else {
            player = MatchManager.instance.GetOpponent();
        }
        playerCharacterController = player.GetComponent<CharacterController>();
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(projectileSpeed * dir, ForceMode.VelocityChange);
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(range/projectileSpeed);
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        StopCoroutine(deathCoroutine);
        hit = true;
        if (other.gameObject != player)
        {
            Health otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth != null) 
            {
                otherHealth.TakeDamage(damageMagnitude);
            }
            MovePlayerToTarget(other.gameObject);
        }
    }

    private void MovePlayerToTarget(GameObject target) 
    {
        float distance = Vector3.Distance(player.transform.position, target.transform.position);
        direction = (target.transform.position - player.transform.position).normalized;
        remainingDuration = distance/pullSpeed;
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        PlayerManager targetPlayerManager = target.GetComponent<PlayerManager>();
        playerManager.StunForDuration(remainingDuration);
        if (targetPlayerManager)
        {
            targetPlayerManager.StunForDuration(remainingDuration);
        }
    }
}
