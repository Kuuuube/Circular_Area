using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Non-axial ½-Punch Mapping")]
    public class Circular_Non_axial_Half_Punch_Mapping : CircularBase
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
            double sgnuv = (absu * absv) / (u * v);

            if (Math.Abs(v) < 0.00001)
            {
                var circle = new Vector2(
                        (float)(sgnu * u2),
                        (float)((sgnuv / u) * Math.Sqrt((1 - Math.Sqrt(1 - 4 * u2 * v2 * Math.Pow((u2 + v2), 2))) / (2 * (u2 + v2))))
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
                if (Math.Abs(u) < 0.00001)
                {
                    var circle = new Vector2(
                        (float)((sgnuv / v) * Math.Sqrt((1 - Math.Sqrt(1 - 4 * u2 * v2 * Math.Pow((u2 + v2), 2))) / (2 * (u2 + v2)))),
                        (float)(sgnv * v2)
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
                        (float)((sgnuv / v) * Math.Sqrt((1 - Math.Sqrt(1 - 4 * u2 * v2 * Math.Pow((u2 + v2), 2))) / (2 * (u2 + v2)))),
                        (float)((sgnuv / u) * Math.Sqrt((1 - Math.Sqrt(1 - 4 * u2 * v2 * Math.Pow((u2 + v2), 2))) / (2 * (u2 + v2))))
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));

        public override PipelinePosition Position => PipelinePosition.PostTransform;
    }
}