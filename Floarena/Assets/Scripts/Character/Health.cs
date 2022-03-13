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

    void Update()
    {
        if (hasBar) {
            healthBar.SetHealth(currentHealth);
        }

        // Testing
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.TakeDamage(10);
        }
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
        if (hasBar) {
            GameObject.Destroy(healthBar.gameObject);
        }
        GameObject.Destroy(gameObject);
    }

    public override void OnStopClient()
    {
        if (hasBar) {
            GameObject.Destroy(healthBar.gameObject);
        }
    }
}
