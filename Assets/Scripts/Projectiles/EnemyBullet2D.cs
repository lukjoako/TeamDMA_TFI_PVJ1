using UnityEngine;

public class EnemyBullet2D : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private float lifeTime = 4f;
    [SerializeField] private int damage = 10;

    private Vector2 moveDirection = Vector2.down;

    public void SetDirection(Vector2 dir)
    {
        if (dir.sqrMagnitude > 0.001f)
        {
            moveDirection = dir.normalized;
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }
}