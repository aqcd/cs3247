using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VineAttack : NetworkBehaviour, ISkill
{
    private GameObject player;
    private GameObject projectilePrefab;

    // Start is called before the first frame update
    void Start() {
        projectilePrefab = transform.GetComponent<ProjectileManager>().projectilePrefabs[0];
        if (!isServer) {
            player = MatchManager.instance.GetPlayer();
        }
    }

    public void Execute(Vector3 skillPosition)
    {
        SpawnProjectile(skillPosition.normalized, player.transform.position, MatchManager.instance.GetPlayerNum());
        AudioManager.instance.PlaySound(AudioIndex.VINE_ATTACK_AUDIO, skillPosition);
    }

    [Command(requiresAuthority = false)]
    void SpawnProjectile(Vector3 spawnDir, Vector3 pos, int spawnPlayerNum) 
    {
        Quaternion qt = Quaternion.FromToRotation(new Vector3(0, 0, 1), spawnDir);
        GameObject projectile = GameObject.Instantiate(projectilePrefab, pos, qt);
        NetworkServer.Spawn(projectile);
        projectile.GetComponent<VineAttackProjectile>().OnSpawn(spawnDir, spawnPlayerNum);
    }
}
