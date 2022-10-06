using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Biased Squelch Vertical")]
    public class Circular_Biased_Squelch_Vertical : CircularBase
    {
        public Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;            
            
            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            float B = Math.Clamp(B_raw, 0.01f, 1);

            var circle = new Vector2(
            (float)(u / Math.Sqrt(1 - B * v2)),
            (float)(v / Math.Sqrt(1 - u2))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(DiscardTructation(CircleToSquare(ApplyTruncation(ToUnit(input))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("β")]
        public float B_raw { set; get; }
    }
}