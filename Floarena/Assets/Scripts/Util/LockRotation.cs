using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    Quaternion initialRotation;
    void Start() {
    initialRotation = transform.rotation;
    }
    void Update() {
    transform.rotation = initialRotation;
    }
}
