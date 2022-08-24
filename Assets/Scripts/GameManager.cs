using Assets.Scripts;
using Assets.Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int TOWER_COST = 100;

    public GameObject enemyPrefab;
    public GameObject waypointPrefab;
    public GameObject towerPrefab;
    public Tilemap tilemap;
    public ButtonBehavior placeTowerButton;
    public CameraController camController;

    public string mapName = "map1";

    private Transform[] enemyPathWaypoints;

    public int enemiesPerWave = 10;
    public int secondsBetweenWaves = 30;
    public float secondsBetweenSpawns = 1.0f;

    float timeSinceLastSpawn = -1000;
    bool currentlySpawning = false;

    public bool placeMode;

    private static GameState gameState;

    public MoneyManager MoneyManager { get; private set; }

    private List<GameObject> enemies = new List<GameObject>();

    public List<IPlayerMoneyChangedListener> moneyChangedListeners = new List<IPlayerMoneyChangedListener>();

    private void Start()
    {
        GameState.Instance = new GameState(mapName);
        gameState = GameState.Instance;

        var waypoints = gameState.GetWaypoints();

        MoneyManager = new MoneyManager(500);

        enemyPathWaypoints = new Transform[waypoints.Length];
        for(var k = 0; k < waypoints.Length; k++)
        {
            var obj = Instantiate(waypointPrefab, GameState.GridToWorldPoint(new Vector2(waypoints[k].X, waypoints[k].Y)), Quaternion.identity);
            enemyPathWaypoints[k] = obj.transform;
        }

        gameState.SetLevelWaypoints(enemyPathWaypoints.ToList());

        var mapRenderer = new MapRenderer(tilemap);
        mapRenderer.RenderMap(gameState.GetGameGrid());
    }

    private void Update() {
        if(timeSinceLastSpawn + secondsBetweenWaves <= Time.time && !currentlySpawning){
            currentlySpawning = true;
            StartCoroutine(SpawnWave());
        }
    }

    public void ProcessClick(int x, int y)
    {
        if (!placeMode || !TryPlaceTowerAt(x, y))
            return;

        Instantiate(towerPrefab, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
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

    public void TogglePlaceMode()
    {
        placeMode = !placeMode;
        placeTowerButton.SetSelected(placeMode);
    }

    public static float GetDistanceFromBase(EnemyBehavior enemy) =>
        Vector2.Distance(enemy.transform.position, gameState.LevelWaypoints[enemy.NextWaypoint].position)
            + gameState.distances[enemy.NextWaypoint];
}
