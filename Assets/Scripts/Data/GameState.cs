using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static int height = 31;
    public static int width = 53;

    public List<Transform> LevelWaypoints { get; private set; }
    public List<float> distances { get; private set; }

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

    public static Vector3 GridToWorldPoint(Vector2 gridPoint) => new Vector3(gridPoint.x - (width / 2) + 0.5f, gridPoint.y - (height / 2) + 0.5f, 0);

    public static int GetGridX(int xPos) => xPos + (width / 2) + 1;

    public static int GetGridY(int yPos) => yPos + (height / 2) + 1;
}
