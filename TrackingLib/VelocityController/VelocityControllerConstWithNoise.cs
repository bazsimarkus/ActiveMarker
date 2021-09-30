using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class VelocityControllerConstWithNoise:VelocityControllerBase
    {
        double NoiseAmplitude = 0;

        //konstruktor
        public VelocityControllerConstWithNoise(double speedparam, double noiseAmplitude) {
            NoiseAmplitude = noiseAmplitude;
            controlledSpeed = speedparam;
        }

     //sebesség szabályzó
        public override void SetSpeed(Car car) {
            RandNum randNum = new RandNum();
            //0.8 és 1.2 közötti értékkel szorozza meg a jelenlegi értéket
            car.Speed = controlledSpeed + (randNum.GenerateDouble()*NoiseAmplitude - NoiseAmplitude/2);
        }
    }
}
