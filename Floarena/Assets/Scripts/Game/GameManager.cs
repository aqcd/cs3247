using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    public static GameManager instance;
    public Loadout loadout;
    private NetworkManager networkManager;
    private GameObject playerRef;
    private GameObject opponentRef;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }        
        networkManager = transform.GetComponent<NetworkManager>();
        DontDestroyOnLoad(gameObject.transform);
        // Register handler for when server asks client to start a game
        NetworkClient.RegisterHandler<LoadGameNetworkMessage>(LoadGame);
        NetworkClient.RegisterHandler<ReadyGameNetworkMessage>(ReadyGame);
    }

    public void HostGame() {
        // Only attempt to host game when no connection is established yet
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkManager.networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                networkManager.StartHost(); // Call superclass StartHost function
                NetworkClient.Ready();
            }
        }
    }

    public void JoinGame() {
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkManager.networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                networkManager.StartClient(); // Call superclass StartClient function
                NetworkClient.Ready();
            }
        }
    }

    // To call at the start of a game. Called on a client individually
    void LoadGame(LoadGameNetworkMessage msg) {
        if (msg.started) {
            loadout = LoadoutManager.instance.GetLoadout();
            // Load multiplayer scene and then continue loading other objects
            StartCoroutine(LoadMultiplayerMapSceneCoroutine(msg));
        }
    }

    IEnumerator LoadMultiplayerMapSceneCoroutine(LoadGameNetworkMessage msg) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapWithPlayer");
        while (!asyncLoad.isDone) {
            yield return null;
        }
        NetworkClient.AddPlayer(); // Once loading is complete, only then do we spawn the player
        SkillManager.instance.LoadSkills(loadout.skills); // Load skill prefabs
        MapGenerator.instance.GenerateMap(msg.mapSeed); // Generate map based on random seed         
    }

    void ReadyGame(ReadyGameNetworkMessage msg) {
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
    }

    public GameObject GetPlayer() {
        return playerRef;
    }

    public GameObject GetOpponent() {
        return opponentRef;
    }
}
