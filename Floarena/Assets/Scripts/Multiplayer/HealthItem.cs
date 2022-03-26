using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HealthItem : NetworkBehaviour {
   // public AudioClip _audio; 

    private void OnTriggerEnter() {
        //AudioSource.PlayClipAtPoint(_audio, this.transform.position);
        Destroy(this.gameObject); // Destroy HealthConsumable
    }
}
