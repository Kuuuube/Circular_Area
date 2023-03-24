using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Tapered3 Mapping")]
    public class Circular_Tapered3_Mapping : CircularBase
    {
        public static string Filter_Name = "Circular Tapered3 Mapping";

        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double u4 = Math.Pow(u, 4);
            double v4 = Math.Pow(v, 4);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnuv = (absu * absv) / (u * v);

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
                    (float)((sgnuv / v) * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 2 * u2 * v2 * (3 - u4 - 2 * u2 * v2 - v4)))) / (3 - u4 - 2 * u2 * v2 - v4))),
                    (float)((sgnuv / u) * Math.Sqrt((u2 + v2 - Math.Sqrt((u2 + v2) * (u2 + v2 - 2 * u2 * v2 * (3 - u4 - 2 * u2 * v2 - v4)))) / (3 - u4 - 2 * u2 * v2 - v4)))
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

        public Vector2 Filter(Vector2 input)
        {
            if (CheckQuadrant(ToUnit(input), Filter_Name))
            {
                return input;
            }
            return FromUnit(Clamp(DiscardTruncation(CircleToSquare(ApplyTruncation(ToUnit(input), Filter_Name)), Filter_Name)));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}