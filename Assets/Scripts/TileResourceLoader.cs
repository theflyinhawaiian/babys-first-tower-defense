using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public static class TileResourceLoader
    {
        private const string horizontal = "0110";
        private const string vertical = "1001";
        private const string blankSpace = "0000";
        private const string crossLane = "1111";
        private const string LUCorner = "1100";
        private const string LDCorner = "0101";
        private const string RUCorner = "1010";
        private const string RDCorner = "0011";
        private const string LRDTile = "0111";
        private const string LUDTile = "1101";
        private const string URDTile = "1011";
        private const string LURTile = "1110";
        private const string Invalid = "invalid";
        private const string Blue = "blue";
        private const string Green = "green";
        private const string Orange = "orange";
        private const string Yellow = "yellow";

        public static Tile GetBlankSpace() => GetTileByName(blankSpace);
        public static Tile GetHorizontal() => GetTileByName(horizontal);
        public static Tile GetVertical() => GetTileByName(vertical);
        public static Tile GetCrossLane() => GetTileByName(crossLane);
        public static Tile GetLUCorner() => GetTileByName(LUCorner);
        public static Tile GetLDCorner() => GetTileByName(LDCorner);
        public static Tile GetRUCorner() => GetTileByName(RUCorner);
        public static Tile GetRDCorner() => GetTileByName(RDCorner);
        public static Tile GetLRDTile() => GetTileByName(LRDTile);
        public static Tile GetLUDTile() => GetTileByName(LUDTile);
        public static Tile GetURDTile() => GetTileByName(URDTile);
        public static Tile GetLURTile() => GetTileByName(LURTile);
        public static Tile GetInvalid() => GetTileByName(Invalid);
        public static Tile GetBlue() => GetTileByName(Blue);
        public static Tile GetGreen() => GetTileByName(Green);
        public static Tile GetOrange() => GetTileByName(Orange);
        public static Tile GetYellow() => GetTileByName(Yellow);

        private static Tile GetTileByName(string name) => Resources.Load($"Tiles/{name}", typeof(Tile)) as Tile;

    }
}
