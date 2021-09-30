using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DrawTheoreticalMarker
    {
        public Point2D Position = new Point2D();
        public double Speed; // sebesség
        public double Orientation; //x tengellyel bezárt szög radiánban
        public double Distance;
        public CarID ID;
     

        public DrawTheoreticalMarker(MarkerTheoretical mt)
        {
            Position.X = mt.Position.X;
            Position.Y = mt.Position.Y;
            Speed = mt.Speed;
            Orientation = mt.Orientation;
            Distance = mt.Distance;
            ID = mt.CarID;
        }
    }
}
