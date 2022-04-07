using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip WinGameClip;
    public AudioClip LoseGameClip;
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    
    public void StopAudio() {
        audioSource.Stop();
    }

    public void PlayWinAudio() {
        audioSource.PlayOneShot(WinGameClip, 1.0f);
    }

    public void PlayLossAudio() {
        audioSource.PlayOneShot(LoseGameClip, 1.0f);
    }
}
