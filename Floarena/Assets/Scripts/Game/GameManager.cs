using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    Loadout loadout;
    private NetworkManager networkManager;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        networkManager = transform.GetComponent<NetworkManager>();
        DontDestroyOnLoad(gameObject.transform);
    }

    void Update() {
        // If client is now ready
        if (NetworkClient.isConnected && !NetworkClient.ready) {
            // Debug.Log("Client is ready");
            NetworkClient.Ready();
            if (NetworkClient.localPlayer == null) {
                NetworkClient.AddPlayer();
            }
        }

        // if (NetworkServer.active && NetworkClient.active) {
        //     Debug.Log("Host is running");
        // } else if (NetworkClient.isConnected) {
        //     Debug.Log("Client connected!");
        // }
    }

    public void HostGame() {
        // Only attempt to host game when no connection is established yet
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkManager.networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                networkManager.StartHost(); // Call superclass StartHost function
            }
        }
    }

    public void JoinGame() {
        if (!NetworkClient.isConnected && !NetworkServer.active) {
            networkManager.networkAddress = "127.0.0.1";
            
            if (!NetworkClient.active) {
                networkManager.StartClient(); // Call superclass StartClient function
            }
        }
    }


    // To call at the start of each round.
    void StartGame() {
        this.loadout = LoadoutManager.instance.GetLoadout();
    }
}
