using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class PathInfinite:PathBase
    {
        double arcLength;
        double requiredNumberOfNodes;
        double angleStep;

        //szinuszfüggvény ívhosszának számítása elliptikus integrál segítségével
        double SineArcLength(double lowerparam, double upperparam)
        {
            double lower = lowerparam;
            double upper = upperparam;
            double arclen = 0.0;    
            double inc = 0.000001; //felosztás
            //elliptikus integrál diszkretizálása
            while (lower < upper)
            {
                //kis téglalapok numerikus összegzése
                arclen += (inc * Math.Sqrt(1.0 + (Math.Sin(lower)) * (Math.Sin(lower))));
                lower += inc;
            }
            Console.WriteLine("ArcLength: " + arclen.ToString());
            return arclen;
        }

        //size: a hullámfüggvény amplitúdója (alap szinusznál 1-től -1ig terjed, ezt szorozza az érték, ha pl 4, akkor -4-től +4ig terjed, és a hossza is ennek megfelelően változik)      

public override void CreatePath(double size, double stepsize, double xcenter, double ycenter, double noiseamplitude)
        { 
    PathNode prev_node = new PathNode();

    arcLength = SineArcLength(0, 2 * Math.PI) * size;

    requiredNumberOfNodes = arcLength / stepsize; //hány darab Node-ból fog állni a pályánk?
    angleStep = 360 / requiredNumberOfNodes;

    RandNum randnum = new RandNum();

    int indexer = 0;

    for (double i = -180; i <= 180; i += angleStep)
    {

        double angle = i * System.Math.PI / 180;
                double noiseX = 0;
                double noiseY = 0;

                if (i <= -90)
                {
                    noiseX = Math.Sin(angle) / 2 + 0.5;
                    noiseY = -Math.Sin(angle) / 2 + 0.5;
                }
                if (i > -90 && i <= 0)
                {
                    noiseX = Math.Sin(angle) / 2 + 0.5;
                    noiseY = Math.Sin(angle) / 2 - 0.5;
                }
                if (i > 0 && i <= 90)
                {
                    noiseX = -Math.Sin(angle) / 2 + 0.5;
                    noiseY = -Math.Sin(angle) / 2 - 0.5;
                }
                if (i > 90 && i < 180)
                {
                    noiseX = -Math.Sin(angle) / 2 + 0.5;
                    noiseY = Math.Sin(angle) / 2 + 0.5;
                }


                double linearNoise = (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter + (0 + angle) * size + noiseX* linearNoise;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = ycenter + size * (System.Math.Sin(0 + angle)) + noiseY *linearNoise;

        Point2D temp_point2D = new Point2D(x, y);
        double orientation = temp_point2D.GetAngleToOriginRadian();

        PathNode temp_node = new PathNode(new Point2D(x, y), orientation, null);


        Nodes.Add(temp_node);
        if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
        indexer++;
    }


    for (double i = 180; i >= -180; i -= angleStep)
    {

        double angle = i * System.Math.PI / 180;

                double noiseX = 0;
                double noiseY = 0;

                if (i <= -90)
                {
                    noiseX = Math.Sin(angle) / 2 + 0.5;
                    noiseY = Math.Sin(angle) / 2 - 0.5;
                }
                if (i > -90 && i <= 0)
                {
                    noiseX = Math.Sin(angle) / 2 + 0.5;
                    noiseY = -Math.Sin(angle) / 2 + 0.5;
                }
                if (i > 0 && i <= 90)
                {
                    noiseX = -Math.Sin(angle) / 2 + 0.5;
                    noiseY = Math.Sin(angle) / 2 + 0.5;
                }
                if (i > 90 && i < 180)
                {
                    noiseX = -Math.Sin(angle) / 2 + 0.5;
                    noiseY = -Math.Sin(angle) / 2 - 0.5;
                }

                double linearNoise = (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter + angle * size + noiseX* linearNoise;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = ycenter + System.Math.Cos(Math.PI / 2 + angle) * size + noiseY * linearNoise;

        Point2D temp_point2D = new Point2D(x, y);
        double orientation = temp_point2D.GetAngleToOriginRadian();

        PathNode temp_node = new PathNode(new Point2D(x, y), orientation, null);

        Nodes.Add(temp_node);
        if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
        indexer++;
    }

    Nodes[indexer - 1].NextNode = Nodes[0];
}

    }
}
