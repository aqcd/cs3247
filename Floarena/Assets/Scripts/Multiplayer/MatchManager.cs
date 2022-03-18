using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchManager : NetworkBehaviour {
    public static MatchManager instance;
    private GameObject playerRef;
    private GameObject opponentRef;
    private Vector3 localPlayerSpawnPos;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    void Start() {
        
    }

    void Update() {
        
    }

    [Command]
    public void NewRound() {

    }

    public GameObject GetPlayer() {
        return playerRef;
    }

    public GameObject GetOpponent() {
        return opponentRef;
    }

    [TargetRpc]
    public void SetLocalPlayerSpawnPosition(NetworkConnection target, Vector3 position) {
        Debug.Log("This player's spawn position has been set!");
        localPlayerSpawnPos = position;

        // Get player and opponent references now that we know both players have 
        // successfully joined and are ready to start the game
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerObjs.Length; i++) {
            GameObject curPlayer = playerObjs[i];
            if (curPlayer.GetComponent<MultiplayerThirdPersonController>().isLocalPlayer) {
                playerRef = curPlayer;
            } else {
                opponentRef = curPlayer;
            }
        }

        if (playerRef == null || opponentRef == null) {
            throw new System.Exception("Player or opponent gameobject not found during match initialization");
        }

        // Set own player's position
        playerRef.transform.position = localPlayerSpawnPos;
        Debug.Log("Player spawned and ready!");
    }
}
