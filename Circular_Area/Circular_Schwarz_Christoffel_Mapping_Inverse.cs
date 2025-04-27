using System;
using System.Numerics;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace Circular_Area
{
    [PluginName("Circular Schwarz-Christoffel Mapping Inverse")]
    public class Circular_Schwarz_Christoffel_Mapping_Inverse : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        public static string Filter_Name = "Circular Schwarz-Christoffel Mapping Inverse";
        public static Vector2 SquareToCircle(Vector2 input)
        {
            double k = 1.854074677301371918433850347195260046217598823521766905586;
            double z_re = input.X / 2.0 - input.Y / 2.0;
            double z_im = input.X / 2.0 + input.Y / 2.0;
            var (ru, rv) = SchwarzChristoffelBase.ccn(k * (1.0 - z_re), -k * z_im);
            return new Vector2(
                (float)((ru + rv) * Math.Sqrt(1.0 / 2.0)),
                (float)((rv - ru) * Math.Sqrt(1.0 / 2.0))
            );
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
    }
}