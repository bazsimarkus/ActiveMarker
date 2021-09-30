using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    //A képkocka osztály
    public class Frame
    {
        //a "valós" világban történtek az engine - cars párosban, míg a percepció a frame-marker párosban van tárolva - ezek az adatok mennek át a visualizerhez!

        //néhai MarkerDetected, most LedCaptured, ez egy villanás, amit a kamera felfogott és a frame-be tett.
        public List<LedCaptured> CapturedLeds = new List<LedCaptured>();
        //Elméleti markerek, a Cars lista másolata, neki van speedje, orientációja, stb.
        public List<MarkerTheoretical> TheoreticalMarkers = new List<MarkerTheoretical>();

        //A kirajzoláshoz szükséges lista, hogy ne  aközvetlen frame-adatokat kérjük le a visualizerben, hisz később nem csak szimulációnál lesz használva
        public DrawList DrawList = new DrawList();
    }
}
