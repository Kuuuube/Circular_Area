using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Numerics;

namespace Circular_Area
{
    [PluginName("CircularControlPanel")]
    public class CircularControlPanel : IPositionedPipelineElement<IDeviceReport>
    {
        private static bool Enabled = false; //This does not become false when the filter is disabled. That needs to be fixed somehow.
        public static float Get_Expander()
        {
            if (Enabled)
            {
                return Math.Clamp(Expander_raw, 0.00001f, float.MaxValue);
            }
            return 0;
        }

        [Property("Expander"), DefaultPropertyValue(1f)]
        public static float Expander_raw { set; get; }

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            Enabled = true;
            Emit?.Invoke(value);
        }
        public PipelinePosition Position => PipelinePosition.PostTransform;
    }
}
