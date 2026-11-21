using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("Identidad")]
    public string enemyName;

    [Header("Animación (1 solo estado)")]
    public AnimationClip mainAnimationClip; // Animación

    [Header("Estadísticas Básicas")]
    public int maxHealth = 50;
    public float moveSpeed = 3.5f;
    public int contactDamage = 10;

    [Header("Ataque")]
    public bool canShoot = true;          // Enemigo cuerpo a cuerpo
    public GameObject projectilePrefab;
    public ProjectileDataSO projectileData;
    public float fireRate = 1.5f;         // Cada cuántos segundos dispara este enemigo

    [Header("Recompensas")]
    public int scorePoints = 100;
    public int expReward = 25;
}