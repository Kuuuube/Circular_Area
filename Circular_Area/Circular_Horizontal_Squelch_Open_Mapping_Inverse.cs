﻿using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Tablet;
using OpenTabletDriver.Plugin.Output;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("Circular Horizontal Squelch Open Mapping Inverse")]
    public class Circular_Horizontal_Squelch_Open_Mapping_Inverse : CircularBase
    {
        public static string Filter_Name = "Circular Horizontal Squelch Open Mapping Inverse";

        public static Vector2 SquareToCircle(Vector2 input)
        {
            double x = input.X;
            double y = input.Y;

            double y2 = Math.Pow(y, 2);

            var circle = new Vector2(
                (float)(x * Math.Sqrt(1 - y2)),
                (float)(y)
            );

            return No_NaN(circle, input);
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