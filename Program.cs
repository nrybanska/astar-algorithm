using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var map = new bool[10, 10];
        for (int x = 0; x < 10; x++)
            for (int y = 0; y < 10; y++)
                map[x, y] = true; // All walkable for now

        var pathfinder = new AStarPathfinder(10, 10, map);
        var planner = new RoutePlanner(pathfinder);

        var passengers = new List<Passenger>();
        var start = (0, 0);

        while (true)
        {
            Console.WriteLine("Enter pickup (x y) and dropoff (x y), or 'exit' (format = ' 1 1 1 1'):");
            var input = Console.ReadLine();

            if (input.ToLower() == "exit") break;

            var tokens = input.Split(' ');
            if (tokens.Length != 4)
            {
                Console.WriteLine("Invalid input.");
                continue;
            }

            var pickup = (int.Parse(tokens[0]), int.Parse(tokens[1]));
            var dropoff = (int.Parse(tokens[2]), int.Parse(tokens[3]));

            var newPassenger = new Passenger(pickup.Item1, pickup.Item2, dropoff.Item1, dropoff.Item2);
            passengers.Add(newPassenger);

            var result = planner.FindBestPath(start, passengers);

            if (result.HasValue)
            {
                var (path, order) = result.Value;

                Console.WriteLine("New passenger added.");
                Console.WriteLine("Updated passenger order:");
                foreach (var p in order)
                    Console.WriteLine($"{p.label} ({p.pos.x},{p.pos.y})");

                Console.WriteLine("Full path:");
                foreach (var n in path)
                    Console.WriteLine($"({n.X}, {n.Y})");
            }
            else
            {
                Console.WriteLine("No valid path found.");
            }
        }
    }
}
