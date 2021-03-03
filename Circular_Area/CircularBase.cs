using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    public class CircularBase
    {
        protected static Vector2 ToUnit(Vector2 input)
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

        protected static Vector2 FromUnit(Vector2 input)
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

        protected static Vector2 Clamp(Vector2 input)
        {
            return new Vector2(
            Math.Clamp(input.X, -1, 1),
            Math.Clamp(input.Y, -1, 1)
            );
        }

        protected static Vector2 Expand(Vector2 input)
        {
            return new Vector2(
            (float)(input.X * Math.Sqrt(8) / 2),
            (float)(input.Y * Math.Sqrt(8) / 2)
            );
        }
    }
}
