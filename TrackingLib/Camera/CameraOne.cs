using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class CameraOne : CameraBase
    {
        //A nyitott zárt jelző változó (igaz, ha éppen exponálunk)
        public bool isExposing = true;

        public CameraOne()
        {
            FrameInterval = 250;
            ExposureTime = 0;
        }

        //A kamerában is mindig lefut egy szimulációs lépés, maximum nem készít képet, de tudnia kell követni
        public override void CameraSimulationStep()
        {
            if (CaptureCounter >= FrameInterval)
            {
                CaptureCounter = 0;
                FrameCapture();
                ActualFrame = null;
            }
            CaptureCounter += Engine.E.SimulationTimeStep;
        }

        //A képkészítést végző függvény, az érzékelt LED-eket eltárolja, adott esetben hibát is itt tudunk generálni
        public override void FrameCapture()
        {
            RandNum randnum = new RandNum(); //A randomszám generáláshoz példányosítunk egy osztályt

            bool extraMarkerError = false; // megjelenjenek-e néha az eredeti villanások mellett téves markerek?
            double extraMarkerErrorProbability = 0.5; // 0-1 közötti valószínűség érték
          
            bool takeOutError = false; // kihagyjunk-e néha pár markert?
            double takeOutErrorProbability = 0.2; // 0-1 közötti valószínűség érték

            foreach (var c in Engine.E.Cars)
            {
                //Az elméleti markerekhez hozzáadjuk mindig
                ActualFrame.TheoreticalMarkers.Add(new MarkerTheoretical(c));
                ActualFrame.DrawList.AddTheoreticalMarker(new DrawTheoreticalMarker(new MarkerTheoretical(c)));

                //Ideális esetben csak akkor csinál detektált markert, ha világít az autón a led
                if (c.IsLedOn == true)
                {
                    //Ha nincs eltűnés-hiba, azaz mindig amikor villannia kéne, tényleg villan
                    if (takeOutError == false)
                    {
                        //feltételezzük, hogy minden led-et sikerül detektálni, a capturedled a kulcs, a detektor, és a dekóder ezen adatok alapján dolgozik, a való életben a kamera lehet a dekódertől teljesen független, és ezek az adatok be akár soros üzenetként is beérkezhetnek a detektor/dekóder számára, itt különül el a szimulátor és a detektor/dekóder
                        ActualFrame.CapturedLeds.Add(new LedCaptured(new Point2D(c.ActualNode.Position2D.X, c.ActualNode.Position2D.Y), FrameCounter, Engine.E.SimulationTime));
                        ActualFrame.DrawList.AddCapturedLed(new DrawCapturedLed(new LedCaptured(new Point2D(c.ActualNode.Position2D.X, c.ActualNode.Position2D.Y), FrameCounter, Engine.E.SimulationTime)));

                        //Helyhibás detektálások, a helyes villanással egy időben megjelenik a közelben egy másik szellemvillanás is
                        if (extraMarkerError == true)
                        {
                            double generatedNumber = randnum.GenerateDouble();

                            if (generatedNumber < extraMarkerErrorProbability)
                            {
                                ActualFrame.CapturedLeds.Add(new LedCaptured(new Point2D(c.ActualNode.Position2D.X + randnum.GenerateDouble() * 10, c.ActualNode.Position2D.Y + randnum.GenerateDouble() * 10), FrameCounter, Engine.E.SimulationTime));
                                ActualFrame.DrawList.AddCapturedLed(new DrawCapturedLed(new LedCaptured(new Point2D(c.ActualNode.Position2D.X + randnum.GenerateDouble() * 10, c.ActualNode.Position2D.Y + randnum.GenerateDouble() * 10), FrameCounter, Engine.E.SimulationTime)));
                            }
                        }
                    }
                    else
                    {
                        double generatedNumber = randnum.GenerateDouble();
                        if (generatedNumber < takeOutErrorProbability)
                        {
                            //nem adunk hozzá, kihagyjuk a villanást, nem teszünk semmit
                        }
                        else
                        {
                            ActualFrame.CapturedLeds.Add(new LedCaptured(new Point2D(c.ActualNode.Position2D.X, c.ActualNode.Position2D.Y), FrameCounter, Engine.E.SimulationTime));
                            ActualFrame.DrawList.AddCapturedLed(new DrawCapturedLed(new LedCaptured(new Point2D(c.ActualNode.Position2D.X, c.ActualNode.Position2D.Y), FrameCounter, Engine.E.SimulationTime)));
                        }
                    }
                }
                else  //időhibás detektálások, olyankor világít amikor nem szabadna neki
                {

                }
            }
          //  Engine.E.Tracker.Step();
            FrameCounter++;

            //hozzáadjuk a frame-eket tartalmazó láncolt listánkhoz
            Frames.AddLast(ActualFrame);

            //generálunk egy eventet a kirajzoláshoz
            GenerateCaptureEvent();
        }
    }
}
