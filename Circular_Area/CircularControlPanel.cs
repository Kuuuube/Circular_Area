using OpenTabletDriver.Plugin.Attributes;
using OpenTabletDriver.Plugin.Output;
using OpenTabletDriver.Plugin.Tablet;
using System;

namespace Circular_Area
{
    [PluginName("Circular Control Panel")]
    public class CircularControlPanel : IPositionedPipelineElement<IDeviceReport>
    {
        private static bool[] Enabled = { false, false, false, false, false, false, false, false, false, false, false, false };
        public static float GetTruncation(bool reset, string filter_name)
        {
            switch (CheckName(filter_name))
            {
                case 1:
                    if (EnabledCheck(0, reset))
                    {
                        return Math.Clamp(Truncation_raw_1, 0.00001f, float.MaxValue);
                    }
                    return 1;
                case 2:
                    if (EnabledCheck(1, reset))
                    {
                        return Math.Clamp(Truncation_raw_2, 0.00001f, float.MaxValue);
                    }
                    return 1;
                case 3:
                    if (EnabledCheck(2, reset))
                    {
                        return Math.Clamp(Truncation_raw_3, 0.00001f, float.MaxValue);
                    }
                    return 1;
                case 4:
                    if (EnabledCheck(3, reset))
                    {
                        return Math.Clamp(Truncation_raw_4, 0.00001f, float.MaxValue);
                    }
                    return 1;
                default:
                    return 1;
            }
        }

        public static bool GetQuadrant(int quadrant, string filter_name)
        {
            switch (CheckName(filter_name))
            {
                case 1:
                    if (EnabledCheck(4))
                    {
                        switch (quadrant)
                        {
                            case 1:
                                return Disable_Q1_1;
                            case 2:
                                return Disable_Q2_1;
                            case 3:
                                return Disable_Q3_1;
                            case 4:
                                return Disable_Q4_1;
                            default:
                                break;
                        }
                    }
                    return false;
                case 2:
                    if (EnabledCheck(5))
                    {
                        switch (quadrant)
                        {
                            case 1:
                                return Disable_Q1_2;
                            case 2:
                                return Disable_Q2_2;
                            case 3:
                                return Disable_Q3_2;
                            case 4:
                                return Disable_Q4_2;
                            default:
                                break;
                        }
                    }
                    return false;
                case 3:
                    if (EnabledCheck(6))
                    {
                        switch (quadrant)
                        {
                            case 1:
                                return Disable_Q1_3;
                            case 2:
                                return Disable_Q2_3;
                            case 3:
                                return Disable_Q3_3;
                            case 4:
                                return Disable_Q4_3;
                            default:
                                break;
                        }
                    }
                    return false;
                case 4:
                    if (EnabledCheck(7))
                    {
                        switch (quadrant)
                        {
                            case 1:
                                return Disable_Q1_4;
                            case 2:
                                return Disable_Q2_4;
                            case 3:
                                return Disable_Q3_4;
                            case 4:
                                return Disable_Q4_4;
                            default:
                                break;
                        }
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static bool GetDisableExpand(bool enabled_quad, bool disabled_quad, string filter_name)
        {
            switch(CheckName(filter_name))
            {
                case 1:
                    if (EnabledCheck(8))
                    {
                        if (enabled_quad && Disable_Expand_1)
                        {
                            return true;
                        }
                        if (disabled_quad && Disable_Expand_Disabled_1)
                        {
                            return true;
                        }
                    }
                    return false;
                case 2:
                    if (EnabledCheck(9))
                    {
                        if (enabled_quad && Disable_Expand_2)
                        {
                            return true;
                        }
                        if (disabled_quad && Disable_Expand_Disabled_2)
                        {
                            return true;
                        }
                    }
                    return false;
                case 3:
                    if (EnabledCheck(10))
                    {
                        if (enabled_quad && Disable_Expand_3)
                        {
                            return true;
                        }
                        if (disabled_quad && Disable_Expand_Disabled_3)
                        {
                            return true;
                        }
                    }
                    return false;
                case 4:
                    if (EnabledCheck(11))
                    {
                        if (enabled_quad && Disable_Expand_4)
                        {
                            return true;
                        }
                        if (disabled_quad && Disable_Expand_Disabled_4)
                        {
                            return true;
                        }
                    }
                    return false;
                default:
                    return false;
            }
        }

        public static int CheckName(string filter_name)
        {
            if (filter_name == Mapping_Name_1)
            {
                return 1;
            }
            if (filter_name == Mapping_Name_2)
            {
                return 2;
            }
            if (filter_name == Mapping_Name_3)
            {
                return 3;
            }
            if (filter_name == Mapping_Name_4)
            {
                return 4;
            }
            return 0;
        }

        public static bool EnabledCheck(int enabled_id)
        {
            if (Enabled[enabled_id])
            {
                Enabled[enabled_id] = false;
                return true;
            }
            return false;
        }

        public static bool EnabledCheck(int enabled_id, bool reset)
        {
            if (Enabled[enabled_id])
            {
                if (reset)
                {
                    Enabled[enabled_id] = false;
                }
                return true;
            }
            return false;
        }

        private const string Mapping_Name_Tooltip = "Circular Control Panel:\n\n" +
            "Mapping Name: The name of the Circular area mapping to apply the below settings to.";

        private const string Truncation_Tooltip = "Circular Control Panel:\n\n" +
            "Truncation: Scales down the area to replace the distortion closer to the edges with the distortion closer to the center.\n" +
            "This can be used to transform some forward mapping area shapes into squircles.\n" +
            "Values higher or lower than 1 will truncate the distortion by the ratio of that value to 1. When set to 1, there is no change.";

        private const string Disable_Q1_Tooltip = "Circular Control Panel:\n\n" +
            "Disable Quadrant 1: Disables the circular area mapping in the top right quadrant of the screen.";

        private const string Disable_Q2_Tooltip = "Circular Control Panel:\n\n" +
            "Disable Quadrant 2: Disables the circular area mapping in the top left quadrant of the screen.";

        private const string Disable_Q3_Tooltip = "Circular Control Panel:\n\n" +
            "Disable Quadrant 3: Disables the circular area mapping in the bottom left quadrant of the screen.";

        private const string Disable_Q4_Tooltip = "Circular Control Panel:\n\n" +
            "Disable Quadrant 4: Disables the circular area mapping in the bottom right quadrant of the screen.";

        private const string Disable_Expand_Tooltip = "Circular Control Panel:\n\n" +
            "Disable Expand in Enabled Quadrants: Disables the expanding of inverse mappings in enabled quadrants.\n" +
            "Expanding allows reaching the entire screen when using an inverse mapping by scaling up the mapping and effectively making the tablet area smaller by sqrt(2).";

        private const string Disable_Expand_Disabled_Tooltip = "Circular Control Panel:\n\n" +
            "Disable Expand in Disabled Quadrants: Disables the expanding of inverse mappings in disabled quadrants.\n" +
            "Expanding allows reaching the entire screen when using an inverse mapping by scaling up the mapping and effectively making the tablet area smaller by sqrt(2).";

        //Mapping 1
        [Property("Mapping Name"), ToolTip(Mapping_Name_Tooltip)]
        public static string Mapping_Name_1 { set; get; }

        [Property("Truncation"), DefaultPropertyValue(1f), ToolTip(Truncation_Tooltip)]
        public static float Truncation_raw_1 { set; get; }

        [BooleanProperty("Disable Quadrant 1", ""), ToolTip(Disable_Q1_Tooltip)]
        public static bool Disable_Q1_1 { set; get; }

        [BooleanProperty("Disable Quadrant 2", ""), ToolTip(Disable_Q2_Tooltip)]
        public static bool Disable_Q2_1 { set; get; }

        [BooleanProperty("Disable Quadrant 3", ""), ToolTip(Disable_Q3_Tooltip)]
        public static bool Disable_Q3_1 { set; get; }

        [BooleanProperty("Disable Quadrant 4", ""), ToolTip(Disable_Q4_Tooltip)]
        public static bool Disable_Q4_1 { set; get; }

        [BooleanProperty("Disable Expand in Enabled Quadrants", ""), ToolTip(Disable_Expand_Tooltip)]
        public static bool Disable_Expand_1 { set; get; }

        [BooleanProperty("Disable Expand in Disabled Quadrants", ""), ToolTip(Disable_Expand_Disabled_Tooltip)]
        public static bool Disable_Expand_Disabled_1 { set; get; }

        //Mapping 2
        [Property("Mapping Name"), ToolTip(Mapping_Name_Tooltip)]
        public static string Mapping_Name_2 { set; get; }

        [Property("Truncation"), DefaultPropertyValue(1f), ToolTip(Truncation_Tooltip)]
        public static float Truncation_raw_2 { set; get; }

        [BooleanProperty("Disable Quadrant 1", ""), ToolTip(Disable_Q1_Tooltip)]
        public static bool Disable_Q1_2 { set; get; }

        [BooleanProperty("Disable Quadrant 2", ""), ToolTip(Disable_Q2_Tooltip)]
        public static bool Disable_Q2_2 { set; get; }

        [BooleanProperty("Disable Quadrant 3", ""), ToolTip(Disable_Q3_Tooltip)]
        public static bool Disable_Q3_2 { set; get; }

        [BooleanProperty("Disable Quadrant 4", ""), ToolTip(Disable_Q4_Tooltip)]
        public static bool Disable_Q4_2 { set; get; }

        [BooleanProperty("Disable Expand in Enabled Quadrants", ""), ToolTip(Disable_Expand_Tooltip)]
        public static bool Disable_Expand_2 { set; get; }

        [BooleanProperty("Disable Expand in Disabled Quadrants", ""), ToolTip(Disable_Expand_Disabled_Tooltip)]
        public static bool Disable_Expand_Disabled_2 { set; get; }

        //Mapping 3
        [Property("Mapping Name"), ToolTip(Mapping_Name_Tooltip)]
        public static string Mapping_Name_3 { set; get; }

        [Property("Truncation"), DefaultPropertyValue(1f), ToolTip(Truncation_Tooltip)]
        public static float Truncation_raw_3 { set; get; }

        [BooleanProperty("Disable Quadrant 1", ""), ToolTip(Disable_Q1_Tooltip)]
        public static bool Disable_Q1_3 { set; get; }

        [BooleanProperty("Disable Quadrant 2", ""), ToolTip(Disable_Q2_Tooltip)]
        public static bool Disable_Q2_3 { set; get; }

        [BooleanProperty("Disable Quadrant 3", ""), ToolTip(Disable_Q3_Tooltip)]
        public static bool Disable_Q3_3 { set; get; }

        [BooleanProperty("Disable Quadrant 4", ""), ToolTip(Disable_Q4_Tooltip)]
        public static bool Disable_Q4_3 { set; get; }

        [BooleanProperty("Disable Expand in Enabled Quadrants", ""), ToolTip(Disable_Expand_Tooltip)]
        public static bool Disable_Expand_3 { set; get; }

        [BooleanProperty("Disable Expand in Disabled Quadrants", ""), ToolTip(Disable_Expand_Disabled_Tooltip)]
        public static bool Disable_Expand_Disabled_3 { set; get; }

        //Mapping 4
        [Property("Mapping Name"), ToolTip(Mapping_Name_Tooltip)]
        public static string Mapping_Name_4 { set; get; }

        [Property("Truncation"), DefaultPropertyValue(1f), ToolTip(Truncation_Tooltip)]
        public static float Truncation_raw_4 { set; get; }

        [BooleanProperty("Disable Quadrant 1", ""), ToolTip(Disable_Q1_Tooltip)]
        public static bool Disable_Q1_4 { set; get; }

        [BooleanProperty("Disable Quadrant 2", ""), ToolTip(Disable_Q2_Tooltip)]
        public static bool Disable_Q2_4 { set; get; }

        [BooleanProperty("Disable Quadrant 3", ""), ToolTip(Disable_Q3_Tooltip)]
        public static bool Disable_Q3_4 { set; get; }

        [BooleanProperty("Disable Quadrant 4", ""), ToolTip(Disable_Q4_Tooltip)]
        public static bool Disable_Q4_4 { set; get; }

        [BooleanProperty("Disable Expand in Enabled Quadrants", ""), ToolTip(Disable_Expand_Tooltip)]
        public static bool Disable_Expand_4 { set; get; }

        [BooleanProperty("Disable Expand in Disabled Quadrants", ""), ToolTip(Disable_Expand_Disabled_Tooltip)]
        public static bool Disable_Expand_Disabled_4 { set; get; }

        public event Action<IDeviceReport> Emit;

        public void Consume(IDeviceReport value)
        {
            int i = 0;
            foreach (var enabled_value in Enabled)
            {
                Enabled[i] = true;
                i++;
            }
            Emit?.Invoke(value);
        }
        public PipelinePosition Position => PipelinePosition.PreTransform;
    }
}
