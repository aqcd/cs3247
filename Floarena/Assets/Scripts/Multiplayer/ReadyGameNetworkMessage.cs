using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public struct ReadyGameNetworkMessage : NetworkMessage {
    public bool ready;
}
