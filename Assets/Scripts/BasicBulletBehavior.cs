using UnityEngine;

public class BasicBulletBehavior : MonoBehaviour
{
    public Vector2 direction;
    public float velocity;

    private void Start()
    {
        Destroy(gameObject, 15);
    }

    void FixedUpdate()
    {
        transform.position += (Vector3)direction.normalized * velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
            Destroy(gameObject);
    }
}
