[<- Back](https://github.com/Kuuuube/Circular_Area/blob/main/README.md)

# Layman's Guide to Circular Area

This guide simplifies various concepts down to how they effectively work from the perspective of the user. 

Many parts of Circular Area work in more complex ways but are simplified to minimize confusion.

## Why you should use Circular Area

Rectangles are unnatural to the wrists and fingers. The natural full range of motion of your wrists and fingers is circular. 

Forward mappings match this natural motion by making the tablet area an ellipse. This creates a feeling of evenness when reaching for any point on the tablet area and removes strain caused by reaching for corners on a rectangular area.

Inverse mappings make the tablet area a half-face superellipse. These are useful for an entirely different reason than forward mappings. When gripping a tablet pen it should be the most stable to move around the center of the tablet area. This stability decreases when reaching for the corners of the tablet area. Inverse mappings compensate for this by effectively increasing the tablet area when moving to the corners.

For more information on how forward and inverse mappings work see below:

<br>

## Forward vs Inverse Mappings

### Forward Mappings:

- Forward mappings are any mappings that do not include "inverse" in their name. 

- These mappings make the tablet area an ellipse, transform its proportions based on the tablet area then map it to the rectangular monitor area.

- Forward mappings will decrease the physical distance required to move from one corner to another while keeping the side to side distance the same.

- Forward mapping area shape examples:

![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/wiki/images/area_visualizations/ellipse_transformation_examples.png)

### Inverse Mappings:

- Inverse mappings are any mappings that include "inverse" in their name.

- These mappings make the tablet area a half-face superellipse, transform its proportions based on the tablet area then map it to the rectangular monitor area.

- Inverse mappings will decrease the physical distance required to move from one side to another while keeping the corner to corner distance the same.

- Inverse mapping area shape examples:

![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/wiki/images/area_visualizations/half-face_superellipse_transformation_examples.png)

<br>

## Primary Mapping Styles

These are the mappings all other mappings are based off. This applies to both forward and inverse mappings unless explicitly stated otherwise.

### FG-Squircular Mapping:

- When moving straight outwards from the center, the input will always stay straight.

- Movement around the center of the area will feel almost the same as a rectangular area and more distorted towards the edges.

- Secondary mapping types allow for lots of customizability in how the distortion is handled. Different parts of areas can have more or less distortion depending on the mapping.

### Elliptical Grid Mapping:

- When moving straight outwards from the center, the input will curve towards the corner to corner diagonals or the axes depending on whether the mapping is forward or inverse respectively.

- Movement around the center of the area will feel almost the same as a rectangular area and more distorted towards the edges.

- Secondary mapping types have the option to add more distortion towards the corners or distribute distortion axially. Axial distribution of the distortion stretches either the X or Y axis (Horizontal or Vertical) more than the other axis.

### Simple Stretch:

- Moving straight outwards from the center the input will always stay straight.

- Moving around any part of the area and crossing any line of symmetry will make input change direction often unexpectedly to the user. The lines of symmetry are the X and Y axis as well as the corner to corner diagonals.

- Secondary mapping types distribute distortion axially in different ways. These mappings will feel very similar to the user. Axial distribution of the distortion stretches either the X or Y axis (Horizontal or Vertical) more than the other axis.

### Lamé-based Mappings:

Note: These mappings are not direct counterparts of each other. Lamé-based Mappings cannot be explicitly reversed.

- Forward mapping: 
    - When moving straight outwards from the center, the input will curve towards the corners to an extreme extent.
    - On small areas or low/medium LPI tablets, input will jump when crossing the axes far from the center due to the extreme distortion.
    - Movements get faster non-linearly towards the corners and sides of the area.

<div>

- Inverse mapping:
    - When moving straight outwards from the center, the input will always stay straight. But, when offset towards any corner input will bubble outwards towards that corner.
    - On small areas or low/medium LPI tablets, input will jump when crossing the axes near the center due to the extreme distortion.
    - Movements get slower non-linearly towards the corners and sides of the area.

<br>

## Secondary Mapping Styles

These mappings are based off and will be compared to their respective primary mappings.

### FG-Squircular Mapping Based:

- **2-Squircular Mapping and 3-Squircular Mapping**
    - Forward mappings: Far corners pull the cursor more. The middle and sides distort less. 3-Squircular Mapping does this slightly more than 2-Squircular Mapping.
    - Inverse mappings: Excluding the axes, the sides and corners pull inwards less. Also excluding the axes, the corners are further away relative to the sides. 3-Squircular Mapping does this slightly more than 2-Squircular Mapping.

<div>

- **Cornerific Tapered2 Mapping and Tapered4 Mapping**
    - Forward mappings: Far corners pull the cursor less. The middle and sides distort more. Cornerific Tapered2 Mapping does this slightly more than Tapered4 Mapping.
    - Inverse mappings: Excluding the axes, the sides and corners pull inwards more. Also excluding the axes, the corners are closer relative to the sides. Cornerific Tapered2 Mapping does this slightly more than Tapered4 Mapping.
    
<div>

- **Non-Axial 2-Pinch Mapping**
    - Forward mapping: The middle of the area is a lot faster than the sides and corners. Far corners pull the cursor a lot less. The sides distort a lot less. On small areas or low/medium LPI tablets, input will jump when crossing the axes near the center due to the extreme distortion.
    - Inverse mapping: The middle of the area is a lot slower than the sides and corners. Excluding the axes, the sides and corners pull inwards a lot less.
    
<div>

- **Non-Axial ½-Punch Mapping**
    - Forward mapping: The middle of the area is a lot slower than the sides and corners. Far corners pull the cursor a lot more. The sides distort a lot more.
    - Inverse mapping: The middle of the area is a lot faster than the sides and corners. Excluding the axes, the sides and corners pull inwards a lot more. On small areas or low/medium LPI tablets, input will jump when crossing the axes near the center due to the extreme distortion.

<div>

- **Sham Quartic Mapping**
    - Forward mapping: This mapping is bugged. If you know how to solve a quartic equation in code and want to help add this please contact me on discord: Kuuube#6878. Serious inquiries only.
    - Inverse mapping: The middle of the area is faster than the sides and corners. Excluding the axes, the sides and corners pull inwards less. Also excluding the axes, the corners are further away relative to the sides.

<div>

### Elliptical Grid Mapping Based:

 - **Squelched Grid Open Mapping**
    - Forward mapping: Corner to corner diagonals pull the cursor more from all positions. This effect is strengthened closer to the sides and corners.
    - Inverse mapping: Axes pull the cursor more from all positions. This effect is strengthened closer to the sides and corners.
    
<div>

- **Vertical Squelch Open Mapping**
    - Forward mappings: Corner to corner diagonals pull the cursor more from the X-axis and less from the Y-axis. This effect is strengthened closer to the sides and corners.
    - Inverse mappings: Axes pull the cursor more from the X-axis and less from the Y-axis. This effect is strengthened closer to the sides and corners.

<div>

- **Horizontal Squelch Open Mapping**
    - Forward mappings: Corner to corner diagonals pull the cursor more from the Y-axis and less from the X-axis. This effect is strengthened closer to the sides and corners.
    - Inverse mappings: Axes pull the cursor more from the Y-axis and less from the X-axis. This effect is strengthened closer to the sides and corners.

<div>

### Simple Stretch Based:

- **Concentric Mapping**
    - Forward mapping: On the X-axis input will be offset towards the corner to corner diagonals more. On the Y-axis input will be offset towards the axes more.
    - Inverse mapping: On the Y-axis input will be offset towards the corner to corner diagonals more. On the X-axis input will be offset towards the axes more. Excluding the axes, the corners are further away relative to the sides.
    
<div>

- **Approximate Equal Area**
    - Forward mapping: On the Y-axis input will be offset towards the corner to corner diagonals more. On the X-axis input will be offset towards the axes more.
    - Inverse mapping: On the X-axis input will be offset towards the corner to corner diagonals more. On the Y-axis input will be offset towards the axes more. Excluding the axes, the corners are further away relative to the sides.
    
<div>

- **Approximate Equal Area 2 Vertical**
    - Forward mapping: On the Y-axis input will be offset towards the corner to corner diagonals more. On the X-axis input will be offset towards the axes more.
    
<div>

- **Approximate Equal Area 2 Horizontal**
    - Forward mapping: On the Y-axis input will be offset towards the axes more. On the X-axis input will be offset towards the corner to corner diagonals more.

<br>

## Tertiary Mapping Styles

These mappings mix between primary and secondary mappings. Changing the value of `β` changes how the tertiary mapping is mixed. `β` accepts any value from 0 to 1.

Below are `β` values where tertiary mappings match primary or secondary mappings.

### FG-Squircular Mapping Based:

- **Power2 Blend**
    - 2-Squircular Mapping: β = 0
    - FG-Squircular Mapping: β = 0.5
    - Cornerific Tapered2 Mapping: β = 1

<div>

- **Power3 Blend**
    - 3-Squircular Mapping: β = 0
    - FG-Squircular Mapping: β = 0.66
    - Tapered4 Mapping: β = 1

<div>

### Elliptical Grid Mapping Based:

- **Blended E-Grid Mapping**
    - Squelched Grid Open Mapping: β = 0
    - Elliptical Grid Mapping: β = 1

<div>

- **Biased Squelch Blended Mapping**
    - Vertical Squelch Open Mapping: β = 0
    - Elliptical Grid Mapping: β = 0.5
    - Horizontal Squelch Open Mapping: β = 1

<div>

- **Biased Squelch Horizontal**
    - Horizontal Squelch Open Mapping: β = 0
    - Squelched Grid Open Mapping: β = 1

<div>

- **Biased Squelch Vertical**
    - Vertical Squelch Open Mapping: β = 0
    - Squelched Grid Open Mapping: β = 1