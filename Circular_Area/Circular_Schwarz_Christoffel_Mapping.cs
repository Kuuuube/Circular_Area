using System;
using System.Numerics;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace Circular_Area
{
    [PluginName("Circular Schwarz-Christoffel Mapping")]
    public class Circular_Schwarz_Christoffel_Mapping : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        public static string Filter_Name = "Circular Schwarz-Christoffel Mapping";
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double k = 1.854074677301371918433850347195260046217598823521766905586;
            double u = input.X;
            double v = input.Y;

            double ru = (u - v) * Math.Sqrt(1.0 / 2.0);
            double rv = (u + v) * Math.Sqrt(1.0 / 2.0);
            double a_big = ru * ru + rv * rv;
            double b_big = ru * ru - rv * rv;
            double u_big = 1.0 + 2.0 * b_big - a_big * a_big;
            double t_big = Math.Sqrt((1.0 + a_big * a_big) * (1.0 + a_big * a_big) - 4.0 * b_big * b_big);
            double cos_a = (2.0 * a_big - t_big) / u_big;
            double cos_b = u_big / (2.0 * a_big + t_big);
            double a = Math.Acos(Math.Min(Math.Max(cos_a, -1.0), 1.0));
            double b = Math.Acos(Math.Min(Math.Max(cos_b, -1.0), 1.0));
            double rx = Math.Sign(ru) * (1.0 - SchwarzChristoffelBase.landen_elliptic_f(a) / (2.0 * k));
            double ry = Math.Sign(rv) * (SchwarzChristoffelBase.landen_elliptic_f(b) / (2.0 * k));
            return new Vector2(
                (float)(rx + ry),
                (float)(ry - rx)
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
                return input;
            }
            return FromUnit(Clamp(DiscardTruncation(CircleToSquare(ApplyTruncation(ToUnit(input), Filter_Name)), Filter_Name)));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}