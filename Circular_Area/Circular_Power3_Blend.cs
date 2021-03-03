using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Power3 Blend")]
    public class Circular_Power3_Blend : CircularBase, IFilter
    {
        public Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double u4 = Math.Pow(u, 4);
            double v4 = Math.Pow(v, 4);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnuv = (absu * absv) / (u * v);

            float B = Math.Clamp(B_raw, 0.01f, 1);

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
                    (float)((sgnuv * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 2 * u2 * v2 * (3 * B - (3 * B - 2) * u4 - 2 * (3 * B - 2) * u2 * v2 - (3 * B - 2) * v4)))) / (3 * B - (3 * B - 2) * u4 - 2 * (3 * B - 2) * u2 * v2 - (3 * B - 2) * v4))) * (1 / v)),
                    (float)((sgnuv * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 2 * u2 * v2 * (3 * B - (3 * B - 2) * u4 - 2 * (3 * B - 2) * u2 * v2 - (3 * B - 2) * v4)))) / (3 * B - (3 * B - 2) * u4 - 2 * (3 * B - 2) * u2 * v2 - (3 * B - 2) * v4))) * (1 / u))
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

        [Property("β")]
        public float B_raw { set; get; }
    }
}