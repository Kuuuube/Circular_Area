using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Power3 Blend Inverse")]
    public class Circular_Power3_Blend_Inverse : CircularBase
    {
        public static string Filter_Name = "Circular Power3 Blend Inverse";

        public Vector2 SquareToCircle(Vector2 input)
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

            float B = Math.Clamp(B_raw, 0.01f, 1);

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
                    (float)(((sgnxy / (Math.Sqrt(x2 + y2))) * (Math.Sqrt((1 - Math.Sqrt(1 - 2 * (3 * B - 2) * x4 * y2 - 2 * (3 * B - 2) * x2 * y4 + 3 * B * (3 * B - 2) * x4 * y4)) / (3 * B - 2)))) * (1 / y)),
                    (float)(((sgnxy / (Math.Sqrt(x2 + y2))) * (Math.Sqrt((1 - Math.Sqrt(1 - 2 * (3 * B - 2) * x4 * y2 - 2 * (3 * B - 2) * x2 * y4 + 3 * B * (3 * B - 2) * x4 * y4)) / (3 * B - 2)))) * (1 / x))
                );

                return No_NaN(circle, input);
            }
        }

        public override event Action<IDeviceReport> Emit;

        public override void Consume(IDeviceReport value)
        {
            if (value is IAbsolutePositionReport report)
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
                if (GetDisableExpand(false, true, Filter_Name))
                {
                    return input;
                }
                return FromUnit(Clamp(Expand(ToUnit(input))));
            }
            if (GetDisableExpand(true, false, Filter_Name))
            {
                return FromUnit(Clamp(DiscardTruncation(SquareToCircle(ApplyTruncation(ToUnit(input), Filter_Name)), Filter_Name)));
            }
            return FromUnit(Clamp(Expand(DiscardTruncation(SquareToCircle(ApplyTruncation(ToUnit(input), Filter_Name)), Filter_Name))));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("β")]
        public float B_raw { set; get; }
    }
}