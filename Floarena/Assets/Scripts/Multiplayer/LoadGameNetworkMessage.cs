using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct LoadGameNetworkMessage : NetworkMessage {
    public bool started;
}
