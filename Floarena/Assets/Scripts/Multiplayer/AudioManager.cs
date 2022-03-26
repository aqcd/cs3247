using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AudioManager : NetworkBehaviour {

    public AudioSource audioSource;
    public AudioClip[] clips;

    /*public AudioClip audioClip;
    
    public AudioClip increaseHealthAudio;
    public AudioClip decreaseHealthAudio;
    public AudioClip deathAudio;
    public AudioClip attackedAudio;
    public AudioClip pickupBerryAudio;
    public AudioClip rushAudio;
    public AudioClip vinePullAudio;*/

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
        //audioSource.PlayOneShot(clips[id]);
        AudioSource.PlayClipAtPoint(clips[id], position, 0.2f);
    }
    /*
    [Command(requiresAuthority = false)]
    void CmdPlayAudio(AudioClip audioClip, Vector3 position) {
        RpcPlayAudio(audioClip, position);
    }

    [ClientRpc]
    public void RpcPlayAudio(AudioClip audioClip, Vector3 position) {
        AudioSource.PlayClipAtPoint(audioClip, position, 0.2f);
    }*/
}
