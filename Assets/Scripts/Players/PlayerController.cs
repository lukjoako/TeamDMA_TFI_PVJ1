using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Boundary
{
    public float xMinimum = -8.3f;
    public float xMaximum = 8.45f;
    public float yMinimum = -4.24f;
    public float yMaximum = 4.15f;
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Datos")]
    [SerializeField] private PlayerStatsSO stats;

    [Header("Límites y Disparo")]
    [SerializeField] private Boundary boundary;
    [SerializeField] private Transform firePoint;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;
    private float nextFireTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        if (stats != null && anim != null && stats.animatorController != null)
        {
            anim.runtimeAnimatorController = stats.animatorController;
        }
    }

    private void Update()
    {
        ReadMovementInput();
        HandleShooting();
    }

    private void FixedUpdate()
    {
        float speed = stats != null ? stats.moveSpeed : 7f;
        Vector2 targetPos = rb.position + moveInput * speed * Time.fixedDeltaTime;

        targetPos.x = Mathf.Clamp(targetPos.x, boundary.xMinimum, boundary.xMaximum);
        targetPos.y = Mathf.Clamp(targetPos.y, boundary.yMinimum, boundary.yMaximum);

        rb.MovePosition(targetPos);
    }

    private void ReadMovementInput()
    {
        Vector2 input = Vector2.zero;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) input.x -= 1f;
            if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) input.x += 1f;
            if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) input.y -= 1f;
            if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) input.y += 1f;
        }
        moveInput = input.normalized;
    }

    private void HandleShooting()
    {
        if (stats == null || stats.projectilePrefab == null) return;

        bool fire = (Keyboard.current != null && Keyboard.current.spaceKey.isPressed) ||
                    (Mouse.current != null && Mouse.current.leftButton.isPressed);

        if (fire && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + stats.fireRate;
        }
    }

    private void Shoot()
    {
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject projObj = Instantiate(stats.projectilePrefab, spawnPos, Quaternion.identity);

        if (projObj.TryGetComponent(out Projectile proj))
        {
            proj.Initialize(stats.projectileData, Vector2.up, gameObject.tag);
        }
    }
}