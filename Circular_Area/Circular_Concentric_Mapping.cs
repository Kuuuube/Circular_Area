using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Concentric Mapping")]
    public class Circular_Concentric_Mapping : CircularBase
    {
        public static string Filter_Name = "Circular Concentric Mapping";

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

            if (u2 > v2)
            {
                var circle = new Vector2(
                (float)(sgnu * Math.Sqrt(u2 + v2) * 1),
                (float)(sgnu * Math.Sqrt(u2 + v2) * (4 / Math.PI * Math.Atan(v / u)))
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                (float)(sgnv * Math.Sqrt(u2 + v2) * (4 / Math.PI * Math.Atan(u / v))),
                (float)(sgnv * Math.Sqrt(u2 + v2) * 1)
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