using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Egy adott led-állapot (ki-be) időzítését, hosszát leíró osztály. Egy szimbólum kettő ilyenből áll (először a be, majd a ki). Egy villogássorozat ilyenekből épül fel
    public class LedTiming
    {
        public bool On;
        public int Time;
    }
}
