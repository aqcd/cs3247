using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMainMenuBGM : MonoBehaviour {

    private AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
    }
}
