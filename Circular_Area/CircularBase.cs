﻿using OpenTabletDriver;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Linq;
using System.Numerics;

namespace Circular_Area
{
    public abstract class CircularBase : CircularControlPanel, IPositionedPipelineElement<IDeviceReport>
    {
        protected Vector2 ToUnit(Vector2 input)
        {
            if (outputMode is not null)
            {
                var display = outputMode?.Output;
                var offset = (Vector2)(outputMode?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                return new Vector2(
                    (input.X - shiftoffX) / display.Width * 2 - 1,
                    (input.Y - shiftoffY) / display.Height * 2 - 1
                    );
            }
            else
            {
                tryResolveOutputMode();
                return default;
            }
        }

        protected Vector2 FromUnit(Vector2 input)
        {
            if (outputMode is not null)
            {
                var display = outputMode?.Output;
                var offset = (Vector2)(outputMode?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                return new Vector2(
                    (input.X + 1) / 2 * display.Width + shiftoffX,
                    (input.Y + 1) / 2 * display.Height + shiftoffY
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
            (float)(input.X * Math.Sqrt(2)), //input.X * Math.Sqrt(8) / 2 simplifies to input.X * Math.Sqrt(2)
            (float)(input.Y * Math.Sqrt(2))
            );
        }

        protected static Vector2 No_NaN(Vector2 circle, Vector2 input)
        {
            Vector2 max = new Vector2(input.X * 10, input.Y * 10);

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
                Math.Clamp(max.X, -1, 1),
                Math.Clamp(max.Y, -1, 1)
                );
            }
        }

        protected static Vector2 ApplyTruncation(Vector2 input, string filter_name)
        {
            float Truncation = GetTruncation(false, filter_name);

            if (Truncation > 1)
            {
                return input / Truncation;
            }
            else if (Truncation < 1)
            {
                return input * Truncation;
            }
            return input;
        }

        protected static Vector2 DiscardTruncation(Vector2 input, string filter_name)
        {
            float Truncation = GetTruncation(true, filter_name);

            if (Truncation > 1)
            {
                return input * Truncation;
            }
            else if (Truncation < 1)
            {
                return input / Truncation;
            }
            return input;
        }

        protected static bool CheckQuadrant(Vector2 input, string filter_name)
        {
            //Due to how OTD sends input, the Y axis ends up flipped. Normally the Y axis for these quadrants would be reversed.
            //for the quadrant checks, true = disabled

            if (input.X > 0 && input.Y < 0)
            {
                if (GetQuadrant(1, filter_name))
                {
                    return true;
                }
            }
            if (input.X < 0 && input.Y < 0)
            {
                if (GetQuadrant(2, filter_name))
                {
                    return true;
                }
            }
            if (input.X < 0 && input.Y > 0)
            {
                if (GetQuadrant(3, filter_name))
                {
                    return true;
                }
            }
            if (input.X > 0 && input.Y > 0)
            {
                if (GetQuadrant(4, filter_name))
                {
                    return true;
                }
            }
            return false;
        }

        [Resolved]
        public IDriver driver;
        private AbsoluteOutputMode outputMode;
        private void tryResolveOutputMode()
        {
            if (driver is Driver drv)
            {
                IOutputMode output = drv.InputDevices
                    .Where(dev => dev?.OutputMode?.Elements?.Contains(this) ?? false)
                    .Select(dev => dev?.OutputMode).FirstOrDefault();

                if (output is AbsoluteOutputMode absOutput)
                    outputMode = absOutput;
            }
        }

        public abstract event Action<IDeviceReport> Emit;
        public abstract void Consume(IDeviceReport value);
        public abstract PipelinePosition Position { get; }
    }
}
