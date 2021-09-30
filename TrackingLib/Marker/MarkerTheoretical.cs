using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Elméleti marker, az autó képmása, a szimuláció vizsgálatához szükséges csupán
    public class MarkerTheoretical
    {  
        //Referencia az autóra
        public Car Car;
        public CarID CarID;

        public PathBase Path;
        public Flasher Flasher;

        public Point2D Position = new Point2D(); // ez a legfontosabb
        public double Speed; // sebesség
        public double Orientation; //x tengellyel bezárt szög radiánban

        public double Distance; // A következő autótól való távolság a pályán

        public bool IsLedOn;

        public MarkerTheoretical(Car car) {
            Position = car.actualNode.Position2D;
            Speed = car.Speed;
            Orientation = car.Orientation;
            Distance = car.Distance;
            CarID = car.ID;
        }
    }
}
