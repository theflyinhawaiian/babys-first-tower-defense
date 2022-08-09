using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public static class TileResourceLoader
    {
        private const string horizontal = "horizontal_lane";
        private const string vertical = "vertical_lane";
        private const string blankSpace = "blank_space";
        private const string crossLane = "cross_lane";
        private const string LUCorner = "lu_lane";
        private const string LDCorner = "ld_lane";
        private const string RUCorner = "ru_lane";
        private const string RDCorner = "rd_lane";
        private const string Invalid = "invalid";

        public static Tile GetBlankSpace() => GetTileByName(blankSpace);
        public static Tile GetHorizontal() => GetTileByName(horizontal);
        public static Tile GetVertical() => GetTileByName(vertical);
        public static Tile GetCrossLane() => GetTileByName(crossLane);
        public static Tile GetLUCorner() => GetTileByName(LUCorner);
        public static Tile GetLDCorner() => GetTileByName(LDCorner);
        public static Tile GetRUCorner() => GetTileByName(RUCorner);
        public static Tile GetRDCorner() => GetTileByName(RDCorner);
        public static Tile GetInvalid() => GetTileByName(Invalid);

        private static Tile GetTileByName(string name) => Resources.Load(name, typeof(Tile)) as Tile;
    }
}
