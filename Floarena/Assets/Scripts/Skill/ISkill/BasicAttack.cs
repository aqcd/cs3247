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
        player = GameManager.instance.GetPlayer();
    }

    public void Execute(Vector3 skillPosition) 
    {   
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, radius);
        foreach (Collider collider in hitColldiers) {
            collider.gameObject.SendMessage("TakeDamage", damageMagnitude);
        }
    }
}
