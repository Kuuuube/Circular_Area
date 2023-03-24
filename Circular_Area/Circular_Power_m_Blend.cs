using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Power-m Blend")]
    public class Circular_Power_m_Blend : CircularBase
    {
        public static string Filter_Name = "Circular Power-m Blend";

        public Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            float m = Math.Clamp(m_raw, 0, float.MaxValue);
            if (m == 1) //1 causes division by zero
            {
                m = 1.0000001f;
            }

            float B = Math.Clamp(B_raw, 0.01f, 1);

            if (Math.Abs(v) < 0.00001 || Math.Abs(u) < 0.00001)
            {
                var circle = new Vector2(
                    (float)(u),
                    (float)(v)
                );

                return No_NaN(circle, input);
            }
            else
            {
                var circle = new Vector2(
                    (float)(sgnu * Math.Sqrt(((1 + (u2 / v2)) / (2 * ((m / (m - 1)) * B + (1 - (m / (m - 1)) * B) * Math.Pow(Math.Sqrt(u2 + v2), 2 * (m - 1))))) - Math.Sqrt(Math.Pow((1 + (u2 / v2)) / (2 * ((m / (m - 1)) * B + (1 - (m / (m - 1)) * B) * Math.Pow(Math.Sqrt(u2 + v2), 2 * (m - 1)))), 2) - (u2 / v2) * (Math.Pow(Math.Sqrt(u2 + v2), 2) / ((m / (m - 1)) * B + (1 - (m / (m - 1)) * B) * Math.Pow(Math.Sqrt(u2 + v2), 2 * (m - 1))))))),
                    (float)(sgnv * Math.Sqrt(((1 + (1 / (u2 / v2))) / (2 * ((m / (m - 1)) * B + (1 - (m / (m - 1)) * B) * Math.Pow(Math.Sqrt(u2 + v2), 2 * (m - 1))))) - Math.Sqrt(Math.Pow((1 + (1 / (u2 / v2))) / (2 * ((m / (m - 1)) * B + (1 - (m / (m - 1)) * B) * Math.Pow(Math.Sqrt(u2 + v2), 2 * (m - 1)))), 2) - (1 / (u2 / v2)) * (Math.Pow(Math.Sqrt(u2 + v2), 2) / ((m / (m - 1)) * B + (1 - (m / (m - 1)) * B) * Math.Pow(Math.Sqrt(u2 + v2), 2 * (m - 1)))))))
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
                return input;
            }
            return FromUnit(Clamp(DiscardTruncation(CircleToSquare(ApplyTruncation(ToUnit(input), Filter_Name)), Filter_Name)));
        }

        public override PipelinePosition Position => PipelinePosition.PostTransform;

        [Property("m")]
        public float m_raw { set; get; }

        [Property("β")]
        public float B_raw { set; get; }
    }
}