using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    private const int TOWER_COST = 100;

    public GameObject enemyPrefab;
    public GameObject waypointPrefab;
    public Tilemap tilemap;

    private Transform[] enemyPathWaypoints;

    public int enemiesPerWave = 10;
    public int secondsBetweenWaves = 30;
    public float secondsBetweenSpawns = 1.0f;

    float timeSinceLastSpawn = -1000;
    bool currentlySpawning = false;

    private static GameState gameState;

    public MoneyManager MoneyManager { get; private set; }

    private List<GameObject> enemies = new List<GameObject>();

    public List<IPlayerMoneyChangedListener> moneyChangedListeners = new List<IPlayerMoneyChangedListener>();

    private void Start()
    {
        gameState = GameState.Instance;

        var waypoints = gameState.waypoints;

        MoneyManager = new MoneyManager(500);

        enemyPathWaypoints = new Transform[waypoints.Length];
        for(var k = 0; k < waypoints.Length; k++)
        {
            var obj = Instantiate(waypointPrefab, GameState.GridToWorldPoint(new Vector2(waypoints[k].X, waypoints[k].Y)), Quaternion.identity);
            enemyPathWaypoints[k] = obj.transform;
        }

        gameState.SetLevelWaypoints(enemyPathWaypoints.ToList());

        var mapRenderer = new MapRenderer();
        mapRenderer.RenderMap(tilemap, gameState.GetGameGrid());
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
            var enemy = Instantiate(enemyPrefab, enemyPathWaypoints[0].position, Quaternion.identity).GetComponent<EnemyBehavior>();
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
        MoneyManager.AddBalance(10);
        enemies.Remove(enemy);
    }

    public bool CanPlaceTowerAt(int x, int y)
    {
        if (!MoneyManager.CanAfford(TOWER_COST))
            return false;

        return !gameState.IsOccupied(x, y);
    }

    public bool TryPlaceTowerAt(int x, int y)
    {
        if (!CanPlaceTowerAt(x, y))
            return false;

        MoneyManager.TrySubtractBalance(TOWER_COST);
        gameState.TryPlaceTower(x, y);
        return true;
    }

    public static float GetDistanceFromBase(EnemyBehavior enemy) =>
        Vector2.Distance(enemy.transform.position, gameState.LevelWaypoints[enemy.NextWaypoint].position)
            + gameState.distances[enemy.NextWaypoint];
}
