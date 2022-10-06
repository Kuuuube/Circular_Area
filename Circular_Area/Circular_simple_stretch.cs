﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Simple Stretch")]
    public class Circular_Simple_Stretch : CircularBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;            
            
            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            if (u2 >= v2)
            {
                var circle = new Vector2(
                (float)(sgnu * Math.Sqrt(u2 + v2)),
                (float)(sgnu * (v / u) * Math.Sqrt(u2 + v2))
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                (float)(sgnv * (u / v) * Math.Sqrt(u2 + v2)),
                (float)(sgnv * Math.Sqrt(u2 + v2))
                );

                return No_NaN(circle, input);
            }
        }

        public override event Action<IDeviceReport> Emit;

        public override void Consume(IDeviceReport value)
        {
            if (value is ITabletReport report)
            {
                report.Position = Filter(report.Position);
                value = report;
            }

            Emit?.Invoke(value);
        }

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(DiscardTructation(CircleToSquare(ApplyTruncation(ToUnit(input))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}