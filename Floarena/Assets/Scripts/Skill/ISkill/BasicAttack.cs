using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, ISkill
{
    private GameObject player;
    private GameObject opponent;
    private float damageMagnitude = (float) SkillConstants.BASIC_ATTACK_DAMAGE;
    private float range = (float) SkillConstants.BASIC_ATTACK_RANGE;

    void Start()
    {
        player = MatchManager.instance.GetPlayer();
    }

    public void Execute(Vector3 skillPosition) 
    {   
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, range);
        GameObject bestHit = null;
        float bestDistance = Mathf.Infinity;
        foreach (Collider collider in hitColliders) {
            GameObject hitObject = collider.gameObject;
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
        bestHit.SendMessage("TakeDamage", damageMagnitude);
    }
}
