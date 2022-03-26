using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VineAttackProjectile : NetworkBehaviour
{
    private float range = SkillConstants.VINE_ATTACK_RANGE;
    private float projectileSpeed = SkillConstants.VINE_ATTACK_PROJECTILE_SPEED;

    private Rigidbody rb;

    public AudioSource audioSource;
    public AudioClip vineAttackAudio;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathRoutine());
    }


    [ClientRpc]
    public void OnSpawn(Vector3 dir) 
    {
        audioSource.PlayOneShot(vineAttackAudio, 0.5f);
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(projectileSpeed * dir, ForceMode.VelocityChange);
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(range/projectileSpeed);
        GameObject.Destroy(gameObject);
    }
}
