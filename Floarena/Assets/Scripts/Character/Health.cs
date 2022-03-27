using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateMaxHealth))]
    public float maxHealth;
    [SyncVar(hook = nameof(UpdateHealth))]
    public float currentHealth = 1;

    public bool hasBar = true;
    public HealthBar healthBar;

    public AudioManager audioManager;

    void Start() {
        if (isLocalPlayer) {
            float temp  = GameManager.instance.loadout.GetLoadoutStats().GetAttributeValue(Attribute.HP);
            SetMaxHealth(temp);
        }
        audioManager = GetComponent<AudioManager>();
    }

    void Update() {
    }

    // Hook to currentHealth SyncVar
    void UpdateHealth(float oldHealth, float newHealth) {
        if (hasBar) {
            healthBar.SetHealth(newHealth);    
        }
    }

    void UpdateMaxHealth(float oldMaxHealth, float newMaxHealth) {
        if (hasBar) {
            healthBar.Init(newMaxHealth);
        }
    }

    public void TakeDamage(float damage) {
        CmdTakeDamage(damage, MatchManager.instance.GetOpponentNum());
    }

    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(float damage, int sourcePlayer) {
        currentHealth -= damage;
        audioManager.PlaySound(AudioIndex.DECREASE_HEALTH_AUDIO, transform.position);
        if (currentHealth <= 0) {
            audioManager.PlaySound(AudioIndex.DEATH_AUDIO, transform.position);
            StartCoroutine(StartNewRound(sourcePlayer));
        }
    }

    IEnumerator StartNewRound(int sourcePlayer) {
        yield return new WaitForSeconds(1f);
        MatchManager.instance.NewRound(sourcePlayer);
    }

    [Command(requiresAuthority=false)]
    public void TakeHealing(float healing) {
        if (currentHealth + healing > maxHealth) {
            currentHealth = maxHealth;
        } else {
            currentHealth += healing;
        }
        audioManager.PlaySound(AudioIndex.INCREASE_HEALTH_AUDIO, transform.position);
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

    [Command(requiresAuthority = false)]
    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
    }
}
