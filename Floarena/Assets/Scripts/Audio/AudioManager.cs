using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AudioManager : NetworkBehaviour {
    public static AudioManager instance;
    public AudioClip[] clips;


    void Awake() {
        if (instance == null) {
            instance = this;
        }
    }

    public void PlaySound(int id, Vector3 position) {
        if (id >= 0 && id < clips.Length) {
            if (isServer) {
                RpcSendSoundIdToClients(id, position);
            } else {
                CmdSendServerSoundID(id, position);
            }
        }
    }

    [Command(requiresAuthority = false)]
    void CmdSendServerSoundID(int id, Vector3 position) {
        RpcSendSoundIdToClients(id, position);
    }

    [ClientRpc]
    void RpcSendSoundIdToClients(int id, Vector3 position) {
        if (id == AudioIndex.CHANNEL_AUDIO) {
            AudioSource.PlayClipAtPoint(clips[id], position, 0.2f);
        } else {
            AudioSource.PlayClipAtPoint(clips[id], position, 1.0f);
        }
    }
}