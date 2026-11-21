using UnityEngine;

public class SimpleBullet : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private bool moveIn2D = true;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        if (moveIn2D)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime, Space.Self);
        }
        else
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }
}