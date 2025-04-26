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
        public static string Filter_Name = "Circular Schwarz-Christoffel Mapping Inverse";
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