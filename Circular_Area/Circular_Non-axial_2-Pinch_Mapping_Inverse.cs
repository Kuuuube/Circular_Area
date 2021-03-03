using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Non-axial 2-Pinch Mapping Inverse")]
    public class Circular_Non_axial_2_Pinch_Mapping_Inverse : CircularBase, IFilter
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            float xmax = (float)(x * 9);
            float ymax = (float)(y * 9);

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            var circle = new Vector2(
                (float)((x * Math.Sqrt(x2 + y2)) / (1 + x2 * y2)),
                (float)((y * Math.Sqrt(x2 + y2)) / (1 + x2 * y2))
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
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(SquareToCircle(ToUnit(input)))));

        public FilterStage FilterStage => FilterStage.PostTranspose;
    }
}