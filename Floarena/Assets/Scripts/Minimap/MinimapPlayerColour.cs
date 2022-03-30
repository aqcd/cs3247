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
        if (set) { return; }
        set = SetMaterials();
    }

    private bool SetMaterials() {
        GameObject parent = gameObject.transform.parent.gameObject;
        GameObject playerRef = MatchManager.instance.GetPlayer();
        if (playerRef == null) { return false; }

        Debug.Log("MPC: " + parent);
        Debug.Log("MPC: " + playerRef);

        Renderer r = gameObject.GetComponent<Renderer>();

        if (parent == playerRef) {
            r.material = playerMaterial;
        } else {
            r.material = opponentMaterial;
        }

        return true;
    }
}
