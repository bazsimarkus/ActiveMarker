using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    class PathTriangle:PathBase
    {
        public override void CreatePath(double size)
        {
            PathNode prev_node = new PathNode();
         
            int indexer = 0;

            for (double i = -100; i < 100; i += 1)
                {
                    indexer++;

                    double x = i*size;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                    double y = 0 * size;

                    PathNode temp_node = new PathNode(new Point2D(x,y), 0, null);

                    Nodes.Add(temp_node);
                    if(indexer !=0) Nodes[indexer - 1].NextNode = temp_node; 
                }

            for (double i = 0; i < 50; i += 1)
            {
                indexer++;

                double x = (100-i)*size;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = i*size;

                PathNode temp_node = new PathNode(new Point2D(x, y), 0, null);

                Nodes.Add(temp_node);
                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
            }

            for (double i = 200; i > -200; i -= 1)
            {
                indexer++;

                double x = i * size;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = -100 * size;

                PathNode temp_node = new PathNode(new Point2D(x, y), 0, null);

                Nodes.Add(temp_node);
                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
            }

            for (double i = -100; i < +100; i += 1)
            {
                indexer++;

                double x = -200 * size;   //a 0 módosításával el lehet tolni a kör középpontját x-y irányba!         
                double y = i * size;

                PathNode temp_node = new PathNode(new Point2D(x, y), 0, null);

                Nodes.Add(temp_node);
                if (indexer != 0) Nodes[indexer - 1].NextNode = temp_node;
            }
        }

      
        
    }
}
