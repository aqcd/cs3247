using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CustomNetworkManager : NetworkManager {

    public override void OnStartHost() {
        base.OnStartHost();
        Debug.Log("Host started!");
    }

    public override void OnServerConnect(NetworkConnectionToClient conn) {
        base.OnServerConnect(conn);
        Debug.Log("Server got client connection: " + conn.address);
    }

    public override void OnStartClient() {
        base.OnStartClient();
        Debug.Log("Client started!");
    }

    public override void OnClientConnect() {
        base.OnClientConnect();
        Debug.Log("This client connected to server");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
