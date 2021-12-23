using OpenTabletDriver;
using OpenTabletDriver.Plugin;
using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.DependencyInjection;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;
using System.Linq;
using System.Numerics;

namespace Circular_Area
{
    public abstract class CircularBase : IPositionedPipelineElement<IDeviceReport>
    {
        protected Vector2 ToUnit(Vector2 input)
        {
            if (outputMode is not null)
            {
                var area = outputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = outputMode?.Output;
                var offset = (Vector2)(outputMode?.Output?.Position);
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
                tryResolveOutputMode();
                return default;
            }
        }

        protected Vector2 FromUnit(Vector2 input)
        {
            if (outputMode is not null)
            {
                var area = outputMode.Input;
                var size = new Vector2(area.Width, area.Height);
                var half = size / 2;
                var display = outputMode?.Output;
                var offset = (Vector2)(outputMode?.Output?.Position);
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

        protected static Vector2 Clamp(Vector2 input)
        {
            return new Vector2(
            Math.Clamp(input.X, -1, 1),
            Math.Clamp(input.Y, -1, 1)
            );
        }

        protected static Vector2 Expand(Vector2 input)
        {
            return new Vector2(
            (float)(input.X * Math.Sqrt(8) / 2),
            (float)(input.Y * Math.Sqrt(8) / 2)
            );
        }


        [Resolved]
        public IDriver driver;
        private AbsoluteOutputMode outputMode;
        private void tryResolveOutputMode()
        {
            if (driver is Driver drv)
            {
                IOutputMode output = drv.InputDevices
                    .Where(dev => dev?.OutputMode?.Elements?.Contains(this) ?? false)
                    .Select(dev => dev?.OutputMode).FirstOrDefault();

                if (output is AbsoluteOutputMode absOutput)
                    outputMode = absOutput;
            }
        }

        public abstract event Action<IDeviceReport> Emit;
        public abstract void Consume(IDeviceReport value);
        public abstract PipelinePosition Position { get; }
    }
}
