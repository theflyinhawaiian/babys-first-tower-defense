using Assets.Scripts.Data;
using Assets.Scripts.Util;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class LevelEditorManager : MonoBehaviour
    {
        public Tilemap Tilemap;

        MapRenderer renderer;

        EditMode currentMode;

        private int[,] Grid;

        Vector2Int start, end;

        void Start()
        {
            Grid = new int[GameConstants.Width, GameConstants.Height];

            start = new Vector2Int(-1, -1);
            end = new Vector2Int(-1, -1);

            for (var i = 0; i < GameConstants.Height; i++) {
                for(var j = 0; j < GameConstants.Width; j++) {
                    Grid[j, i] = 0;
                }
            }

            renderer = new MapRenderer(Tilemap);
            renderer.RenderMap(Grid);
        }

        public void SetGridValue(int x, int y)
        {
            var val = GetValue(currentMode);

            if (Grid[x, y] == val)
                return;

            if (currentMode == EditMode.Start) {
                ReplaceStart(x, y);
            }else if (currentMode == EditMode.Finish) {
                ReplaceEnd(x, y);
            }

            Debug.Log($"Setting value at ({x}, {y}) => {currentMode}");
            Grid[x, y] = val;

            renderer.RenderMap(Grid);
        }

        private void ReplaceStart(int x, int y)
        {
            if (start.x != -1 && start.y != -1) {
                Grid[start.x, start.y] = 0;
            }

            start.x = x;
            start.y = y;
        }

        private void ReplaceEnd(int x, int y)
        {
            if (end.x != -1 && end.y != -1) {
                Grid[end.x, end.y] = 0;
            }

            end.x = x;
            end.y = y;
        }

        private int GetValue(EditMode mode) =>
            mode switch
            {
                EditMode.Path => 1,
                EditMode.Start => 3,
                EditMode.Finish => 4,
                _ => 0,
            };

        public void TestPathfinding()
        {
            if(start.x == -1 || start.y == -1 || end.x == -1 || end.y == -1) {
                Debug.Log("Can't start pathfinding -- end or beginning not set");
                return;
            }

            var pathfinder = new PathFinder(Grid, start, end)
            {
                renderer = renderer
            };
            Task.Run(async () => await pathfinder.IllustrateAStar());
        }

        public void SetModeClear() => currentMode = EditMode.Clear;

        public void SetModePath() => currentMode = EditMode.Path;

        public void SetModeStart() => currentMode = EditMode.Start;

        public void SetModeFinish() => currentMode = EditMode.Finish;

        public void SaveMap()
        {
            var map = new GameMap
            {
                Grid = Grid,
                Height = GameConstants.Height,
                Width = GameConstants.Width
            };
            FileHandler.SaveToJSON(map, $"map-{DateTime.Now.ToString("dd-mm-yy")}");
        }

        private enum EditMode
        {
            Clear,
            Path,
            Start,
            Finish
        }
    }

}