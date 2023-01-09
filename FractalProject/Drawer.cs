using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Fucktal
{
    public static class Drawer
    {
        public static Canvas grid { get; set; }

        public static void Draw(Polyline polyline)
        {
            grid.Children.Add(polyline);
        }

        public static void Draw(Label label)
        {
            grid.Children.Add(label);
        }

        public static void Draw(Ellipse ellipse)
        {
            grid.Children.Add(ellipse);
        }

        public static void Draw(List<Polyline> polylines)
        {
            foreach (var p in polylines)
            {
                Draw(p);
            }
        }

        public static void Draw(List<Label> labels)
        {
            foreach (var l in labels)
            {
                Draw(l);
            }
        }

        public static void Erase(List<Polyline> polylines)
        {
            foreach (var p in polylines)
            {
                Erase(p);
            }
        }

        public static void Erase(Polyline polyline)
        {
            grid.Children.Remove(polyline);
        }

        public static void Erase(Ellipse ellipse)
        {
            grid.Children.Remove(ellipse);
        }

        public static void Clear()
        {
            grid.Children.Clear();
        }

        public static Polyline Polyline(Point start, Point end)
        {
            Polyline line = new Polyline();

            line.Points.Add(start);
            line.Points.Add(end);
            line.Stroke = Brushes.Navy;
            line.StrokeThickness = 2;

            return line;
        }
    }
}
