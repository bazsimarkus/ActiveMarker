using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class SymbolOneWithError:SymbolBase
    {
        RandNum randnum = new RandNum();
        public SymbolOneWithError()
        {
            SymbolTimings = new Dictionary<int, Tuple<int, int>>() { //Az első szám a bekapcsolt idő, a második a szünet hossza
                {0, new Tuple<int, int>(250,750+(randnum.GenerateInt(3)-1)*250)},
                {1, new Tuple<int, int>(250,1500+(randnum.GenerateInt(3)-1)*250) },
                {2, new Tuple<int, int>(250,2250+(randnum.GenerateInt(3)-1)*250) },
            };
        }

        public override void RefreshSymbol()
        {
            SymbolTimings = new Dictionary<int, Tuple<int, int>>() { //Az első szám a bekapcsolt idő, a második a szünet hossza
                {0, new Tuple<int, int>(250,750+(randnum.GenerateInt(3)-1)*250)},
                {1, new Tuple<int, int>(250,1500+(randnum.GenerateInt(3)-1)*250) },
                {2, new Tuple<int, int>(250,2250+(randnum.GenerateInt(3)-1)*250) },
            };
        }
    }
}
