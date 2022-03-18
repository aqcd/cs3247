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

    // Runs on server
    [Command(requiresAuthority=false)]
    public void NewRound() {
        Debug.Log("Server asking clients to restart round");
        RpcNewRound();
    }   

    // Runs on client to restart the round
    [ClientRpc]
    private void RpcNewRound() {
        ResetPlayerPosition();
    }

    public GameObject GetPlayer() {
        return playerRef;
    }

    public GameObject GetOpponent() {
        return opponentRef;
    }

    // Runs independently on each client to spawn the map and place the player in the approriate location
    [TargetRpc]
    public void InitMatch(NetworkConnection target, Vector3 position, int mapSeed) {
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

        Debug.Log("Player name: " + playerRef.name);
        Debug.Log("Opponent name: " + opponentRef.name);

        if (playerRef == null || opponentRef == null) {
            throw new System.Exception("Player or opponent gameobject not found during match initialization");
        }

        // Spawn player in the appropriate position
        ResetPlayerPosition();
        // Load skill prefabs
        SkillManager.instance.LoadSkills(GameManager.instance.loadout.skills);
        // Generate map based on random seed         
        MapGenerator.instance.GenerateMap(mapSeed); 
        Debug.Log("Game ready to start!");
    }

    private void ResetPlayerPosition() {
        // Need to disable character controller before teleporting player
        // see https://forum.unity.com/threads/unity-multiplayer-through-mirror-teleporting-player-inconsistent.867079/
        playerRef.GetComponent<CharacterController>().enabled = false;

        // Set own player's position
        playerRef.transform.position = localPlayerSpawnPos;
        Debug.Log("New round started!");

        playerRef.GetComponent<CharacterController>().enabled = true;
    }
}
