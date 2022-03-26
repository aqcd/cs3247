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

    public AudioSource audioSource;
    public AudioClip deathAudio;
    public AudioClip attackedAudio;
    public AudioClip increaseHealthAudio;
    public AudioClip decreaseHealthAudio;

    void Start() {
        if (isLocalPlayer) {
            float temp  = GameManager.instance.loadout.GetLoadoutStats().GetAttributeValue(Attribute.HP);
            SetMaxHealth(temp);
        }
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

    [ClientRpc]
    public void RpcPlayDecreaseHealthAudio() {
        audioSource.PlayOneShot(attackedAudio, 0.5f);
        audioSource.PlayOneShot(decreaseHealthAudio, 0.5f);
    }

    [ClientRpc]
    public void RpcPlayDeathAudio() {
        audioSource.PlayOneShot(deathAudio, 0.5f);
    }

    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(float damage, int sourcePlayer) {
        currentHealth -= damage;
        RpcPlayDecreaseHealthAudio();
        if (currentHealth <= 0) {
            RpcPlayDeathAudio();
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
        RpcPlayIncreaseHealthAudio();
    }

    [ClientRpc]
    public void RpcPlayIncreaseHealthAudio() {
        audioSource.PlayOneShot(increaseHealthAudio, 0.5f);
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
