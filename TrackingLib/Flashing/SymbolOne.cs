using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class SymbolOne:SymbolBase
    {
        public SymbolOne()
        {
            SymbolTimings = new Dictionary<int, Tuple<int, int>>() { //Az első szám a bekapcsolt idő, a második a szünet hossza
                {0, new Tuple<int, int>(250,750) },
                {1, new Tuple<int, int>(250,1500) },
                {2, new Tuple<int, int>(250,2250) },
            };
        }
    }
}
