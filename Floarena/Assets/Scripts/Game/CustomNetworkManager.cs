using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager {
    private int joinConfirmations = 0;
    private NetworkConnectionToClient player1Conn;
    private NetworkConnectionToClient player2Conn;
    private Vector3 player1SpawnPos = new Vector3(5, 0, 5);
    private Vector3 player2SpawnPos = new Vector3(55, 0, 55);

    [Header("Initialization Prefabs")]
    public GameObject MultiplayerManagersPrefab;

    public override void Awake() {
        base.Awake();
        joinConfirmations = 0;
        spawnPrefabs.Add(MultiplayerManagersPrefab);
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
            // Spawn MultiplayerManagers on both clients
            GameObject multiplayerManagers = Instantiate(MultiplayerManagersPrefab);
            NetworkServer.Spawn(multiplayerManagers);

            // Send initial spawn positions to MatchManager via a TargetRpc
            MatchManager.instance.SetLocalPlayerSpawnPosition(player1Conn, player1SpawnPos);
            MatchManager.instance.SetLocalPlayerSpawnPosition(player2Conn, player2SpawnPos);
        }
    }
}
