using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VineAttack : NetworkBehaviour, ISkill
{
    private GameObject player;
    public GameObject projectilePrefab;
    private float damageMagnitude = SkillConstants.VINE_ATTACK_DAMAGE;

    // Start is called before the first frame update
    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
    }

    public void Execute(Vector3 skillPosition)
    {
        SpawnProjectile(skillPosition.normalized);
    }

    [Command]
    void SpawnProjectile(Vector3 spawnDir) 
    {
        Vector3 pos = player.transform.position;
        Quaternion qt = Quaternion.FromToRotation(new Vector3(1, 0, 0), spawnDir);
        GameObject projectile = GameObject.Instantiate(projectilePrefab, pos, qt);
        NetworkServer.Spawn(projectile);
        projectile.GetComponent<VineAttackProjectile>().OnSpawn(spawnDir);
    }
}
