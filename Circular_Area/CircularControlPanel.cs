using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;

namespace Circular_Area
{
    [PluginName("CircularControlPanel")]
    public class CircularControlPanel : IPositionedPipelineElement<IDeviceReport>
    {
        private static bool[] Enabled = { false, false };
        public static float GetTruncation(bool reset)
        {
            if (Enabled[0])
            {
                if (reset)
                {
                    Enabled[0] = false;
                }
                return Math.Clamp(Truncation_raw, 0.00001f, float.MaxValue);
            }
            return 1;
        }

        public static bool GetQuadrant(int quadrant)
        {
            if (Enabled[1])
            {
                Enabled[1] = false;
                switch(quadrant)
                {
                    case 1:
                        return Disable_Q1;
                    case 2:
                        return Disable_Q2;
                    case 3:
                        return Disable_Q3;
                    case 4:
                        return Disable_Q4;
                    default:
                        break;
                }
            }
            return false;
        }

        [Property("Truncation"), DefaultPropertyValue(1f), ToolTip
            ("Circular Area:\n\n" +
            "Truncation: Truncates the mapping by scaling down the area to remove the distortion closer to the edges and replace it with the distortion closer to the center.\n" +
            "Values higher or lower than 1 will truncate the distortion by the ratio of that value to 1. When set to 1, there is no change.")]
        public static float Truncation_raw { set; get; }

        [BooleanProperty("Disable Quadrant 1", "")]
        public static bool Disable_Q1 { set; get; }

        [BooleanProperty("Disable Quadrant 2", "")]
        public static bool Disable_Q2 { set; get; }

        [BooleanProperty("Disable Quadrant 3", "")]
        public static bool Disable_Q3 { set; get; }

        [BooleanProperty("Disable Quadrant 4", "")]
        public static bool Disable_Q4 { set; get; }

        [BooleanProperty("Disable Expand (inverse mappings)", "")]
        public static bool Disable_Expand { set; get; }

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            Enabled[0] = true;
            Enabled[1] = true;
            Emit?.Invoke(value);
        }
        public PipelinePosition Position => PipelinePosition.PreTransform;
    }
}
