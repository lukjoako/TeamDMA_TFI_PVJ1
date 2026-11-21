using UnityEngine;

[CreateAssetMenu(fileName = "BossDataSO", menuName = "Scriptable Objects/BossDataSO")]
public class BossDataSO : ScriptableObject
{
    [Header("Identidad")]
    public string bossName;

    [Header("Estadísticas Básicas")]
    public int maxHealth = 1000;
    public float moveSpeed = 3f;
    public int contactDamage = 25;

    [Header("Ataque y Fases")]
    public GameObject projectilePrefab;
    public ProjectileDataSO projectileData;

    [Tooltip("Porcentaje de vida restante (0.5 = 50%) para pasar a Fase 2")]
    [Range(0.1f, 0.9f)]
    public float phaseTwoHealthThreshold = 0.5f;

    [Header("Fase 1")]
    public float phaseOneFireRate = 0.8f;
    public float phaseOneDamageMultiplier = 1f;
    public float phaseOneSpeedMultiplier = 1f;

    [Header("Fase 2 (Solo aumenta cadencia, daño y velocidad de la bala)")]
    public float phaseTwoFireRate = 0.45f;
    public float phaseTwoDamageMultiplier = 1.5f;
    public float phaseTwoSpeedMultiplier = 1.35f;

    [Header("Recompensas")]
    public int scorePoints = 5000;
    public int expReward = 500;
}