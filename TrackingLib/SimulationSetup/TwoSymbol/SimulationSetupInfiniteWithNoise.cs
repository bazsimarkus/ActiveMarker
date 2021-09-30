using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SimulationSetupInfiniteWithNoise:SimulationSetupBase
    {
        public SimulationSetupInfiniteWithNoise() {

        }

        public override void SimulationStart() {
            Engine.E.PathList.Add(new PathInfinite());

            //a stepsize javasolt értéke 1 pixel, ekkor a sebesség mértékegysége nem csak node / 0.1ms, hanem pixel/0.1ms!
            Engine.E.PathList[0].CreatePath(160, 1, 0, 0, 20);

            Car Car1 = new Car(new CarID(4), Engine.E.PathList[0].Nodes[0], new Flasher(new SymbolListTwoSymbol(new CarID(4))), new VelocityControllerConst(0.01));
            Car Car2 = new Car(new CarID(8), Engine.E.PathList[0].Nodes[400], new Flasher(new SymbolListTwoSymbol(new CarID(8))), new VelocityControllerConst(0.01));

            Engine.E.Cars.Add(Car1);
            Engine.E.Cars.Add(Car2);
        }
    }
}
