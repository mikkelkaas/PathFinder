using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using PathFinderStandard.Models;
using PathFinderStandard.Services;

namespace PathFinderStandard
{
    internal class Program
    {
        private static readonly Random random = new Random();
        private static DistanceService _distanceService;
        private static int _checkpointsetidcounter = 1;

        private static void Main(string[] args)
        {

            var routesToGenerate = 5000; //The bigger the number, the closer to the desired distance the routes will be.
            var targetDistanceInMeters = 3000; //How long should the routes be from Homebase, to checkpoints and back to Homebase again
            var routesToFind = 40; //How many routes should be found?
            var checkpointsPerRoute = 5; //Checkpoints per route
            var cps = GetCheckpoints();
            var checkpointsetidcounter = 1;
            var homebase = new Checkpoint(56.435056, 12.838587, "Homebase", true);

            if (routesToGenerate < routesToFind)
            {
                routesToFind = routesToGenerate;
            }

            _distanceService = new DistanceService(cps, homebase);
            Console.WriteLine($"Made {_distanceService.Calculations} calculations");
            List<Route> finalRoutes = new List<Route>();
            for (int i = 0; i < routesToFind; i++)
            {
                if (_distanceService.Checkpoints.Count(cp => cp.IsHomeBase == false) < checkpointsPerRoute)
                {
                    _distanceService.Checkpoints = GetCheckpoints();
                }
                var orderedRoutes = GenerateRoutes(routesToGenerate, checkpointsPerRoute).OrderBy(r => r.TotalDistance).ToList();
                var indexResult = orderedRoutes.BinarySearch(new Route(new List<Checkpoint>(), targetDistanceInMeters),
                    new RouteComparer());
                if (indexResult < 0) indexResult = ~indexResult;
                var foundRoute = GetBestRoute(orderedRoutes, indexResult);
                foreach (var checkpoint in foundRoute.Checkpoints.Where(cp=>cp.IsHomeBase != true)){               
                        var itemToRemove = _distanceService.Checkpoints.Single(r => r.Id == checkpoint.Id);
                    _distanceService.Checkpoints.Remove(itemToRemove);
                    }
                //PrintRoute(foundRoute);
                finalRoutes.Add(foundRoute);
            }



            //var finalRoutes = GetRoutes(orderedRoutes, indexResult, routesToFind);
            var checkpointCounts = finalRoutes.SelectMany(p => p.Checkpoints).Where(c => c.IsHomeBase == false).Select(c => c.Id).GroupBy(item => item).Select(group => new
            {
                group.Key,
                Count = group.Count()
            }).OrderByDescending(g => g.Count);

            for (int i = 0; i < finalRoutes.Count; i=i+2)
            {
                PrintRouteSiriusTwo(finalRoutes[i], finalRoutes[i+1]);
            }
            //foreach (var route in finalRoutes)
            //    PrintRouteSirius(route);

            Console.WriteLine("Checkpoints used");
            foreach (var checkpointCount in checkpointCounts)
            {
                Console.WriteLine($"{checkpointCount.Key}: {checkpointCount.Count}");
            }
            Console.WriteLine($"Used {checkpointCounts.Count()} checkpoints of a total {cps.Count}");
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
            //for (var i = 0; i < 42; i++)
            //    returnList.Add(new Checkpoint(RandomNumberBetween(56.1694925, 56.172670),
            //        RandomNumberBetween(10.092656, 10.109312), i.ToString(), false));
            
           
            returnList.Add(new Checkpoint(56.432731316570646, 12.836718485260432, "379138", false));
            returnList.Add(new Checkpoint(56.43366123854465, 12.832062873184002, "379137", false));
            returnList.Add(new Checkpoint(56.43508561847326, 12.832926705486999, "379136", false));
            returnList.Add(new Checkpoint(56.43563400054597, 12.831945073738437, "379135", false));
            returnList.Add(new Checkpoint(56.4367226595041, 12.827799935984505, "379134", false));
            returnList.Add(new Checkpoint(56.43804709549685, 12.829393082444849, "379133", false));
            returnList.Add(new Checkpoint(56.43677119215807, 12.833669333747826, "379132", false));
            returnList.Add(new Checkpoint(56.436206727935144, 12.834496284097959, "379131", false));
            returnList.Add(new Checkpoint(56.436639330647765, 12.839335336812718, "379130", false));
            returnList.Add(new Checkpoint(56.4354081175576, 12.842436012765708, "379129", false));
            returnList.Add(new Checkpoint(56.43585815062755, 12.843785185855774, "379128", false));
            returnList.Add(new Checkpoint(56.4316706491744, 12.84454414687547, "379127", false));
            returnList.Add(new Checkpoint(56.43022230039363, 12.845400662357989, "379126", false));
            returnList.Add(new Checkpoint(56.42839126043241, 12.847677380309625, "379125", false));
            returnList.Add(new Checkpoint(56.42810542193616, 12.849178737308616, "379124", false));
            returnList.Add(new Checkpoint(56.42702145395237, 12.852426218264432, "379123", false));
            returnList.Add(new Checkpoint(56.42738805131339, 12.854047682591194, "379122", false));
            returnList.Add(new Checkpoint(56.429021259591664, 12.854437707442733, "379121", false));
            returnList.Add(new Checkpoint(56.43041826608892, 12.854998043392785, "379120", false));
            returnList.Add(new Checkpoint(56.4304718264544, 12.84866607333295, "379119", false));
            returnList.Add(new Checkpoint(56.429605027482175, 12.846923488938787, "379118", false));
            returnList.Add(new Checkpoint(56.428603584367856, 12.844636377819214, "379117", false));
            returnList.Add(new Checkpoint(56.42951726357732, 12.84152834406912, "379116", false));
            returnList.Add(new Checkpoint(56.42996327111031, 12.840345819930718, "379115", false));
            returnList.Add(new Checkpoint(56.430316742815826, 12.838573446273655, "379114", false));
            returnList.Add(new Checkpoint(56.43110928063944, 12.837009419100426, "379113", false));
            returnList.Add(new Checkpoint(56.427806294068105, 12.833201924877532, "379112", false));
            returnList.Add(new Checkpoint(56.428554471418025, 12.83546973652144, "379111", false));
            returnList.Add(new Checkpoint(56.42956129696965, 12.836382838316078, "379110", false));
            returnList.Add(new Checkpoint(56.43059835904641, 12.836063244207812, "379109", false));
            returnList.Add(new Checkpoint(56.43163811731996, 12.833362931745537, "379108", false));
            returnList.Add(new Checkpoint(56.43248043951735, 12.834226954545565, "379107", false));
            returnList.Add(new Checkpoint(56.43315740791674, 12.835829493681095, "379106", false));
            returnList.Add(new Checkpoint(56.4326935938469, 12.836906841959282, "379105", false));
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
        private static Route GetBestRoute(List<Route> orderedRoutes, int indexToTakeFrom)
        {
         return orderedRoutes[indexToTakeFrom];
            
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
                    $"    CP {route.Checkpoints[i].Id} CP {route.Checkpoints[i + 1].Id}. Distance: {Math.Round(_distanceService.GetDistance(route.Checkpoints[i], route.Checkpoints[i + 1]), 2)} m");
            Console.WriteLine($"Total Distance: {Math.Round(route.TotalDistance / 1000, 2)} km");
        }
        private static void PrintRouteSirius(Route route)
        {
            Console.WriteLine($"INSERT INTO `quiz`.`CheckpointSets` (`CheckpointSetID`, `LocationID`, `Name`) VALUES (NULL, '181', 'Hotel Skansen {_checkpointsetidcounter}');");

            for (var i = 1; i < 12; i++) { 
                if (i < 6)
                {
                    Console.WriteLine($"INSERT INTO `quiz`.`CheckpointSetRelation` (`CheckpointSetID`, `CheckpointID`, `Sequence`) VALUES ({6482 + _checkpointsetidcounter}, {route.Checkpoints[i].Id}, {i});");

                }
            if (i == 6)
                {
                    Console.WriteLine($"INSERT INTO `quiz`.`CheckpointSetRelation` (`CheckpointSetID`, `CheckpointID`, `Sequence`) VALUES ({6482 + _checkpointsetidcounter}, null, {i});");

                }
                if (i > 6)
                {
                    Console.WriteLine($"INSERT INTO `quiz`.`CheckpointSetRelation` (`CheckpointSetID`, `CheckpointID`, `Sequence`) VALUES ({6482 + _checkpointsetidcounter}, {route.Checkpoints[i-1].Id}, {i});");

                }
            }
            _checkpointsetidcounter++;

        }
        private static void PrintRouteSiriusTwo(Route finalRoute, Route route)
        {
            finalRoute.Checkpoints.RemoveAt(finalRoute.Checkpoints.Count - 1);
            //Console.WriteLine($"This first part of the route is {finalRoute.TotalDistance}");
            route.Checkpoints.RemoveAt(0);
            //Console.WriteLine($"This second part of the route is {route.TotalDistance}");
            finalRoute.Checkpoints.AddRange(route.Checkpoints);
            PrintRouteSirius(finalRoute);
        }

    }
}