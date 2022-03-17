using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager {
    private int joinConfirmations = 0;

    public override void Awake() {
        joinConfirmations = 0;
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
            LoadGameNetworkMessage msg = new LoadGameNetworkMessage();
            msg.started = true;
            msg.mapSeed = Random.Range(int.MinValue, int.MaxValue);
            NetworkServer.SendToAll<LoadGameNetworkMessage>(msg);
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

    public override void OnServerAddPlayer(NetworkConnectionToClient conn) {
        base.OnServerAddPlayer(conn);
        joinConfirmations++;
        
        if (joinConfirmations == 2) {
            Debug.Log("Both players are ready, instructing clients to perform further setup");
            ReadyGameNetworkMessage msg = new ReadyGameNetworkMessage();
            msg.ready = true;
            NetworkServer.SendToAll<ReadyGameNetworkMessage>(msg);
        }
    }
}
