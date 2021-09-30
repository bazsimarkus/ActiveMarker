using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Egy autó villogója
    public class Flasher
    {
        public int flashingTime = 0; //Az adott bit világítási, vagy nemvilágítási idejét mérő változó

        bool gap = false;
        int gapTime = 0;
        int desiredGap = 5000;

        int currentBit = 0;

        public SymbolListBase SymbolListToFlash;
        public List<LedTiming> LedTimings;

        //Bool, de akár int is lehet, ha pl RGB LED színeket akarunk, persze ehhez a villogó többi részét is fejleszteni kell
        public bool isLedOn = false;

        public Flasher(SymbolListBase symbollist)
        {
            SymbolListToFlash = symbollist;
            LedTimings = SymbolListToFlash.ToLedTimings();
        }

        public void SimulationStep()
        {
            //ha pl az 1-es bithez 0.2 bitidő (szimbólumidő) tartozik, akkor 0.2 másodpercig aktív lesz a LED
            //majd ha utána egy 0-ás bit következik, amihez 0.1 bitidő tartozik, akkor a nemvilágítás csak 0.1 ideig fog tartani
            //ha az egyik bittel végeztünk, a flashingTime nullázódik, és a currentBit-hez hozzáadunk egyet, így már a következő bitet vizsgáljuk

            //megnézzük, át kell-e ugranunk a következő bitre, azaz levillogtuk-e már az aktuálisat
            if (gap == false)
            {
                //Azért elöl kell lennie, mert amíg hátul volt csak a bitváltások után történt meg a villanás! így pl az első frame-n nem szerepelt
                if (LedTimings[currentBit].On == true)
                {
                    isLedOn = true;
                }
                else
                {
                    isLedOn = false;
                }

                if (flashingTime >= LedTimings[currentBit].Time)
                {
                    flashingTime = 0;
                    currentBit++;
                    RefreshSymbols();
                    if (currentBit == LedTimings.Count)
                    {
                        gap = true;
                        isLedOn = false;
                        currentBit = 0; //ha az üzenet végére értünk, 0-ra állítjuk az iterátorunkat, hogy újból kezdje a villogást
                        
                        return;
                    }
                }
              
                flashingTime += Engine.E.SimulationTimeStep;
            }

            if (gap == true)
            {
                isLedOn = false;
                flashingTime = 0;
                if (gapTime >= desiredGap)
                {
                    gapTime = 0;
                    gap = false;
                }
                gapTime += Engine.E.SimulationTimeStep;
            }
        }

        public void RefreshSymbols()
        {
            SymbolListBase tempSymbolList = SymbolListToFlash;
            List<LedTiming> tempLedTimings;

            foreach (var sym in tempSymbolList.Symbols)
            {
                sym.RefreshSymbol();
            }

            tempLedTimings = tempSymbolList.ToLedTimings();

            LedTimings = tempLedTimings;
            SymbolListToFlash = tempSymbolList;
        }
     
    }
    
}
