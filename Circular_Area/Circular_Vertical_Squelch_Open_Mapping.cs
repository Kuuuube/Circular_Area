using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Vertical Squelch Open Mapping")]
    public class Circular_Vertical_Squelch_Open_Mapping : CircularBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;            
            
            double u2 = Math.Pow(u, 2);

            var circle = new Vector2(
            (float)(u),
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(DiscardTruncation(CircleToSquare(ApplyTruncation(ToUnit(input))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}