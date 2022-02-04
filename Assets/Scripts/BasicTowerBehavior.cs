using System.Linq;
using UnityEngine;

public class BasicTowerBehavior : MonoBehaviour
{
    public float fireRateInSeconds;
    public GameObject bulletPrefab;
    public float range = 5;
    public GameManager gameManager;

    private float lastFireTime = -1000;

    void Update()
    {
        var enemiesInRange = Physics2D.OverlapCircleAll(transform.position, range).Select(obj => obj.GetComponent<EnemyBehavior>()).Where(enemy => enemy != null);
        var closestEnemy = enemiesInRange.OrderBy(e => GameManager.GetDistanceFromBase(e)).FirstOrDefault();

        if (closestEnemy == null)
            return;

        if(lastFireTime + fireRateInSeconds <= Time.time)
        {
            var projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BasicBulletBehavior>();
            projectile.targetTransform = closestEnemy.transform;

            lastFireTime = Time.time;
        }
    }
}
