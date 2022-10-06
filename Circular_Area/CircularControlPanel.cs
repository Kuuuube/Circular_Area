using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;

namespace Circular_Area
{
    [PluginName("CircularControlPanel")]
    public class CircularControlPanel : IPositionedPipelineElement<IDeviceReport>
    {
        private static bool Enabled = false;
        public static float GetTructation(bool reset)
        {
            if (Enabled)
            {
                if (reset)
                {
                    Enabled = false;
                }
                return Math.Clamp(Tructation_raw, 0.00001f, float.MaxValue);
            }
            return 1;
        }

        [Property("Tructation"), DefaultPropertyValue(1f), ToolTip
            ("Circular Area:\n\n" +
            "Truncation: Truncates the mapping by scaling down the area to remove the distortion closer to the edges and replace it with the distortion closer to the center.\n" +
            "Values higher or lower than 1 will truncate the distortion by the ratio of that value to 1. When set to 1, there is no change.")]
        public static float Tructation_raw { set; get; }

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            Enabled = true;
            Emit?.Invoke(value);
        }
        public PipelinePosition Position => PipelinePosition.PreTransform;
    }
}
