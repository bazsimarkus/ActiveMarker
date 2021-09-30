using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class PathRectangle:PathBase
    {
        //size: a téglalap rövidebbik oldala
        //stepsize: hány pixelenként legyen egy node

        public override void CreatePath(double size, double stepsize, double xcenter, double ycenter, double noiseamplitude)
        {
            PathNode prev_node = new PathNode();

            //itt nem kell kiszámolni, hány node-ból fog állni a pályánk, hiszen az egyenes pálya miatt a for ciklusban lehet a stepsize-al iterálni

            int indexer = 0;

            RandNum randnum = new RandNum();

            for (double i = -size; i < +size; i += stepsize)
            {
                double linearNoise = (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter + i;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = ycenter + size / 2 + linearNoise;

                Point2D temp_point2D = new Point2D(x, y);
                double orientation = temp_point2D.GetAngleToOriginRadian();

                PathNode temp_node = new PathNode(new Point2D(x, y), orientation, null);


                Nodes.Add(temp_node);
                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
                indexer++;
            }



            for (double i = size / 2; i > -size / 2; i -= stepsize)
            {
                double linearNoise = (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter + size + linearNoise;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = ycenter + i;

                Point2D temp_point2D = new Point2D(x, y);
                double orientation = temp_point2D.GetAngleToOriginRadian();

                PathNode temp_node = new PathNode(new Point2D(x, y), orientation, null);


                Nodes.Add(temp_node);
                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
                indexer++;
            }



            for (double i = size; i > -size; i -= stepsize)
            {
                double linearNoise = (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter + i;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = ycenter - size / 2 + linearNoise;

                Point2D temp_point2D = new Point2D(x, y);
                double orientation = temp_point2D.GetAngleToOriginRadian();

                PathNode temp_node = new PathNode(new Point2D(x, y), orientation, null);


                Nodes.Add(temp_node);
                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
                indexer++;
            }



            for (double i = -size / 2; i < size / 2; i += stepsize)
            {
                double linearNoise = (randnum.GenerateDouble() - 0.5) * noiseamplitude;

                double x = xcenter -size + linearNoise;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = ycenter + i;

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
