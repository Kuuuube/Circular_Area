[<- Back](https://github.com/Kuuuube/Circular_Area/blob/main/README.md#circular-area-plugin-for-opentabletdriver-)

# Dev Docs

For a simplified guide on how things work from the perspective of the user, read the [Layman's Guide to Circular Area](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/layman_s_guide_to_circular_area.md#laymans-guide-to-circular-area) instead of this guide.

This is a technical guide on how Circular Area's code functions. 

The functions of OpenTabletDriver.Plugin or other dependencies are not covered.

## Basic Function

Circular Area converts the cursor position input on the monitor to a unit coordinate, transforms that coordinate by the set Circular Area mapping, converts out of unit coordinates, and returns the transformed cursor position.

<br>

## The Input Pipeline

Circular Control Panel settings alter this and are addressed in [Circular Control Panel](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#circular-control-panel).

- [**ToUnit:**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#tounit) Input is converted to unit coordinates: [-1,1].

- [**CircleToSquare and SquareToCircle:**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#circletosquare-and-squaretocircle) The input is filtered and run through the mapping formula. 

    CircleToSquare is used for forward mappings.

    SquareToCircle is used for inverse mappings.

- [**Expand:**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#expand-inverse-mappings-only) (Inverse mappings only) The input is scaled up by `Sqrt(2)`.

- [**Clamp:**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#clamp) The input is clamped to [-1,1].

- [**FromUnit:**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#fromunit) Input is converted out of unit coordinates.

<br>

## ToUnit

```csharp
var display = outputMode?.Output;
var offset = (Vector2)(outputMode?.Output?.Position);
var shiftoffX = offset.X - (display.Width / 2);
var shiftoffY = offset.Y - (display.Height / 2);
return new Vector2(
    (input.X - shiftoffX) / display.Width * 2 - 1,
    (input.Y - shiftoffY) / display.Height * 2 - 1
    );
```

- `display`

    Contains the width and height of the monitor area in pixels.

- `offset`
    
    Contains the position of the monitor area in pixels. OpenTabletDriver measures offset from the top left to the center of the monitor area. 

    For example, a 1920 x 1080 monitor area placed with its top left corner aligned to the top left corner of the usable monitor area will have an offset of 960 x 540.

## CircleToSquare and SquareToCircle

### All CircleToSquare and SquareToCircle mappings

- Floats and doubles:

    All mapping formula calculations **must** be done using doubles. Using floats in the mapping formulas can cause large inaccuracies and even entirely break some mappings. In OpenTabletDriver, input must be returned as a float after calculation.

- (x,y) and (u,v):

    (u,v) is used for the X and Y axis of input in forward mappings.

    (x,y) is used for the X and Y axis of input in inverse mappings.

- `return No_NaN(circle, input);`

    Ignoring NaN input can make it hard to reach corners on some mappings. Instead, `No_NaN` filters out all NaN input and forces input to go far beyond the closest corner. This input is then clamped back to the closest corner. NaN input can only occur when unintended input (such as going out of bounds) is put into a mapping formula.

### Specific CircleToSquare and SquareToCircle mappings

**FG-Squircular Based Mappings:**

```csharp
if (Math.Abs(/*X axis input*/) < 0.00001 || Math.Abs(/*Y axis input*/) < 0.00001)
{
    var circle = new Vector2(
            (float)(/*X axis input*/),
            (float)(/*Y axis input*/)
            );

    return No_NaN(circle, input);
}
else
{
    var circle = new Vector2(
        (float)(/*Mapping formula for the X axis*/),
        (float)(/*Mapping formula for the Y axis*/)
        );

    return No_NaN(circle, input);
}
```

- `if (Math.Abs(/*X axis input*/) < 0.00001 || Math.Abs(/*Y axis input*/) < 0.00001)`

    Filters out input where either X or Y is 0. The FG-Squircular based mapping formulas (with a few exceptions) cannot handle zeros, and points along the axes are not transformed.

**Simple Stretch Based Mappings:**

```csharp
if (/*Comparison to decide which formula*/)
{
    var circle = new Vector2(
    (float)(/*Mapping formula for the X axis*/),
    (float)(/*Mapping formula for the Y axis*/)
    );

    return No_NaN(circle, input);
}
else
{
    var circle = new Vector2(
    (float)(/*Mapping formula for the X axis*/),
    (float)(/*Mapping formula for the Y axis*/)
    );

    return No_NaN(circle, input);
}
```

- `if (/*Comparison to decide which formula*/)`:

    Simple Stretch based mappings have two mapping formulas that are used based off a set condition.

**Elliptical-Grid Based Mappings:**

```csharp
var circle = new Vector2(
    (float)(/*Mapping formula for the X axis*/),
    (float)(/*Mapping formula for the Y axis*/)
    );

return No_NaN(circle, input);
```

- Elliptical-Grid based mappings (with a few exceptions) do not require any special handling.

**Lamé Based Mappings:**

```csharp
var circle = new Vector2(
    (float)(/*Mapping formula for the X axis*/),
    (float)(/*Mapping formula for the Y axis*/)
    );

return No_NaN(circle, input);
```

- Lamé-based mappings do not require any special handling.

### Special Cases

**Biased Squelch Blended Mapping and Blended E-Grid Mapping:**

```csharp
if (Math.Abs(/*X axis input*/) < 0.00001 || Math.Abs(/*Y axis input*/) < 0.00001)
{
    var circle = new Vector2(
        (float)(/*X axis input*/),
        (float)(/*Y axis input*/)
        );

    return No_NaN(circle, input);
}
else
{
    var circle = new Vector2(
        (float)(/*Mapping formula for the X axis*/),
        (float)(/*Mapping formula for the Y axis*/)
        );

    return No_NaN(circle, input);
}
```

- `if (Math.Abs(/*X axis input*/) < 0.00001 || Math.Abs(/*Y axis input*/) < 0.00001)`

    Filters out input where either X or Y is 0. Despite being Elliptical-Grid based mappings, Biased Squelch Blended Mapping and Blended E-Grid Mapping mapping formulas cannot handle zeros, and points along the axes are not transformed.

**Sham Quartic Mapping Inverse, Non-axial 2-Pinch Mapping Inverse, and Non-axial Half-Punch Mapping Inverse:**

```csharp
var circle = new Vector2(
    (float)(/*Mapping formula for the X axis*/),
    (float)(/*Mapping formula for the Y axis*/)
    );

return No_NaN(circle, input);
```

- Despite being FG-Squircular based mappings, Sham Quartic Mapping Inverse, Non-axial 2-Pinch Mapping Inverse, and Non-axial Half-Punch Mapping Inverse do not require any special handling.

**Non-axial 2-Pinch Mapping and Non-axial Half-Punch Mapping**

```csharp
if (Math.Abs(/*Y axis input*/) < 0.00001)
{
    var circle = new Vector2(
            (float)(/*Mapping formula for the X axis*/),
            (float)(/*Mapping formula for the Y axis*/)
            );

    return No_NaN(circle, input);
}
else
{
    if (Math.Abs(/*X axis input*/) < 0.00001)
    {
        var circle = new Vector2(
            (float)(/*Mapping formula for the X axis*/),
            (float)(/*Mapping formula for the Y axis*/)
            );

        return No_NaN(circle, input);
    }
    else
    {
        var circle = new Vector2(
            (float)(/*Mapping formula for the X axis*/),
            (float)(/*Mapping formula for the Y axis*/)
            );

        return No_NaN(circle, input);
    }
}
```

- `if (Math.Abs(/*X/Y axis input*/) < 0.00001)`:

    Non-axial 2-Pinch Mapping and Non-axial Half-Punch Mapping have three mapping formulas. They handle when X input is 0, when Y input is 0, and when neither input is 0.

<br>

## Expand (inverse mappings only)

```csharp
return new Vector2(
(float)(input.X * Math.Sqrt(2)),
(float)(input.Y * Math.Sqrt(2))
);
```

- Used by inverse mappings to expand the input, which is mapped to a circle in order to cover the entire monitor.
    
    In unit coordinates, 2 is the diameter of the mapped circle, and Sqrt(8) is the length of a square's corner to corner diagonals. To expand this circle's diameter to the same size as the square's corner to corner diagonals, the following can be done: `circle * (Sqrt(8) / 2)`. This simplifies to `circle * Sqrt(2)`.

<br>

## Clamp

```csharp
return new Vector2(
Math.Clamp(input.X, -1, 1),
Math.Clamp(input.Y, -1, 1)
);
```

- Before converting from unit coordinates back to normal coordinates, input is clamped to the minimum and maximum unit coordinates.

<br>

## FromUnit

```csharp
var display = outputMode?.Output;
var offset = (Vector2)(outputMode?.Output?.Position);
var shiftoffX = offset.X - (display.Width / 2);
var shiftoffY = offset.Y - (display.Height / 2);
return new Vector2(
    (input.X + 1) / 2 * display.Width + shiftoffX,
    (input.Y + 1) / 2 * display.Height + shiftoffY
);
```

- `display`

    Contains the width and height of the monitor area in pixels.

- `offset`
    
    Contains the position of the monitor area in pixels. OpenTabletDriver measures offset from the top left to the center of the monitor area. 

    For example, a 1920 x 1080 monitor area placed with its top left corner aligned to the top left corner of the usable monitor area will have an offset of 960 x 540.

<br>

## Input β

```csharp
[Property("β")]
public float B_raw { set; get; }
```

```csharp
float B = Math.Clamp(B_raw, 0.01f, 1);
```

- Blended or biased mappings allow for β to be used to mix between mappings. β allows any float as an input.

- β must never be 0.

## Circular Control Panel

Circular Control Panel edits the input pipeline by adding various options. The majority of the code used for this is specific to working with OpenTabletDriver.Plugin and will not be covered as it should not pertain to other implementations.

There are three main parts of Circular Control Panel:

-[**Truncation**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#truncation) changes the distortion by scaling the input smaller, applying the mapping, and scaling it back up.

-[**Quadrant Disabling**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#quadrant-disabling) disables a mapping for a quadrant of input and reports the raw input back instead of applying the mapping.

-[**Expand Disabling**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#expand-disabling) disables [Expand](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#expand-inverse-mappings-only) either when a quadrant is disabled or when a quadrant is not disabled.

## Truncation

**ApplyTruncation**

```csharp
if (Truncation > 1)
{
    return input / Truncation;
}
else if (Truncation < 1)
{
    return input * Truncation;
}
return input;
```

**DiscardTruncation**

```csharp
if (Truncation > 1)
{
    return input * Truncation;
}
else if (Truncation < 1)
{
    return input / Truncation;
}
return input;
```

- `Truncation`

    Contains the user input for the truncation to be applied

**Example Input Pipeline**

- [**ToUnit**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#tounit)

- **ApplyTruncation**

- [**CircleToSquare and SquareToCircle**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#circletosquare-and-squaretocircle)

- [**Expand**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#expand-inverse-mappings-only) (Inverse mappings only)

- **DiscardTruncation**

- [**Clamp**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#clamp)

- [**FromUnit**](https://github.com/Kuuuube/Circular_Area/blob/main/wiki/dev_docs.md#fromunit)

## Quadrant Disabling

```csharp
if (input.X > 0 && input.Y > 0)
{
    //Check if the option to disable Q1 is enabled. If so, return true.
    if (/*Q1 disabled setting*/)
    {
        return true;
    }
}
if (input.X < 0 && input.Y > 0)
{
    //Check if the option to disable Q2 is enabled. If so, return true.
    if (/*Q2 disabled setting*/)
    {
        return true;
    }
}
if (input.X < 0 && input.Y < 0)
{
    //Check if the option to disable Q3 is enabled. If so, return true.
    if (/*Q3 disabled setting*/)
    {
        return true;
    }
}
if (input.X > 0 && input.Y < 0)
{
    //Check if the option to disable Q4 is enabled. If so, return true.
    if (/*Q4 disabled setting*/)
    {
        return true;
    }
}
//If no quadrants are found (either X and/or Y is 0), return false.
//If none of the disable quadrant settings are enabled also return false.
return false;
```

```csharp
if (CheckQuadrant(ToUnit(input)))
{
    return input;
}
//else, handle input normally
```

- CheckQuadrant should be called before handling the input normally. If the current quadrant is disabled the input should not be run through the pipeline.

- The Y axis checks are reversed in the actual plugin code due to how OTD handles input.

## Expand Disabling

```csharp
if (/*Setting to disable expanding if a quadrant is disabled*/)
{
    return true;
}
if (/*Setting to disable expanding if a quadrant is not disabled*/)
{
    return true;
}
```

```csharp
if (GetDisableExpand())
{
    return FromUnit(Clamp(DiscardTruncation(SquareToCircle(ApplyTruncation(ToUnit(input))))));
}
```

- GetDisableExpand should be called before handling the input normally. If the current input should not be expanded it should be run through a modified pipeline without expand.