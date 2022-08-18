using UnityEngine;

public class EnemyBehavior : MonoBehaviour, IDamageable
{
    public int NextWaypoint => targetIndex;

    public Transform[] waypoints;
    public float moveSpeed = 1f;
    public int maxHealth = 100;

    public delegate void OnDeathHandler(GameObject self);
    public event OnDeathHandler OnDeath;

    private int currentHealth;
    private int targetIndex = 1;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            if (OnDeath != null)
                OnDeath.Invoke(gameObject);
            Destroy(gameObject);
            return;
        }
        
        if (targetIndex >= waypoints.Length) {
            Destroy(gameObject);
            return;
        }

        if ((Vector2)transform.position == (Vector2)waypoints[targetIndex].position)
            targetIndex++;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[targetIndex].position, Time.deltaTime * moveSpeed);
    }

    public void ApplyDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
    }

}
