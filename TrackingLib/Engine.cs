using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;
using System.Timers;
using System.IO;

namespace TrackingLib
{
    //A szimuláció motorja
    public class Engine
    {
        private static Engine _engine = new Engine();
        private static readonly object padlock = new object();

        Engine()
        {
        }

        public static Engine E
        {
            get
            {
                lock (padlock)
                {
                    if (_engine == null)
                    {
                        _engine = new Engine();
                    }
                    return _engine;
                }
            }
        }

        public bool isRunning = true; // a szimuláció épp fut, vagy szünetel? ezt jelzi, és a space lenyomásával állíthatjuk

        // Szimulációs idő felső korlátja, itt 200 sec, a 10 ezres szorás miatt lesz 20000000
        const int simulationTimeLimit = 1000000;

        //Egy szimulációs lépés 1/10 ms-nak felel meg a modellünkben
        //átváltás: 1 egység 1/10 milliszekundumnak felel meg, így a másodperces double értékeket meg kell szorozni 10000-el és így kapható meg az int-tel reprezentált érték
        public readonly int SimulationTimeStep = 1;
        public double SimulationTimeDouble = 0;

        const int divisionOfSecond = 10000; // 1/10 ms-os felbontás

        //A szimulációs lépéseket számoló változó, jelen esetben megegyezik a szimulációs idővel
        public int SimulationStepCount = 0;

        //Az utak listája, egyelőre 2 kör utat tartalmaz
        List<PathBase> pathList = new List<PathBase>();
        public List<PathBase> PathList
        {
            get
            {
                lock (this)
                {
                    return pathList;
                }
            }
        }

        CameraBase camera = new CameraOne();
        public CameraBase Camera { get { return camera; } set { camera = value; } }

        //Autók listája
        List<Car> cars = new List<Car>();
        public List<Car> Cars
        {
            get
            {
                lock (this)
                {
                    return cars;
                }
            }
        }

        public SimulationSetupBase SimulationSetup = new SimulationSetupInfiniteWithNoise();

        DetectorBase detector = new DetectorOne();
        public DetectorBase Detector { get { return detector; } set { detector = value; } }

        DecoderBase decoder = new DecoderTwoSymbol();
        public DecoderBase Decoder { get { return decoder; } set { decoder = value; } }

        TrackerBase tracker = new TrackerOne();
        public TrackerBase Tracker { get { return tracker; } set { tracker = value; } }

        // Szimulációs idő, a MÉRTÉKEGYSÉGE 1/10 MILLISEC, az fő időmérő szám a szimulációban, régen double-ben volt és másodperc volt a mértékegység, de elcsúszott a double pontatlansága miatt
        //átváltás: 1 egység 1/10 milliszekundumnak felel meg, így a másodperces double értékeket meg kell szorozni 10000-el és így kapható meg az int-tel reprezentált érték
        int simulationTime = 0;
        public int SimulationTime
        {
            get
            {
                lock (this)
                {
                    return simulationTime;
                }
            }
        }

        public void Reset()
        {
            lock (this)
            {
                Console.WriteLine("reset");
                simulationTime = 0;
            }
        }

        public void Start()
        {
            //A szimuláció beállítását egy SimulationSetup osztály végzi el - most SimulationBasic
            SimulationSetup.SimulationStart();
            Detector.Init();
            Tracker.Init();

            Reset();
            Task.Factory.StartNew(() =>
            {
                int i = 0;

                while (true)
                {
                    if (SimulationTime > simulationTimeLimit) break; // ha nem akarjuk hogy a végtelenségig fusson

                    while (isRunning == false) { } // space lenyomása esetén ebben a ciklusban van, míg mégegyszer meg nem nyomjuk

                    SimulationStep();

                    i++;

                    //Az alábbi rész kikommentezésével a szimulációs idő közel valóságszerűen telik
                    if ((i % 40) == 0)
                    {
                        Thread.Sleep(1);
                    }

                }

                Console.WriteLine("Simulation completed");
            });
        }

        void SimulationStep()
        {
            lock (this)
            {
                simulationTime += SimulationTimeStep; // mérjük az eltelt időt
                SimulationTimeDouble = (double)simulationTime / divisionOfSecond;
                SimulationStepCount += 1;
                if (Camera.ActualFrame == null)
                {
                    Camera.ActualFrame = new Frame();
                    foreach (var path in PathList)
                    {
                        Camera.ActualFrame.DrawList.AddPath(new DrawPath(path));
                    }
                }

                foreach (var c in cars)
                {
                    c.SimulationStep();
                }

                Camera.CameraSimulationStep(); //bizonyos időközönként fényképezünk
            }
        }
    }
}

