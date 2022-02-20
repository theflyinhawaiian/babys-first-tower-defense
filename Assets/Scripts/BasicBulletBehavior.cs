using UnityEngine;

public class BasicBulletBehavior : MonoBehaviour
{
    public Transform targetTransform;
    public float velocity;
    public int power;

    private Vector3 lastKnownTargetPosition;

    void FixedUpdate()
    {
        var target = targetTransform != null ? targetTransform.position : lastKnownTargetPosition;

        var move = Vector2.MoveTowards(transform.position, target, velocity);

        if (move == Vector2.zero)
            Destroy(gameObject);

        transform.position = move;
        lastKnownTargetPosition = target;
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

    public void SetTarget(Transform target)
    {
        targetTransform = target;
        lastKnownTargetPosition = target.position;
    }
}
