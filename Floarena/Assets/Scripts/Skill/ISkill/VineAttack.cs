using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineAttack : MonoBehaviour, ISkill
{
    private GameObject player;
    private float damageMagnitude = SkillConstants.VINE_ATTACK_DAMAGE;
    private float range = SkillConstants.VINE_ATTACK_RANGE;
    private float projectileSpeed = SkillConstants.VINE_ATTACK_PROJECTILE_SPEED;

    // Start is called before the first frame update
    void Start()
    {
        player = MatchManager.instance.GetPlayer();
    }

    public void Execute(Vector3 skillPosition)
    {
        
    }
}
