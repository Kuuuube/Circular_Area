using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Elliptical Grid Mapping")]
    public class Circular_Elliptical_Grid_Mapping : CircularBase, IFilter
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);
            var twosqrttwo = 2 * Math.Sqrt(2);

            var circle = new Vector2(
                (float)(0.5f * Math.Sqrt(2 + u2 - v2 + twosqrttwo * u) - 0.5f * Math.Sqrt(2 + u2 - v2 - twosqrttwo * u)),
                (float)(0.5f * Math.Sqrt(2 - u2 + v2 + twosqrttwo * v) - 0.5f * Math.Sqrt(2 - u2 + v2 - twosqrttwo * v))
                );


            if ((circle.X >= 0 || circle.X <= 0) && (circle.Y >= 0 || circle.Y <= 0))
            {
                return new Vector2(
                circle.X,
                circle.Y
                );
            }
            else
            {
                return new Vector2(
                Math.Clamp(umax, -1, 1),
                Math.Clamp(vmax, -1, 1)
                );
            }

        }
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public FilterStage FilterStage => FilterStage.PostTranspose;
    }
}