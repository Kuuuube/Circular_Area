using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular 3-Squircular Mapping Inverse")]
    public class Circular_3_Squircular_Mapping_Inverse : CircularBase
    {
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

            double x4 = Math.Pow(x, 4);
            double y4 = Math.Pow(y, 4);

            double absx = Math.Abs(x);
            double absy = Math.Abs(y);

            double sgnxy = (absx * absy) / (x * y);

            if (Math.Abs(y) < 0.00001 || Math.Abs(x) < 0.00001)
            {
                var circle = new Vector2(
                        (float)(x),
                        (float)(y)
                        );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                    (float)((sgnxy / y) * Math.Sqrt((-1 + Math.Sqrt(1 + 4 * x4 * y2 + 4 * x2 * y4)) / (2 * (x2 + y2)))),
                    (float)((sgnxy / x) * Math.Sqrt((-1 + Math.Sqrt(1 + 4 * x4 * y2 + 4 * x2 * y4)) / (2 * (x2 + y2))))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(DiscardTructation(SquareToCircle(ApplyTruncation(ToUnit(input)))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}