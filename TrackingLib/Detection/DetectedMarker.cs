using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //A felfogott, követett marker (néhai FollowedMarker)
    //A kamera tolja ki magából a captured ledek alapján a detectionöket, amikben szerepel az idő, és a framenumber is
    public class DetectedMarker
    {
        //idő
        public int Time;
        //képkockaszám
        public int FrameNumber;
        //pozíció
        public Point2D Position = new Point2D(0, 0);

        // public bool Skip = false; Ha egy adott bejárást nem akarunk mindig megejteni, ezt használhatjuk
        public int Depth = 0;

        public DetectedMarker(int time, int framenumber, Point2D position) {
            Time = time;
            FrameNumber = framenumber;
            Position = position;
            Position = position;
        }

        public MarkerSymbol ToMarkerSymbol(int value) {
            MarkerSymbol markersymbol = new MarkerSymbol(value, Time, FrameNumber, Position);
            return markersymbol;
        }

        public List<DetectedMarker> NextCandidates = new List<DetectedMarker>(); //A lehetséges következő jelöltek
        public List<DetectedMarker> PreviousCandidates = new List<DetectedMarker>();//A lehetséges előző jelöltek
    }
}
