using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Approximate Equal Area 2 Horizontal")]
    public class Circular_Approximate_Equal_Area_2_Horizontal : CircularBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;            
            
            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double ffv2 = (5 / 4) * v2;

            double t = Math.Sqrt(u2 + v2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            if (ffv2 >= u2)
            {
                var circle = new Vector2(
                (float)((3 / 2) * u),
                (float)(sgnv * t)
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                (float)(sgnu * t),
                (float)(v * Math.Sqrt((3 * t) / (t + absu)))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(DiscardTructation(CircleToSquare(ApplyTruncation(ToUnit(input))))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}