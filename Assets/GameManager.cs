using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform spawnPoint;
    public Transform[] enemyPathWaypoints;

    public int enemiesPerWave = 10;
    public int secondsBetweenWaves = 30;
    public float secondsBetweenSpawns = 1.0f;

    float timeSinceLastSpawn = -1000;
    bool currentlySpawning = false;

    private static GameState gameState;

    private List<GameObject> enemies = new List<GameObject>();

    private void Start() {
        gameState = GameState.Instance;
        gameState.SetLevelWaypoints(enemyPathWaypoints.ToList());
    }

    private void Update() {
        if(timeSinceLastSpawn + secondsBetweenWaves <= Time.time && !currentlySpawning){
            currentlySpawning = true;
            StartCoroutine(SpawnWave());
        }
    }

    private IEnumerator SpawnWave()
    {
        for (var i = 0; i < enemiesPerWave; i++) {
            var enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity).GetComponent<EnemyBehavior>();
            enemy.waypoints = enemyPathWaypoints;
            enemy.OnDeath += OnEnemyKilled;

            enemies.Add(enemy.gameObject);

            yield return new WaitForSeconds(secondsBetweenSpawns);

            timeSinceLastSpawn = Time.time;
            currentlySpawning = false;
        }
    }

    private void OnEnemyKilled(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public static float GetDistanceFromBase(EnemyBehavior enemy) => 
        Vector2.Distance(enemy.transform.position, gameState.LevelWaypoints[enemy.NextWaypoint].position) 
            + gameState.distances[enemy.NextWaypoint];
}
