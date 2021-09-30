using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{

    public class TrackerOne : TrackerBase //Követő osztály
    {
        public int DetectedSymbolsCount = 0; //az eddig detektált szimbólumok száma, gyakorlatilag framenként 1-el növekszik
        public int NumberOfTrackedMarkers = 0;

        public static int TimeTolerance = 100000; //kétszimbólumosnál 2500, 3nál 4000
        public static double DistanceTolerance = 100; //kétszimbólumosnál 20 3nál 40

        List<LedCaptured> CurrentCapturedLeds = new List<LedCaptured>();

        public TrackerOne()
        {

        }

        public override void Init()
        {
            //A kamera által készített LedCaptured-ök alapján készítjük a detekciókat, várakozási sor, mint a frameknél
            TrackingLib.Engine.E.Camera.OnNewFrame += (frame) =>
            {
                if (frame.CapturedLeds.Count == 0)
                {
                    return;
                }

                CurrentCapturedLeds = frame.CapturedLeds;
                Step();

            };
        }

        //Jelenleg az engine-ben van meghatározva egyfajta típusú detektor, amit az összes felvillanó markerhez használ
        public override void Step() //gyakorlatilag olyan mint a SimulationStep, csak framenként, a detektor számára
        {
            TrackedMarkers.RemoveAll(trackedmarker => trackedmarker.LastTimeTracked < Engine.E.SimulationTime - TimeTolerance);

            for (int i = 0; i < CurrentCapturedLeds.Count; i++)
            {
                int ClosestMarkerIndex = 0;
                bool FoundOne = false;
                if (TrackedMarkers.Count != 0)
                {
                    double distanceActual = Math.Sqrt(Math.Pow(CurrentCapturedLeds[i].Position.X - TrackedMarkers[0].Position.X, 2) + Math.Pow(CurrentCapturedLeds[i].Position.Y - TrackedMarkers[0].Position.Y, 2));
                    
                    for (int j = 0; j < TrackedMarkers.Count; j++)
                    {
                        double distanceFromDetectedX = CurrentCapturedLeds[i].Position.X - TrackedMarkers[j].Position.X;
                        double distanceFromDetectedY = CurrentCapturedLeds[i].Position.Y - TrackedMarkers[j].Position.Y;
                        double distanceFull = Math.Sqrt(Math.Pow(distanceFromDetectedX, 2) + Math.Pow(distanceFromDetectedY, 2));
                        //Pitagorasz tétel
                        if (distanceFull < DistanceTolerance)
                        {
                            FoundOne = true;
                            if (distanceFull < distanceActual)
                            {
                                distanceActual = distanceFull;
                                ClosestMarkerIndex = j;
                            }
                            
                        }
                    }
                }
                if (FoundOne == true) TrackedMarkers[ClosestMarkerIndex].Position = CurrentCapturedLeds[i].Position;
                else
                {
                    TrackedMarkers.Add(new TrackedMarker(CurrentCapturedLeds[i].Position, CurrentCapturedLeds[i].Time));
                }
            }
        }
    }
}
