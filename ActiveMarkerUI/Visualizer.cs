using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackingLib;

namespace ActiveMarkerUI
{
    public partial class Visualizer : Form
    {     
        //Engedélyezzük a Konzol és a Windows Form párhuzamos futását
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        //A megjelenítőhöz a framek várakozási sora, ha esetleg több frame is lenne akkor a legutolsót jeleníti meg
        //Így lehetséges, hogy az időzítő timer periódusát nagyobbra állítva tehermentesítsük a számítógép grafikus processzorát
        //Úgy hogy az időzítés helyessége megmarad
        Q<Frame> FrameQueue = new Q<Frame>();

        public Visualizer()
        {
           InitializeComponent();
            this.DoubleBuffered = true; //a megjelenítés smooth-ságát biztosító változó (automatic page flipping aktiválása)

            AllocConsole(); //A konzol megnyitásának meghívása

            //A frame-ek újrarajzolását egy külön időzítő  ütemezi, ha magasabbra állítjuk a periódust, kisebb FPS-el fog megjelenni, de a program futása változatlan (a virtuális FPS megmarad)
            Timer timer = new Timer();
            timer.Tick += (s, e) => {
                if (!FrameQueue.IsEmpty())
                {
                    //kirajzolás
                    Invalidate();
                }
            };
            timer.Interval = 30;
            timer.Start();

            //Ha új Frame-et készített a kamera, akkor beadjuk a várakozási sorunkba, hogy egyszer majd sorra kerüljön, feliratkozik az eventre, úgyhogy ez folyamatos
            TrackingLib.Engine.E.Camera.OnNewFrame += (frame) =>
            {
                FrameQueue.Add(frame);
            };

            //Elindítjuk az Engine-t
            TrackingLib.Engine.E.Start();
        }

        //OnPaint kirajzoló függvény, invalidate-re hívódik

        protected override void OnPaint(PaintEventArgs e)
        {
            var frame = FrameQueue.GetMostRecent();
            if (frame == null) { return; }
            DrawFrame(frame,e.Graphics);
        }

        //A teljes frame kirajzolását leíró, univerzális függvény
        public void DrawFrame(Frame frame, Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;


            double Xoffset = DrawList.centerX;
            double Yoffset = DrawList.centerY;

            //SetupCanvas szekció (régen külön függvény volt)

            Pen CornFlowerBluePen = new Pen(Color.FromArgb(70, 213, 249), 1.0f);

            g.DrawLine(CornFlowerBluePen, new Point((int)DrawList.centerX-1, 0), new Point((int)DrawList.centerX-1, (int)DrawList.centerY*2));
            g.DrawLine(CornFlowerBluePen, new Point(0, (int)DrawList.centerY-1), new Point((int)DrawList.centerX*2, (int)DrawList.centerY-1));
            g.DrawString("Aktív marker követés szimuláció", this.Font, Brushes.White, new Point(10, 10));
            g.DrawString("Frame: " + Engine.E.Camera.FrameCounter.ToString(), this.Font, Brushes.White, new Point(10, 30));
            g.DrawString("Simulation Time: " + Engine.E.SimulationTimeDouble.ToString("N3") + " sec", this.Font, Brushes.White, new Point(10, 50));
            g.DrawString("DetectedMarkers: " + Engine.E.Detector.DetectedMarkers.Count(), this.Font, Brushes.White, new Point(10, 70));

            //Útvonalak kirajzolása szekció

            foreach (var drawpath in frame.DrawList.Paths)
            {
                Point2D firstPoint = new Point2D();
                Point2D secondPoint = new Point2D();
                Pen DarkGreenPen = new Pen(Color.FromArgb(131, 183, 34), 1.0f);
                PointF[] curvepoints = new PointF[drawpath.PathPoints.Count+1];

                for (int i = 0; i < drawpath.PathPoints.Count; i++)
                {
                    //   firstPoint = drawpath.PathPoints[i - 1];
                    //   secondPoint = drawpath.PathPoints[i];
                    curvepoints[i] = new PointF((float)(drawpath.PathPoints[i].X + Xoffset), (float)(Yoffset-drawpath.PathPoints[i].Y));
                  //  g.DrawLine(DarkGreenPen, new Point((int)(firstPoint.X + Xoffset), (int)(Yoffset-firstPoint.Y)), new Point((int)(secondPoint.X+Xoffset), (int)(Yoffset - secondPoint.Y)));
                }
                //azért hogy körbeérjen a görbe, az utolsó pontból az elsőbe is mutasson színes vonal
                curvepoints[drawpath.PathPoints.Count] = new PointF((float)(drawpath.PathPoints[0].X + Xoffset), (float)(Yoffset - drawpath.PathPoints[0].Y));
                g.DrawLines(DarkGreenPen, curvepoints);
                //g.DrawCurve(DarkGreenPen, curvepoints);
            }

            //Elméleti markerek (Cars - TheoreticalMarker) kirajzolása

            foreach (var drawtheoreticalmarker in frame.DrawList.TheoreticalMarkers)
            {
                Pen yellowPen = new Pen(Color.FromArgb(254, 203, 45));
                SolidBrush fillYellow = new SolidBrush(Color.Yellow);
                SolidBrush fillRed = new SolidBrush(Color.Red);

                int xpos = (int)(drawtheoreticalmarker.Position.X + Xoffset) - 10; //a minusz tiz az offszet, mert 20x20as pottyok vannak
                int ypos = (int)(Yoffset-drawtheoreticalmarker.Position.Y) - 10;

                Rectangle rect = new Rectangle(xpos - 5, ypos - 5, 30, 30);

                g.DrawRectangle(yellowPen, rect);

                g.DrawString(drawtheoreticalmarker.Orientation.ToString("N2") + " rad", this.Font, Brushes.White, new Point(xpos + 30, ypos));
                g.DrawString(drawtheoreticalmarker.Position.ToString(), this.Font, Brushes.White, new Point(xpos + 30, ypos + 15));
                g.DrawString("Dist: " + drawtheoreticalmarker.Distance.ToString(), this.Font, Brushes.White, new Point(xpos + 30, ypos + 30));
                g.DrawString(drawtheoreticalmarker.Speed.ToString("N4") + " px/s", this.Font, Brushes.White, new Point(xpos + 30, ypos+45));
            }

            //Captured LEDs szekció (Kamera által érzékelt LED-ek)
            foreach (var drawcapturedled in frame.DrawList.CapturedLeds)
            {   
                SolidBrush fillRed = new SolidBrush(Color.FromArgb(207, 71, 45));

                int xpos = (int)(drawcapturedled.Position.X + Xoffset) - 10; //a minusz tiz az offszet, mert 20x20as pottyok vannak
                int ypos = (int)(Yoffset - drawcapturedled.Position.Y) - 10;

                Rectangle rect = new Rectangle(xpos, ypos, 20, 20);

                g.FillEllipse(fillRed, rect);
            }

            //Követett markerek szekció
            for (int i = 0; i < Engine.E.Tracker.TrackedMarkers.Count; i++)
            {
                Pen LightBlue = new Pen(Color.FromArgb(3, 179, 178));

                int xpos = (int)(Engine.E.Tracker.TrackedMarkers[i].Position.X + Xoffset) - 10; //a minusz tiz az offszet, mert 20x20as pottyok vannak
                int ypos = (int)(Yoffset - Engine.E.Tracker.TrackedMarkers[i].Position.Y) - 10;

                Rectangle rect = new Rectangle(xpos, ypos, 18, 18);
                g.DrawString("ID: " + i.ToString(), this.Font, Brushes.White, new Point(xpos, ypos - 30));

                g.DrawRectangle(LightBlue, rect);
            }

        }
        
        //Billenyűzet szekció (space lenyomásával a szimuláció szüneteltethető)

        private bool isSpacePressed = false;

        private void Visualizer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !isSpacePressed)
            {
                isSpacePressed = true;

                if (Engine.E.isRunning == true) Engine.E.isRunning = false;
                else Engine.E.isRunning = true;
            }
        }

        private void Visualizer_KeyUp(object sender, KeyEventArgs e)
        {
            if (isSpacePressed)
                isSpacePressed = false;
        }
    }
}
