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
            var projectile = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<BasicBulletBehavior>();
            projectile.targetTransform = enemy;

            lastFireTime = Time.time;
        }
    }
}
