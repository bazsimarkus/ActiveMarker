using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class VelocityControllerBase
    {
        public double controlledSpeed = 0;
        //lásd leszármazottak!
        public virtual void SetSpeed(Car car) { }
    }
}
