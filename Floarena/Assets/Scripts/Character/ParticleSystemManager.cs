using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleSystemManager : MonoBehaviour {
    [SerializeField]
    public ParticleSystem ADUP, ASUP, MSUP, HEAL, DMG;

    private void Awake() {
        ADUP.Stop();
        ASUP.Stop();
        MSUP.Stop();
        HEAL.Stop();
        DMG.Stop();
    }

    void Update() {
        
    }

    public void PlayADUP() {
        ADUP.Play();
    }

    public void StopADUP() {
        ADUP.Stop();
    }

    public void PlayASUP() {
        ASUP.Play();
    }

    public void StopASUP() {
        ASUP.Stop();
    }

    public void PlayMSUP() {
        MSUP.Play();
    }

    public void StopMSUP() {
        MSUP.Stop();
    }

    public void PlayDMG() {
        DMG.Play();
    }

    public void PlayHeal() {
        HEAL.Play();
    }
}
