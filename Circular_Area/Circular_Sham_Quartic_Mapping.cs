using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Linq;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Sham Quartic Mapping")]
    public class Circular_Sham_Quartic_Mapping : CircularBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            float umax = (float)(u * 9);
            float vmax = (float)(v * 9);

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);
            
            double u4 = Math.Pow(u, 4);
            double v4 = Math.Pow(v, 4);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            double toppart2 = (((-(8 * ((2 + v2 + 1) * u2 + v2))) / (v2 + v4)));
            double toppart3 = ((2 * v2 + 1) * u2 + v2) / (v4);
            double toppart4 = (2 + u2) / v2;
            double toppart5 = 2 / v4;

            double toppart2sol12 = - (v2 * toppart2);
            double toppart2sol34 = v2 * toppart2;

            double bottompart2 = 4 * Math.Sqrt((-v2) - u2 + 1);

            double endpart1 = 1 / (2 * v2);
            double endpart2 = (Math.Sqrt((-v2) - u2 + 1)) / (2 * v2);

            double solution1 = (-(Math.Sqrt((toppart2sol12 / bottompart2) - toppart3 - toppart4 - toppart5)) / 2) + endpart1 - endpart2;
            double solution2 = ((Math.Sqrt((toppart2sol12 / bottompart2) - toppart3 - toppart4 + toppart5)) / 2) + endpart1 + endpart2;
            double solution3 = (-(Math.Sqrt((toppart2sol34 / bottompart2) - toppart3 - toppart4 - toppart5)) / 2) + endpart1 - endpart2;
            double solution4 = ((Math.Sqrt((toppart2sol34 / bottompart2) - toppart3 - toppart4 + toppart5)) / 2) + endpart1 + endpart2; ;

            double[] solutions = { solution1, solution2, solution3, solution4 };

            double q0 = solutions.OrderBy(i => i).SkipWhile(i => i <= 0).First();
            double q0sqrt = Math.Sqrt(q0);

            var circle = new Vector2(
                (float)(sgnu * q0sqrt),
                (float)(sgnv * Math.Abs(v / u) * q0sqrt)
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