using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bullet : NetworkBehaviour {

    public float initForce = 20f;
    public float existTime = 5f;
    public float damage = 5f;

    private Rigidbody rb;

    void Start() {
        StartCoroutine(DeathRoutine());
    }

    [ClientRpc]
    public void Spawn(Vector3 dir) {
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(initForce * dir, ForceMode.Impulse);
    }

    private IEnumerator DeathRoutine() {
        yield return new WaitForSeconds(existTime);
        GameObject.Destroy(gameObject);
    }
}
