using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class LevelEditorManager : MonoBehaviour
    {
        public Tilemap Tilemap;

        private int[,] Grid;

        void Start()
        {
            Grid = new int[GameConstants.Width, GameConstants.Height];

            for(var i = 0; i < GameConstants.Height; i++) {
                for(var j = 0; j < GameConstants.Width; j++) {
                    Grid[j, i] = 0;
                }
            }

            var renderer = new MapRenderer();
            renderer.RenderMap(Tilemap, Grid);
        }
    }

}