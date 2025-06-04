using System;
using System.Collections.Generic;

public class AStarPathfinder
{
    private int width, height;
    private Node[,] grid;

    public AStarPathfinder(int width, int height, bool[,] walkableMap)
    {
        this.width = width;
        this.height = height;
        grid = new Node[width, height];
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y] = new Node(x, y, walkableMap[x, y]);
    }

    public List<Node> FindPath((int x, int y) start, (int x, int y) goal)
    {
        var startNode = grid[start.x, start.y];
        var goalNode = grid[goal.x, goal.y];
        var open = new List<Node> { startNode };
        var closed = new HashSet<Node>();

        foreach (var node in grid)
        {
            node.G = node.H = 0;
            node.Parent = null;
        }

        while (open.Count > 0)
        {
            open.Sort((a, b) => a.F.CompareTo(b.F));
            var current = open[0];
            if (current.Equals(goalNode))
                return Reconstruct(current);

            open.Remove(current);
            closed.Add(current);

            foreach (var neighbor in GetNeighbors(current))
            {
                if (!neighbor.Walkable || closed.Contains(neighbor)) continue;
                int tentativeG = current.G + 1;
                if (tentativeG < neighbor.G || !open.Contains(neighbor))
                {
                    neighbor.G = tentativeG;
                    neighbor.H = Math.Abs(neighbor.X - goalNode.X) + Math.Abs(neighbor.Y - goalNode.Y);
                    neighbor.Parent = current;
                    if (!open.Contains(neighbor)) open.Add(neighbor);
                }
            }
        }

        return null;
    }

    private List<Node> Reconstruct(Node end)
    {
        var path = new List<Node>();
        for (var n = end; n != null; n = n.Parent)
            path.Add(n);
        path.Reverse();
        return path;
    }

    private IEnumerable<Node> GetNeighbors(Node node)
    {
        int[] dx = { 0, 1, 0, -1 };
        int[] dy = { 1, 0, -1, 0 };

        for (int i = 0; i < 4; i++)
        {
            int nx = node.X + dx[i];
            int ny = node.Y + dy[i];
            if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                yield return grid[nx, ny];
        }
    }
}