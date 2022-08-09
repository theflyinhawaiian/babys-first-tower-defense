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

            var config = y == 0 ? 0 : gameGrid[x, y-1];
            config += x == 0 ? 0 : gameGrid[x-1, y] << 1;
            config += x == width - 1 ? 0 : gameGrid[x+1, y] << 1;
            config += y == height - 1 ? 0 : gameGrid[x, y + 1];

            Debug.Log($"Grid[{x},{y}]: {gameGrid[x, y]}, tile config: {config} ");

            switch (config)
            {
                // 0011 3: right and down. RDCorner.
                case 3:
                    return TileResourceLoader.GetRDCorner();
                // 0101 5: left and down. LDCorner.
                case 5:
                    return TileResourceLoader.GetLDCorner();
                // 0110 6: left and right. Horizontal.
                case 6:
                    return TileResourceLoader.GetHorizontal();
                // 1001 9: up and below. Vertical.
                case 9:
                    return TileResourceLoader.GetVertical();
                // 1010 10: up and right. URCorner.
                case 10:
                    return TileResourceLoader.GetRUCorner();
                // 1100 12: up and left, LUCorner
                case 12: 
                    return TileResourceLoader.GetLUCorner();
                // 0111 7: left, right, and down. LRD, Invalid for now
                // 1011 11: up, right, down. URD, invalid for now
                // 1101 13: up, left, and down. ULD, invalid for now
                // 1110 14: up, left, and right. ULR, invalid for now.
                // 1111 15: cross. invalid for now.
                default:
                    return TileResourceLoader.GetInvalid();
            }
        }
    }
}
