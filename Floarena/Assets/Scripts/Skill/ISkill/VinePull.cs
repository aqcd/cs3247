using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class VinePull : NetworkBehaviour, ISkill
{
    private GameObject player;
    public GameObject projectilePrefab;

    // Start is called before the first frame update
    void Awake()
    {
        player = MatchManager.instance.GetPlayer();
        GameManager.instance.spawnPrefabs.Add(projectilePrefab);
    }

    public void Execute(Vector3 skillPosition)
    {
        SpawnProjectile(skillPosition.normalized);
    }

    [Command(requiresAuthority = false)]
    void SpawnProjectile(Vector3 spawnDir) 
    {
        Vector3 pos = player.transform.position;
        Quaternion qt = Quaternion.FromToRotation(new Vector3(1, 0, 0), spawnDir);
        GameObject projectile = GameObject.Instantiate(projectilePrefab, pos, qt);
        NetworkServer.Spawn(projectile);
        projectile.GetComponent<VinePullProjectile>().OnSpawn(spawnDir);
    }
}
