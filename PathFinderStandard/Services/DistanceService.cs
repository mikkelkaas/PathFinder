using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using PathFinderStandard.Models;

namespace PathFinderStandard.Services
{
    public class DistanceService
    {
        private readonly List<Checkpoint> _checkpoints;

        private readonly Dictionary<Tuple<Checkpoint, Checkpoint>, double> _distances =
            new Dictionary<Tuple<Checkpoint, Checkpoint>, double>();

        public int Calculations;

        public DistanceService(List<Checkpoint> checkpoints, Checkpoint homebase)
        {
            Homebase = homebase;
            _checkpoints = checkpoints;
            _checkpoints.Add(Homebase);
            foreach (var firstCheckpoint in checkpoints)
            foreach (var secondCheckpoint in checkpoints)
            {
                if (firstCheckpoint == secondCheckpoint)
                    continue;
                CalculateDistance(firstCheckpoint, secondCheckpoint);
            }
        }

        public Checkpoint Homebase { get; }


        public void CalculateDistance(Checkpoint cp1, Checkpoint cp2)
        {
            var tupleToCheckInverted = new Tuple<Checkpoint, Checkpoint>(cp2, cp1);
            if (_distances.ContainsKey(tupleToCheckInverted))
                return;

            var distance = cp1.Coordinate.GetDistanceTo(cp2.Coordinate);
            Calculations++;
            _distances.Add(new Tuple<Checkpoint, Checkpoint>(cp1, cp2), distance);
        }

        public double GetDistance(Checkpoint cp1, Checkpoint cp2)
        {
            var tupleToCheck = new Tuple<Checkpoint, Checkpoint>(cp1, cp2);
            var tupleToCheckInverted = new Tuple<Checkpoint, Checkpoint>(cp2, cp1);
            var dist = _distances.ContainsKey(tupleToCheck)
                ? _distances[tupleToCheck]
                : _distances[tupleToCheckInverted];
            return dist;
        }

        public Route GetRoute(int length)
        {

            List<Checkpoint> cpsToUseForRoute = new List<Checkpoint> {Homebase};
            cpsToUseForRoute.AddRange(_checkpoints.Where(cp => cp.IsHomeBase == false).OrderBy(x => Guid.NewGuid()).Take(length)
                .ToList());
            cpsToUseForRoute.Add(Homebase);
            double totalDistance = 0;
            for (var i = 0; i < cpsToUseForRoute.Count - 1; i++)
                totalDistance = totalDistance + GetDistance(cpsToUseForRoute[i], cpsToUseForRoute[i + 1]);
            
            return new Route(cpsToUseForRoute, totalDistance);
        }
    }

    public class RouteComparer : Comparer<Route>
    {
        public override int Compare(Route x, Route y)
        {
            if (x == null) return -1;
            if (y == null) return 1;

            if (x.TotalDistance == y.TotalDistance) return 0;

            return x.TotalDistance > y.TotalDistance ? 1 : -1;
        }
    }
    public class Route
    {
        public Route(List<Checkpoint> checkpoints, double totalDistance) 
        {
            Checkpoints = checkpoints;
            TotalDistance = totalDistance;
        }

        public List<Checkpoint> Checkpoints { get; }
        public double TotalDistance { get; }
    }
}