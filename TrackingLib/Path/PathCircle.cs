using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class PathCircle:PathBase
    {

        double Perimeter;
        double requiredNumberOfNodes;
        double angleStep;

        //Normalizált verzió
        //radius: a kerület pixelben megadva!
        //stepsize: egy szimulációs lépés alatt hány pixelt ugorjon az autó a pályán?
        //pl ha 1 pixel a stepsize, és 360 pixel a kerület, akkor pont 360 szim. lépés alatt ér körbe

        public override void CreatePath(double radius, double stepsize, double xcenter, double ycenter, double noiseamplitude)
        {
            RandNum randnum = new RandNum();

            Perimeter = 2 * radius * System.Math.PI;

            //a pixelben megadott stepsize-t át kell alakítanunk a parametrikus egyenletben fok-ra!

            requiredNumberOfNodes = Perimeter / stepsize; //hány darab Node-ból fog állni a körpályánk?
            angleStep = 360 / requiredNumberOfNodes; // hány fokonként kell léptetni a paramétert, hogy pont annyi Node legyen a végén, amennyit az előbb számoltunk?

            //kör parametrikus egyenlete alapján =>  x=x_offset+r*cos(teta) | y=y_offset+r*cos(teta)
            //teta a paraméter, végigmegyünk a körön 0-tól 360 fokig a kívánt lépésnagysággal
            int indexer = 0; // hogy legyen egy lépésszámláló, ami alapján az újonnan hozzáadott Node-okat indexeljük
            for (double i = 0; i < 360; i += angleStep)
            {
                double angle = i * System.Math.PI / 180; //átszámoljuk radiánba

                double noiseX = 0;
                double noiseY = 0;

                //sugárirányú pályazaj meghatározása, merőleges mindig az adott pontra
                if (angle <= 90)
                {
                    noiseX = (90 - i) / 90;
                    noiseY = i / 90;
                }
                if (i > 90 && i <= 180)
                {
                    noiseX = (i - 90) / 90;
                    noiseY = (i - 180) / 90;
                }
                if (i > 180 && i <= 270)
                {
                    noiseX = (270 - i) / 90;
                    noiseY = (i - 180) / 90;
                }
                if (i > 270 && i < 360)
                {
                    noiseX = (i - 270) / 90;
                    noiseY = (i - 360) / 90;
                }
         
                double linearNoise =  (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter + radius * System.Math.Cos(angle) + noiseX * linearNoise; //kör parametrikus egyenletrendszere       
                double y = ycenter + radius * System.Math.Sin(angle) + noiseY * linearNoise;

                Point2D temp_point2D = new Point2D(x, y);
                //temp_point2D.Offset(-xcenter, -ycenter);
                double orientation = temp_point2D.GetAngleToOriginRadian();

                PathNode temp_node = new PathNode(new Point2D(x, y), orientation, null);
                Nodes.Add(temp_node);

                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
                indexer++;
            }

            Nodes[indexer - 1].NextNode = Nodes[0]; //hogy a pálya utolsó eleme az elsőre mutasson, így lesz végtelen a pálya
        }
    }
}
