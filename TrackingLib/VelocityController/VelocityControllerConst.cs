using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class VelocityControllerConst:VelocityControllerBase
    {

        public VelocityControllerConst(double speedparam) {
            controlledSpeed = speedparam;
        }

        public override void SetSpeed(Car car) {
            car.Speed = controlledSpeed;
        }
    }
}
