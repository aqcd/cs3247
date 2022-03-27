using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BrushAudio : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip _audio;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !audioSource.isPlaying) {
            audioSource.PlayOneShot(_audio, 0.5f);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && !audioSource.isPlaying) {
            audioSource.PlayOneShot(_audio, 0.5f);
        }
    }
}
