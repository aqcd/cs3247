using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, ISkill
{
    private GameObject player;
    private float damageMagnitude = SkillConstants.BASIC_ATTACK_DAMAGE;
    private float range = SkillConstants.BASIC_ATTACK_RANGE;

    void Start()
    {
        player = MatchManager.instance.GetPlayer();
    }

    public void Execute(Vector3 skillPosition) 
    {   
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, range);
        foreach (Collider collider in hitColliders) {
            collider.gameObject.SendMessage("TakeDamage", damageMagnitude);
        }
    }
}
