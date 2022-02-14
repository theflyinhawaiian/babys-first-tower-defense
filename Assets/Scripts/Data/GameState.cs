using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static int height = 31;
    public static int width = 53;

    public (int x, int y)[] waypoints;

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
        waypoints = new[]
        {
            (-1, 3),
            (2, 3),
            (2, 28),
            (26, 28),
            (26, 3),
            (51, 3),
            (51, 28),
            (53, 28)
        };

        gameGrid = new int[width, height];

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                gameGrid[j, i] = 0;
            }
        }

        for(var i = 0; i < waypoints.Length - 1; i++)
        {
            var currPoint = waypoints[i];
            var nextPoint = waypoints[i + 1];

            var dx = nextPoint.x - currPoint.x;
            var dy = nextPoint.y - currPoint.y;

            if(dx != 0 && dy != 0)
            {
                Debug.Log("INVALID STATE: We don't support diagonal paths");
                return;
            }else if(dx == 0 && dy == 0)
            {
                Debug.Log("INVALID STATE: We don't support identical points");
                return;
            }

            int current, target, diff, fix;
            bool horizontal;

            if(dx != 0)
            {
                current = currPoint.x;
                target = nextPoint.x;
                fix = currPoint.y;
                diff = dx > 0 ? 1 : -1;
                horizontal = true;
            }
            else
            {
                current = currPoint.y;
                target = nextPoint.y;
                diff = dy > 0 ? 1 : -1;
                fix = currPoint.x;
                horizontal = false;
            }

            for(var j = current; j != target; j += diff)
            {
                var xValue = horizontal ? j : fix;
                var yValue = horizontal ? fix : j;

                if (xValue < 0 || xValue >= gameGrid.GetLength(0) || yValue < 0 || yValue >= gameGrid.GetLength(0))
                    continue;

                gameGrid[xValue, yValue] = 2;
            }
        }
    }

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

    public bool TryPlace(int placementX, int placementY)
    {
        var gridX = GetGridX(placementX);
        var gridY = GetGridY(placementY);

        if (gameGrid[gridX, gridY] != 0)
            return false;

        gameGrid[gridX, gridY] = 1;
        return true;
    }

    public bool isOccupied(int queryX, int queryY)
    {
        var gridX = GetGridX(queryX);
        var gridY = GetGridY(queryY);

        if (gridX < 0 || gridX >= width || gridY < 0 || gridY >= height)
            return true;

        return gameGrid[gridX, gridY] != 0;
    }

    public static Vector3 GridToWorldPoint(Vector2 gridPoint) => new Vector3(gridPoint.x - 1 - (width / 2) + 0.5f, gridPoint.y - 1 - (height / 2) + 0.5f, 0);

    public static int GetGridX(int xPos) => xPos + (width / 2) + 1;

    public static int GetGridY(int yPos) => yPos + (height / 2) + 1;
}
