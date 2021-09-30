using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Egy levillogandó szimbólum, a CarID egy bitje villogásra transzformálva
    public class SymbolBase
    {
        public int ID;
        public virtual List<LedTiming> ToLedTimings()
        {
            List<LedTiming> list = new List<LedTiming>();
            list.Add(new LedTiming() { On = true, Time = SymbolTimings[ID].Item1 });
            list.Add(new LedTiming() { On = false, Time = SymbolTimings[ID].Item2 });
            return list;
        }

        public Dictionary<int, Tuple<int,int>> SymbolTimings;

        public virtual void RefreshSymbol() { }
    }
}
