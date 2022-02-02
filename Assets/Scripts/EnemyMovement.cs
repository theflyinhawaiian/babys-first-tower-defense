using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 1f;

    private int targetIndex = 1;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (targetIndex >= waypoints.Length)
            return;

        if ((Vector2)transform.position == (Vector2)waypoints[targetIndex].position)
        {
            Debug.Log($"Waypoint {targetIndex} reached. Distance to next point: {Vector2.Distance(transform.position, waypoints[targetIndex + 1].position)}. Time: {Time.time}");
            targetIndex++;
        }

        transform.position = Vector2.MoveTowards(transform.position, waypoints[targetIndex].position, Time.deltaTime * moveSpeed);
    }
}
