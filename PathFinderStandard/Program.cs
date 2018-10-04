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
            var routesToGenerate = 500; //The bigger the number, the closer to the desired distance the routes will be.
            var targetDistanceInMeters = 3200; //How long should the routes be from Homebase, to checkpoints and back to Homebase again
            var routesToFind = 20; //How many routes should be found?
            var checkpointsPerRoute = 5; //Checkpoints per route
            var cps = GetCheckpoints();
            var homebase = new Checkpoint(56.435056, 12.838587, "Homebase", true);

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

            var finalRoutes = GetRoutes(orderedRoutes, indexResult, routesToFind);
            var checkpointCounts = finalRoutes.SelectMany(p => p.Checkpoints).Where(c=>c.IsHomeBase == false).Select(c => c.Id).GroupBy(item=>item).Select(group => new
            {
                group.Key,
                Count = group.Count()
            }).OrderByDescending(g=>g.Count);

            foreach (var route in finalRoutes)
                PrintRoute(route);

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
            /*
            returnList.Add(new Checkpoint(56.0459198, 9.9436, "379001", false));
            returnList.Add(new Checkpoint(56.0453368, 9.944817, "379002", false));
            returnList.Add(new Checkpoint(56.0471142, 9.942117, "379003", false));
            returnList.Add(new Checkpoint(56.0453368, 9.946571, "379004", false));
            returnList.Add(new Checkpoint(56.0445385, 9.942934, "379005", false));
            returnList.Add(new Checkpoint(56.0463854, 9.94325, "379006", false));
            returnList.Add(new Checkpoint(56.0451709, 9.9471331, "379009", false));
            returnList.Add(new Checkpoint(56.0446994, 9.9473759, "379010", false));
            returnList.Add(new Checkpoint(56.0443468, 9.9482257, "379011", false));
            returnList.Add(new Checkpoint(56.0438253, 9.9494691, "379012", false));
            returnList.Add(new Checkpoint(56.0440324, 9.9516842, "379013", false));
            returnList.Add(new Checkpoint(56.0447122, 9.9529136, "379014", false));
            returnList.Add(new Checkpoint(56.0450071, 9.9533766, "379015", false));
            returnList.Add(new Checkpoint(56.0462275, 9.9554817, "379016", false));
            returnList.Add(new Checkpoint(56.0472517, 9.9553072, "379017", false));
            returnList.Add(new Checkpoint(56.0483537, 9.9614396, "379018", false));
            returnList.Add(new Checkpoint(56.0496038, 9.9607848, "379019", false));
            returnList.Add(new Checkpoint(56.0502989, 9.9603843, "379021", false));
            returnList.Add(new Checkpoint(56.051412, 9.958814, "379022", false));
            returnList.Add(new Checkpoint(56.0527901, 9.9576598, "379023", false));
            returnList.Add(new Checkpoint(56.05413209, 9.95659535, "379024", false));
            returnList.Add(new Checkpoint(56.0539119, 9.9553621, "379025", false));
            returnList.Add(new Checkpoint(56.0535471, 9.9534777, "379026", false));
            returnList.Add(new Checkpoint(56.0530787, 9.9508875, "379027", false));
            returnList.Add(new Checkpoint(56.053024, 9.950133, "379028", false));
            returnList.Add(new Checkpoint(56.0513493, 9.9512533, "379029", false));
            returnList.Add(new Checkpoint(56.0515436, 9.9522344, "379030", false));
            returnList.Add(new Checkpoint(56.0519147, 9.9539149, "379031", false));
            returnList.Add(new Checkpoint(56.051732, 9.9551076, "379032", false));
            returnList.Add(new Checkpoint(56.0511325, 9.9520576, "379033", false));
            returnList.Add(new Checkpoint(56.050744, 9.9489561, "379034", false));
            returnList.Add(new Checkpoint(56.0508182, 9.9478105, "379035", false));
            returnList.Add(new Checkpoint(56.0505375, 9.9466025, "379036", false));
            returnList.Add(new Checkpoint(56.0505262, 9.9459205, "379037", false));
            returnList.Add(new Checkpoint(56.0504125, 9.9446716, "379038", false));
            returnList.Add(new Checkpoint(56.0499125, 9.9439673, "379039", false));
            returnList.Add(new Checkpoint(56.0475397, 9.9446626, "379040", false));
            returnList.Add(new Checkpoint(56.0475476, 9.9445295, "379041", false));
            */
            returnList.Add(new Checkpoint(56.432731316570646, 12.836718485260432, "38", false));
            returnList.Add(new Checkpoint(56.43366123854465, 12.832062873184002, "37", false));
            returnList.Add(new Checkpoint(56.43508561847326, 12.832926705486999, "36", false));
            returnList.Add(new Checkpoint(56.43563400054597, 12.831945073738437, "35", false));
            returnList.Add(new Checkpoint(56.4367226595041, 12.827799935984505, "34", false));
            returnList.Add(new Checkpoint(56.43804709549685, 12.829393082444849, "33", false));
            returnList.Add(new Checkpoint(56.43677119215807, 12.833669333747826, "32", false));
            returnList.Add(new Checkpoint(56.436206727935144, 12.834496284097959, "31", false));
            returnList.Add(new Checkpoint(56.436639330647765, 12.839335336812718, "30", false));
            returnList.Add(new Checkpoint(56.4354081175576, 12.842436012765708, "29", false));
            returnList.Add(new Checkpoint(56.43585815062755, 12.843785185855774, "28", false));
            returnList.Add(new Checkpoint(56.4316706491744, 12.84454414687547, "27", false));
            returnList.Add(new Checkpoint(56.43022230039363, 12.845400662357989, "26", false));
            returnList.Add(new Checkpoint(56.42839126043241, 12.847677380309625, "25", false));
            returnList.Add(new Checkpoint(56.42810542193616, 12.849178737308616, "24", false));
            returnList.Add(new Checkpoint(56.42702145395237, 12.852426218264432, "23", false));
            returnList.Add(new Checkpoint(56.42738805131339, 12.854047682591194, "22", false));
            returnList.Add(new Checkpoint(56.429021259591664, 12.854437707442733, "21", false));
            returnList.Add(new Checkpoint(56.43041826608892, 12.854998043392785, "20", false));
            returnList.Add(new Checkpoint(56.4304718264544, 12.84866607333295, "19", false));
            returnList.Add(new Checkpoint(56.429605027482175, 12.846923488938787, "18", false));
            returnList.Add(new Checkpoint(56.428603584367856, 12.844636377819214, "17", false));
            returnList.Add(new Checkpoint(56.42951726357732, 12.84152834406912, "16", false));
            returnList.Add(new Checkpoint(56.42996327111031, 12.840345819930718, "15", false));
            returnList.Add(new Checkpoint(56.430316742815826, 12.838573446273655, "14", false));
            returnList.Add(new Checkpoint(56.43110928063944, 12.837009419100426, "13", false));
            returnList.Add(new Checkpoint(56.427806294068105, 12.833201924877532, "12", false));
            returnList.Add(new Checkpoint(56.428554471418025, 12.83546973652144, "11", false));
            returnList.Add(new Checkpoint(56.42956129696965, 12.836382838316078, "10", false));
            returnList.Add(new Checkpoint(56.43059835904641, 12.836063244207812, "9", false));
            returnList.Add(new Checkpoint(56.43163811731996, 12.833362931745537, "8", false));
            returnList.Add(new Checkpoint(56.43248043951735, 12.834226954545565, "7", false));
            returnList.Add(new Checkpoint(56.43315740791674, 12.835829493681095, "6", false));
            returnList.Add(new Checkpoint(56.4326935938469, 12.836906841959282, "5", false));
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