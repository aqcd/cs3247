using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    public static GameManager instance;
    public Loadout loadout;
    private NetworkManager networkManager;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }        
        DontDestroyOnLoad(gameObject.transform);
        // Register handler for when server asks client to start a game
        NetworkClient.RegisterHandler<LoadGameNetworkMessage>(LoadGame);
    }

    private void Start() {
        networkManager = CustomNetworkManager.instance;
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
            // Store player loadout here to later so MatchManager can later request for it
            loadout = LoadoutManager.instance.GetLoadout();
            // Load multiplayer scene and then continue loading other objects
            StartCoroutine(LoadMultiplayerMapSceneCoroutine());
        }
    }

    IEnumerator LoadMultiplayerMapSceneCoroutine() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapWithPlayer");
        while (!asyncLoad.isDone) {
            yield return null;
        }
        NetworkClient.AddPlayer(); // Once loading is complete, only then do we spawn the player
    }
}
