using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class PathBase
    {
        public List<PathNode> Nodes = new List<PathNode>();

        public virtual void CreatePath(double size, double resolution, double xcenter, double ycenter, double noiseamplitude) { }
    }
}
