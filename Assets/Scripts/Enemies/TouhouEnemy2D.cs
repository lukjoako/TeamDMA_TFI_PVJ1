using System;
using System.Collections;
using UnityEngine;

public class TouhouEnemy2D : MonoBehaviour
{
    public enum EnemyState { Patrol, Chase, Attack }

    [Header("Datos Opcionales (ScriptableObject)")]
    [SerializeField] private ScriptableObject enemyDataSO; // Podés asignar tu EnemyDataSO si querés
    [SerializeField] private int maxHP = 30;
    [SerializeField] private int contactOrBulletDamage = 10;
    [SerializeField] private int scoreValue = 100;

    [Header("Movimiento y FSM")]
    [SerializeField] private EnemyState currentState = EnemyState.Patrol;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float patrolRangeX = 2.5f;
    [SerializeField] private float chaseSpeed = 3.2f;
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float attackRadius = 4f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Disparo (Corrutina)")]
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 1.2f;
    [SerializeField] private bool aimAtPlayer = true;

    public static event Action<int> OnEnemyKilled;

    private int currentHP;
    private Vector3 startPos;
    private int patrolDir = 1;
    private Transform playerTarget;
    private Coroutine fireRoutine;

    private void Awake()
    {
        startPos = transform.position;
        currentHP = maxHP;
    }

    private void OnEnable()
    {
        fireRoutine = StartCoroutine(AttackLoop());
    }

    private void OnDisable()
    {
        if (fireRoutine != null)
        {
            StopCoroutine(fireRoutine);
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        DetectPlayerAndUpdateState();

        switch (currentState)
        {
            case EnemyState.Patrol:
                DoPatrol();
                break;
            case EnemyState.Chase:
                DoChase();
                break;
            case EnemyState.Attack:
                DoPatrol(); // Se mueve suave lateralmente mientras ataca (bien estilo Touhou)
                break;
        }
    }

    private void DetectPlayerAndUpdateState()
    {
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerHit == null)
        {
            playerTarget = null;
            currentState = EnemyState.Patrol;
            return;
        }

        playerTarget = playerHit.transform;
        float dist = Vector2.Distance(transform.position, playerTarget.position);

        if (dist <= attackRadius)
        {
            currentState = EnemyState.Attack;
        }
        else
        {
            currentState = EnemyState.Chase;
        }
    }

    private void DoPatrol()
    {
        transform.position += Vector3.right * (patrolDir * patrolSpeed * Time.deltaTime);

        if (transform.position.x > startPos.x + patrolRangeX)
        {
            patrolDir = -1;
        }
        else if (transform.position.x < startPos.x - patrolRangeX)
        {
            patrolDir = 1;
        }
    }

    private void DoChase()
    {
        if (playerTarget == null) return;

        Vector2 dir = (playerTarget.position - transform.position).normalized;
        transform.position += (Vector3)(dir * chaseSpeed * Time.deltaTime);
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireCooldown);

            if (Time.timeScale > 0f && currentState == EnemyState.Attack && enemyBulletPrefab != null)
            {
                Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
                GameObject bulletObj = Instantiate(enemyBulletPrefab, spawnPos, Quaternion.identity);

                EnemyBullet2D bullet = bulletObj.GetComponent<EnemyBullet2D>();
                if (bullet != null)
                {
                    bullet.SetDamage(contactOrBulletDamage);

                    if (aimAtPlayer && playerTarget != null)
                    {
                        Vector2 dirToPlayer = (playerTarget.position - spawnPos).normalized;
                        bullet.SetDirection(dirToPlayer);
                    }
                    else
                    {
                        bullet.SetDirection(Vector2.down);
                    }
                }
            }
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            OnEnemyKilled?.Invoke(scoreValue);
            gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = Application.isPlaying ? startPos : transform.position;

        // Rango de patrulla
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin + Vector3.left * patrolRangeX, origin + Vector3.right * patrolRangeX);

        // Rango de detección (Chase)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Rango de ataque (Attack)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}