using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHostButton : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        transform.GetComponent<Button>().onClick.AddListener(GameManager.instance.HostGame);
    }
}
