using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class RandNum
    {
        //azért van szükség külön statikus Random számra, hogy az egyes kocsiknál külön-külön szám generálódjon
        // ha a Random.Next()-et használnánk, a rövid seed idő miatt gyakori generálás esetén mindig ugyanazt az értéket kapnánk
        //így viszont mindig új hívódik

        private static Random rnd;
        static RandNum()
        {
            rnd = new Random(Guid.NewGuid().GetHashCode());
        }

        //0 és 1 közti double generálása
        public double GenerateDouble()
        {
            double r1 = rnd.NextDouble();
            return r1;
        }

        //0 és maxVal közti double generálása
        public int GenerateInt(int maxVal)
        {
            int r1 = rnd.Next(0,maxVal);
            return r1;
        }
    }
}
