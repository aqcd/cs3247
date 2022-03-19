using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    public float maxHealth;
    [SyncVar(hook = nameof(UpdateHealth))]
    public float currentHealth = 0;

    public bool hasBar = true;
    public HealthBar healthBar;

    void Awake()
    {
        maxHealth = GameManager.instance.loadout.GetItemNetEffects().GetAttributeValue(Attribute.HP);
        currentHealth = maxHealth;
    }

    void Update() {
        // Simulate round ending event (player has died)
        if (isLocalPlayer) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                MatchManager.instance.NewRound(MatchManager.instance.GetOpponentNum());
            }
        }
    }

    // Hook to currentHealth SyncVar
    void UpdateHealth(float oldHealth, float newHealth) {
        if (hasBar) {
            healthBar.SetHealth(newHealth);    
        }
    }

    [Command]
    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            DestroyRoutine();
        }
    }

    [Command]
    public void TakeHealing(float healing) {
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
