using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //A kamera által érzékelt villanás
    public class LedCaptured
    {
        //Csak pozíciót és időt tudunk, de ez elég is mindenre
        public Point2D Position = new Point2D();
        public int FrameNumber;
        public int Time;

        public LedCaptured(Point2D position, int framenumber, int time)
        {
            Position = position;
            FrameNumber = framenumber;
            Time = time;
        }

    }
}
