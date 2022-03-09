using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class BasicController : NetworkBehaviour {
    
    public float speed = 2.0f;
    public GameObject bulletObject;
    public float bulletFreq = 0.5f;

    private bool spawnVectorReady = false;
    private Vector3 spawnDirection = new Vector3(0, 0, 0);

    [SyncVar]
    public float health = 200f;
    public Text healthText;

    void Start() {
        if (isLocalPlayer) {
            StartCoroutine(BulletLoop());
        }
    }

    void Update() {
        if (isLocalPlayer) {
            HandleMovement();
            HandleShooting();
        }
        healthText.text = health.ToString();
    }

    void HandleMovement() {
        if (Input.GetKey(KeyCode.W)) {
            gameObject.transform.Translate(new Vector3(0, 0, -1 * Time.deltaTime * speed));
        } else if (Input.GetKey(KeyCode.S)) {
            gameObject.transform.Translate(new Vector3(0, 0, 1 * Time.deltaTime * speed));
        }

        if (Input.GetKey(KeyCode.A)) {
            gameObject.transform.Translate(new Vector3(1 * Time.deltaTime * speed, 0, 0));
        } else if (Input.GetKey(KeyCode.D)) {
            gameObject.transform.Translate(new Vector3(-1 * Time.deltaTime * speed, 0, 0));
        }
    }

    void HandleShooting() {
        spawnVectorReady = false;
        
        // Key is pressed down, bullet spawn direction is affected
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            spawnDirection.z = -1;
        } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
            spawnDirection.z = 1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            spawnDirection.x = 1;
        } else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            spawnDirection.x = -1;
        }

        // Key is pressed up 
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            spawnDirection.z = 0;
        } else if (Input.GetKeyUp(KeyCode.DownArrow)) {
            spawnDirection.z = 0;
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            spawnDirection.x = 0;
        } else if (Input.GetKeyUp(KeyCode.RightArrow)) {
            spawnDirection.x = 0;
        }

        spawnDirection.Normalize();
        if (spawnDirection != Vector3.zero) {
            spawnVectorReady = true;
        }
    }

    IEnumerator BulletLoop() {
        while (true) {
            if (spawnVectorReady) {
                BulletSpawn(spawnDirection);
            }
            yield return new WaitForSeconds(bulletFreq);
        }
    }

    [Command]
    void BulletSpawn(Vector3 dirToSpawn) {
        Vector3 pos = transform.position + new Vector3(0, 1.5f, 0) + 3f * dirToSpawn;
        Quaternion qt = Quaternion.FromToRotation(new Vector3(1, 0, 0), dirToSpawn);
        GameObject bullet = GameObject.Instantiate(bulletObject, pos, qt);
        NetworkServer.Spawn(bullet);
        bullet.GetComponent<Bullet>().Spawn(dirToSpawn);
    }

    private void OnTriggerEnter(Collider other) {
        if (isLocalPlayer) {
            
        }
        if (other.tag == "bullet") {
            TakeDamage(other.GetComponent<Bullet>().damage);
            GameObject.Destroy(other.gameObject);
        }
    }

    [Command]
    void TakeDamage(float damage) {
        health -= damage;
        if (health <= 0) {
            DestoryRoutine();
        }
    }

    [ClientRpc]
    void DestoryRoutine() {
        GameObject.Destroy(gameObject);
    }
}
