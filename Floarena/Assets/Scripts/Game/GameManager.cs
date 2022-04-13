using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkManager {
    public static GameManager instance;
    
    private int joinConfirmations = 0;
    public NetworkConnectionToClient player1Conn;
    public NetworkConnectionToClient player2Conn;
    private Vector3 player1SpawnPos = new Vector3(5, 0, 5);
    private Vector3 player2SpawnPos = new Vector3(55, 0, 55); // (8, 0, 8)

    public Loadout loadout;

    [Header("Initialization Prefabs")]
    public GameObject MatchManagerPrefab;
    public GameObject SkillManagerPrefab;
    public GameObject AudioManagerPrefab;

    public override void Awake() {
        Debug.Log("Server awake");
        if (instance == null) {
            instance = this;
        }
        base.Awake();
        joinConfirmations = 0;
        spawnPrefabs.Add(MatchManagerPrefab);
        spawnPrefabs.Add(SkillManagerPrefab);
        spawnPrefabs.Add(AudioManagerPrefab);
    }

    public void HostGame() {
        // Only attempt to host game when no connection is established yet
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                StartHost(); // Call superclass StartHost function
            }
        }
    }

    public void JoinGame() {
        Debug.Log("Clicked");
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkAddress = "13.215.67.49";
            // networkAddress = "localhost";
            Debug.Log("Not connected");

            if (!NetworkClient.active) {
                UISwitchHandler.instance.DisplayWaitScreen();
                StartClient(); // Call superclass StartClient function
            }
        }
    }

    public override void OnStartServer() {
        Debug.Log("Started server");
        joinConfirmations = 0;
        base.OnStartServer();
    }

    public override void OnStartHost() {
        base.OnStartHost();
        Debug.Log("Host started!");
    }

    // Runs on server when a server receives a connection from the client
    public override void OnServerConnect(NetworkConnectionToClient conn) {
        base.OnServerConnect(conn);
        Debug.Log("Server got client connection: " + conn.address + ". Total connections: " + NetworkServer.connections.Count);

        if (NetworkServer.connections.Count == 2) {
            ServerChangeScene("MapWithPlayer");
            Debug.Log("All players connected, loading new scene");
        } else if (NetworkServer.connections.Count > 2) {
            NetworkServer.RemoveConnection(conn.connectionId);
            Debug.Log("Removing extra connection");
        }
    }

    // Ran on a client when it requests to connect to a server
    // By this point, user has already selected loadout, so store this one
    public override void OnStartClient() {
        base.OnStartClient();
        loadout = LoadoutManager.instance.GetLoadout();
        Debug.Log("Client started!");
    }

    // Ran on client when it established connection to a server
    public override void OnClientConnect() {
        base.OnClientConnect();
        Debug.Log("Client connected to server");
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);
        if (newSceneName == "MapWithPlayer") {
            Debug.Log("Client asked to load scene...");
        }
    }

    public override void OnClientSceneChanged() {
        if (SceneManager.GetActiveScene().name == "MapWithPlayer") {
            NetworkClient.Ready();
            NetworkClient.AddPlayer();
        }
    }

    public override void OnServerSceneChanged(string sceneName) {
        base.OnServerSceneChanged(sceneName);

    }

    // Runs on a server when a client requests to be added as a player
    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        base.OnServerAddPlayer(conn);
        joinConfirmations++;
        
        if (joinConfirmations == 1) {
            player1Conn = conn;
            Debug.Log("Player 1 Added");
        } else if (joinConfirmations == 2) {
            player2Conn = conn;
            Debug.Log("Player 2 Added");

            Debug.Log("Both players are ready, instructing clients to perform further setup");
            // Spawn MatchManagers on both clients
            GameObject matchManager = Instantiate(MatchManagerPrefab);
            NetworkServer.Spawn(matchManager);

            // Spawn AudioManager on both clients
            GameObject audioManager = Instantiate(AudioManagerPrefab);
            NetworkServer.Spawn(audioManager);

            // Send initial spawn positions and map seed to MatchManager via a TargetRpc
            int mapSeed = Random.Range(int.MinValue, int.MaxValue);
            MatchManager.instance.InitPlayer(player1Conn, player2SpawnPos, mapSeed, 2, 1);
            MatchManager.instance.InitPlayer(player2Conn, player1SpawnPos, mapSeed, 1, 2);

            // Spawn SkillManagers on both clients
            GameObject skillManager = Instantiate(SkillManagerPrefab);
            NetworkServer.Spawn(skillManager);

            // Trigger respective spell loading for each client
            SkillManager.instance.LoadSkills();

            // Spawn both players
            MatchManager.instance.RpcRespawnPlayer(player1Conn);
            MatchManager.instance.RpcRespawnPlayer(player2Conn);

            MatchManager.instance.StartMatch();
        }
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn) {
        base.OnServerDisconnect(conn);
        
        if (conn == player1Conn) {
            Debug.Log("Player 1 disconnected");
        } else if (conn == player2Conn) {
            Debug.Log("Player 2 disconnected");
        } else {
            Debug.Log("Unknown client disconnected!");
        }
        Debug.Log("Disconnecting all players");

        if (NetworkServer.connections.Count < 2) {
            ServerChangeScene("MainPage");
            joinConfirmations = 0;
            StopServer();
            networkSceneName = "MainPage";
            StartServer();
        }
    }

    public override void OnClientDisconnect() {
        base.OnClientDisconnect();
        Debug.Log("Client lost connection to server!");
        StopClient();
        SceneManager.LoadScene("MainPage");
    }
}
