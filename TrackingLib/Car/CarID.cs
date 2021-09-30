using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //Az autó egyedi azonosító osztálya
    public class CarID
    {
        public int ID; //Az azonosítót tároló változó, a rendszer bővítésekor változhat, ezért is van külön osztályban

        //Konstruktor
        public CarID(int id)
        {
            ID = id;
        }

        //Az azonosító kiolvasása stringként, kijelzéshez
        public override string ToString()
        {
            return "Car ID: " + ID.ToString();
        }
    }
}
