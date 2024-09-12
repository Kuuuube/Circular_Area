﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Biased Squelch Blended Mapping")]
    public class Circular_Biased_Squelch_Blended_Mapping : CircularBase
    {
        public static string Filter_Name = "Circular Biased Squelch Blended Mapping";

        public Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            float B = Math.Clamp(B_raw, 0.01f, 0.99f);

            if (Math.Abs(v) < 0.00001 || Math.Abs(u) < 0.00001)
            {
                var circle = new Vector2(
                    (float)(u),
                    (float)(v)
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                    (float)((sgnu / (Math.Sqrt(2 * (1 - B)))) * Math.Sqrt(1 + (1 - B) * u2 - B * v2 - Math.Sqrt(Math.Pow((1 + (1 - B) * u2 - B * v2), 2) - 4 * (1 - B) * u2))),
                    (float)((sgnv / (Math.Sqrt(2 * B))) * Math.Sqrt(1 - (1 - B) * u2 + B * v2 - Math.Sqrt(Math.Pow((1 - (1 - B) * u2 + B * v2), 2) - 4 * B * v2)))
                );

                return No_NaN(circle, input);
            }
        }

        public override event Action<IDeviceReport> Emit;

        public override void Consume(IDeviceReport value)
        {
            if (value is IAbsolutePositionReport report)
            {
                report.Position = Filter(report.Position);
                value = report;
            }

            Emit?.Invoke(value);
        }

        public Vector2 Filter(Vector2 input)
        {
            if (CheckQuadrant(ToUnit(input), Filter_Name))
            {
                return input;
            }
            return FromUnit(Clamp(DiscardTruncation(CircleToSquare(ApplyTruncation(ToUnit(input), Filter_Name)), Filter_Name)));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("β")]
        public float B_raw { set; get; }
    }
}