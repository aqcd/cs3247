using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ParticleSystemManager : NetworkBehaviour {
    [SerializeField]
    public ParticleSystem ADUP, ASUP, MSUP, HEAL, DMG;

    private void Awake() {
        ADUP.Stop();
        ASUP.Stop();
        MSUP.Stop();
        HEAL.Stop();
        DMG.Stop();
    }

    [Command(requiresAuthority = false)]
    public void PlayADUP() {
        RpcPlayADUP();
    }
    [ClientRpc]
    private void RpcPlayADUP() {
        ADUP.Play();
    }



    [Command(requiresAuthority = false)]
    public void StopADUP() {
        RpcStopADUP();
    }
    [ClientRpc]
    private void RpcStopADUP() {
        ADUP.Stop();
    }



    [Command(requiresAuthority = false)]
    public void PlayASUP() {
        RpcPlayASUP();
    }
    [ClientRpc]
    private void RpcPlayASUP() {
        ASUP.Play();
    }



    [Command(requiresAuthority = false)]
    public void StopASUP() {
        RpcStopASUP();
    }
    [ClientRpc]
    private void RpcStopASUP() {
        ASUP.Stop();
    }



    [Command(requiresAuthority = false)]
    public void PlayMSUP() {
        RpcPlayMSUP();
    }
    [ClientRpc]
    private void RpcPlayMSUP() {
        MSUP.Play();
    }



    [Command(requiresAuthority = false)]
    public void StopMSUP() {
        RpcStopMSUP();
    }
    [ClientRpc]
    private void RpcStopMSUP() {
        MSUP.Stop();
    }



    [Command(requiresAuthority = false)]
    public void PlayDMG() {
        RpcPlayDMG();
    }
    [ClientRpc]
    private void RpcPlayDMG() {
        DMG.Play();
    }

    // Runs on server
    public void PlayHeal() {
        RpcPlayHeal();
    }
    [ClientRpc]
    private void RpcPlayHeal() {
        HEAL.Play();
    }
}
