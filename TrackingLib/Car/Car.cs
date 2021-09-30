using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class Car
    {
        public CarID ID;

        public double Speed = 1; //Sebesség, mértékegysége pixel / 0.1ms
        public double AngularVelocity; //Szögsebesség radián/sec-ban, a dinamikus paraméterek becsléséhez
        public double Orientation; //X tengellyel bezárt szög radiánban
        public double Distance = 0; //Távolság az autó előtt a pályán legközelebb lévő kocsitól

        double SpeedCounter = 0; //A sebesség beállításához szükséges késleltető számláló

        public Car(CarID carID, PathNode actualNode, Flasher flasherparam, VelocityControllerBase velocitycontrollerparam)
        {
            ID = carID;
            ActualNode = actualNode;
            Flasher = flasherparam;
            velocityController = velocitycontrollerparam;
        }

        public PathNode ActualNode;

        public PathNode actualNode
        {
            get
            {
                return ActualNode;
            }
            set
            {
                ActualNode = value;
            }
        }

        VelocityControllerBase VelocityController;

        public VelocityControllerBase velocityController
        {
            get
            {
                return VelocityController;
            }
            set
            {
                VelocityController = value;
            }
        }

        Flasher flasher;

        public Flasher Flasher
        {
            get
            {
                return flasher;
            }
            set
            {
                flasher = value;
            }
        }

        public bool IsLedOn { get { return flasher.isLedOn; } }

        //Az autó szimulációs lépése, feladata a mozgatás a pályán, valamint az autó villogójának működtetése
        public void SimulationStep()
        {
            //speed mértékegysége = pixel / 0.1ms VAGY 10 pixel / ms VAGY 10000 pixel / s
            Orientation = ActualNode.Orientation;
            PathNode temp_node = ActualNode.NextNode;
            Distance = 0;

            //Distance kiszámítása: megnézzük, milyen messze van a következő autótól, végigiterálunk a path-en
            while (temp_node != ActualNode) {
                Distance++;
                if (temp_node.isCarOnNode == true) break; //ha van autó kilépünk a ciklusból, ellenkező esetben visszaér önmagába
                temp_node = temp_node.NextNode;
            }

            if (Speed < 1) {
                if (SpeedCounter < 1) {
                    SpeedCounter += Speed; 
                }
                if (SpeedCounter >= 1)
                {
                    ActualNode.isCarOnNode = false;
                    ActualNode = ActualNode.NextNode;
                    ActualNode.isCarOnNode = true;
                    SpeedCounter = 0;
                }
            }
            else if (Speed >= 1)
            {
                for (int i = 0; i < (int)(Speed); i++)
                {
                    ActualNode.isCarOnNode = false;
                    ActualNode = ActualNode.NextNode;
                    ActualNode.isCarOnNode = true;
                }
            }

            velocityController.SetSpeed(this);
            flasher.SimulationStep();
        }
    }
}
