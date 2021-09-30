using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DecoderBase
    {
        //A szimbólumlista
        public List<int> DecodingBuffer = new List<int>();
        //Az időzítéslista
        public List<int> OneZeroNumbers = new List<int>();
        //A sikeresen dekódolt értékek listája
        public List<DecodedValue> SuccessfullyDecodedValues = new List<DecodedValue>();

        //Egy szekvenciát dekódoló függvény
        public virtual void Decode(MarkerSequence frameparam)
        {
         //   return -1;
        }

        public virtual double DecodeSequenceList(List<MarkerSequence> sequencelist)
        {
            return -1;
        }

        public virtual int GetBitNumberOfSequence(MarkerSequence sequenceparam)
        {
            return 0;
        }

        //A legjobb dekódolt értékeket visszaadó függvény, ha több dekódolt értékünk is volt, ráadásul ezek közül azonos értékűek is, akkor a legjobb dekódolásokat adja vissza
        public List<DecodedValue> getBestDecodedValues()
        {
            List<DecodedValue> bestvalues = new List<DecodedValue>();
            if (SuccessfullyDecodedValues.Count > 0)
            {
                //Ha több azonos, különböző hibával dekódolt érték van, akkor mindegyikből egyet hozzáadunk, és megkeressük a legalacsonyabb hibájú dekódolást
                foreach (var value in SuccessfullyDecodedValues)
                {
                    DecodedValue decodedvalue = new DecodedValue(value.Value, 0); //Először nullával adjuk hozzá, hogy csak egy darab legyen minden értékből
                    if(bestvalues.Contains(decodedvalue)==false) bestvalues.Add(decodedvalue);
                }

                foreach (var valuetofind in bestvalues)
                {
                    //Az első dekódolás hibáját beállítjuk minden értékhez
                    valuetofind.Error = SuccessfullyDecodedValues.Find(dm => dm.Value == valuetofind.Value).Error;
                    //Ha találtunk egy számhoz olyan dekódolást, ami alacsonyabb hibával rendelkezik, akkor a hibát felülírjuk.
                    foreach (var value in SuccessfullyDecodedValues)
                    {
                        if (value.Value == valuetofind.Value && valuetofind.Error < value.Error) valuetofind.Error = value.Error;
                    }
                }
            }
            return bestvalues;
        }
    }
}
