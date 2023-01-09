using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Fucktal
{
    public class Fractal
    {
        public string axiom;
        public string logic;
        public double angle;
        public int depth;
        public double segment;
        public Point start;
        public List<Polyline> fractal;
        public Fractal(string axiom, string logic, double angle, int depth, double segment, Point start)
        {
            this.axiom = axiom;
            this.logic = logic;
            this.angle = angle;
            this.depth = depth;
            this.segment = segment;
            this.start = start;

            fractal = new List<Polyline>();
        }

        public void Build()
        {
            string buff = string.Empty;

            for (int i = 1; i < depth; i++)
            {
                foreach (var j in axiom)
                {
                    if (j.ToString() == "F")
                    {
                        buff += logic;
                    }
                    else
                    {
                        buff += j;
                    }
                }
                axiom = buff;
                buff = string.Empty;
            }

            double angleBuff = 0;
            Point p1 = start;
            //MessageBox.Show(axiom);
            foreach (var i in axiom)
            {
                if (i.ToString() == "+")
                {
                    angleBuff -= angle;
                }
                else if (i.ToString() == "-")
                {
                    angleBuff += angle;
                }
                else
                {
                    Point p2 = Move(p1, angleBuff);
                    var line = Drawer.Polyline(p1, p2);
                    fractal.Add(line);
                    p1 = p2;
                }
            }
        }

        public Point Move(Point start, double angle)
        {
            double x = start.X + segment * Misc.Cos(angle);
            double y = start.Y - segment * Misc.Sin(angle);
            Point end = new Point(x, y);

            return end;
        }

        public void Clear()
        {
            Drawer.Erase(fractal);
        }

        public void Draw()
        {
            Drawer.Draw(fractal);
        }
    }
}