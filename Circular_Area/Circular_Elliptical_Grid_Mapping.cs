using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Elliptical Grid Mapping")]
    public class Circular_elliptical_grid_mapping : IFilter
    {
        public static Vector2 ToUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode).Output;
                var pxpermm = display.Width / area.Width;
                return (input / pxpermm - half) / half;
            }
            else
            {
                return default;
            }
        }

        private static Vector2 FromUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode).Output;
                var pxpermm = display.Width / area.Width;
                return ((input * half) + half) * pxpermm;
            }
            else
            {
                return default;
            }
        }

        public static Vector2 CircleToSquare(Vector2 input)
        {
                var u2 = MathF.Pow(input.X, 2);
                var v2 = MathF.Pow(input.Y, 2);
                var twosqrttwo = 2 * MathF.Sqrt(2);

                return new Vector2(
                0.5f * MathF.Sqrt(2 + u2 - v2 + twosqrttwo * input.X) - 0.5f * MathF.Sqrt(2 + u2 - v2 - twosqrttwo * input.X),
                0.5f * MathF.Sqrt(2 - u2 + v2 + twosqrttwo * input.Y) - 0.5f * MathF.Sqrt(2 - u2 + v2 - twosqrttwo * input.Y)
                );
        }
        public Vector2 Filter(Vector2 input) => FromUnit(CircleToSquare(ToUnit(input)));


        public FilterStage FilterStage => FilterStage.PostTranspose;

    }
}