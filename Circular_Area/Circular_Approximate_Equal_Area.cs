using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Approximate Equal Area")]
    public class Circular_Approximate_Equal_Area : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            if (u2 > v2)
            {
                var circle = new Vector2(
                (float)(sgnu * Math.Sqrt(u2 + v2)),
                (float)(Math.Sqrt(2) * v)
                );
                if ((circle.X >= 0 || circle.X <= 0) && (circle.Y >= 0 || circle.Y <= 0))
                {
                    return new Vector2(
                    circle.X,
                    circle.Y
                    );
                }
                else
                {
                    return new Vector2(
                    Math.Clamp(umax, -1, 1),
                    Math.Clamp(vmax, -1, 1)
                    );
                }
            }
            else
            {
                var circle = new Vector2(
                (float)(Math.Sqrt(2) * u),
                (float)(sgnv * Math.Sqrt(u2 + v2))
                );
                if ((circle.X >= 0 || circle.X <= 0) && (circle.Y >= 0 || circle.Y <= 0))
                {
                    return new Vector2(
                    circle.X,
                    circle.Y
                    );
                }
                else
                {
                    return new Vector2(
                    Math.Clamp(umax, -1, 1),
                    Math.Clamp(vmax, -1, 1)
                    );
                }
            }
        }

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            if (value is ITabletReport report)
            {
                report.Position = Filter(report.Position);
                value = report;
            }

            Emit?.Invoke(value);
        }

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public PipelinePosition Position => PipelinePosition.PostTransform;
    }
}