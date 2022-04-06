using UnityEngine;
using UnityEngine.UI;
using System;

public class MinimapPlayerColour : MonoBehaviour
{
    [SerializeField]
    public Material playerMaterial;

    [SerializeField]
    public Material opponentMaterial;

    private bool set = false;

    void Start() {

    }

    void Update() {
        // if (set) { return; }
        // set = SetMaterials();
    }

    public void SetMaterials(int playerNum) {
        Renderer r = gameObject.GetComponent<Renderer>();

        if (playerNum == 1) {
            r.material = playerMaterial;
        } else {
            r.material = opponentMaterial;
        }
    }
}
