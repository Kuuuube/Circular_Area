using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Cornerific Tapered2 Mapping")]
    public class Circular_Cornerific_Tapered2_Mapping : CircularBase, IFilter
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

            double sgnuv = (absu * absv) / (u * v);

            if (Math.Abs(v) < 0.1 || Math.Abs(u) < 0.1)
            {
                var circle = new Vector2(
                        (float)(u),
                        (float)(v)
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
                    (float)((sgnuv / v) * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2 * (2 - u2 - v2)))) / (2 * (2 - u2 - v2)))),
                    (float)((sgnuv / u) * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2 * (2 - u2 - v2)))) / (2 * (2 - u2 - v2))))
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