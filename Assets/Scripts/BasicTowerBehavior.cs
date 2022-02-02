using UnityEngine;

public class BasicTowerBehavior : MonoBehaviour
{
    public Transform enemy;
    public float fireRateInSeconds;
    public GameObject bulletPrefab;

    private float lastFireTime = -1000;

    // Update is called once per frame
    void Update()
    {
        if(lastFireTime + fireRateInSeconds <= Time.time)
        {
            var lookDir = enemy.position - transform.position;
            float angle = Mathf.Atan2(lookDir.y, lookDir.x);
            var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            var bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var data = bullet.GetComponent<BasicBulletBehavior>();
            data.direction = direction;

            lastFireTime = Time.time;
        }
    }
}
