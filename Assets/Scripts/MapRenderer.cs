using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public enum MapColor
    {
        Yellow, Orange, Green, Blue
    }

    class MapRenderer
    {
        Tilemap Map;

        public MapRenderer(Tilemap map) => Map = map;

        public void RenderMap(int[,] gameGrid)
        {
            for (var i = 0; i < gameGrid.GetLength(0); i++) {
                for (var j = 0; j < gameGrid.GetLength(1); j++) {
                    Map.SetTile(new Vector3Int(i, j, 0), GetTileForPosition(gameGrid, i, j));
                }
            }
        }

        private Tile GetTileForPosition(int[,] gameGrid, int x, int y)
        {
            var width = gameGrid.GetLength(0);
            var height = gameGrid.GetLength(1);

            if (x >= width || y >= height || gameGrid[x, y] == 0)
                return TileResourceLoader.GetBlankSpace();

            var config = y != height - 1 && gameGrid[x, y + 1] != 0 ? "1" : "0";
            config += x != 0 && gameGrid[x - 1, y] != 0 ? "1" : "0";
            config += x != width - 1 && gameGrid[x + 1, y] != 0 ? "1" : "0";
            config += y != 0 && gameGrid[x, y - 1] != 0 ? "1" : "0";

            Tile res;

            switch (config) {
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

        public void ColorTile(int gridX, int gridY, MapColor color)
        {
            Tile tile;

            switch (color) {
                case MapColor.Blue:
                    tile = TileResourceLoader.GetBlue();
                    break;
                case MapColor.Green:
                    tile = TileResourceLoader.GetGreen();
                    break;
                case MapColor.Yellow:
                    tile = TileResourceLoader.GetYellow();
                    break;
                case MapColor.Orange:
                    tile = TileResourceLoader.GetOrange();
                    break;
                default:
                    tile = TileResourceLoader.GetInvalid();
                    break;
            }

            Map.SetTile(new Vector3Int(gridX, gridY, 0), tile);
        }
    }
}
