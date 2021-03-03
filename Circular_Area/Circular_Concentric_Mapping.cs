using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Concentric Mapping")]
    public class Circular_Concentric_Mapping : CircularBase, IFilter
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            if (u2 > v2)
            {
                var circle = new Vector2(
                (float)(sgnu * Math.Sqrt(u2 + v2) * 1),
                (float)(sgnu * Math.Sqrt(u2 + v2) * (4 / Math.PI * Math.Atan(v / u)))
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
            else
            {
                var circle = new Vector2(
                (float)(sgnv * Math.Sqrt(u2 + v2) * (4 / Math.PI * Math.Atan(u / v))),
                (float)(sgnv * Math.Sqrt(u2 + v2) * 1)
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
        }
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public FilterStage FilterStage => FilterStage.PostTranspose;
    }
}