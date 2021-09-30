using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class TrackedMarker
    {
        public int ID;
        public Point2D Position = new Point2D(); // ez a legfontosabb
        public bool Taken;
        public int LastTimeTracked;

        public TrackedMarker(Point2D position, int lasttime)
        {
            Position = position;
            LastTimeTracked = lasttime;
        }

    }
}
