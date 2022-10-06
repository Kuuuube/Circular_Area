using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular FG-Squircular Mapping Inverse")]
    public class Circular_FG_Squircular_Mapping_Inverse : CircularBase
    {
        public static string Filter_Name = "Circular FG-Squircular Mapping Inverse";

        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double x2 = Math.Pow(x, 2);
            double y2 = Math.Pow(y, 2);

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
                    (float)((x * Math.Sqrt(x2 + y2 - x2 * y2)) / (Math.Sqrt(x2 + y2))),
                    (float)((y * Math.Sqrt(x2 + y2 - x2 * y2)) / (Math.Sqrt(x2 + y2)))
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
    }
}