using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnCollision : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip _audio;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player" && !audioSource.isPlaying) {
            audioSource.PlayOneShot(_audio, 0.2f);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player" && !audioSource.isPlaying) {
            audioSource.PlayOneShot(_audio, 0.2f);
        }
    }
}
