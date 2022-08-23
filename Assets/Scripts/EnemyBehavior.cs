using System;
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

    private Transform Healthbar;
    private Transform LifeRemaining;

    private void Start()
    {
        currentHealth = maxHealth;
        Healthbar = transform.Find("HealthBar");
        LifeRemaining = Healthbar.Find("Bar");
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

        if ((Vector2)transform.position == (Vector2)waypoints[targetIndex].position)
            targetIndex++;

        if (targetIndex >= waypoints.Length) {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[targetIndex].position, Time.deltaTime * moveSpeed);
    }

    public void ApplyDamage(int damageAmount)
    {
        if (!Healthbar.gameObject.activeInHierarchy)
            Healthbar.gameObject.SetActive(true);

        currentHealth -= damageAmount;

        LifeRemaining.localScale = new Vector3(((float)currentHealth) / maxHealth, LifeRemaining.localScale.y, LifeRemaining.localScale.z);
    }

}
