using System;
using System.Collections.Generic;
using System.Linq;

public class RoutePlanner
{
    private AStarPathfinder pathfinder;

    public RoutePlanner(AStarPathfinder pathfinder)
    {
        this.pathfinder = pathfinder;
    }

    public (List<Node> path, List<(string label, (int x, int y) pos)> stops)? FindBestPath((int x, int y) start, List<Passenger> passengers)
    {
        var allStops = new List<(string label, (int x, int y) pos)>();
        foreach (var (p, i) in passengers.Select((p, i) => (p, i)))
        {
            allStops.Add(($"P{i}", p.Pickup));
            allStops.Add(($"D{i}", p.Dropoff));
        }

        var validOrders = GenerateValidStopPermutations(passengers.Count, allStops);
        List<Node> bestPath = null;
        List<(string label, (int x, int y) pos)> bestOrder = null;
        int bestCost = int.MaxValue;

        foreach (var sequence in validOrders)
        {
            var fullStops = new List<(int x, int y)> { start };
            fullStops.AddRange(sequence.Select(s => s.pos));

            List<Node> totalPath = new();
            int totalCost = 0;
            bool failed = false;

            for (int i = 0; i < fullStops.Count - 1; i++)
            {
                var segment = pathfinder.FindPath(fullStops[i], fullStops[i + 1]);
                if (segment == null) { failed = true; break; }
                totalPath.AddRange(segment.Skip(i == 0 ? 0 : 1));
                totalCost += segment.Count;
            }

            if (!failed && totalCost < bestCost)
            {
                bestPath = totalPath;
                bestOrder = sequence;
                bestCost = totalCost;
            }
        }

        return bestPath == null ? null : (bestPath, bestOrder);
    }

    private List<List<(string label, (int x, int y) pos)>> GenerateValidStopPermutations(int passengerCount, List<(string label, (int x, int y) pos)> allStops)
    {
        var result = new List<List<(string label, (int x, int y) pos)>>();
        void Permute(List<(string label, (int x, int y) pos)> current, List<(string label, (int x, int y) pos)> remaining)
        {
            if (remaining.Count == 0)
            {
                if (IsValidSequence(current)) result.Add(new List<(string, (int, int))>(current));
                return;
            }

            for (int i = 0; i < remaining.Count; i++)
            {
                var item = remaining[i];
                current.Add(item);
                var next = new List<(string, (int, int))>(remaining);
                next.RemoveAt(i);
                Permute(current, next);
                current.RemoveAt(current.Count - 1);
            }
        }

        Permute(new List<(string, (int, int))>(), allStops);
        return result;
    }

    private bool IsValidSequence(List<(string label, (int x, int y) pos)> sequence)
    {
        var pickedUp = new HashSet<int>();
        foreach (var stop in sequence)
        {
            if (stop.label.StartsWith("P"))
                pickedUp.Add(int.Parse(stop.label.Substring(1)));
            else if (stop.label.StartsWith("D"))
            {
                if (!pickedUp.Contains(int.Parse(stop.label.Substring(1))))
                    return false;
            }
        }
        return true;
    }
}