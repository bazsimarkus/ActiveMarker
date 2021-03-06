using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SymbolListThreeSymbol:SymbolListBase
    {
        //paraméterek
        int symbolNumber = 3;
        public int UsefulSymbolCount = 10;
        public int ParitySymbolCount = 2;
        public int StopSymbolCount = 2;

        public SymbolListThreeSymbol(List <int> symbolstoflash)
        {   
            int SumEven = 0; // a paritásszimbólumok meghatározásához számoljuk
            int SumOdd = 0; // a paritás meghatározásához számoljuk

            //Useful Symbols
            for (int i = 0; i < UsefulSymbolCount; i++)
            {
                Symbols.Add(new SymbolOne() { ID = symbolstoflash[i] });
                if (i % ParitySymbolCount == 0)
                {
                    SumEven += symbolstoflash[i];
                }
                else
                {
                    SumOdd += symbolstoflash[i];
                }
            }

            //Parity Symbols

            Symbols.Add(new SymbolOne() { ID = (SumEven % symbolNumber) });
            Symbols.Add(new SymbolOne() { ID = (SumOdd % symbolNumber) });


            //Stop Symbols
            Symbols.Add(new SymbolOne() { ID = 1 });
            Symbols.Add(new SymbolOne() { ID = 1 });

        }

    }
}
