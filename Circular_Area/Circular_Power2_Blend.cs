using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Power2 Blend")]
    public class Circular_Power2_Blend : IFilter
    {
        public static Vector2 ToUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode).Output;
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    (input.X / pxpermmw - half.X) / half.X,
                    (input.Y / pxpermmh - half.Y) / half.Y
                    );
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
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    ((input.X * half.X) + half.X) * pxpermmw,
                    ((input.Y * half.Y) + half.Y) * pxpermmh
                );
            }
            else
            {
                return default;
            }
        }

        public Vector2 CircleToSquare(Vector2 input)
        {
            var u = input.X;
            var v = input.Y;

            var u2 = MathF.Pow(u, 2);
            var v2 = MathF.Pow(v, 2);

            var absu = MathF.Abs(u);
            var absv = MathF.Abs(v);

            var sgnuv = (absu * absv) / (u * v);

            var usqrttwo = u * MathF.Sqrt(2);
            var vsqrttwo = v * MathF.Sqrt(2);

            var B = Math.Clamp(B_raw, 0.01f, 1);

            if (MathF.Abs(v) < 0.1 || MathF.Abs(u) < 0.1)
            {
                return new Vector2(
                        u,
                        v
                        );
            }
            else
            {
                return new Vector2(
                    (sgnuv * MathF.Sqrt((u2 + v2 - MathF.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2 * (u2 + v2 + 2 * B * (1 - u2 - v2))))) / (2 * (u2 + v2 + 2 * B * (1 - u2 - v2)))))*(1/v),
                    (sgnuv * MathF.Sqrt((u2 + v2 - MathF.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2 * (u2 + v2 + 2 * B * (1 - u2 - v2))))) / (2 * (u2 + v2 + 2 * B * (1 - u2 - v2)))))*(1/u)
                    );
            }
        }

        public static Vector2 Clamp(Vector2 input)
        {
            return new Vector2(
            Math.Clamp(input.X, -1, 1),
            Math.Clamp(input.Y, -1, 1)
            );
        }
        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));


        public FilterStage FilterStage => FilterStage.PostTranspose;

        [Property("β")]
        public float B_raw { set; get; }
    }
}