using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SimulationSetupBase
    {
  
        public virtual void SimulationStart() {
            Engine.E.isRunning = true;
        }

        public virtual void SimulationStop()
        {
            Engine.E.isRunning = false;
        }
    }
}
