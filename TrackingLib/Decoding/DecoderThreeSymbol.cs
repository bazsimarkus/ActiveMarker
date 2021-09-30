using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DecoderThreeSymbol : DecoderBase
    {
        public DecoderThreeSymbol()
        {
        }

        bool writeDebugInfoToConsole = false;

        //Egy szekvenciát dekódoló függvény
        public override void Decode(MarkerSequence sequenceparam) //gyakorlatilag olyan mint a SimulationStep, csak framenként
        {
            double DecodingError = 0;
            DecodingBuffer.Clear();
            OneZeroNumbers.Clear();

            //a szekvenciából időzítéslista előállítása
            for (int i = 0; i < sequenceparam.MarkerSymbols.Count - 1; i++)
            {
                if (sequenceparam.MarkerSymbols[i].Value == 1)
                {
                    int numberOfOnes = 0;
                    do { numberOfOnes++; }
                    while (sequenceparam.MarkerSymbols[i + numberOfOnes].Value != 0);

                    //   if (numberOfOnes > 2) Console.WriteLine("CODE ERROR");

                    OneZeroNumbers.Add(numberOfOnes);
                    //       Console.WriteLine("numO:" + numberOfOnes.ToString());

                    int numberOfZeros = 0;
                    do { numberOfZeros++; }
                    while (sequenceparam.MarkerSymbols[i + numberOfZeros].Value != 1);
                    numberOfZeros--; //mert az utolsó lefutást ki kell vonnunk

                    OneZeroNumbers.Add(numberOfZeros);
                    //        Console.WriteLine("numZ:" + numberOfZeros.ToString());
                }
            }

            //a dekódololó buffer feltöltése, esetleges időhiba megállapítása
            for (int j = 0; j < OneZeroNumbers.Count; j++)
            {
                //1-esek számát vizsgáljuk (ez mindig 1)
                if (j % 2 == 0)
                {

                }
                //0-ások számát vizsgáljuk (ez lehet vagy 3 vagy 6)
                else if (j % 2 == 1)
                {
                    if (0 < OneZeroNumbers[j] && OneZeroNumbers[j] < 5)
                    {
                        DecodingBuffer.Add(0);
                        DecodingError += Math.Abs((OneZeroNumbers[j] - 3));
                    }
                    else if (4 < OneZeroNumbers[j] && OneZeroNumbers[j] < 8)
                    {
                        DecodingBuffer.Add(1);
                        DecodingError += Math.Abs((OneZeroNumbers[j] - 6));
                    }
                    else if (7 < OneZeroNumbers[j] && OneZeroNumbers[j] < 12)
                    {
                        DecodingBuffer.Add(2);
                        DecodingError += Math.Abs((OneZeroNumbers[j] - 9));
                    }
                }
            }

            //Console.Write("Onezero: ");
            //foreach (var x in OneZeroNumbers)
            //{
            //    Console.Write(x.ToString());
            //}

            //Console.Write("Decoded sequence: ");
            //foreach (var x in DecodingBuffer)
            //{
            //    Console.Write(x.ToString());
            //}
            ////az előbb bináris formába dekódolt üzenetet most átalakítjuk olvasható formába, 14 bites egy üzenet, lást SymbolListOne
            //Console.WriteLine();

            if (DecodingBuffer.Count >= 13) //azért 13, mert bár két stopbit van, a második csak azért kell hogy az elsőt lezárjuk
            {
                if (DecodingBuffer[12] == 1) // 1-esből 0-ba való átmenet a stopbit és startbit, csak ezeket az eseteket kell vizsgálnunk
                {
                    int[] usefulbits = new int[10];
                    for (int k = 0; k < 10; k++)
                    {
                        // Console.WriteLine("decodingbuffer[i]: " + DecodingBuffer[k].ToString());
                        usefulbits[k] = DecodingBuffer[k];
                    }
                    int decodedInteger = BitListToInt(usefulbits);
                    DecodedValue decodedValue = new DecodedValue(decodedInteger, DecodingError);

                    // Paritásbitek ellenőrzése
                    int symbolNumber = 3;
                    int ParitySymbolCount = 2;
                    int SumEven = 0; // a paritásbit meghatározásához számoljuk, hány 1-es bit volt
                    int SumOdd = 0; // a paritásbit meghatározásához számoljuk, hány 1-es bit volt

                    int Parity1 = 0;
                    int Parity2 = 0;

                    for (int i = 0; i < usefulbits.Length; i++)
                    {
                        if (i % ParitySymbolCount == 0)
                        {
                            SumEven += usefulbits[i];
                        }
                        else
                        {
                            SumOdd += usefulbits[i];
                        }
                    }

                    Parity1 = (SumEven % symbolNumber);
                    Parity2 = (SumOdd % symbolNumber);

                    //   Console.WriteLine("Candidate integer to decode: " + decodedInteger.ToString());
                    //        Console.Write("Result: ");
                    if (DecodingBuffer[10] == Parity1 && DecodingBuffer[11] == Parity2)
                    {
                        if (SuccessfullyDecodedValues.Contains(decodedValue) == false)
                        {
                            SuccessfullyDecodedValues.Add(decodedValue);
                            if (writeDebugInfoToConsole)
                            {
                                Console.Write("Decoded sequence: ");
                                foreach (var x in DecodingBuffer)
                                {
                                    Console.Write(x.ToString());
                                }
                                //az előbb bináris formába dekódolt üzenetet most átalakítjuk olvasható formába, 14 bites egy üzenet, lást SymbolListOne
                                Console.WriteLine();
                            }

                        }
                        else
                        {
                            if (writeDebugInfoToConsole) Console.WriteLine("Parity not correct");
                        }
                    }
                    else
                    {
                        if (writeDebugInfoToConsole) Console.WriteLine("Parity format error");     
                    }

                }
            }

        }

        //több szekvencia listáját egyszerre dekódoló függvény
        public override double DecodeSequenceList(List<MarkerSequence> sequencelist)
        {
            foreach (var markersequence in sequencelist)
            {
                if (writeDebugInfoToConsole)
                {
                    Console.WriteLine("Sequence added: ");
                    foreach (var markersymbol in markersequence.MarkerSymbols)
                    {
                        Console.Write(markersymbol.Value.ToString());
                    }
                    Console.WriteLine();
                }
                Decode(markersequence);
            }
            return 1;
        }

        //Egy adott szekvenciában az eredeti üzenet bitjeinek számát visszaadó függvény (gyakorlatilag 1-0 átmeneteket számoló függvény)
        public override int GetBitNumberOfSequence(MarkerSequence sequenceparam)
        {
            int BitCounter = 0;
            for (int i = 1; i < sequenceparam.MarkerSymbols.Count - 1; i++)
            {
                if (sequenceparam.MarkerSymbols[i].Value == 0 && sequenceparam.MarkerSymbols[i - 1].Value == 1)
                {
                    BitCounter++;
                }
            }
            return BitCounter;
        }

        //Kettes számrendszerbeli számot reprezentáló, nullásokat és egyeseket tartalmazó tömböt integerré konvertáló függvány
        int BitListToInt(int[] listparam)
        {
            int a = 0;
            for (int i = 0; i < listparam.Length; i++)
            {
                // Console.WriteLine("listparam[i]: " + listparam[i].ToString());
                a = a + listparam[i] * (int)Math.Pow(10, i);
            }
            // Console.WriteLine("Bitlist: " + a.ToString());
            return a;
        }
    }
}