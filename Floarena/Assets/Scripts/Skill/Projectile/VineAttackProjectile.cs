using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VineAttackProjectile : NetworkBehaviour
{
    private float range = SkillConstants.VINE_ATTACK_RANGE;
    private float projectileSpeed = SkillConstants.VINE_ATTACK_PROJECTILE_SPEED;
    private float damageMagnitude = SkillConstants.VINE_ATTACK_DAMAGE;
    private GameObject spawningPlayer;
    private Rigidbody rb;
    private bool isSpawningClient;

    void Awake()
    {
        StartCoroutine(DeathRoutine());
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
        Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), spawningPlayer.GetComponent<Collider>());
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(projectileSpeed * dir, ForceMode.VelocityChange);
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(range/projectileSpeed);
        GameObject.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject != spawningPlayer && isSpawningClient)
        {
            Health otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth != null)
            {
                otherHealth.TakeDamage(damageMagnitude);
            }
            GameObject.Destroy(gameObject);
        }
    }
}
