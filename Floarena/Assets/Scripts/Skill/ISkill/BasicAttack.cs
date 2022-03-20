using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, ISkill
{
    private GameObject player;
    private GameObject opponent;
    private float attackDamage;
    private float attackRange;
    private float attackCooldown;

    private float timeToAttack = 0.0f;

    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        // get attributes from loadout and calculate attack stats
        PlayerStats stats = GameManager.instance.loadout.GetLoadoutStats();
        attackDamage = stats.GetAttributeValue(Attribute.AD);
        attackRange = stats.GetAttributeValue(Attribute.AR);
        attackCooldown = 1/stats.GetAttributeValue(Attribute.AS);
    }

    public void Execute(Vector3 skillPosition) {
        // Set the boolean to play attack animation to true.
        player.GetComponent<Animator>().SetBool("BasicAttack", true);
        StartCoroutine(ExecuteHit(skillPosition));
    }

    IEnumerator ExecuteHit(Vector3 skillPosition) 
    {   
        if (Time.time > timeToAttack) 
        {
            timeToAttack = Time.time + attackCooldown;
            yield return new WaitForSeconds(0.35f);

            Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, attackRange);
            GameObject bestHit = null;
            float bestDistance = Mathf.Infinity;
            foreach (Collider collider in hitColliders) {
                GameObject hitObject = collider.gameObject;
                if (hitObject.tag != "Player" && hitObject.tag != "Damageable") {
                    continue;
                }

                if (hitObject != player) 
                {
                    if (hitObject == opponent) {
                        bestHit = hitObject;
                        break;
                    } else {
                        float sqrDistance = (player.transform.position - collider.transform.position).sqrMagnitude;
    
                        if (sqrDistance < bestDistance)
                        {
                            bestDistance = sqrDistance;
                            bestHit = hitObject;
                        }
                    }
                }
            }

            if (bestHit != null) {
                bestHit.SendMessage("TakeDamage", attackDamage);
            }
            
        }
    }
}
