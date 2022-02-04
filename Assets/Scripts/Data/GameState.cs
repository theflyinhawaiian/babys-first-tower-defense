using System.Collections.Generic;
using UnityEngine;

public class GameState
{
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
}
