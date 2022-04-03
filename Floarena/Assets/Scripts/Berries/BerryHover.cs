using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryHover : MonoBehaviour {
    
    public float heightDelta = 0.7f;
    public float heightSpeed = 2f;
    public float angleSpeed = 1f;
    private Vector3 startPos;
    private Vector3 startRot;

    void Start() {
        startPos = transform.position;
        startRot = transform.rotation.eulerAngles;
        StartCoroutine(HoverCoroutine());
    }

    IEnumerator HoverCoroutine() {
        while (true) {
            float upAmount = (Mathf.Sin(Time.time * heightSpeed) + 1) * heightDelta;
            transform.position = new Vector3(startPos.x, startPos.y + upAmount, startPos.z);
            transform.localRotation = Quaternion.Euler(startRot.x, transform.localEulerAngles.y + angleSpeed, startRot.z);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
