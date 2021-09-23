using System;
using System.Numerics;
using Circular_Area.CircularMath;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;

namespace Circular_Area
{
    [PluginName("Circular Schwarz-Christoffel Mapping Inverse")]
    public class Circular_Schwarz_Christoffel_Mapping_Inverse : CircularBase, IPositionedPipelineElement<IDeviceReport>
    {
        private const double Ke = 1.854;
        private static readonly Complex NegativeImaginary = new(1, -1);
        private static readonly Complex PositiveImaginary = new(1, 1);
        private static readonly Complex FirstCachedValue = NegativeImaginary / Math.Sqrt(2);
        private static readonly Complex SecondCachedValue = Ke * PositiveImaginary / 2;
        private static readonly double ThirdCachedValue = 1 / Math.Sqrt(2);

        public static Vector2 SquareToCircle(Vector2 input)
        {
            var complexInput = new Complex(input.X, input.Y);
            var mappedValue = FirstCachedValue * SpecialFunctions.Cn(SecondCachedValue * complexInput - Ke, ThirdCachedValue);

            return new Vector2(
                (float)mappedValue.Real,
                (float)mappedValue.Imaginary
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

        public Vector2 Filter(Vector2 input) => FromUnit(Clamp(Expand(SquareToCircle(ToUnit(input)))));

        public PipelinePosition Position => PipelinePosition.PostTransform;
    }
}