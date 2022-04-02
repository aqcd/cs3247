using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Mirror;

public class Health : NetworkBehaviour
{
    [SyncVar(hook = nameof(UpdateMaxHealth))]
    public float maxHealth;
    [SyncVar(hook = nameof(UpdateHealth))]
    public float currentHealth = 1;

    public bool hasBar = true;
    public HealthBar healthBar;
    public UnityEvent damageTakenEvent;
    
    private ParticleSystemManager particleSystemManager;

    void Start() {
        if (isLocalPlayer) {
            float temp  = GameManager.instance.loadout.GetLoadoutStats().GetAttributeValue(Attribute.HP);
            SetMaxHealth(temp);
            damageTakenEvent.AddListener(GameObject.Find("ChannelButton").GetComponent<ChannelButtonController>().InterruptChannel);
        }
        particleSystemManager = gameObject.GetComponent<ParticleSystemManager>();
        Debug.Log("HP: " + particleSystemManager);
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
        damageTakenEvent.Invoke();
        CmdTakeDamage(damage, MatchManager.instance.GetOpponentNum());
        if (particleSystemManager != null) {
            particleSystemManager.PlayDMG();
        }
    }

    [Command(requiresAuthority = false)]
    public void CmdTakeDamage(float damage, int sourcePlayer) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
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
        if (particleSystemManager != null) {
            particleSystemManager.PlayHeal();
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

    [Command(requiresAuthority = false)]
    public void SetMaxHealth(float maxHealth) {
        this.maxHealth = maxHealth;
        currentHealth = this.maxHealth;
    }
}
