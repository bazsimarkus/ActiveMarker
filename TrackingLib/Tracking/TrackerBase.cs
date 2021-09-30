using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class TrackerBase
    {
        public List<TrackedMarker> TrackedMarkers = new List<TrackedMarker>();
        public virtual void Step() { }

        public virtual void Init() { } //A detektor event feliratkozásaitkezelő függvény, az engine hívja meg egyszer a szimuláció elején

    }
}
