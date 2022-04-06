using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VinePull : NetworkBehaviour, ISkill
{
    private GameObject player;
    private GameObject projectilePrefab;

    private AudioManager audioManager;

    // Start is called before the first frame update
    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        projectilePrefab = transform.GetComponent<ProjectileManager>().projectilePrefabs[0];
        audioManager = player.GetComponent<AudioManager>();
    }

    public void Execute(Vector3 skillPosition)
    {
        SpawnProjectile(skillPosition.normalized, player.transform.position, MatchManager.instance.GetPlayerNum());
        audioManager.PlaySound(AudioIndex.VINE_PULL_AUDIO, skillPosition);
    }

    [Command(requiresAuthority = false)]
    void SpawnProjectile(Vector3 spawnDir, Vector3 pos, int spawnPlayerNum) 
    {
        Quaternion qt = Quaternion.FromToRotation(new Vector3(0, 0, 1), spawnDir);
        GameObject projectile = GameObject.Instantiate(projectilePrefab, pos, qt);
        NetworkServer.Spawn(projectile);
        projectile.GetComponent<VinePullProjectile>().OnSpawn(spawnDir, spawnPlayerNum);
    }
}
