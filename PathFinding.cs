using System;
using System.Collections.Generic;

public class AStar {
    private int width, height;
    private Node[,] grid;

    public AStar(int width, int height, bool[,] walkableMap) {
        this.width = width;
        this.height = height;
        grid = new Node[width, height];
        
        for (int x = 0; x < width; x++) {

            for (int y = 0; y < height; y++) {
                grid[x, y] = new Node(x, y, walkableMap[x, y]);
            }

        }
    }

    private int Heuristic(Node a, Node b) {
        // Manhattan distance
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    private IEnumerable<Node> GetNeighbors(Node node) {
        int[] dx = { -1, 1, 0, 0 };
        int[] dy = { 0, 0, -1, 1 };

        for (int i = 0; i < 4; i++) {
            int nx = node.X + dx[i];
            int ny = node.Y + dy[i];

            if (nx >= 0 && nx < width && ny >= 0 && ny < height && grid[nx, ny].Walkable)
                yield return grid[nx, ny];
        }
    }

    public List<Node> FindPath((int x, int y) start, (int x, int y) end) {
        var openSet = new List<Node>();
        var closedSet = new HashSet<Node>();

        Node startNode = grid[start.x, start.y];
        Node endNode = grid[end.x, end.y];

        openSet.Add(startNode);
        startNode.G = 0;
        startNode.H = Heuristic(startNode, endNode);
        startNode.Parent = null;

        while (openSet.Count > 0) {
            openSet.Sort((a, b) => a.F.CompareTo(b.F));
            Node current = openSet[0];
            
            if (current == endNode) {
                // Reconstruct path
                var path = new List<Node>();

                while (current != null) {
                    path.Add(current);
                    current = current.Parent;
                }

                path.Reverse();
                return path;
            }
            
            openSet.Remove(current);
            closedSet.Add(current);
            
            foreach (var neighbor in GetNeighbors(current)) {
                if (closedSet.Contains(neighbor))
                    continue;

                int tentativeG = current.G + 1;

                if (!openSet.Contains(neighbor) || tentativeG < neighbor.G)
                {
                    neighbor.Parent = current;
                    neighbor.G = tentativeG;
                    neighbor.H = Heuristic(neighbor, endNode);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return null; // No path found
    }
}
