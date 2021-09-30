using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TrackingLib;

namespace TrackingLib
{
    //Egy Frame-en a kirajzolandó dolgokat tartalmazó gyűjtőosztály
    public class DrawList
    {
        public static double centerX = 600;
        public static double centerY = 350;

        public List<DrawCapturedLed> CapturedLeds = new List<DrawCapturedLed>();
        public List<DrawTheoreticalMarker> TheoreticalMarkers = new List<DrawTheoreticalMarker>();
        public List<DrawPath> Paths = new List<DrawPath>();
        public DrawList() {

        }

        public void AddCapturedLed(DrawCapturedLed d)
        {
            CapturedLeds.Add(d);
        }

        public void AddTheoreticalMarker(DrawTheoreticalMarker t)
        {
            TheoreticalMarkers.Add(t);
        }

        public void AddPath(DrawPath p)
        {
            Paths.Add(p);
        }
    }
}
