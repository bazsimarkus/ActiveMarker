using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DrawCapturedLed
    {
        public Point2D Position = new Point2D();

        public DrawCapturedLed(LedCaptured ledcaptured)
        {
            Position = ledcaptured.Position;
        }
    }
}
