using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MatchManager : NetworkBehaviour {
    public static MatchManager instance;
    private int playerNum;
    private int opponentNum;
    private GameObject playerRef;
    private GameObject opponentRef;
    private Vector3 localPlayerSpawnPos;
    
    [SyncVar]
    private int player1Score = 0;
    [SyncVar]
    private int player2Score = 0;

    private Text player1ScoreText;
    private Text player2ScoreText;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        player1ScoreText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        player2ScoreText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    void Update() {
        // Need to keep updated with latest value of score since there is some delay with syncing the SyncVars
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();
    }

    [Command(requiresAuthority=false)]
    private void CommandAddScore(int playerNum) {
        Debug.Log("Server updating score");
        if (playerNum == 1) {
            player1Score++;
        } else if (playerNum == 2) {
            player2Score++;
        } else {
            player1Score = 0;
            player2Score = 0;
        }
    }

    // Runs on server
    [Command(requiresAuthority=false)]
    public void NewRound(int winningPlayer) {
        CommandAddScore(winningPlayer);
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

    public int GetPlayerNum() {
        return playerNum;
    }

    public int GetOpponentNum() {
        return opponentNum;
    }

    // Runs independently on each client to spawn the map and place the player in the approriate location
    [TargetRpc]
    public void InitMatch(NetworkConnection target, Vector3 position, int mapSeed, int localPlayerNum, int opponentPlayerNum) {
        playerNum = localPlayerNum;
        opponentNum = opponentPlayerNum;
        localPlayerSpawnPos = position;
        Debug.Log("Player num: " + playerNum + " | Opponent num: " + opponentNum);

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
