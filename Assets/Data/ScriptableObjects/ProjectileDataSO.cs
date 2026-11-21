using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileDataSO", menuName = "Scriptable Objects/ProjectileDataSO")]
public class ProjectileDataSO : ScriptableObject
{
    [Header("Identidad")]
    public string projectileName;

    [Header("Animaciones del Proyectil")]
    public RuntimeAnimatorController animatorController; // Controlador de animaciones (.controller) del proyectil con sus estados (Spawn, Fly, Hit)
    public AnimationClip spawnClip; // Animación al salir (1 vez)
    public AnimationClip flyClip;   // Animación al avanzar (en bucle)
    public AnimationClip hitClip;   // Animación al chocar

    [Header("Estadísticas Base")]
    public int baseDamage = 10;
    public float baseSpeed = 12f;
    public float maxLifeTime = 4f;
    public float hitAnimationDuration = 0.25f;
}