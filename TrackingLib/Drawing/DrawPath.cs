using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DrawPath
    {
        public List<Point2D> PathPoints = new List<Point2D>();

        public DrawPath(PathBase path)
        {
                for (int i = 0; i < path.Nodes.Count; i++)
                {
                Point2D temp_point = path.Nodes[i].Position2D;
                //temp_point.Offset(DrawList.centerX, DrawList.centerY); //Ha offszetelni szeretnénk a kirajzolást a valósághoz képest
                PathPoints.Add(temp_point);             
                }
        }
    }
}
