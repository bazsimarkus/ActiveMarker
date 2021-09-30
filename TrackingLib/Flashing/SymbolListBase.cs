using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //A SymbolBase osztályok listája, a CarID villogásra transzformálva
    public class SymbolListBase
    {
        public List<SymbolBase> Symbols = new List<SymbolBase>();
       
        public List<LedTiming> ToLedTimings()
        {
            List<LedTiming> timings = new List<LedTiming>();

            //Az egyes szimbólumokhoz tartozó különálló LedTiming időzítésekhez most
            foreach (var s in Symbols)
            {
                timings.AddRange(s.ToLedTimings());
            }

            //egyszerűsítés -> on, 1sec; on 1 sec -> on 2sec
            return timings;
        }

        //GetBit működése: ha bitNumber 0, akkor a legkisebb helyiértékű bitet kapjuk meg, ha 1, a második legkisebb, és így tovább
        public int GetBit(int b, int bitNumber)
        {
            var bit = (b & (1 << bitNumber)) != 0;
            if (bit == true) return 1;
            else return 0;
        }
    }
}
