using System;
using System.Numerics;
using Circular_Area.CircularMath;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace Circular_Area
{
    [PluginName("Circular Schwarz-Christoffel Mapping")]
    public class Circular_Schwarz_Christoffel_Mapping : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        private const double Ke = 1.854;
        private static readonly Complex NegativeImaginary = new(1, -1);
        private static readonly Complex PositiveImaginary = new(1, 1);
        private static readonly Complex FirstCachedValue = NegativeImaginary / -Ke;
        private static readonly Complex SecondCachedValue = PositiveImaginary / Math.Sqrt(2);
        private static readonly double ThirdCachedValue = 1 / Math.Sqrt(2);

        public static Vector2 CircleToSquare(Vector2 input)
        {
            var complexInput = new Complex(input.X, input.Y);
            var angle = Complex.Acos(SecondCachedValue * complexInput);

            var jacobian = SpecialFunctions.F(angle, ThirdCachedValue);
            var mappedValue = FirstCachedValue * jacobian;

            return new Vector2(
                (float)(mappedValue.Real + 1),
                (float)(mappedValue.Imaginary - 1)
            );
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