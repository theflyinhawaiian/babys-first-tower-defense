using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class GameMap : ISerializationCallbackReceiver
    {
        public int[,] Grid;
        public int Height, Width;

        // Unity's serializer is annoying: https://forum.unity.com/threads/how-i-serialize-multidimensional-arrays.988704/
        [SerializeField, HideInInspector] private List<SerializablePackage<int>> serializable;

        [Serializable]
        struct SerializablePackage<TVal>
        {
            public int X;
            public int Y;
            public TVal Value;
            public SerializablePackage(int x, int y, TVal val)
            {
                X = x;
                Y = y;
                Value = val;
            }
        }
        public void OnBeforeSerialize()
        {
            serializable = new List<SerializablePackage<int>>();
            for (int i = 0; i < Grid.GetLength(0); i++) {
                for (int j = 0; j < Grid.GetLength(1); j++) {
                    serializable.Add(new SerializablePackage<int>(i, j, Grid[i, j]));
                }
            }
        }

        public void OnAfterDeserialize()
        {
            Grid = new int[Width, Height];
            foreach (var package in serializable) {
                Grid[package.X, package.Y] = package.Value;
            }
        }

        public bool Validate()
        {
            var start = new Vector2Int(-1, -1);
            var end = new Vector2Int(-1, -1);

            for (var i = 0; i < Grid.GetLength(0); i++) {
                for (var j = 0; j < Grid.GetLength(1); j++) {
                    if(Grid[i,j] == 3) {
                        start.x = i;
                        start.y = j;
                    }else if(Grid[i,j] == 4) {
                        end.x = i;
                        end.y = j;
                    }
                }
            }

            if (start.x == -1 || start.y == -1 || end.x == -1 || end.y == -1)
                return false;

            var pathFinder = new PathFinder(Grid, start, end);

            return pathFinder.HasValidPath();
        }
    }
}
