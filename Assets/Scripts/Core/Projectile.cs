using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    private ProjectileDataSO data;
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D col;

    private string ownerTag;
    private bool hasHit = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
    }

    public void Initialize(ProjectileDataSO projectileData, Vector2 direction, string shooterTag)
    {
        data = projectileData;
        ownerTag = shooterTag;

        if (data == null)
        {
            Debug.LogWarning("Proyectil instanciado sin ProjectileDataSO asignado.");
            Destroy(gameObject);
            return;
        }

        if (anim != null && data.animatorController != null)
        {
            anim.runtimeAnimatorController = data.animatorController;
        }

        rb.linearVelocity = direction.normalized * data.baseSpeed;
        Destroy(gameObject, data.maxLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        // Evita golpear a quien disparó o a aliados del mismo bando
        if (!string.IsNullOrEmpty(ownerTag) && collision.CompareTag(ownerTag)) return;

        if (collision.TryGetComponent(out IDamageable target))
        {
            hasHit = true;
            target.TakeDamage(data.baseDamage);
            TriggerImpact();
        }
    }

    private void TriggerImpact()
    {
        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        if (anim != null && data.hitClip != null)
        {
            anim.SetTrigger("Hit");
        }

        Destroy(gameObject, data.hitAnimationDuration);
    }
}