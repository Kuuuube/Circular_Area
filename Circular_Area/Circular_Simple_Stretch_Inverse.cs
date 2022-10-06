using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Simple Stretch Inverse")]
    public class Circular_Simple_Stretch_Inverse : CircularBase
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            var absx = Math.Abs(x);
            var absy = Math.Abs(y);

            var sgnx = absx / x;
            var sgny = absy / y;

            if (x2 >= y2)
            {
                var circle = new Vector2(
                (float)(sgnx * (x2 / (Math.Sqrt(x2 + y2)))),
                (float)(sgnx * (x * y / (Math.Sqrt(x2 + y2))))
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                (float)(sgny * (x * y / (Math.Sqrt(x2 + y2)))),
                (float)(sgny * (y2 / (Math.Sqrt(x2 + y2))))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(DiscardTruncation(SquareToCircle(ApplyTruncation(ToUnit(input)))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}