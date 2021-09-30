using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DetectorBase
    {
        //A detektált marker, a kamera tolja ki magából ebbe a listába a captured ledek alapján a detectionöket, amikben szerepel az idő, és a framenumber is
        public List<DetectedMarker> DetectedMarkers = new List<DetectedMarker>();

        //Az egyes kódszekvenciák, amik közül vannak jobb és rosszabb kódsorozatok, a hibafüggvény mondja meg melyik a legjobb
        public List<MarkerSequence> MarkerSequences = new List<MarkerSequence>();

        public virtual void Init() { } //A detektor event feliratkozásaitkezelő függvény, az engine hívja meg egyszer a szimuláció elején

        public virtual void Detect() { }  //A teljes detekciós folyamatot kezelő függvény (A Detect függvény a NextCandidate-ket feltöltő függvény)
    }
}
