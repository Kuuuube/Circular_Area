using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Elliptical Grid Mapping")]
    public class Circular_Elliptical_Grid_Mapping : CircularBase
    {
        public static string Filter_Name = "Circular Elliptical Grid Mapping";

        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;            
            
            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);
            var twosqrttwo = 2 * Math.Sqrt(2);

            var circle = new Vector2(
                (float)(0.5f * Math.Sqrt(2 + u2 - v2 + twosqrttwo * u) - 0.5f * Math.Sqrt(2 + u2 - v2 - twosqrttwo * u)),
                (float)(0.5f * Math.Sqrt(2 - u2 + v2 + twosqrttwo * v) - 0.5f * Math.Sqrt(2 - u2 + v2 - twosqrttwo * v))
                );

            return No_NaN(circle, input);
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