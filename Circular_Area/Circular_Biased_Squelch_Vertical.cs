﻿using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Biased Squelch Vertical")]
    public class Circular_Biased_Squelch_Vertical : IFilter
    {
        public static Vector2 ToUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode)?.Output;
                var offset = (Vector2)((Info.Driver.OutputMode as AbsoluteOutputMode)?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    ((input.X - shiftoffX) / pxpermmw - half.X) / half.X,
                    ((input.Y - shiftoffY) / pxpermmh - half.Y) / half.Y
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
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode)?.Output;
                var offset = (Vector2)((Info.Driver.OutputMode as AbsoluteOutputMode)?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    ((input.X * half.X) + half.X) * pxpermmw + shiftoffX,
                    ((input.Y * half.Y) + half.Y) * pxpermmh + shiftoffY
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

            var umax = u * 9;
            var vmax = v * 9;

            var u2 = MathF.Pow(u, 2);
            var v2 = MathF.Pow(v, 2);

            var B = Math.Clamp(B_raw, 0.01f, 1);

            var circle = new Vector2(
            u / MathF.Sqrt(1 - B * v2),
            v / MathF.Sqrt(1 - u2)
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