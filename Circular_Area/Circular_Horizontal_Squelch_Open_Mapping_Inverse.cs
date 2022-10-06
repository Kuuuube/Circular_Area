using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Horizontal Squelch Open Mapping Inverse")]
    public class Circular_Horizontal_Squelch_Open_Mapping_Inverse : CircularBase
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double y2 = Math.Pow(y, 2);

            var circle = new Vector2(
            (float)(x * Math.Sqrt(1 - y2)),
            (float)(y)
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
            if (CheckQuadrant(ToUnit(input)))
            {
                if (Disable_Expand)
                {
                    return input;
                }
                return FromUnit(Clamp(Expand(ToUnit(input))));
            }
            return FromUnit(Clamp(Expand(DiscardTruncation(SquareToCircle(ApplyTruncation(ToUnit(input)))))));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}