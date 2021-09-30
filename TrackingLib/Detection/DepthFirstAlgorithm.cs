using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackingLib
{
    public class DepthFirstAlgorithm
    {
        //Gráf mélységi bejárása (DFS Traverse)
        //A markersequences listát tölti fel lehetséges üzenet szekvenciákkal, amiket a dekóder dekódolni tud
        public List<MarkerSequence> Traverse(DetectedMarker root)
        {
            List<MarkerSequence> sequencelist = new List<MarkerSequence>();
            DetectedMarker lastMarker = root;
            int lastSymbol = 0;

            MarkerSequence binarySequence = new MarkerSequence();

            Stack<DetectedMarker> s = new Stack<DetectedMarker>(); //Iteratív stack-es DFS megvalósítás
            s.Push(root);
            root.Depth = 1;

            while (s.Count > 0)
            {
                var n = s.Pop();

                if (n.FrameNumber >= lastMarker.FrameNumber) //ha visszaugrás történt
                {
                    if (sequencelist.Contains(binarySequence) == false) sequencelist.Add(binarySequence); // ha eljutottunk egy gráfútvonal végéig, akkor az egy markerszekvencia, amit hozzáadunk a szekvenciák listájához
                                                                                                          // break; //Ha csak az első bejárást szeretnénk eltárolni, kikommentezendő
                                                                                                          //mivel Preorder DFS a bejárás, ezért a legkésőbbi még nem meglátogatott pontra ugrik vissza. a markerszekvenciának azonban a korábbi szimnólumokat is tartalmaznia kell, ezek a szimbólumok az előzővel megegyeznek, így "átmásoljuk őket", gyakorlatilag az előzőt felhasználjuk, úgy hogy a szükségtelen markersymbolokat kitöröljük.
                    binarySequence.MarkerSymbols.RemoveAll(symbol => symbol.FrameNumber < n.FrameNumber); //ehhez lambda expression predikátot használunk
                }

                //Mi van ha az előző Candidate nem az előző frame-en van? Az azt jelenti, hogy a kettő között a LED nem világított, így azt nullás bitekkel kell feltöltenünk.
                if (n.FrameNumber < (lastMarker.FrameNumber - 1))
                {
                    int FrameCounterToFill = lastMarker.FrameNumber - 1;
                    while (FrameCounterToFill != n.FrameNumber) //azért kell a -1, mert az utolsó előtti "slot"-ig kell feltöltenünk nullásokkal, ha nem ez lenne, eggyel több nullást rakna be mindig!
                    {
                        binarySequence.MarkerSymbols.Insert(0, new MarkerSymbol(0, n.Time + (FrameCounterToFill - n.FrameNumber) * Engine.E.Camera.FrameInterval, n.FrameNumber + (FrameCounterToFill - n.FrameNumber), n.Position));
                        lastSymbol = 0;
                        FrameCounterToFill--;
                    }
                }
                if (lastSymbol != 1)
                    binarySequence.MarkerSymbols.Insert(0, n.ToMarkerSymbol(1));
                lastSymbol = 1;
                lastMarker = n;

                //Ez kikommentezendő, ha nem szretnénk, hogy depth korlátozás legyen
                //foreach (var child in n.PreviousCandidates.ToArray().Reverse())
                //{
                //    s.Push(child);
                //}

                //14-ig le kell menni, hisz az az értelmes üzenetsor hossza, ez alatt nem lehetünk biztosak benne hogy helyes
                if (n.Depth <= 14) //14, mert startbit, 10 data bit, 2 paritásbit, 2 stopbit = 14 hosszú
                {
                    for (int i = n.PreviousCandidates.Count - 1; i >= 0; i--)
                    {
                        n.PreviousCandidates[i].Depth = n.Depth + 1;
                        s.Push(n.PreviousCandidates[i]);
                    }
                }
            }

            //14 bitnél rövidebb szekvenciát nem adunk vissza, hosszabbat sem
            sequencelist.RemoveAll(sequence => Engine.E.Decoder.GetBitNumberOfSequence(sequence) < 13);
            sequencelist.RemoveAll(sequence => Engine.E.Decoder.GetBitNumberOfSequence(sequence) > 14);
            //if (sequencelist.Count != 0)
            //{
            //    Console.WriteLine("x");
            //}
            return sequencelist;
        }
    }
}


