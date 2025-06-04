using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var map = new bool[10, 10];
        for (int x = 0; x < 10; x++)
            for (int y = 0; y < 10; y++)
                map[x, y] = true; // all walkable for now

        var pathfinder = new AStarPathfinder(10, 10, map);
        var planner = new RoutePlanner(pathfinder);

        var passengers = new List<Passenger>
        {
            new Passenger(1, 2, 1, 3),
            new Passenger(1, 1, 8, 8),
            new Passenger(1, 4, 0, 0)
        };

        var start = (0, 0);
        var result = planner.FindBestPath(start, passengers);

        if (result.HasValue)
        {
            var (path, order) = result.Value;
            Console.WriteLine("Passenger Order:");
            foreach (var p in order)
                Console.WriteLine($"{p.label} ({p.pos.x},{p.pos.y})");

            Console.WriteLine("Full Path:");
            foreach (var n in path)
                Console.WriteLine($"({n.X}, {n.Y})");

            // TODO: Move Unity twin and send goals to ROS2
        }
        else
        {
            Console.WriteLine("No valid path found.");
        }
    }
}