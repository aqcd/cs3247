using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour {
    public static GameManager instance;
    Loadout loadout;
    private NetworkManager networkManager;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }        
        networkManager = transform.GetComponent<NetworkManager>();
        DontDestroyOnLoad(gameObject.transform);
        // Register handler for when server asks client to start a game
        NetworkClient.RegisterHandler<StartGameNetworkMessage>(StartGame);

        // Load EditLoadoutPage additively to prevent 
    }

    public void HostGame() {
        // Only attempt to host game when no connection is established yet
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkManager.networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                networkManager.StartHost(); // Call superclass StartHost function
                NetworkClient.Ready();
                NetworkClient.AddPlayer();
            }
        }
    }

    public void JoinGame() {
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkManager.networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                networkManager.StartClient(); // Call superclass StartClient function
                NetworkClient.Ready();
                NetworkClient.AddPlayer();
            }
        }
    }

    IEnumerator LoadMultiplayerMapSceneCoroutine() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MapWithPlayer");

        while (!asyncLoad.isDone) {
            Debug.Log("Progress: " + asyncLoad.progress);
            yield return null;
        }
    }

    // To call at the start of each round. Called on a client individually
    void StartGame(StartGameNetworkMessage msg) {
        if (msg.started) {
            StartCoroutine(LoadMultiplayerMapSceneCoroutine());
            loadout = LoadoutManager.instance.GetLoadout();
            Debug.Log("Skills for this player:");
            foreach (var skill in loadout.skills) {
                Debug.Log(skill.GetDescription());
            }
        }
    }
}
