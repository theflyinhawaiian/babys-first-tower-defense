using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2Int ToVector(Point p)
        {
            return new Vector2Int(p.X, p.Y);
        }
    }
}