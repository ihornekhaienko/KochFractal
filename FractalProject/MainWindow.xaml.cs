using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Fucktal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public double X1;
        public double X2;
        public double Y1;
        public double Y2;

        public double oX;
        public double oY;

        public List<Polyline> gridList = new List<Polyline>();
        public List<Label> gridLabels = new List<Label>();
        public Fractal fractal;
        public MainWindow()
        {
            InitializeComponent();
        }

        #region INIT
        private void SetDefaults()
        {
            dxTB.Text = "0";
            dyTB.Text = "0";

            xTB.Text = "0";
            yTB.Text = "0";
            angleTB.Text = "0";
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            X1 = 10;
            X2 = grid.ActualWidth - 10;
            Y1 = 10;
            Y2 = grid.ActualHeight - 10;
            oX = X2 / 2;
            oY = Y2 / 2;

            Misc.X1 = X1;
            Misc.X2 = X2;
            Misc.Y1 = Y1;
            Misc.Y2 = Y2;
            Misc.oX = oX;
            Misc.oY = oY;

            Drawer.grid = grid;
        }

        private void defaultButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaults();
        }
        #endregion

        #region MISC
        private void KeyPress(object sender, KeyEventArgs e)
        {

        }
        #endregion

        #region BUILD
        private void gridButton_Click(object sender, RoutedEventArgs e)
        {
            Grid();
        }

        private void Grid()
        {
            Grid grid = new Grid();

            grid.Build();

            gridList = grid.gridList;
            gridLabels = grid.gridLabels;

            Drawer.Clear();

            Drawer.Draw(gridList);
            Drawer.Draw(gridLabels);
        }

        private void buildButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(oxTB.Text) || string.IsNullOrWhiteSpace(oyTB.Text) || string.IsNullOrWhiteSpace(lengthTB.Text) ||
                string.IsNullOrWhiteSpace(axiomTB.Text) || string.IsNullOrWhiteSpace(logicTB.Text) || string.IsNullOrWhiteSpace(angTB.Text) ||
                string.IsNullOrWhiteSpace(depthTB.Text))
            {
                return;
            }

            double x = Misc.GetX(Convert.ToDouble(oxTB.Text));
            double y = Misc.GetY(Convert.ToDouble(oyTB.Text));
            double segment = Convert.ToDouble(lengthTB.Text);
            string axiom = axiomTB.Text;
            string logic = logicTB.Text;
            double angle = Convert.ToDouble(angTB.Text);
            int depth = Convert.ToInt32(depthTB.Text);
            Point start = new Point(x, y);

            fractal = new Fractal(axiom, logic, angle, depth, segment, start);
            fractal.Build();
            DrawAnime();
        }

        private void defaultsButton_Click(object sender, RoutedEventArgs e)
        {
            SetDefaults();
        }

        public void DrawAnime()
        {
            foreach (var i in fractal.fractal)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => Drawer.Draw(i))).Wait();
            }
        }
        #endregion

        #region ROTATION
        private void angleSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            angleTB.Text = angleSlider.Value.ToString();
        }

        private void angleTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(angleTB.Text) && angleTB.Text != "-")
                {
                    double angle = Convert.ToDouble(angleTB.Text);
                    if (angle is >= 0 and <= 360)
                    {
                        if (angleSlider != null)
                            angleSlider.Value = angle;
                    }
                }
            }
            catch
            {

            }
        }

        private void rotateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(xTB.Text) && string.IsNullOrWhiteSpace(yTB.Text) && string.IsNullOrWhiteSpace(angleTB.Text))
                {
                    return;
                }

                double angle = Convert.ToDouble(angleTB.Text);
                double x = Misc.XToShiza(Convert.ToDouble(oxTB.Text));
                double y = Misc.YToShiza(Convert.ToDouble(oyTB.Text));

                RotateAnime(angle, x, y);
                FinishRotate(-angle, Convert.ToDouble(oxTB.Text), Convert.ToDouble(oyTB.Text));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void RotateAnime(double angle, double x, double y)
        {
            double sign = 1;
            if (angle < 0)
            {
                sign *= -1;
                angle *= -1;
            }

            for (double i = 0; i < angle; i++)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => Rotate(sign * i, x, y))).Wait();
            }
        }

        private void Rotate(double angle, double x, double y)
        {
            foreach (var line in fractal.fractal)
            {
                line.RenderTransform = new RotateTransform(angle, x, y);
            }
        }

        private void FinishRotate(double angle, double dx, double dy)
        {
            fractal.Clear();

            foreach (var line in fractal.fractal)
            {
                line.RenderTransform = null;
                for (int i = 0; i < line.Points.Count; i++)
                {
                    double x = Misc.ShizaToX(line.Points[i].X) - dx;
                    double y = Misc.ShizaToY(line.Points[i].Y) - dy;

                    double u = x * Misc.Cos(angle) - y * Misc.Sin(angle);
                    double v = x * Misc.Sin(angle) + y * Misc.Cos(angle);

                    u += dx;
                    v += dy;

                    line.Points[i] = new Point(Misc.XToShiza(u), Misc.YToShiza(v));
                }
            }

            fractal.Draw();
        }
        #endregion

        #region OFFSET
        private void offsetButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dxTB.Text) && string.IsNullOrWhiteSpace(dyTB.Text))
                {
                    return;
                }

                double dx = Misc.CellsToPixels(Convert.ToDouble(dxTB.Text));
                double dy = Misc.CellsToPixels(Convert.ToDouble(dyTB.Text));

                OffsetAnime(dx, dy);
                FinishOffset(dx, dy);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void OffsetAnime(double dx, double dy)
        {
            double i = 0;
            double j = 0;
            bool flag = true;

            while (flag)
            {
                flag = false;

                if (Math.Abs(i) < Math.Abs(dx))
                {
                    flag = true;
                    if (dx > 0)
                    {
                        i += 0.1;
                    }
                    else
                    {
                        i -= 0.1;
                    }
                }

                if (Math.Abs(j) < Math.Abs(dy))
                {
                    flag = true;
                    if (dy > 0)
                    {
                        j -= 0.1;
                    }
                    else
                    {
                        j += 0.1;
                    }
                }

                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => Offset(i, j))).Wait();
            }
        }

        private void Offset(double dx, double dy)
        {
            foreach (var line in fractal.fractal)
            {
                line.RenderTransform = new TranslateTransform(dx, dy);
            }
        }

        private void FinishOffset(double dx, double dy)
        {
            fractal.Clear();

            foreach (var line in fractal.fractal)
            {
                line.RenderTransform = null;
                for (int i = 0; i < line.Points.Count; i++)
                {
                    line.Points[i] = new Point(line.Points[i].X + dx, line.Points[i].Y - dy);
                }
            }

            fractal.Draw();
        }
        #endregion

        private void patternCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = ((ComboBoxItem)patternCB.SelectedItem).Content;

            if (selected as string == "Koch's curve")
            {
                axiomTB.Text = "F";
                logicTB.Text = "F-F++F-F";
                angTB.Text = "60";
                lengthTB.Text = "20";
            }
            else if (selected as string == "Koch's snowflake")
            {
                axiomTB.Text = "F++F++F";
                logicTB.Text = "F-F++F-F";
                angTB.Text = "60";
                lengthTB.Text = "10";
            }
            else
            {
                axiomTB.Text = "F+F+F+F";
                logicTB.Text = "F+F-F-FF+F+F-F";
                angTB.Text = "90";
                lengthTB.Text = "4";
            }
        }
    }
}
