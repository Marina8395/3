using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3
{
    public class Train
    {
        public string startingPoint { get; set; }
        public string destination { get; set; }
        public string carriageType { get; set; }
        public int travelTime { get; set; }
        public bool delay { get; set; }
        
        public Train(string StartingPoint, string Destination, string CarriageType, int TravelTime, bool Delay)
        {
            startingPoint = StartingPoint;
            destination = Destination;
            carriageType = CarriageType;
            travelTime = TravelTime;
            delay = Delay;
        }
        
        public Train()
        {

        }

    }
}
