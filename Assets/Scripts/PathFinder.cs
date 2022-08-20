using Assets.Scripts.Data;
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

        public MapRenderer renderer;

        public PathFinder(int[,] grid, Vector2Int start, Vector2Int end)
        {
            Grid = grid;

            startPos = start;
            endPos = end;
        }

        public async Task IllustrateAStar()
        {
            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(new Node(null, startPos.x, startPos.y, 0, Vector2Int.Distance(startPos, endPos)));
            Node current = null;

            while (open.Count > 0) {
                current = open.OrderBy(x => x.totalCost).First();

                if (current.distanceFromGoal == 1) {
                    await IllustratePath(current, renderer);
                    return;
                }

                var neighbors = GetNeighbors(current);

                foreach (var neighbor in neighbors) {
                    if (closed.Contains(neighbor))
                        continue;

                    if (Equals(new Vector2Int(current.xPos, current.yPos), endPos)) {
                        return;
                    }

                    if (open.Contains(neighbor)) {
                        var openNeighbor = open.FirstOrDefault(x => x == neighbor);
                        if (openNeighbor == null)
                            continue;
                        if (openNeighbor.costFromStart > neighbor.costFromStart) {
                            openNeighbor.costFromStart = neighbor.costFromStart; 
                            openNeighbor.parent = current;
                        }
                    } else {
                        open.Add(neighbor);
                        renderer.ColorTile(neighbor.xPos, neighbor.yPos, MapColor.Green);
                    }
                }

                open.Remove(current);
                closed.Add(current);
                renderer.ColorTile(current.xPos, current.yPos, MapColor.Orange);

                await Task.Delay(20);
            }
        }

        async Task IllustratePath(Node node, MapRenderer renderer)
        {
            var path = GetPath(node);

            foreach(var coord in path) {
                renderer.ColorTile(coord.X, coord.Y, MapColor.Blue);
                await Task.Delay(20);
            }
        }

        public List<Point> FindPath()
        {
            var open = new List<Node>();
            var closed = new List<Node>();

            open.Add(new Node(null, startPos.x, startPos.y, 0, Vector2Int.Distance(startPos, endPos)));
            Node current = null;

            while (open.Count > 0) {
                current = open.OrderBy(x => x.totalCost).First();

                if (current.distanceFromGoal == 0) {
                    return GetPath(current);
                }

                var neighbors = GetNeighbors(current);

                foreach (var neighbor in neighbors) {
                    if (closed.Contains(neighbor))
                        continue;

                    if (open.Contains(neighbor)) {
                        var openNeighbor = open.FirstOrDefault(x => x == neighbor);
                        if (openNeighbor == null)
                            continue;
                        if (openNeighbor.costFromStart > neighbor.costFromStart) {
                            openNeighbor.costFromStart = neighbor.costFromStart;
                            openNeighbor.parent = current;
                        }
                    } else {
                        open.Add(neighbor);
                    }
                }

                open.Remove(current);
                closed.Add(current);
            }

            return new List<Point>();
        }

        List<Point> GetPath(Node endpoint)
        {
            var path = new List<Point>();

            var currNode = endpoint;

            while (currNode != null) {
                path.Add(new Point(currNode.xPos, currNode.yPos));
                currNode = currNode.parent;
            }

            path.Reverse();

            var compressedPath = new List<Point>();
            var diffVector = Vector2Int.zero;
            var idx = 1;
            var curr = Point.ToVector(path[0]);
            while(idx < path.Count) {
                var next = Point.ToVector(path[idx]);
                var diff = curr - next;

                if (diffVector != diff) {
                    compressedPath.Add(path[idx-1]);
                    diffVector = diff;
                }

                curr = next;
                idx++;
            }

            compressedPath.Add(path.Last());

            return compressedPath;
        }

        public bool HasValidPath() => FindPath().Count > 0;

        private List<Node> GetNeighbors(Node curr)
        {
            var nodes = new List<Node>();

            if (curr.xPos > 0 && IsWalkable(Grid[curr.xPos - 1, curr.yPos])) {
                nodes.Add(new Node(curr, curr.xPos - 1, curr.yPos, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos - 1, curr.yPos), endPos)));
            }

            if (curr.xPos < Grid.GetLength(0) - 1 && IsWalkable(Grid[curr.xPos + 1, curr.yPos])) {
                nodes.Add(new Node(curr, curr.xPos + 1, curr.yPos, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos + 1, curr.yPos), endPos)));
            }

            if (curr.yPos > 0 && IsWalkable(Grid[curr.xPos, curr.yPos - 1])) {
                nodes.Add(new Node(curr, curr.xPos, curr.yPos - 1, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos, curr.yPos - 1), endPos)));
            }

            if (curr.yPos < Grid.GetLength(1) - 1 && IsWalkable(Grid[curr.xPos, curr.yPos + 1])) {
                nodes.Add(new Node(curr, curr.xPos, curr.yPos + 1, curr.costFromStart + 1, Vector2Int.Distance(new Vector2Int(curr.xPos, curr.yPos + 1), endPos)));
            }

            return nodes;
        }

        private bool IsWalkable(int positionValue) => positionValue == 1 || positionValue == 3 || positionValue == 4;

        private class Node
        {
            public int xPos;
            public int yPos;
            public float costFromStart;
            public float distanceFromGoal;
            public Node parent;

            public float totalCost => costFromStart + distanceFromGoal;

            public Node(Node parent, int x, int y, float cost, float distFromGoal)
            {
                this.parent = parent;
                xPos = x;
                yPos = y;
                costFromStart = cost;
                distanceFromGoal = distFromGoal;
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
