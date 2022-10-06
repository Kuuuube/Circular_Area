[<- Back](https://github.com/Kuuuube/Circular_Area/blob/main/README.md#circular-area-plugin-for-opentabletdriver-)

# Circular Control Panel

## Explanation of the Values:

**Mapping Name:** The name of the filter to apply the below values to. See [Filter List](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/filter_list.md#filter-list) for a list of valid filter names.

**Truncation:** Scales down the area to replace the distortion closer to the edges with the distortion closer to the center. This can be used to transform some forward mapping area shapes into squircles.

Values higher or lower than 1 will truncate the distortion by the ratio of that value to 1. When set to 1, there is no change.

**Disable Quadrant 1:** Disables the circular area mapping in the top right quadrant of the screen.

**Disable Quadrant 2:** Disables the circular area mapping in the top left quadrant of the screen.

**Disable Quadrant 3:** Disables the circular area mapping in the bottom left quadrant of the screen.

**Disable Quadrant 4:** Disables the circular area mapping in the bottom right quadrant of the screen.

**Disable Expand in Enabled Quadrants:** Disables the expanding of inverse mappings in enabled quadrants.

Expanding allows reaching the entire screen when using an inverse mapping by scaling up the mapping and effectively making the tablet area smaller by sqrt(2).

**Disable Expand in Disabled Quadrants:** Disables the expanding of inverse mappings in disabled quadrants.

Expanding allows reaching the entire screen when using an inverse mapping by scaling up the mapping and effectively making the tablet area smaller by sqrt(2).