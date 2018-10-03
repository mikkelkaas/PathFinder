using System;
using System.Collections.Generic;
using System.Linq;
using PathFinderStandard.Models;
using PathFinderStandard.Services;

namespace PathFinderStandard
{
    internal class Program
    {
        private static readonly Random random = new Random();
        private static DistanceService _distanceService;

        private static void Main(string[] args)
        {
            var routesToGenerate = 5000; //The bigger the number, the closer to the desired distance the routes will be.
            var targetDistanceInMeters = 3000; //How long should the routes be from Homebase, to checkpoints and back to Homebase again
            var routesToFind = 6; //How many routes should be found?
            var checkpointsPerRoute = 5; //Checkpoints per route
            var cps = GetCheckpoints();
            var homebase = new Checkpoint(56.169117, 10.101123, "Homebase", true);

            if (routesToGenerate < routesToFind)
            {
                routesToFind = routesToGenerate;
            }

            _distanceService = new DistanceService(cps, homebase);
            Console.WriteLine($"Made {_distanceService.Calculations} calculations");
            
            var orderedRoutes = GenerateRoutes(routesToGenerate, checkpointsPerRoute).OrderBy(r => r.TotalDistance).ToList();

            var indexResult = orderedRoutes.BinarySearch(new Route(new List<Checkpoint>(), targetDistanceInMeters),
                new RouteComparer());
            if (indexResult < 0) indexResult = ~indexResult;

            foreach (var route in GetRoutes(orderedRoutes, indexResult, routesToFind))
                PrintRoute(route);


            Console.ReadKey();
        }

        private static List<Route> GenerateRoutes(int routesToGenerate, int checkpointsPerRoute)
        {
            List<Route> routes = new List<Route>();
            for (var i = 0; i < routesToGenerate; i++)
                routes.Add(_distanceService.GetRoute(checkpointsPerRoute));
            return routes;
        }

        private static List<Checkpoint> GetCheckpoints()
        {
            var returnList = new List<Checkpoint>();
            for (var i = 0; i < 42; i++)
                returnList.Add(new Checkpoint(RandomNumberBetween(56.1694925, 56.172670),
                    RandomNumberBetween(10.092656, 10.109312), i.ToString(), false));
            return returnList;
        }

        private static List<Route> GetRoutes(List<Route> orderedRoutes, int indexToTakeFrom, int routesToTake)
        {
            var finalRoutes = new List<Route>();
            //Check if there are enough routes
            if (indexToTakeFrom + 1 + routesToTake > orderedRoutes.Count)
            {
                //Not enough routes... Take the biggest ones then
                indexToTakeFrom = orderedRoutes.Count - routesToTake;
            }
            for (var i = 0; i < routesToTake; i++)
                finalRoutes.Add(orderedRoutes[indexToTakeFrom + i]);
            return finalRoutes;
        }

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + next * (maxValue - minValue);
        }

        private static void PrintRoute(Route route)
        {
            Console.WriteLine("The route consists of:");
            for (var i = 0; i < route.Checkpoints.Count - 1; i++)
                Console.WriteLine(
                    $"    CP {route.Checkpoints[i].Id} CP {route.Checkpoints[i + 1].Id}. Distance: {Math.Round(_distanceService.GetDistance(route.Checkpoints[i], route.Checkpoints[i + 1]) ,2)} m");
            Console.WriteLine($"Total Distance: {Math.Round(route.TotalDistance / 1000,2)} km");
        }
    }
}