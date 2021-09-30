using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //A felfogott marker, a detectedmarker világítási értékkel (szimbólummal) kiegészített változata, a dekódoláshoz
    public class MarkerSymbol
    {
        //kikapcsolt vagy bekapcsolt-e a led?
        public int Value;

        //idő
        public int Time;
        //képkockaszám
        public int FrameNumber;
        //pozíció
        public Point2D Position = new Point2D(0, 0);

        public MarkerSymbol(int value, int time, int framenumber, Point2D position) {
            Value = value;
            Time = time;
            FrameNumber = framenumber;
            Position = position;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

    }
}
