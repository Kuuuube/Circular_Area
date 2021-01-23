using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_simple_stretch
{
    [PluginName("Circular Simple Stretch")]
    public class Circular_simple_stretch : IFilter
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

            var sgnu = absu / input.X;
            var sgnv = absv / input.Y;

            if (u2 >= v2)
            {
                return new Vector2(
                sgnu * MathF.Sqrt(u2 + v2),
                sgnu * (input.Y / input.X) * MathF.Sqrt(u2 + v2)
                );
            }
            else
            {
                return new Vector2(
                sgnv * (input.X / input.Y) * MathF.Sqrt(u2 + v2),
                sgnv * MathF.Sqrt(u2 + v2)
                );
            }
        }
        public Vector2 Filter(Vector2 input) => FromUnit(CircleToSquare(ToUnit(input)));


        public FilterStage FilterStage => FilterStage.PostTranspose;

    }
}