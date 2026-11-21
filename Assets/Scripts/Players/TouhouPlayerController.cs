using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class TouhouPlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float focusSpeed = 3.5f;

    [Header("Disparo (Corrutina)")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireCooldown = 0.12f;

    [Header("Detección por Físicas y LayerMask")]
    [SerializeField] private float hitCheckRadius = 0.25f;
    [SerializeField] private LayerMask enemyAndBulletMask;

    public static event Action<int> OnPlayerDamaged;

    private Rigidbody2D rb2D;
    private Rigidbody rb3D;
    private bool is2D;
    private bool isFiring;
    private int hp = 100;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb3D = GetComponent<Rigidbody>();
        is2D = rb2D != null;
    }

    private void OnEnable()
    {
        StartCoroutine(FireRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            isFiring = false;
            return;
        }

        var kb = Keyboard.current;
        var mouse = Mouse.current;

        bool fireKey = kb != null && (kb.zKey.isPressed || kb.spaceKey.isPressed);
        bool fireMouse = mouse != null && mouse.leftButton.isPressed;
        isFiring = fireKey || fireMouse;
    }

    private void FixedUpdate()
    {
        var kb = Keyboard.current;
        float h = 0f;
        float v = 0f;

        if (kb != null)
        {
            if (kb.leftArrowKey.isPressed || kb.aKey.isPressed) h -= 1f;
            if (kb.rightArrowKey.isPressed || kb.dKey.isPressed) h += 1f;
            if (kb.downArrowKey.isPressed || kb.sKey.isPressed) v -= 1f;
            if (kb.upArrowKey.isPressed || kb.wKey.isPressed) v += 1f;
        }

        Vector2 dir = new Vector2(h, v).normalized;

        bool focus = kb != null && (kb.leftShiftKey.isPressed || kb.rightShiftKey.isPressed);
        float currentSpeed = focus ? focusSpeed : moveSpeed;

        if (is2D)
        {
            rb2D.linearVelocity = dir * currentSpeed;
        }
        else if (rb3D != null)
        {
            rb3D.linearVelocity = new Vector3(dir.x * currentSpeed, 0f, dir.y * currentSpeed);
        }

        CheckDangerOverlap();
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            if (isFiring && bulletPrefab != null && firePoint != null && Time.timeScale > 0f)
            {
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            }
            yield return new WaitForSeconds(fireCooldown);
        }
    }

    private void CheckDangerOverlap()
    {
        if (is2D)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, hitCheckRadius, enemyAndBulletMask);
            if (hit != null)
            {
                ApplyHit(10);
                if (hit.CompareTag("EnemyBullet"))
                {
                    Destroy(hit.gameObject);
                }
            }
        }
        else
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, hitCheckRadius, enemyAndBulletMask);
            if (hits.Length > 0)
            {
                ApplyHit(10);
                if (hits[0].CompareTag("EnemyBullet"))
                {
                    Destroy(hits[0].gameObject);
                }
            }
        }
    }

    private void ApplyHit(int damage)
    {
        hp = Mathf.Max(0, hp - damage);
        OnPlayerDamaged?.Invoke(hp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hitCheckRadius);
    }
}