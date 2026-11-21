using UnityEngine;

public class PlayerBullet2D : MonoBehaviour
{
    [SerializeField] private float speed = 16f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += Vector3.up * (speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TouhouEnemy2D enemy = other.GetComponent<TouhouEnemy2D>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}