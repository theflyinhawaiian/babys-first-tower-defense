using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PathFinder
    {
        int[,] Grid;

        Vector2Int startPos, endPos;

        public PathFinder(int[,] grid, int startX, int startY, int endX, int endY)
        {
            Grid = grid;

            startPos = new Vector2Int(startX, startY);
            endPos = new Vector2Int(endX, endY);
        }

        public async void IllustratePath(MapRenderer renderer)
        {
            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(new Node(startPos.x, startPos.y, 0, Vector2Int.Distance(startPos, endPos)));
            Node current = null;

            while (open.Count > 0) {
                current = open.OrderBy(x => x.totalCost).First();

                if (current.distanceFromGoal == 1)
                    return;

                var neighbors = GetNeighbors(current);

                foreach (var neighbor in neighbors) {
                    if (closed.Contains(neighbor))
                        continue;


                    if (open.Contains(neighbor)) {
                        var openNeighbor = open.FirstOrDefault(x => x == neighbor);
                        if (openNeighbor == null)
                            continue;
                        if (openNeighbor.costFromStart > neighbor.costFromStart)
                            openNeighbor.costFromStart = neighbor.costFromStart;
                    } else {
                        open.Add(neighbor);
                        renderer.ColorTile(neighbor.xPos, neighbor.yPos, MapColor.Green);
                    }
                }

                open.Remove(current);
                closed.Add(current);
                renderer.ColorTile(current.xPos, current.yPos, MapColor.Orange);

                await Task.Delay(10);
            }
        }

        public bool HasValidPath()
        {
            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(new Node(startPos.x, startPos.y, 0, Vector2Int.Distance(startPos, endPos)));
            Node current = null;

            while(open.Count > 0) {
                current = open[0];

                var neighbors = GetNeighbors(current);

                foreach(var neighbor in neighbors) {
                    if (closed.Contains(neighbor))
                        continue;

                    if (open.Contains(neighbor)) {
                        var x = open.First(x => x == neighbor);
                        if (x.costFromStart > neighbor.costFromStart)
                            x.costFromStart = neighbor.costFromStart;
                    }else {
                        open.Add(neighbor);
                    }
                }

                open.Remove(current);
                closed.Add(current);
            }

            var hasValidPath = current != null && current.totalCost == current.costFromStart + 1;
            return hasValidPath;
        }

        private List<Node> GetNeighbors(Node curr)
        {
            var nodes = new List<Node>();

            if (curr.xPos > 0 && Grid[curr.xPos - 1, curr.yPos] == 1) {
                nodes.Add(new Node(curr.xPos - 1, curr.yPos, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos - 1, curr.yPos), endPos)));
            }

            if (curr.xPos < Grid.GetLength(0) - 1 && Grid[curr.xPos + 1, curr.yPos] == 1) {
                nodes.Add(new Node(curr.xPos + 1, curr.yPos, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos + 1, curr.yPos), endPos)));
            }

            if (curr.yPos > 0 && Grid[curr.xPos, curr.yPos - 1] == 1) {
                nodes.Add(new Node(curr.xPos, curr.yPos - 1, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos, curr.yPos - 1), endPos)));
            }

            if (curr.yPos < Grid.GetLength(1) - 1 && Grid[curr.xPos, curr.yPos + 1] == 1) {
                nodes.Add(new Node(curr.xPos, curr.yPos + 1, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos, curr.yPos + 1), endPos)));
            }

            return nodes;
        }

        private class Node
        {
            public int xPos;
            public int yPos;
            public float costFromStart;
            public float distanceFromGoal;

            public float totalCost => costFromStart + distanceFromGoal;

            public Node(int x, int y, float cost, float distFromGoal)
            {
                xPos = x;
                yPos = y;
                costFromStart = cost;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Node node))
                    return false;

                return xPos == node.xPos && yPos == node.yPos;
            }
        }
    }
}
