using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class MatchManager : NetworkBehaviour {
    public static MatchManager instance;
    
    // Player references and numbers
    private int playerNum;
    private int opponentNum;
    private GameObject playerRef;
    private GameObject opponentRef;
    private Vector3 localPlayerSpawnPos;
    
    // Player score variables
    [SyncVar(hook = nameof(UpdateScoreboardPlayer1))]
    private int player1Score = 0;
    [SyncVar(hook = nameof(UpdateScoreboardPlayer2))]
    private int player2Score = 0;
    private Text player1ScoreText;
    private Text player2ScoreText;

    // Countdown variables
    [SyncVar(hook = nameof(UpdateCountdown))]
    private int countdownVal = 0;
    private Text countdownText;
    private GameObject countdownOverlay;

    void Awake() {
        if (instance == null) {
            instance = this;
        }

        player1ScoreText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        player2ScoreText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        player1ScoreText.text = player1Score.ToString();
        player2ScoreText.text = player2Score.ToString();

        countdownText = transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Text>();
        countdownOverlay = transform.GetChild(0).GetChild(3).gameObject;
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

    private void UpdateScoreboardPlayer1(int oldScore, int newScore) {
        player1ScoreText.text = newScore.ToString();
    }

    private void UpdateScoreboardPlayer2(int oldScore, int newScore) {
        player2ScoreText.text = newScore.ToString();
    }

    private void UpdateCountdown(int oldCount, int newCount) {
        if (newCount == 0) {
            countdownText.text = "Start!";
        } else {
            countdownText.text = newCount.ToString();
        }
    }

    [Command(requiresAuthority=false)]
    private void StartCountdown() {
        StartCoroutine(CountdownCoroutine(5));
    }

    IEnumerator CountdownCoroutine(int startVal) {
        countdownVal = startVal;
        while (countdownVal > 0) {
            yield return new WaitForSeconds(1f);
            countdownVal--;
        }

        // Once countdown has ended, tell clients to fade out the countdown
        // text and panel
        FadeoutCountdown();
    }

    [ClientRpc]
    private void FadeoutCountdown() {
        Debug.Log("Starting fade out");
        StartCoroutine(FadeoutCountdownCoroutine());
    }

    // Takes 1 second to fade out
    IEnumerator FadeoutCountdownCoroutine() {
        var colorRef = countdownOverlay.GetComponent<Image>().color;

        while (colorRef.a > 0) {
            colorRef.a -= 0.01f;
            SetCountdownOpacity(colorRef.a);
            yield return new WaitForSeconds(0.01f);
        }

        countdownOverlay.SetActive(false);
        Debug.Log("Faded out!");
    }

    private void SetCountdownOpacity(float opacity) {
        var panelColorRef = countdownOverlay.GetComponent<Image>().color;
        var textColorRef = countdownOverlay.transform.GetChild(0).GetComponent<Text>().color;
        panelColorRef.a = opacity;
        textColorRef.a = opacity;
        countdownOverlay.GetComponent<Image>().color = panelColorRef;
        countdownOverlay.transform.GetChild(0).GetComponent<Text>().color = textColorRef;
    }

    [ClientRpc]
    private void ResetCountdownOpacity() {
        countdownOverlay.SetActive(true);
        SetCountdownOpacity(1f);
    }

    // Runs on server to instruct all clients to restart the round
    // Also updates score based on winning player
    [Command(requiresAuthority=false)]
    public void NewRound(int winningPlayer) {
        CommandAddScore(winningPlayer);
        NewRound();
    }   

    // Runs on server to instruct all clients to restart the round
    [Command(requiresAuthority=false)]
    public void NewRound() {
        ResetCountdownOpacity();
        ResetPlayerPosition();
        StartCountdown();
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

        // Load skill prefabs
        SkillManager.instance.LoadSkills(GameManager.instance.loadout.skills);
        // Generate map based on random seed         
        MapGenerator.instance.GenerateMap(mapSeed); 
        Debug.Log("Game ready to start!");
    }

    [ClientRpc]
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
