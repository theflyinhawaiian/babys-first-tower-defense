using Assets.Scripts;
using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameState
{
    private Point[] waypoints;

    public List<Transform> LevelWaypoints { get; private set; }
    public List<float> distances { get; private set; }

    private int[,] gameGrid;

    private Vector2Int Start, End;

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
        var map = FileHandler.ReadFromJSON<GameMap>("map1");
        InitializeState(map);
    }

    public GameState(GameMap map)
    {
        InitializeState(map);
    }

    private void InitializeState(GameMap map)
    {
        gameGrid = map.Grid;

        for(var i = 0; i < gameGrid.GetLength(0); i++) {
            for(var j = 0; j < gameGrid.GetLength(1); j++) {
                if (gameGrid[i,j] == 3)
                    Start = new Vector2Int(i,j);
                if (gameGrid[i,j] == 4)
                    End = new Vector2Int(i,j);
            }
        }
    }

    public int[,] GetGameGrid() => gameGrid;

    public Point[] GetWaypoints()
    {
        if (waypoints != null && waypoints.Length > 0)
            return waypoints;

        var pathFinder = new PathFinder(gameGrid, Start, End);
        waypoints = pathFinder.FindPath().ToArray();
        return waypoints;
    }

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
        if (gridX < 0 || gridX >= GameConstants.Width || gridY < 0 || gridY >= GameConstants.Height)
            return true;

        return gameGrid[gridX, gridY] != 0;
    }

    public static Vector3 GridToWorldPoint(Vector2 gridPoint) => new Vector3(gridPoint.x - 1 - (GameConstants.Width / 2) + 0.5f, gridPoint.y - 1 - (GameConstants.Height / 2) + 0.5f, 0);

    private static int GetGridX(int xPos) => xPos + (GameConstants.Width / 2) + 1;

    private static int GetGridY(int yPos) => yPos + (GameConstants.Height / 2) + 1;
}
