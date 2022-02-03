using UnityEngine;

public class BasicBulletBehavior : MonoBehaviour
{
    public Transform targetTransform;
    public float velocity;
    public int power;

    private void Start()
    {
        Destroy(gameObject, 15);
    }

    void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetTransform.position, velocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            var entity = collision.gameObject.GetComponent<IDamageable>();
            entity.ApplyDamage(power);
            Destroy(gameObject);
        }
    }
}
