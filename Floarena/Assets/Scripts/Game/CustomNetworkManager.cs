using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager {

    private GameManager gameManager;

    public override void Awake() {
        base.Awake();
        gameManager = transform.GetComponent<GameManager>();
    }

    // public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
    //     NetworkServer.AddPlayerForConnection(conn, null);
    // }

    public override void OnStartHost() {
        base.OnStartHost();
        Debug.Log("Host started!");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn) {
        base.OnServerConnect(conn);
        Debug.Log("Server got client connection: " + conn.address + ". Total connections: " + NetworkServer.connections.Count);

        if (NetworkServer.connections.Count == 2) {
            Debug.Log("All players connected, loading new scene");
            gameManager.LoadMultiplayerMapScene();
        }
    }

    public override void OnStartClient() {
        base.OnStartClient();
        Debug.Log("Client started!");
    }

    public override void OnClientConnect() {
        base.OnClientConnect();
        Debug.Log("This client connected to server");
    }

    // Handle changing of scene on a client
    public override void OnClientSceneChanged() {
        base.OnClientSceneChanged();
        Debug.Log("I have finished changing scene!");
    }
}
