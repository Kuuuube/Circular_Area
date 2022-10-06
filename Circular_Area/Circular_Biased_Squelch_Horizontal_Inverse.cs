using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Biased Squelch Horizontal Inverse")]
    public class Circular_Biased_Squelch_Horizontal_Inverse : CircularBase
    {
        public Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            float B = Math.Clamp(B_raw, 0.01f, 1);

            var circle = new Vector2(
            (float)(x * Math.Sqrt((1 - y2) / (1 - B * x2 * y2))),
            (float)(y * Math.Sqrt((1 - B * x2) / (1 - B * x2 * y2)))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(DiscardTructation(SquareToCircle(ApplyTruncation(ToUnit(input)))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("β")]
        public float B_raw { set; get; }
    }
}