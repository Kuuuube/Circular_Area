﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Power2 Blend")]
    public class Circular_Power2_Blend : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        public Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnuv = (absu * absv) / (u * v);

            float B = Math.Clamp(B_raw, 0.01f, 1);

            if (Math.Abs(v) < 0.00001 || Math.Abs(u) < 0.00001)
            {
                var circle = new Vector2(
                        (float)(u),
                        (float)(v)
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
            else
            {
                var circle = new Vector2(
                    (float)((sgnuv * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2 * (u2 + v2 + 2 * B * (1 - u2 - v2))))) / (2 * (u2 + v2 + 2 * B * (1 - u2 - v2)))))*(1/v)),
                    (float)((sgnuv * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 4 * u2 * v2 * (u2 + v2 + 2 * B * (1 - u2 - v2))))) / (2 * (u2 + v2 + 2 * B * (1 - u2 - v2)))))*(1/u))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("β")]
        public float B_raw { set; get; }
    }
}