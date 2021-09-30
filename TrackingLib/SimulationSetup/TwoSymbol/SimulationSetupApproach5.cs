using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SimulationSetupApproach5:SimulationSetupBase
    {
        public SimulationSetupApproach5() {

        }

        public override void SimulationStart() {
            Engine.E.PathList.Add(new PathCircle());
            Engine.E.PathList.Add(new PathCircle());
            Engine.E.PathList.Add(new PathCircle());
            Engine.E.PathList.Add(new PathCircle());
            Engine.E.PathList.Add(new PathCircle());

            //a stepsize javasolt értéke 1 pixel, ekkor a sebesség mértékegysége nem csak node / 0.1ms, hanem pixel/0.1ms!
            Engine.E.PathList[0].CreatePath(300, 1, 0, 0,0);
            Engine.E.PathList[1].CreatePath(225, 1, 75, 0, 0);
            Engine.E.PathList[2].CreatePath(150, 1, 150, 0, 0);
            Engine.E.PathList[3].CreatePath(75, 1, 225, 0, 0);
            Engine.E.PathList[4].CreatePath(30, 1, 270, 0, 0);

            Car Car1 = new Car(new CarID(123), Engine.E.PathList[0].Nodes[0], new Flasher(new SymbolListTwoSymbol(new CarID(123))), new VelocityControllerConst(0.01));
            Car Car2 = new Car(new CarID(218), Engine.E.PathList[1].Nodes[0], new Flasher(new SymbolListTwoSymbol(new CarID(218))), new VelocityControllerConst(0.0075));
            Car Car3 = new Car(new CarID(326), Engine.E.PathList[2].Nodes[0], new Flasher(new SymbolListTwoSymbol(new CarID(326))), new VelocityControllerConst(0.005));
            Car Car4 = new Car(new CarID(458), Engine.E.PathList[3].Nodes[0], new Flasher(new SymbolListTwoSymbol(new CarID(458))), new VelocityControllerConst(0.0025));
            Car Car5 = new Car(new CarID(805), Engine.E.PathList[4].Nodes[0], new Flasher(new SymbolListTwoSymbol(new CarID(805))), new VelocityControllerConst(0.00125));



            Engine.E.Cars.Add(Car1);
            Engine.E.Cars.Add(Car2);
            Engine.E.Cars.Add(Car3);
            Engine.E.Cars.Add(Car4);
            Engine.E.Cars.Add(Car5);
        }
    }
}
