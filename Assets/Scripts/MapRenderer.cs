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

            var config = y != height - 1 && gameGrid[x, y+1] != 0 ? "1" : "0";
            config += x != 0 && gameGrid[x-1, y] != 0 ? "1" : "0";
            config += x != width - 1 && gameGrid[x+1, y] != 0 ? "1" : "0";
            config += y != 0 && gameGrid[x, y - 1] != 0 ? "1" : "0";

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
                // 0111 7: left, right, and down. LRDTile
                case "0111":
                    res = TileResourceLoader.GetLRDTile();
                    break;
                // 1011 11: up, right, down. URDTile
                case "1011":
                    res = TileResourceLoader.GetURDTile();
                    break;
                // 1101 13: up, left, and down. LUDTile
                case "1101":
                    res = TileResourceLoader.GetLUDTile();
                    break;
                // 1110 14: up, left, and right. LURTile
                case "1110":
                    res = TileResourceLoader.GetLURTile();
                    break;
                // 1111 15: cross.
                case "1111":
                    res = TileResourceLoader.GetCrossLane();
                    break;
                default:
                    res = TileResourceLoader.GetInvalid();
                    break;
            }

            return res;
        }
    }
}
