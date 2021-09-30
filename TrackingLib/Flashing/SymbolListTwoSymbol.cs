using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class SymbolListTwoSymbol : SymbolListBase
    {
        //paraméterek
        int symbolNumber = 2;
        public int UsefulSymbolCount = 10;
        public int ParitySymbolCount = 2;
        public int StopSymbolCount = 2;

        public SymbolListTwoSymbol(CarID id)
        {
            int IdToCode = id.ID;
            int SumEven = 0; // a paritásbit meghatározásához számoljuk, hány 1-es bit volt
            int SumOdd = 0; // a paritásbit meghatározásához számoljuk, hány 1-es bit volt

            //Useful Symbols
            for (int i = 0; i < UsefulSymbolCount; i++)
            {
                Symbols.Add(new SymbolOne() { ID = GetBit(IdToCode, i) });
                if (i % ParitySymbolCount == 0)
                {
                    SumEven += GetBit(IdToCode, i);
                }
                else
                {
                    SumOdd += GetBit(IdToCode, i);
                }
            }

            //Parity Symbols - kétszimbóumos eset
            // ha páros számú 1es van a 0dik, 2ik, 4ik... helyen , a ParityBit1 0 lesz
            // ha páratlan számú 1es van a 0dik, 2ik, 4ik... helyen , a ParityBit1 1 lesz
            Symbols.Add(new SymbolOne() { ID = (SumEven % symbolNumber) });
            // ha páros számú 1es van a 1, 3, 5... helyen , a ParityBit2 0 lesz
            // ha páratlan számú 1es van a 1, 3, 5... helyen , a ParityBit2 1 lesz 
            Symbols.Add(new SymbolOne() { ID = (SumOdd % symbolNumber) });


            //Stop Symbols
            Symbols.Add(new SymbolOne() { ID = 1 });
            Symbols.Add(new SymbolOne() { ID = 1 });

        }

    }
}
