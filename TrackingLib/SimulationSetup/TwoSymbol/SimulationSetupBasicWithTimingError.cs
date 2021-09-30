using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SimulationSetupBasicWithTimingError:SimulationSetupBase
    {
        public SimulationSetupBasicWithTimingError() {

        }

        public override void SimulationStart() {
            Engine.E.PathList.Add(new PathCircle());

            //a stepsize javasolt értéke 1 pixel, ekkor a sebesség mértékegysége nem csak node / 0.1ms, hanem pixel/0.1ms!
            Engine.E.PathList[0].CreatePath(250, 1, 0, 0,0);
            
            Car Car1 = new Car(new CarID(213), Engine.E.PathList[0].Nodes[0], new Flasher(new SymbolListTwoSymbolWithError(new CarID(213))), new VelocityControllerConst(0.01));

            Engine.E.Cars.Add(Car1);
        }
    }
}
