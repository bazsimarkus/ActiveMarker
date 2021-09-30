using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class PathNode
    {
        public Point2D Position2D;
        public double Orientation;
        public bool isCarOnNode;

        private PathNode nextNode; //A következő útvonal pont

        public PathNode NextNode
        { 
          get
            {
                return nextNode;
            }
          set
            {
                nextNode = value;
            }
        }

        public PathNode()
        {
            Position2D = new Point2D(0, 0);
            Orientation = 0;
            NextNode = null;
        }

        public PathNode(Point2D position2D, double orientation, PathNode nextNode)
        {
            Position2D = position2D;
            Orientation = orientation;
            NextNode = nextNode;
        }
    }
}
