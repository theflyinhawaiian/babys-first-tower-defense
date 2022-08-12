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
        //var map = FileHandler.ReadFromJSON<GameMap>("foomap");
        var grid = new int[width, height];

        for(var i = 0; i < height; i++) {
            for(var j = 0; j < width; j++) {
                grid[j, i] = 1;
            }
        }

        grid[0, 3] = 3;
        grid[43, 28] = 4;

        var map = new GameMap
        {
            Grid = grid
        };
        InitializeState(map);
    }

    public GameState(GameMap map)
    {
        InitializeState(map);
    }

    private void InitializeState(GameMap map)
    {
        /*waypoints = new Point[]
            {
                new Point(0, 3),
                new Point(2, 3),
                new Point(2, 28),
                new Point(20, 28),
                new Point(20, 3),
                new Point(40, 3),
                new Point(40, 28),
                new Point(43, 28)
            };*/

        waypoints = new Point[]
        {
            new Point(0,3),
            new Point(43,28)
        };

        gameGrid = map.Grid;
    }

    public int[,] GetGameGrid() => gameGrid;

    public void SetLevelWaypoints(List<Transform> levelWaypoints)
    {
        LevelWaypoints = levelWaypoints;
        distances = new List<float>(new float[levelWaypoints.Count]);

        for(var i = distances.Count-2; i >= 0; i--)
        {
            distances[i] = distances[i + 1] + Vector2.Distance(levelWaypoints[i + 1].position, levelWaypoints[i].position);
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
