using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager {

    public override void Awake() {
        base.Awake();
    }

    public override void OnStartHost() {
        base.OnStartHost();
        Debug.Log("Host started!");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn) {
        base.OnServerConnect(conn);
        Debug.Log("Server got client connection: " + conn.address + ". Total connections: " + NetworkServer.connections.Count);

        if (NetworkServer.connections.Count == 2) {
            Debug.Log("All players connected, loading new scene");
            StartGameNetworkMessage msg = new StartGameNetworkMessage();
            msg.started = true;
            NetworkServer.SendToAll(msg);
        }
    }

    // Ran on a client when it first boots up
    public override void OnStartClient() {
        base.OnStartClient();
        Debug.Log("Client started!");
    }

    // Ran on client when it established connection to a server
    public override void OnClientConnect() {
        base.OnClientConnect();
        Debug.Log("This client connected to server. Connections: " + NetworkServer.connections.Count);
    }
}
