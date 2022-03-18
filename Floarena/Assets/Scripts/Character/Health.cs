using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    public int maxHealth = 200;
    [SyncVar]
    public int currentHealth = 0;

    public bool hasBar = true;
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update() {
        if (hasBar) {
            healthBar.SetHealth(currentHealth);
        }

        // Simulate round ending event (player has died)
        if (isLocalPlayer) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("Pressed");
                MatchManager.instance.NewRound();
            }
        }
    }

    [Command]
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            DestroyRoutine();
        }
    }

    [Command]
    public void TakeHealing(int healing) {
        if (currentHealth + healing > maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth += healing;
        }
    }

    [ClientRpc]
    public void DestroyRoutine() {
        if (hasBar) {
            GameObject.Destroy(healthBar.gameObject);
        }
        GameObject.Destroy(gameObject);
    }
}
