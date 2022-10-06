using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Non-axial 2-Pinch Mapping Inverse")]
    public class Circular_Non_axial_2_Pinch_Mapping_Inverse : CircularBase
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            var circle = new Vector2(
                (float)((x * Math.Sqrt(x2 + y2)) / (1 + x2 * y2)),
                (float)((y * Math.Sqrt(x2 + y2)) / (1 + x2 * y2))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(DiscardTruncation(SquareToCircle(ApplyTruncation(ToUnit(input)))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}