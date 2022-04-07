using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InvulSphere : NetworkBehaviour
{

    // private float duration = SkillConstants.INVUL_DURATION;
    // private GameObject spawningPlayer;
    // private Rigidbody rb;

    // private float remainingDuration = 0.0f;
    // private IEnumerator deathCoroutine;
    // private bool isSpawningClient;

    // void Awake()
    // {
    //     deathCoroutine = DeathRoutine();
    //     StartCoroutine(deathCoroutine);
    // }

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (remainingDuration > 0.0f)
    //     {
    //         remainingDuration -= Time.deltaTime;
    //     }

    //     if (remainingDuration <= 0.0f)
    //     {
    //         GameObject.Destroy(gameObject);
    //     }
    // }

    // [ClientRpc]
    // public void OnSpawn(Vector3 dir, int spawnPlayerNum) 
    // {
    //     if (MatchManager.instance.GetPlayerNum() == spawnPlayerNum) {
    //         spawningPlayer = MatchManager.instance.GetPlayer();
    //         isSpawningClient = true;
    //     } else {
    //         spawningPlayer = MatchManager.instance.GetOpponent();
    //         isSpawningClient = false;
    //     }
    // }

    // private IEnumerator DeathRoutine() {
    //     yield return new WaitForSeconds(duration);
    //     GameObject.Destroy(gameObject);
    // }

}
