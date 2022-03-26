using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : MonoBehaviour {
    private Vector3 position;
    public AudioClip _audio; 

    public Vector3 Position { get => position; set => position = value; }

    public Berry(Vector3 position) {
        this.Position = position;
    }

    private void OnTriggerEnter() {
        AudioSource.PlayClipAtPoint(_audio, this.transform.position);
        Destroy(this.gameObject);
    }
}
