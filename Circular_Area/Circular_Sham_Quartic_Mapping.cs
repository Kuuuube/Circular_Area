using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Linq;
using System.Numerics;
using OpenTabletDriver.Plugin;

namespace Circular_Area
{
    [PluginName("Circular Sham Quartic Mapping")]
    public class Circular_Sham_Quartic_Mapping : CircularBase, QuarticBase
    {
        public static Vector2 CircleToSquare(Vector2 input)
        {
            double u = input.X;
            double v = input.Y;

            double u2 = Math.Pow(u, 2);
            double v2 = Math.Pow(v, 2);

            double u4 = Math.Pow(u, 4);
            double v4 = Math.Pow(v, 4);

            double absu = Math.Abs(u);
            double absv = Math.Abs(v);

            double sgnu = absu / u;
            double sgnv = absv / v;

            Complex[] roots = QuarticBase.Quartic(v4, -2 * v2, u2 + v2 + 2 * u2 * v2, -2 * u2, u4);
            double[] real_roots = new double[roots.Count()];
            for (int i = 0; i <= roots.Count() - 1; i++)
            {
                real_roots[i] = roots[i].Real;
            }

            double q0 = real_roots.OrderBy(i => i).SkipWhile(i => i <= 0).First();
            double q0sqrt = Math.Sqrt(q0);

            var circle = new Vector2(
                (float)(sgnu * q0sqrt),
                (float)(sgnv * Math.Abs(v / u) * q0sqrt)
                );

            return No_NaN(circle, input);
        
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