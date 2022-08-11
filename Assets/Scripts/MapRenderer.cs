using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    class MapRenderer
    {
        public void RenderMap(Tilemap map, int[,] gameGrid)
        {
            for(var i = 0; i < gameGrid.GetLength(0); i++)
            {
                for(var j = 0; j < gameGrid.GetLength(1); j++)
                {
                    map.SetTile(new Vector3Int(i, j, 0), GetTileForPosition(gameGrid, i, j));
                }
            }
        }

        private Tile GetTileForPosition(int[,] gameGrid, int x, int y)
        {
            var width = gameGrid.GetLength(0);
            var height = gameGrid.GetLength(1);

            if (x >= width || y >= height || gameGrid[x,y] == 0)
                return TileResourceLoader.GetBlankSpace();

            var left = x == 0 ? 0 : gameGrid[x - 1, y];
            var up = y == 0 ? 0 : gameGrid[x, y - 1];
            var right = x == width - 1 ? 0 : gameGrid[x + 1, y];
            var down = y == height - 1 ? 0 : gameGrid[x, y + 1];

            var config = y == 0 ? "0" : gameGrid[x, y+1].ToString();
            config += x == 0 ? "0" : gameGrid[x-1, y].ToString();
            config += x == width - 1 ? "0" : gameGrid[x+1, y].ToString();
            config += y == height - 1 ? "0" : gameGrid[x, y - 1].ToString();

            Debug.Log($"Grid[{x},{y}]: {down}{left}{right}{up}, tile config: {config} ");

            Tile res;

            switch (config)
            {
                // 0011 3: right and down. RDCorner.
                case "0011":
                    res = TileResourceLoader.GetRDCorner();
                    break;
                // 0101 5: left and down. LDCorner.
                case "0101":
                    res = TileResourceLoader.GetLDCorner();
                    break;
                // 0110 6: left and right. Horizontal.
                case "0110":
                    res = TileResourceLoader.GetHorizontal();
                    break;
                // 1001 9: up and below. Vertical.
                case "1001":
                    res = TileResourceLoader.GetVertical();
                    break;
                // 1010 10: up and right. URCorner.
                case "1010":
                    res = TileResourceLoader.GetRUCorner();
                    break;
                // 1100 12: up and left, LUCorner
                case "1100":
                    res = TileResourceLoader.GetLUCorner();
                    break;
                // 0111 7: left, right, and down. LRD, Invalid for now
                // 1011 11: up, right, down. URD, invalid for now
                // 1101 13: up, left, and down. ULD, invalid for now
                // 1110 14: up, left, and right. ULR, invalid for now.
                // 1111 15: cross. invalid for now.
                default:
                    res = TileResourceLoader.GetInvalid();
                    break;
            }

            return res;
        }
    }
}
