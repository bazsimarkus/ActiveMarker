using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class CameraBase //Ez egy közös bázis osztály a kameráknak, hogy akár több, párhuzamosan működő, eltérő paraméterekkel rendelkező kamera is létrehozható legyen
    {
        public event Action<Frame> OnNewFrame;

             //Az elkészült Frame-k listája
             LinkedList<Frame> frames = new LinkedList<Frame>();
              public LinkedList<Frame> Frames
              {
                  get
                  {
                      lock (this)
                      {
                          return frames;
                      }
                  }
              }
              

        //Frame interval - mintavételezési periódusidő - az FPS reciproka, itt 40FPS
        public int FrameInterval = 250;

        //Expozíciós idő
        public int ExposureTime = 0;

        // A legutóbbi capture óta eltelt szimulációs időt tárolja
        public int CaptureCounter = 0;

        //A készített frame-eket számolja
        public int FrameCounter = 0;

        //a kirajzoláshoz is kell, az actualframe lesz megjelenítve, ennek a változása generál egy eventet
        public Frame ActualFrame = new Frame();

        //A kamerában is mindig lefut egy szimulációs lépés, maximum nem készít képet, de tudnia kell követni
        public virtual void CameraSimulationStep()
        {
            
        }

        //A képkészítést végző függvény, az érzékelt LED-eket eltárolja, adott esetben hibát is itt tudunk generálni
        public virtual void FrameCapture()
        {

        }

        //Külön függvény a képkészítés eventjének generálására, hisz a megjelenítőhöz/detektorhoz már csak a pozíció/idő információk jutnak el, kamerafüggetlenül
        protected void GenerateCaptureEvent() {
            //generálunk egy eventet a kirajzoláshoz
            if(OnNewFrame!=null) OnNewFrame?.Invoke(ActualFrame);
        }
    }
}
