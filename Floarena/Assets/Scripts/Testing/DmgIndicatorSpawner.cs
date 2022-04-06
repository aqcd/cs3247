using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgIndicatorSpawner : MonoBehaviour {
    
    public GameObject spawnObj;

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject obj = Instantiate(spawnObj, transform.position, transform.rotation);
            // obj.GetComponent<ShrinkingIndicator>().StartAnim();
        }
    }
}
