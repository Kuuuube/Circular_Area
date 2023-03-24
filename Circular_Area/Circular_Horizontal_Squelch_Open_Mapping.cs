using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Horizontal Squelch Open Mapping")]
    public class Circular_Horizontal_Squelch_Open_Mapping : CircularBase
    {
        public static string Filter_Name = "Circular Horizontal Squelch Open Mapping";

        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            double v2 = Math.Pow(v, 2);

            var circle = new Vector2(
                (float)(u / Math.Sqrt(1 - v2)),
                (float)(v)
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