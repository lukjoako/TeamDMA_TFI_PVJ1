using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSO", menuName = "Scriptable Objects/PlayerStatsSO")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Identidad")]
    public string characterName;
    public Sprite characterPortrait;

    [Header("Animaciones del Jugador")]
    public RuntimeAnimatorController animatorController;
    public AnimationClip idleClip;
    public AnimationClip victoryClip;

    [Header("Estadísticas Principales")]
    public int maxHealth = 100;
    public int maxShield = 50;
    public int startingLives = 3;
    public int baseDefense = 5;

    [Header("Movimiento")]
    public float moveSpeed = 7f;

    [Header("Ataque")]
    public GameObject projectilePrefab;
    public ProjectileDataSO projectileData;
    public float fireRate = 0.25f;
}
