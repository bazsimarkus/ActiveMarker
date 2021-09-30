using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SimulationSetupRectangleWithTimingError:SimulationSetupBase
    {
        public SimulationSetupRectangleWithTimingError() {

        }

        public override void SimulationStart() {
            Engine.E.PathList.Add(new PathRectangle());

            //a stepsize javasolt értéke 1 pixel, ekkor a sebesség mértékegysége nem csak node / 0.1ms, hanem pixel/0.1ms!
            Engine.E.PathList[0].CreatePath(300, 1, 0, 0, 0);
            
            Car Car1 = new Car(new CarID(218), Engine.E.PathList[0].Nodes[0], new Flasher(new SymbolListTwoSymbolWithError(new CarID(218))), new VelocityControllerConstWithNoise(0.01, 0.01));

            Engine.E.Cars.Add(Car1);
        }
    }
}
