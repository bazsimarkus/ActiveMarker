using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Egy dekódolt értéket tároló osztály, több ilyen közül választjuk ki a legjobbat
    public class DecodedValue
    {
        public int Value; //Maga az érték
        public double Error; //A dekódolás hibája

        public DecodedValue(int value, double error){
            Value = value;
            Error = error;
        }
    }
}
