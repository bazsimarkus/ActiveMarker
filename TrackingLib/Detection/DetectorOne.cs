using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DetectorOne : DetectorBase
    {
        public static int TimeTolerance = 2500; //kétszimbólumosnál 2500, 3nál 4000 javasolt
        public static double DistanceTolerance = 20; //kétszimbólumosnál 20, 3nál 40 javasolt

        DepthFirstAlgorithm DFS = new DepthFirstAlgorithm();
        //frame-eket tároljon
        Q<List<LedCaptured>> CapturedLedQueue = new Q<List<LedCaptured>>();

        DecodingStatistics decodingStatistics = new DecodingStatistics();

        public DetectorOne()
        {
            //DFS mélységi bejáró és dekódoló fő taszk
            var DFStask = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (CapturedLedQueue.Event.WaitOne(10)) //Nem pörög folyamatosan, várakozunk ha nincs bejövő detekció 
                    {
                        while (true)
                        {
                            
                            MarkerSequences.Clear(); //A szekvenciák listáját töröljük

                            //Nem frissítjük mindig újra a teljes detekciós listát, hanem mindig, ha új érkezik be, hozzáadjuk a végéhez
                            var detections = CapturedLedQueue.GetMostRecent(); //A  legújabb detekciókat tartalmazó lista lesz
                            if (detections == null) { break; }
                            if (detections.Count == 0) { break; }
                            List<DetectedMarker> recentMarkers = new List<DetectedMarker>();

                            foreach (var d in detections) //A capturedLed is tartalmazza az időt és a framenumbert, a pozíció mellett (ez a 3 infó jön át)
                            {
                                var markerToAdd = new DetectedMarker(d.Time, d.FrameNumber, new Point2D(d.Position.X, d.Position.Y));
                                DetectedMarkers.Add(markerToAdd);
                                recentMarkers.Add(markerToAdd);
                            }

                            //Ha sikeresen frissítettük a DetectedMarkers listát, végigfuttathattjuk a jelöltek feltöltését
                            var watchDetect = new System.Diagnostics.Stopwatch();
                            watchDetect.Start();
                            Detect();
                            watchDetect.Stop();
                            decodingStatistics.DetectTimes.Add(watchDetect.Elapsed.Ticks); //1 Elapsed.Tick == 100 nanosec
                            watchDetect.Reset();

                            //Nem futtatjuk le mindig a bejárást sem, csak ha volt bejövő új adat, hisz csak akkor állhat be változás
                            if (recentMarkers.Count > 0)
                            {
                                //A legutóbbi markereket bejárjuk, létrehozzuk a szekvenciákat
                                foreach (var marker in recentMarkers)
                                {
                                    MarkerSequences.AddRange(DFS.Traverse(marker));
                                }

                                Engine.E.Decoder.SuccessfullyDecodedValues.Clear();

                                var watchDecode = new System.Diagnostics.Stopwatch();
                                watchDecode.Start();
                             
                                //Az előbb létrehozott szekvenciákat dekódoljuk
                                for (int i = 0; i < MarkerSequences.Count; i++)
                                {
                                    //Console.WriteLine(i.ToString() + ". Sequence added: ");
                                    //foreach (var markersymbol in MarkerSequences[i].MarkerSymbols)
                                    //{
                                    //    Console.Write(markersymbol.Value.ToString());
                                    //}
                                    //Console.WriteLine();
                                    

                                    Engine.E.Decoder.Decode(MarkerSequences[i]);
                                    decodingStatistics.AddAttempt();
                                }
                                watchDecode.Stop();
                                decodingStatistics.DecodeTimes.Add(watchDecode.Elapsed.Ticks); //1 Elapsed.Tick == 100 nanosec
                                watchDecode.Reset();


                                //A legjobb dekódolt értékeket kiíratjuk
                                for (int i = 0; i < Engine.E.Decoder.getBestDecodedValues().Count; i++)
                                {
                                    Console.WriteLine("Correct value: " + Engine.E.Decoder.getBestDecodedValues()[i].Value.ToString() + " Error: " + Engine.E.Decoder.getBestDecodedValues()[i].Error.ToString() + " ");
                                    decodingStatistics.AddSuccess();
                                    if (Engine.E.Decoder.getBestDecodedValues()[i].Error > 0)
                                    {

                                    }
                                    if (i == Engine.E.Decoder.getBestDecodedValues().Count()-1)
                                    {
                                        Console.WriteLine(
                                            "Current Ratio: " + decodingStatistics.GetRatio().ToString("N3") +
                                            " Attempts: " + decodingStatistics.Attempts.ToString() +
                                            " Success: " + decodingStatistics.Success.ToString() +
                                            " DetectTime: " + decodingStatistics.DetectTimes[decodingStatistics.DetectTimes.Count-1].ToString() +
                                            " DecodeTime: " + decodingStatistics.DecodeTimes[decodingStatistics.DecodeTimes.Count-1].ToString() +
                                            " When: " + Engine.E.SimulationTime.ToString()
                                            );
                                    }
                                }

                            }
                        }
                    }
                }
            });
        }

        public override void Init() {
            //Ez eredetileg a konstruktorban volt, de TypeInitializationExceptiont okozott, mert a kamerára akart feliratkozni úgy hogy még nem jött létre teljesen
            //A kamera által készített LedCaptured-ök alapján készítjük a detekciókat, várakozási sor, mint a frameknél
            TrackingLib.Engine.E.Camera.OnNewFrame += (frame) =>
            {
                if (frame.CapturedLeds.Count == 0)
                {
                    return;
                }
                CapturedLedQueue.Add(frame.CapturedLeds);
            };
        }

        //A Detect függvény a NextCandidate-ket feltöltő függvény
        public override void Detect()
        {
            //feltöltjük a detektált markereknél a PreviousCandidates-ket
            //ehhez végigmegyünk az összes detektált markeren

            //de előbb kitöröljük a nagyon régieket a listából, hiszen azok nem érdekelnek minket (most a 3 másodpercnél régebbiektől szabadulunk meg), optimalizálás céljából, enélkül is működik, hisz a dfs bejáró csak 14 mélységig megy, de feleslegesen ne tároljuk a régi detekciókat!
            DetectedMarkers.RemoveAll(dm => dm.Time < Engine.E.SimulationTime-30000);

            //végigmegyünk a régiektől megtisztított listán
            for (int i = DetectedMarkers.Count - 1; i >= 0; i--) //azért -1 mert az utolsónak nem tudunk candidate-t állítani, hisz nincs utolsó utáni elem
            {
                DetectedMarkers[i].NextCandidates.Clear(); //ha nincs, akkor bennemaradt pár
                DetectedMarkers[i].PreviousCandidates.Clear();

                //minden detektált markerre megnézzük az időben utána következőket, és a nextCandidates-be belerakjuk a gyanúsítottakat
                for (int j = i; j >= 0 ; j--)
                {
                    //ugyanazon a frame-en lévő két marker nem lehet egymás previous/nextCandidate-je
                    if (DetectedMarkers[i].FrameNumber == DetectedMarkers[j].FrameNumber)
                    {

                    }
                    else
                    {
                        //ha 100-nál közelebb van egy detektált marker, akkor azt jelöltnek nyilvánítjuk, persze csak akkor ha időben közel történt
                        //persze később itt a sebességet, orientációt is figyelembe fogom venni (vagy talán azt a hibafüggvényben kéne?)
                        int timeDifference = DetectedMarkers[i].Time - DetectedMarkers[j].Time;

                        //mivel időrendi sorrendben vannak a DetectedMarkersben, ezért egy bizonyos időlimit felett már abbahagyhatjuk a candidatek keresését, mert biztosan nem hozzá tartozik
                        if (timeDifference > 10000)
                        {
                            break;
                        }

                        if (timeDifference < TimeTolerance)
                        {
                            double distanceX = DetectedMarkers[j].Position.X - DetectedMarkers[i].Position.X;
                            double distanceY = DetectedMarkers[j].Position.Y - DetectedMarkers[i].Position.Y;
                            //pitagorasz tételel vizsgáljuk a két marker közötti távolságot
                            double distanceSq = distanceX * distanceX + distanceY * distanceY;// Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

                            if (distanceSq < DistanceTolerance*DistanceTolerance)
                            {
                                if (DetectedMarkers[i].PreviousCandidates.Contains(DetectedMarkers[j]) == false)
                                {
                                    DetectedMarkers[i].PreviousCandidates.Add(DetectedMarkers[j]);
                                    DetectedMarkers[j].NextCandidates.Add(DetectedMarkers[i]);
                                    continue;
                                }                       
                            }
                        }
                    }
                }

            }
        }
    }
}
       

