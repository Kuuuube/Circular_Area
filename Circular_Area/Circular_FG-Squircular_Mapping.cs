using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular FG-Squircular Mapping")]
    public class Circular_FG_Squircular_Mapping : IFilter
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

            var absu = MathF.Abs(input.X);
            var absv = MathF.Abs(input.Y);

            var sgnuv = (absu * absv) / (input.X * input.Y);

            var usqrttwo = input.X * MathF.Sqrt(2);
            var vsqrttwo = input.Y * MathF.Sqrt(2);

            return new Vector2(
                sgnuv / vsqrttwo * MathF.Sqrt(u2 + v2 - MathF.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2))),
                sgnuv / usqrttwo * MathF.Sqrt(u2 + v2 - MathF.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2)))
                );
        }
        public Vector2 Filter(Vector2 input) => FromUnit(CircleToSquare(ToUnit(input)));


        public FilterStage FilterStage => FilterStage.PostTranspose;

    }
}