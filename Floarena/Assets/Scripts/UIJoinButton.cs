using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJoinButton : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        transform.GetComponent<Button>().onClick.AddListener(GameManager.instance.JoinGame);
    }
}
