using System.Device.Location;

namespace PathFinderStandard.Models
{
    public class Checkpoint
    {
        public Checkpoint(double lat, double lng, string id, bool isHomeBase)
        {
            Coordinate = new GeoCoordinate(lat, lng);
            Id = id;
            IsHomeBase = isHomeBase;
        }
        public GeoCoordinate Coordinate { get; set; }
        public string Id { get; set; }
        public bool IsHomeBase { get; set; }
        
    }
}