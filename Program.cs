using System;
using System.Collections.Generic;
using System.Linq;

class Program {
    static void Main(string[] args)
    {
        // Create a 10x10 grid where all cells are walkable
        var map = new bool[10, 10];
        for (int x = 0; x < 10; x++)
            for (int y = 0; y < 10; y++)
                map[x, y] = true;

        var astar = new AStar(10, 10, map);

        // Define start and passengers
        var start = (0, 0);
        var passengers = new List<(int, int)> {
            (2, 3),
            (5, 1),
            (8, 8)
        };

        // Run the algorithm
        var result = astar.FindBestVisitOrder(start, passengers);

        if (result != null)
        {
            var (path, passengerOrder) = result.Value;

            Console.WriteLine("Visit passengers in this order:");
            foreach (var passenger in passengerOrder)
                Console.WriteLine($"Passenger at: ({passenger.x}, {passenger.y})");

            Console.WriteLine("\nFull path to walk:");
            foreach (var node in path)
                Console.WriteLine($"({node.X}, {node.Y})");
        }
        else
        {
            Console.WriteLine("No valid path found.");
        }
    }
}
