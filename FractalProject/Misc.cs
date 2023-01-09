﻿using System;
using System.Windows;

namespace Fucktal
{
    public static class Misc
    {
        public static double X1 { get; set; }
        public static double X2 { get; set; }
        public static double Y1 { get; set; }
        public static double Y2 { get; set; }
        public static double oX { get; set; }
        public static double oY { get; set; }

        public static double PixelsToCells(double pixels) => pixels / 20;

        public static double CellsToPixels(double cells) => cells * 20;

        public static double ShizaToX(double x) => x - oX;

        public static double ShizaToY(double y) => oY - y;

        public static double XToShiza(double x) => oX + x;

        public static double YToShiza(double y) => oY - y;

        public static double GetX(double x) => XToShiza(CellsToPixels(x));
        public static double GetY(double y) => YToShiza(CellsToPixels(y));

        public static double RadiansToDegrees(double radians) => 180 * radians / Math.PI;

        public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;

        public static double Cos(double degrees) => Math.Cos(DegreesToRadians(degrees));
        public static double Sin(double degrees) => Math.Sin(DegreesToRadians(degrees));
    }
}
