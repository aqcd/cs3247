using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AudioManager : NetworkBehaviour {

    public AudioSource audioSource;
    public AudioClip[] clips;

    void Start() {
        audioSource = GetComponent<AudioSource>(); 
    }

    public void PlaySound(int id, Vector3 position) {
        if (id >= 0 && id < clips.Length) {
            CmdSendServerSoundID(id, position);
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