using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static int height = 31;
    public static int width = 53;

    public Point[] waypoints;

    public List<Transform> LevelWaypoints { get; private set; }
    public List<float> distances { get; private set; }

    private int[,] gameGrid;

    private static GameState _instance;
    public static GameState Instance { 
        get {
            if (_instance == null)
                _instance = new GameState();

            return _instance;
        }

        private set
        {
            _instance = value;
        }
    }

    public GameState()
    {
        var map = new MapInfo
        {
            Waypoints = new Point[]
            {
                new Point(-1, 3),
                new Point(2, 3),
                new Point(2, 28),
                new Point(20, 28),
                new Point(20, 3),
                new Point(40, 3),
                new Point(40, 28),
                new Point(43, 28)
            },
            Height = 31,
            Width = 53
        };
        InitializeState(map);
        Debug.Log(Application.persistentDataPath);
        FileHandler.SaveToJSON(map, "foomap");
    }

    public GameState(MapInfo map)
    {
        InitializeState(map);
    }

    private void InitializeState(MapInfo map)
    {
        waypoints = map.Waypoints;
        gameGrid = new int[map.Width, map.Height];

        for (var i = 0; i < height; i++) {
            for (var j = 0; j < width; j++) {
                gameGrid[j, i] = 0;
            }
        }

        for (var i = 0; i < waypoints.Length - 1; i++) {
            var currPoint = waypoints[i];
            var nextPoint = waypoints[i + 1];

            var dx = nextPoint.X - currPoint.X;
            var dy = nextPoint.Y - currPoint.Y;

            if (dx != 0 && dy != 0) {
                Debug.Log("INVALID STATE: We don't support diagonal paths");
                return;
            } else if (dx == 0 && dy == 0) {
                Debug.Log("INVALID STATE: We don't support identical points");
                return;
            }

            int current, target, diff, fix;
            bool horizontal;

            if (dx != 0) {
                current = currPoint.X;
                target = nextPoint.X;
                fix = currPoint.Y;
                diff = dx > 0 ? 1 : -1;
                horizontal = true;
            } else {
                current = currPoint.Y;
                target = nextPoint.Y;
                diff = dy > 0 ? 1 : -1;
                fix = currPoint.X;
                horizontal = false;
            }

            for (var j = current; j != target; j += diff) {
                var xValue = horizontal ? j : fix;
                var yValue = horizontal ? fix : j;

                if (xValue < 0 || xValue >= gameGrid.GetLength(0) || yValue < 0 || yValue >= gameGrid.GetLength(0))
                    continue;

                gameGrid[xValue, yValue] = 1;
            }
        }
    }

    public int[,] GetGameGrid() => gameGrid;

    public void SetLevelWaypoints(List<Transform> levelWaypoints)
    {
        LevelWaypoints = levelWaypoints;
        distances = new List<float>(new float[levelWaypoints.Count]);

        for(var i = distances.Count-2; i >= 0; i--)
        {
            distances[i] = distances[i + 1] + Vector2.Distance(levelWaypoints[i + 1].position, levelWaypoints[i].position);
            Debug.Log($"Waypoint Distance entry {i}: {distances[i]}");
        }
    }

    public bool TryPlaceTower(int placementX, int placementY)
    {
        var gridX = GetGridX(placementX);
        var gridY = GetGridY(placementY);

        if (IsOccupiedInternal(gridX, gridY))
            return false;

        gameGrid[gridX, gridY] = 2;
        return true;
    }

    public bool IsOccupied(int queryX, int queryY)
    {
        var gridX = GetGridX(queryX);
        var gridY = GetGridY(queryY);

        return IsOccupiedInternal(gridX, gridY);
    }

    private bool IsOccupiedInternal(int gridX, int gridY)
    {
        if (gridX < 0 || gridX >= width || gridY < 0 || gridY >= height)
            return true;

        return gameGrid[gridX, gridY] != 0;
    }

    public static Vector3 GridToWorldPoint(Vector2 gridPoint) => new Vector3(gridPoint.x - 1 - (width / 2) + 0.5f, gridPoint.y - 1 - (height / 2) + 0.5f, 0);

    private static int GetGridX(int xPos) => xPos + (width / 2) + 1;

    private static int GetGridY(int yPos) => yPos + (height / 2) + 1;
}
