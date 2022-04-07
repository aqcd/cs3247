using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBGM : MonoBehaviour {

    private AudioSource audioSource;
    public AudioClip WinGameClip;
    public AudioClip LoseGameClip;
    public AudioClip DrawGameClip;
    
    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
    
    public void StopAudio() {
        audioSource.Stop();
    }

    public void PlayWinAudio() {
        audioSource.PlayOneShot(WinGameClip, 0.8f);
    }

    public void PlayLossAudio() {
        audioSource.PlayOneShot(LoseGameClip, 0.8f);
    }

    public void PlayDrawAudio() {
        audioSource.PlayOneShot(DrawGameClip, 0.8f);
    }
}
