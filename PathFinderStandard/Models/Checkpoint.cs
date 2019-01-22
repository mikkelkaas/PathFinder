using System;
using System.Collections.Generic;
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

    public class CheckpointEuqalityComparer : IEqualityComparer<Tuple<Checkpoint,Checkpoint>>
    {

        public bool Equals(Tuple<Checkpoint, Checkpoint> x, Tuple<Checkpoint, Checkpoint> y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return (x.Item1.Id == y.Item1.Id) && (x.Item2.Id == y.Item2.Id);
        }

        public int GetHashCode(Tuple<Checkpoint, Checkpoint> obj)
        {
           return obj.Item1.Id.GetHashCode() ^ obj.Item2.Id.GetHashCode();
        }
    }
}