using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    public Transform[] waypoints;
    public float moveSpeed = 1f;
    public int maxHealth = 100;

    private int currentHealth;
    private int targetIndex = 1;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
            Destroy(gameObject);
        
        if (targetIndex >= waypoints.Length)
            return;

        if ((Vector2)transform.position == (Vector2)waypoints[targetIndex].position)
            targetIndex++;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[targetIndex].position, Time.deltaTime * moveSpeed);
    }

    public void ApplyDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }
}
