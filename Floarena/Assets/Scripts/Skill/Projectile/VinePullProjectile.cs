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
    private GameObject spawningPlayer;
    private CharacterController playerCharacterController;
    private Rigidbody rb;

    private Vector3 direction = new Vector3();
    private float remainingDuration = 0.0f;
    private IEnumerator deathCoroutine;
    private bool hit = false;
    private bool isSpawningClient;
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
            Debug.Log("Remaining Duration: " + remainingDuration);
            playerCharacterController.Move(direction * pullSpeed * Time.deltaTime);
            remainingDuration -= Time.deltaTime;
        }
        if (hit && remainingDuration <= 0.0f)
        {
            CmdDestroy();
        }
    }

    [ClientRpc]
    public void OnSpawn(Vector3 dir, int spawnPlayerNum) 
    {
        if (MatchManager.instance.GetPlayerNum() == spawnPlayerNum) {
            spawningPlayer = MatchManager.instance.GetPlayer();
            isSpawningClient = true;
        } else {
            spawningPlayer = MatchManager.instance.GetOpponent();
            isSpawningClient = false;
        }
        Debug.Log("DISABLING: " + gameObject.GetComponent<Collider>() + "AND" + spawningPlayer.GetComponent<Collider>());
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), spawningPlayer.GetComponent<Collider>());
        playerCharacterController = spawningPlayer.GetComponent<CharacterController>();
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(projectileSpeed * dir, ForceMode.VelocityChange);
    }
    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(range/projectileSpeed);
        CmdDestroy();
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject != spawningPlayer && isSpawningClient && !hit)
        {
            CmdStopCoroutine();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero; 
            // gameObject.GetComponent<Collider>().enabled = false;
            rb.detectCollisions = false;
            hit = true;
            Health otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth != null) 
            {
                otherHealth.TakeDamage(damageMagnitude);
            }
            MovePlayerToTarget(other.gameObject);
        }
    }
    [Command(requiresAuthority = false)]
    private void CmdDestroy() {
        NetworkServer.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    private void CmdStopCoroutine() {
        StopCoroutine(deathCoroutine);
        RpcStopCoroutine();
    }

    [ClientRpc]
    private void RpcStopCoroutine() {
        StopCoroutine(deathCoroutine);
    }

    private void MovePlayerToTarget(GameObject target) 
    {
        float distance = Vector3.Distance(spawningPlayer.transform.position, target.transform.position);
        Debug.Log("Distance to target:" + distance);
        direction = (target.transform.position - spawningPlayer.transform.position).normalized;
        remainingDuration = distance/pullSpeed;
        PlayerManager playerManager = spawningPlayer.GetComponent<PlayerManager>();
        PlayerManager targetPlayerManager = target.GetComponent<PlayerManager>();
        playerManager.StunForDuration(remainingDuration);
        if (targetPlayerManager)
        {
            targetPlayerManager.StunForDuration(remainingDuration);
        }
    }
}
