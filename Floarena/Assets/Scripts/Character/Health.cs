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

    public bool isInvulnerable = false;
    
    private ParticleSystemManager particleSystemManager;

    public AudioManager audioManager;

    void Start() {
        if (isLocalPlayer) {
            float temp  = GameManager.instance.loadout.GetLoadoutStats().GetAttributeValue(Attribute.HP);
            SetMaxHealth(temp);
            
        }
        // damageTakenEvent.AddListener(gameObject.GetComponent<BerryPickupManager>().InterruptChannel);
        audioManager = GetComponent<AudioManager>();

        particleSystemManager = gameObject.GetComponent<ParticleSystemManager>();
        Debug.Log("HP: " + particleSystemManager);
    }

    void Update() {
    }

    // Hook to currentHealth SyncVar
    void UpdateHealth(float oldHealth, float newHealth) {
        if (newHealth < oldHealth) {
            damageTakenEvent.Invoke();
        }
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
        if (!isInvulnerable)
        {
            CmdTakeDamage(damage, MatchManager.instance.GetOpponentNum());
            if (particleSystemManager != null) {
                particleSystemManager.PlayDMG();
            }
        }
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

        if (particleSystemManager != null) {
            particleSystemManager.PlayHeal();
        }
    }

    public void BecomeInvulnerable(float duration)
    {
        StartCoroutine(Invulnerable(duration));
    }

    IEnumerator Invulnerable(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    // public void EndInvulnerable()
    // {
    //     isInvulnerable = false;
    // }

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
