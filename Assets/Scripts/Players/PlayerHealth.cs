using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Datos")]
    [SerializeField] private PlayerStatsSO stats;

    [Header("Invulnerabilidad")]
    [SerializeField] private float invulnerabilityDuration = 1.2f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Eventos (Para UI o GameManager)")]
    public UnityEvent<int, int> OnHealthChanged;
    public UnityEvent<int, int> OnShieldChanged;
    public UnityEvent<int> OnLivesChanged;
    public UnityEvent OnPlayerDeath;

    private int currentHealth;
    private int currentShield;
    private int currentLives;
    private bool isInvulnerable;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InitializeStats();
    }

    private void InitializeStats()
    {
        if (stats == null) return;

        currentHealth = stats.maxHealth;
        currentShield = stats.maxShield;
        currentLives = stats.startingLives;

        OnHealthChanged?.Invoke(currentHealth, stats.maxHealth);
        OnShieldChanged?.Invoke(currentShield, stats.maxShield);
        OnLivesChanged?.Invoke(currentLives);
    }

    public void TakeDamage(int rawDamage)
    {
        if (isInvulnerable || currentLives <= 0 || stats == null) return;

        int mitigatedDamage = Mathf.Max(1, rawDamage - stats.baseDefense);

        if (currentShield > 0)
        {
            int shieldAbsorbed = Mathf.Min(currentShield, mitigatedDamage);
            currentShield -= shieldAbsorbed;
            mitigatedDamage -= shieldAbsorbed;
            OnShieldChanged?.Invoke(currentShield, stats.maxShield);
        }

        if (mitigatedDamage > 0)
        {
            currentHealth = Mathf.Max(0, currentHealth - mitigatedDamage);
            OnHealthChanged?.Invoke(currentHealth, stats.maxHealth);
        }

        if (currentHealth <= 0)
        {
            HandleLifeLost();
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private void HandleLifeLost()
    {
        currentLives--;
        OnLivesChanged?.Invoke(currentLives);

        if (currentLives > 0)
        {
            currentHealth = stats.maxHealth;
            currentShield = stats.maxShield;
            OnHealthChanged?.Invoke(currentHealth, stats.maxHealth);
            OnShieldChanged?.Invoke(currentShield, stats.maxShield);

            StartCoroutine(InvulnerabilityRoutine());
        }
        else
        {
            OnPlayerDeath?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0f;

        while (elapsed < invulnerabilityDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.enabled = !spriteRenderer.enabled;

            yield return new WaitForSeconds(0.1f);
            elapsed += 0.1f;
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        isInvulnerable = false;
    }

    public void Heal(int amount)
    {
        if (stats == null) return;
        currentHealth = Mathf.Min(stats.maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, stats.maxHealth);
    }

    public void RechargeShield(int amount)
    {
        if (stats == null) return;
        currentShield = Mathf.Min(stats.maxShield, currentShield + amount);
        OnShieldChanged?.Invoke(currentShield, stats.maxShield);
    }
}