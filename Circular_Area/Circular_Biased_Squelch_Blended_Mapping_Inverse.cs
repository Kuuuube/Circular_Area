using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Biased Squelch Blended Mapping Inverse")]
    public class Circular_Biased_Squelch_Blended_Mapping_Inverse : CircularBase, IFilter
    {
        public Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            float xmax = (float)(x * 9);
            float ymax = (float)(y * 9);

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            float B = Math.Clamp(B_raw, 0.01f, 0.99f);

            if (Math.Abs(y) < 0.1 || Math.Abs(x) < 0.1)
            {
                var circle = new Vector2(
                        (float)(x),
                        (float)(y)
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
                    Math.Clamp(xmax, -1, 1),
                    Math.Clamp(ymax, -1, 1)
                    );
                }
            }
            else
            {
                var circle = new Vector2(
                    (float)(x * Math.Sqrt(1 - B * y2)),
                    (float)(y * Math.Sqrt(1 - (1 - B) * x2))
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
                    Math.Clamp(xmax, -1, 1),
                    Math.Clamp(ymax, -1, 1)
                    );
                }
            }
        }
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(SquareToCircle(ToUnit(input)))));

        public FilterStage FilterStage => FilterStage.PostTranspose;

        [Property("β")]
        public float B_raw { set; get; }
    }
}