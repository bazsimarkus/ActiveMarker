using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class Point2D
    {
        public double x;
        public double y;

        //konstruktorok
        public Point2D()
        {
            this.x = 0;
            this.y = 0;
        }

        public Point2D(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        //desktruktor
        ~Point2D()
        {
            this.x = 0;
            this.y = 0;
        }

        // üres-e?
        public bool IsEmpty
        {
            get
            {
                return x == 0 && y == 0;
            }
        }

        // X property
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        // Y property
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        // operátor túlterhelések

        public static Point2D operator +(Point2D pt1, Point2D pt2)
        {
            return Add(pt1, pt2);
        }

    
        public static Point2D operator -(Point2D pt1, Point2D pt2)
        {
            return Subtract(pt1, pt2);
        }

        //operátor függvények

        public static Point2D Add(Point2D pt1, Point2D pt2)
        {
            return new Point2D(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static Point2D Subtract(Point2D pt1, Point2D pt2)
        {
            return new Point2D(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Point2D Ceiling(Point2D value)
        {
            return new Point2D((int)Math.Ceiling(value.X), (int)Math.Ceiling(value.Y));
        }

        public static Point2D Truncate(Point2D value)
        {
            return new Point2D((int)value.X, (int)value.Y);
        }

        public static Point2D Round(Point2D value)
        {
            return new Point2D((int)Math.Round(value.X), (int)Math.Round(value.Y));
        }

        // origóhoz viszonyított szög radiánban
        public double GetDistanceFromPoint(Point2D value)
        {
            return Math.Sqrt((this.X-value.X)*(this.X - value.X) + (this.Y - value.Y) * (this.Y - value.Y));
        }

        // origóhoz viszonyított szög radiánban
        public double GetAngleToOriginRadian()
        {
            double deltaX = this.x;
            double deltaY = this.y;
            double rad;
                if (deltaY > 0)
                {
                     rad = Math.Atan2(deltaY, deltaX);
                }
                else
                {
                    rad = 2*Math.PI + Math.Atan2(deltaY , deltaX);
                }
            return rad;
        }

        // origóhoz viszonyított szög fokban
        public double GetAngleToOriginDegrees()
        {
            double deltaX = this.x;
            double deltaY = this.y;
            double deg;

            if (deltaY > 0)
            {
                deg = Math.Atan2(deltaY , deltaX) * (180 / Math.PI);
            }
            else
            {
                deg = 360 + Math.Atan2(deltaY,deltaX) * (180 / Math.PI);
            }
            return deg;
        }

        // pont offsetelése adott x-y értékkel
        public void Offset(double dx, double dy)
        {
            X += dx;
            Y += dy;
        }

        // pont offsetelése egy másik ponttal
        public void Offset(Point2D p)
        {
            Offset(p.X, p.Y);
        }

        // pont koordinátáinak kinyerése stringként
        public override string ToString()
        {
            return "{X=" + X.ToString("N2") + ",Y=" + Y.ToString("N2") + "}";
        }
    }
}
