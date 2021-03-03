using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Linq;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Sham Quartic Mapping")]
    public class Circular_Sham_Quartic_Mapping : IFilter
    {
        public static Vector2 ToUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode)?.Output;
                var offset = (Vector2)((Info.Driver.OutputMode as AbsoluteOutputMode)?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    ((input.X - shiftoffX) / pxpermmw - half.X) / half.X,
                    ((input.Y - shiftoffY) / pxpermmh - half.Y) / half.Y
                    );
            }
            else
            {
                return default;
            }
        }


        private static Vector2 FromUnit(Vector2 input)
        {
            if (Info.Driver.OutputMode is AbsoluteOutputMode absoluteOutputMode)
            {
                var area = absoluteOutputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = (Info.Driver.OutputMode as AbsoluteOutputMode)?.Output;
                var offset = (Vector2)((Info.Driver.OutputMode as AbsoluteOutputMode)?.Output?.Position);
                var shiftoffX = offset.X - (display.Width / 2);
                var shiftoffY = offset.Y - (display.Height / 2);
                var pxpermmw = display.Width / area.Width;
                var pxpermmh = display.Height / area.Height;
                return new Vector2(
                    ((input.X * half.X) + half.X) * pxpermmw + shiftoffX,
                    ((input.Y * half.Y) + half.Y) * pxpermmh + shiftoffY
                );
            }
            else
            {
                return default;
            }
        }

        public static Vector2 CircleToSquare(Vector2 input)
        {
            var u = input.X;
            var v = input.Y;

            var umax = u * 9;
            var vmax = v * 9;

            var u2 = MathF.Pow(u, 2);
            var v2 = MathF.Pow(v, 2);
            
            var u4 = MathF.Pow(u, 4);
            var v4 = MathF.Pow(u, 4);

            var absu = MathF.Abs(u);
            var absv = MathF.Abs(v);

            var sgnu = absu / u;
            var sgnv = absv / v;

            var toppart2 = (((-(8 * ((2 + v2 + 1) * u2 + v2))) / (v2 + v4)));
            var toppart3 = ((2 * v2 + 1) * u2 + v2) / (v4);
            var toppart4 = (2 + u2) / v2;
            var toppart5 = 2 / v4;

            var toppart2sol12 = - (v2 * toppart2);
            var toppart2sol34 = v2 * toppart2;

            var bottompart2 = 4 * MathF.Sqrt((-v2) - u2 + 1);

            var endpart1 = 1 / (2 * v2);
            var endpart2 = (MathF.Sqrt((-v2) - u2 + 1)) / (2 * v2);

            var solution1 = (-(MathF.Sqrt((toppart2sol12 / bottompart2) - toppart3 - toppart4 - toppart5)) / 2) + endpart1 - endpart2;
            var solution2 = ((MathF.Sqrt((toppart2sol12 / bottompart2) - toppart3 - toppart4 + toppart5)) / 2) + endpart1 + endpart2;
            var solution3 = (-(MathF.Sqrt((toppart2sol34 / bottompart2) - toppart3 - toppart4 - toppart5)) / 2) + endpart1 - endpart2;
            var solution4 = ((MathF.Sqrt((toppart2sol34 / bottompart2) - toppart3 - toppart4 + toppart5)) / 2) + endpart1 + endpart2; ;

            float[] solutions = { solution1, solution2, solution3, solution4 };

            var q0 = solutions.OrderBy(i => i).SkipWhile(i => i <= 0).First();
            var q0sqrt = MathF.Sqrt(q0);

            var circle = new Vector2(
                sgnu * q0sqrt,
                sgnv * MathF.Abs(v / u) * q0sqrt
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

        private static Vector2 Clamp(Vector2 input)
        {
            return new Vector2(
            Math.Clamp(input.X, -1, 1),
            Math.Clamp(input.Y, -1, 1)
            );
        }

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(CircleToSquare(ToUnit(input))));


        public FilterStage FilterStage => FilterStage.PostTranspose;

    }
}