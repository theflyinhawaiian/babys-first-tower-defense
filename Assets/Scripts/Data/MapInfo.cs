using System;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class MapInfo
    {
        public Point[] Waypoints;
        public int Height;
        public int Width;
    }
}
