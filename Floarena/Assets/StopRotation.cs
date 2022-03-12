using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRotation : MonoBehaviour
{
    Quaternion rotation;
    void Awake()
    {
        //transform.rotation = GameObject.FindWithTag("PlayerCamera").transform.rotation;
        rotation = transform.rotation;
    }

    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
