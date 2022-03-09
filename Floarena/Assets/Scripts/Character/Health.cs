using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{

    public int maxHealth = 200;
    [SyncVar]
    public int currentHealth = 0;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.SetHealth(currentHealth);
    }

    [Command]
    void TakeDamage(int damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            DestroyRoutine();
        }
    }

    [Command]
    void TakeHealing(int healing) {
        if (currentHealth + healing > maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth += healing;
        }
    }

    [ClientRpc]
    void DestroyRoutine() {
        GameObject.Destroy(gameObject);
    }
}
