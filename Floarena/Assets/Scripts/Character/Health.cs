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
    }

    // Hook to currentHealth SyncVar
    void UpdateHealth(float oldHealth, float newHealth) {
        if (hasBar) {
            healthBar.SetHealth(newHealth);    
        }
    }

    [Command(requiresAuthority = false)]
    public void TakeDamage(float damage) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            // Play dying animation here
            MatchManager.instance.NewRound(MatchManager.instance.GetOpponentNum());
        }
    }

    [Command(requiresAuthority=false)]
    public void TakeHealing(float healing) {
        if (currentHealth + healing > maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth += healing;
        }
    }

    [Command(requiresAuthority=false)]
    public void DestroyRoutine() {
        // if (hasBar) {
            // GameObject.Destroy(healthBar.gameObject);
        // }
        // GameObject.Destroy(gameObject);
    }

    [Command(requiresAuthority = false)]
    public void ResetHealth() {
        currentHealth = maxHealth;
    }
}
