﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular 3-Squircular Mapping Inverse")]
    public class Circular_3_Squircular_Mapping_Inverse : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            float xmax = (float)(x * 9);
            float ymax = (float)(y * 9);

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            double x4 = Math.Pow(x, 4);
            double y4 = Math.Pow(y, 4);

            double absx = Math.Abs(x);
            double absy = Math.Abs(y);

            double sgnxy = (absx * absy) / (x * y);

            if (Math.Abs(y) < 0.00001 || Math.Abs(x) < 0.00001)
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
                    (float)((sgnxy / y) * Math.Sqrt((-1 + Math.Sqrt(1 + 4 * x4 * y2 + 4 * x2 * y4)) / (2 * (x2 + y2)))),
                    (float)((sgnxy / x) * Math.Sqrt((-1 + Math.Sqrt(1 + 4 * x4 * y2 + 4 * x2 * y4)) / (2 * (x2 + y2))))
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

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            if (value is ITabletReport report)
            {
                report.Position = Filter(report.Position);
                value = report;
            }

            Emit?.Invoke(value);
        }

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(SquareToCircle(ToUnit(input)))));

        public PipelinePosition Position => PipelinePosition.PostTransform;
    }
}