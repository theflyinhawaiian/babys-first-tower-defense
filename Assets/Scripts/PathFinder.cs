using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class PathFinder
    {
        int[,] Grid;
        int OriginX;
        int OriginY;
        int TerminalX;
        int TerminalY;

        public PathFinder(int[,] grid, int startX, int startY, int endX, int endY)
        {
            Grid = grid;
            OriginX = startX;
            OriginY = startY;
            TerminalX = endX;
            TerminalY = endY;
        }

        public async void IllustratePath(MapRenderer renderer)
        {
            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(new Node(OriginX, OriginY, 0));
            Node current = null;

            while (open.Count > 0) {
                current = open[0];

                var neighbors = GetNeighbors(current);

                foreach (var neighbor in neighbors) {
                    if (closed.Contains(neighbor))
                        continue;


                    if (open.Contains(neighbor)) {
                        var openNeighbor = open.FirstOrDefault(x => x == neighbor);
                        if (openNeighbor == null)
                            continue;
                        if (openNeighbor.leastCostFromStart > neighbor.leastCostFromStart)
                            openNeighbor.leastCostFromStart = neighbor.leastCostFromStart;
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

            open.Add(new Node(OriginX, OriginY, 0));
            Node current = null;

            while(open.Count > 0) {
                current = open[0];

                var neighbors = GetNeighbors(current);

                foreach(var neighbor in neighbors) {
                    if (closed.Contains(neighbor))
                        continue;

                    if (open.Contains(neighbor)) {
                        var x = open.First(x => x == neighbor);
                        if (x.leastCostFromStart > neighbor.leastCostFromStart)
                            x.leastCostFromStart = neighbor.leastCostFromStart;
                    }else {
                        open.Add(neighbor);
                    }
                }

                open.Remove(current);
                closed.Add(current);
            }

            var hasValidPath = current != null && current.GetFitness(TerminalX, TerminalY) == current.leastCostFromStart + 1;
            return hasValidPath;
        }

        private List<Node> GetNeighbors(Node curr)
        {
            var nodes = new List<Node>();

            if (curr.xPos > 0 && Grid[curr.xPos - 1, curr.yPos] == 1) {
                nodes.Add(new Node(curr.xPos - 1, curr.yPos, curr.leastCostFromStart + 1));
            }

            if (curr.xPos < Grid.GetLength(0) - 1 && Grid[curr.xPos + 1, curr.yPos] == 1) {
                nodes.Add(new Node(curr.xPos + 1, curr.yPos, curr.leastCostFromStart + 1));
            }

            if (curr.yPos > 0 && Grid[curr.xPos, curr.yPos - 1] == 1) {
                nodes.Add(new Node(curr.xPos, curr.yPos - 1, curr.leastCostFromStart + 1));
            }

            if (curr.yPos < Grid.GetLength(1) - 1 && Grid[curr.xPos, curr.yPos + 1] == 1) {
                nodes.Add(new Node(curr.xPos, curr.yPos + 1, curr.leastCostFromStart + 1));
            }

            return nodes;
        }

        private class Node
        {
            public int xPos;
            public int yPos;
            public float leastCostFromStart;

            public Node(int x, int y, float cost)
            {
                xPos = x;
                yPos = y;
                leastCostFromStart = cost;
            }

            public float GetFitness(int goalX, int goalY) => leastCostFromStart + Vector2.Distance(new Vector2(xPos, yPos), new Vector2(goalX, goalY));

            public override bool Equals(object obj)
            {
                if (!(obj is Node node))
                    return false;

                return xPos == node.xPos && yPos == node.yPos;
            }
        }
    }
}
