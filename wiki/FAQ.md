[<- Back](https://github.com/Kuuuube/Circular_Area/blob/main/README.md#circular-area-plugin-for-opentabletdriver-)

# FAQ

### 1. Which mapping should I use?

The following four mappings are recommended for anyone new to Circular Area. It is recommended to try at least one forward mapping and one inverse mapping.

- Forward mappings: [FG-Squircular Mapping](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/mappings/fg_squircular_mapping.md#fg-squircular-mapping) or [Elliptical Grid Mapping](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/mappings/elliptical_grid_mapping.md#elliptical-grid-mapping).

- Inverse mappings: [FG-Squircular Mapping Inverse](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/mappings/fg_squircular_mapping.md#fg-squircular-mapping-inverse) or [Elliptical Grid Mapping Inverse](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/mappings/elliptical_grid_mapping.md#elliptical-grid-mapping-inverse).

For more information on mappings see: [Layman's Guide to Circular Area](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/layman_s_guide_to_circular_area.md#laymans-guide-to-circular-area) and [Mappings Index](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/mappings_index.md#mappings-index).

<br>

### 2. Does it really make my area circular?

Yes. Circular Area transforms your tablet area into either an ellipse or a half-face superellipse depending on the mapping type. See below:

![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/wiki/images/area_visualizations/ellipse_transformation_examples.png)

![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/wiki/images/area_visualizations/half-face_superellipse_transformation_examples.png)

For more information on mapping types see: [Forward vs Inverse Mappings](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/layman_s_guide_to_circular_area.md#forward-vs-inverse-mappings)

<br>

### 3. OpenTabletDriver doesn't show my area as an ellipse or half-face superellipse. Is the plugin broken?

No. Circular Area transforms your tablet area in the backend only. There will be no visual changes within OTD's GUI/UX. Currently, OTD does not allow plugins to change GUI. If OTD makes this option available in the future, Circular Area will make use of it.

<br>

### 4. What benefits does a circular area have over a rectangular area?

See: [Why you should use circular area](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/layman_s_guide_to_circular_area.md#why-you-should-use-circular-area)

<br>

### 5. Circular Area has lots of distortion. Wouldn't that make it harder to use?

The distortion is by design and without this distortion, there would be no benefits to using a circular area.

It may look hard to use but, it's easier than it appears. See below:

<br>

### 6. It must take a long time or a lot of practice to get used to playing like this, right?

No. As surprising as it may seem, you can get almost entirely accustomed to Circular Area in a day or two. However, it can take around a week or longer to become completely comfortable with Circular Area.

<br>

### 7. Are there any mappings that make my area a squircle or only somewhat circular?

No. Currently, there are no mappings like this.

If you would like to help on the mathematical side of adding this feature please contact me on discord: `Kuuube#6878`. Serious inquiries only.

<br>

### 8. Can I enable multiple mappings?

Yes and no. There will not be any errors if you enable multiple mappings. But, the results are usually undesirable.

<br>

### 9. Can I mix between two different mappings? I like different parts of two mappings is it possible to combine them?

Yes. Use tertiary mappings. See: [Tertiary Mapping Styles](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/layman_s_guide_to_circular_area.md#tertiary-mapping-styles)

However, some mappings cannot be mixed between because they are too far unrelated.

<br>

### 10. If I spin my pen in a circular motion will the cursor spin in a rectangular motion?

Usually, no. However, it is possible to do this with [Simple Stretch-based mappings](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/mappings_index.md#simple-stretch).
