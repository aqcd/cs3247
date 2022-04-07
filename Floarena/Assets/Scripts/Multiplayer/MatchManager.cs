using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using TMPro;

public class MatchManager : NetworkBehaviour {
    public static MatchManager instance;
    PlayBGM backgroundMusic;

    // Player references and numbers
    private int playerNum;
    private int opponentNum;
    private GameObject playerRef;
    private GameObject opponentRef;
    private Vector3 localPlayerSpawnPos;


    
    // Player score variables and references
    [SyncVar(hook = nameof(UpdateScoreboardPlayer1))]
    private int player1Score = 0;
    [SyncVar(hook = nameof(UpdateScoreboardPlayer2))]
    private int player2Score = 0;
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;
    public GameObject player1ScoreAdd;
    public GameObject player2ScoreAdd;


    
    // Countdown variables
    private int countdownVal = 0;
    public Text countdownText;
    public GameObject countdownOverlay;



    // Overall match state variables and references
    public int maxScore = 20; //50;
    public GameObject winScreen;
    public GameObject lossScreen;
    public TMP_Text timerText;
    public int matchTotalTime = 30; //123;
    [SyncVar(hook = nameof(UpdateMatchTime))]
    private int matchTime;
    private bool matchEnded = false;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        backgroundMusic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PlayBGM>();
    }

    [Command(requiresAuthority = false)]
    public void StartMatch() {
        StartCoroutine(MatchTimerCoroutine(matchTotalTime));
    }

    // Runs on the server
    IEnumerator MatchTimerCoroutine(int startTime) {
        matchTime = startTime;
        
        while (matchTime >= 0) {
            yield return new WaitForSeconds(1f);
            matchTime--;
        }

        // Game will end once coroutine reaches here
        if (!matchEnded) {
            matchEnded = true;
            if (player1Score > player2Score) {            
                ShowEndScreen(GameManager.instance.player1Conn, true);
                ShowEndScreen(GameManager.instance.player2Conn, false);
            } else if (player1Score < player2Score) {
                ShowEndScreen(GameManager.instance.player1Conn, false);
                ShowEndScreen(GameManager.instance.player2Conn, true);
            } else {
                // Handle draws one day lol
                Debug.Log("It's a draw!");
            }
        } 
    }   

    // Hook that triggers locally whenever server updates match time
    private void UpdateMatchTime(int oldTime, int newTime) {
        if (newTime != -1) {
            timerText.text = newTime.ToString();
            if (newTime <= 30) {
                timerText.color = new Color(1, 0, 0, 1);
            }
        } else {
            timerText.text = "END";
        }
    }

    [Command(requiresAuthority = false)]
    public void CommandAddScore(int playerNum, int scoreToAdd) {
        Debug.Log("Server updating score");
        if (playerNum == 1) {
            player1Score += scoreToAdd;

            // Handle end-game if score is >= 50
            if (player1Score >= maxScore) {
                if (!matchEnded) {
                    matchEnded = true;
                    ShowEndScreen(GameManager.instance.player1Conn, true);
                    ShowEndScreen(GameManager.instance.player2Conn, false);
                }
            }

        } else if (playerNum == 2) {
            player2Score += scoreToAdd;

            // Handle end-game if score is >= 50
            if (player2Score >= maxScore) {
                if (!matchEnded) {
                    matchEnded = true;
                    ShowEndScreen(GameManager.instance.player1Conn, false);
                    ShowEndScreen(GameManager.instance.player2Conn, true);
                }
            }
            
        } else {
            player1Score = 0;
            player2Score = 0;
        }
    }

    // Hook that triggers when player 1's score changes
    private void UpdateScoreboardPlayer1(int oldScore, int newScore) {
        player1ScoreText.text = newScore.ToString();
        int scoreChange = newScore - oldScore;
        player1ScoreAdd.GetComponent<ScoreAdd>().StartAnim("+" + scoreChange.ToString());
    }

    // Hook that triggers when player 2's score changes
    private void UpdateScoreboardPlayer2(int oldScore, int newScore) {
        player2ScoreText.text = newScore.ToString();
        int scoreChange = newScore - oldScore;
        player2ScoreAdd.GetComponent<ScoreAdd>().StartAnim("+" + scoreChange.ToString());
    }

    IEnumerator CountdownCoroutine(int startVal) {
        countdownVal = startVal;
        countdownText.text = startVal.ToString();
        while (countdownVal > 0) {
            yield return new WaitForSeconds(1f);
            countdownVal--;
            countdownText.text = countdownVal.ToString();
        }
        countdownText.text = "Start!";

        // Once countdown has ended, tell clients to fade out the countdown
        // text and panel
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
    }

    private void SetCountdownOpacity(float opacity) {
        var panelColorRef = countdownOverlay.GetComponent<Image>().color;
        var textColorRef = countdownOverlay.transform.GetChild(0).GetComponent<Text>().color;
        panelColorRef.a = opacity;
        textColorRef.a = opacity;
        countdownOverlay.GetComponent<Image>().color = panelColorRef;
        countdownOverlay.transform.GetChild(0).GetComponent<Text>().color = textColorRef;
    }

    private void ResetCountdown() {
        countdownOverlay.SetActive(true);
        SetCountdownOpacity(0.7f);
        StartCoroutine(CountdownCoroutine(3));
    }

    // Runs on server
    public void HandleKill(int killerPlayer) {
        CommandAddScore(killerPlayer, 10);
        if (killerPlayer == 1) {
            RespawnPlayer(GameManager.instance.player2Conn);    
        } else if (killerPlayer == 2) {
            RespawnPlayer(GameManager.instance.player1Conn);    
        } else {
            throw new System.Exception("Unknown player killed player!");
        }
    }

    // Return reference to my player object, relative to me
    public GameObject GetPlayer() {
        return playerRef;
    }

    // Return reference to opponent's player object, relative to me
    public GameObject GetOpponent() {
        return opponentRef;
    }

    // Returns what is my player number, relative to me
    public int GetPlayerNum() {
        return playerNum;
    }

    // Returns what is the opponent's player number, relative to me
    public int GetOpponentNum() {
        return opponentNum;
    }

    // Runs independently on each client to spawn the map and place the player in the approriate location
    [TargetRpc]
    public void InitPlayer(NetworkConnection target, Vector3 position, int mapSeed, int localPlayerNum, int opponentPlayerNum) {
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

        Debug.Log("Player num: " + playerNum);
        Debug.Log("Opponent num: " + opponentNum);

        if (playerRef == null || opponentRef == null) {
            throw new System.Exception("Player or opponent gameobject not found during match initialization");
        }

        // Generate map based on random seed         
        MapGenerator.instance.GenerateMap(mapSeed); 
        
        // Set player and opponent colours locally
        playerRef.GetComponent<Health>().healthBar.SetBarColor(GetPlayerNum());
        opponentRef.GetComponent<Health>().healthBar.SetBarColor(GetOpponentNum());
        playerRef.GetComponentInChildren<MinimapPlayerColour>().SetMaterials(GetPlayerNum());
        opponentRef.GetComponentInChildren<MinimapPlayerColour>().SetMaterials(GetOpponentNum());

        // Spawn local player in their default location
        RespawnPlayer(target);

        // Set game timer to 120 seconds
        Debug.Log("Game ready to start!");
    }

    // Function runs on a single client, except a call to server which resets player position
    // on both clients
    [TargetRpc]
    public void RespawnPlayer(NetworkConnection target) {
        // Need to disable character controller before teleporting player
        // see https://forum.unity.com/threads/unity-multiplayer-through-mirror-teleporting-player-inconsistent.867079/
        playerRef.GetComponent<CharacterController>().enabled = false;
        // Reset player position on both clients. This should update transform on both clients 
        // due to the network transform component on the gameobject
        playerRef.transform.position = localPlayerSpawnPos;
        playerRef.GetComponent<CharacterController>().enabled = true;

        // Reset current player health on all clients
        playerRef.GetComponent<Health>().ResetHealth();

        ResetCountdown();
    }

    [TargetRpc]
    private void ShowEndScreen(NetworkConnection target, bool didWin) {
        GameObject endScreen;
        backgroundMusic.StopAudio();
        if (didWin) {
            winScreen.SetActive(true);
            endScreen = winScreen;
            backgroundMusic.PlayWinAudio();
        } else {
            lossScreen.SetActive(true);
            endScreen = lossScreen;
            backgroundMusic.PlayLossAudio();
        }
        
        Color color = endScreen.GetComponent<RawImage>().color;
        color.a = 0;
        endScreen.GetComponent<RawImage>().color = color;
        endScreen.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        StartCoroutine(EndScreenCoroutine(endScreen));
    }

    // Screen takes 1 second to pop out and fade in
    IEnumerator EndScreenCoroutine(GameObject endScreen) {
        Color color = endScreen.GetComponent<RawImage>().color;
        Debug.Log(color);

        while (color.a < 1) {
            color.a += 0.1f;
            endScreen.GetComponent<RawImage>().color = color;
            
            if (endScreen.transform.localScale.x < 1) {
                endScreen.transform.localScale = new Vector3(
                    endScreen.transform.localScale.x + 0.05f,
                    endScreen.transform.localScale.y + 0.05f,
                    endScreen.transform.localScale.z + 0.05f
                );
            }
    
            yield return new WaitForSeconds(0.01f);
        }

        StartCoroutine(EndScreenPulseCoroutine(endScreen));
    }

    IEnumerator EndScreenPulseCoroutine(GameObject endScreen) {        
        for (int i = 0; i < 5; i++) {
            while (endScreen.transform.localScale.x < 1.05) {
                endScreen.transform.localScale = new Vector3(
                    endScreen.transform.localScale.x + 0.0003f,
                    endScreen.transform.localScale.y + 0.0003f,
                    endScreen.transform.localScale.z + 0.0003f
                );
                yield return new WaitForSeconds(0.01f);
            }    

            while (endScreen.transform.localScale.x > 1) {
                endScreen.transform.localScale = new Vector3(
                    endScreen.transform.localScale.x - 0.0003f,
                    endScreen.transform.localScale.y - 0.0003f,
                    endScreen.transform.localScale.z - 0.0003f
                );
                yield return new WaitForSeconds(0.01f);
            }
        }

        Debug.Log("Stopping client");
        GameManager.instance.StopClient();
    }
}
