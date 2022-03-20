using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VineAttackProjectile : NetworkBehaviour
{
    private float range = SkillConstants.VINE_ATTACK_RANGE;
    private float projectileSpeed = SkillConstants.VINE_ATTACK_PROJECTILE_SPEED;
    private float damageMagnitude = SkillConstants.VINE_ATTACK_DAMAGE;
    private GameObject player;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        player = MatchManager.instance.GetPlayer();
        StartCoroutine(DeathRoutine());
    }


    [ClientRpc]
    public void OnSpawn(Vector3 dir) 
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(projectileSpeed * dir, ForceMode.VelocityChange);
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(range/projectileSpeed);
        GameObject.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject != player)
        {
            Health otherHealth = other.gameObject.GetComponent<Health>();
            if (otherHealth != null) 
            {
                otherHealth.TakeDamage(damageMagnitude);
            }
        }
        GameObject.Destroy(gameObject);
    }
}
