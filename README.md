# Circular Area

## Quick start guide:
- Enable one of the mapping types and apply the setting

Note: It is recommended to start with either FG-Squircular or Elliptical Grid and branch out from there


## Explanation of the mappings:
#### Forumla Variables:
**(u,v)** are circular coordinates in the domain {(u,v) | u² + v² ≤ 1}

**(x,y)** are square coordinates in the range [-1,1] x [-1,1]

sgn(x) = |x|/x
<br />
<br />

### Notes: 
- Inverse mappings map the rectangular tablet area to a circle then multiply the circle's size by ≈1.4 so no parts of the screen are unreachable. None of the formulas or diagrams for inverse mappings are shown here.
- Many of the mapping formulas are not listed and no diagrams are shown. The sources and explanations for these can be found in the credits.
------

<br />

### Simple Stretch:
#### Mapping Diagram:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/Simple_Stretch.png)
#### Formula:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/Simple_Stretch_formula.PNG)
<br />
<br />
**Mappings based off Simple Stretch Mapping:** Concentric Mapping, Approximate Equal Area, Approximate Equal Area 2 Horizontal, and Approximate Equal Area 2 Vertical 
<br />
<br />

------

<br />

### FG-Squircular Mapping:
#### Mapping Diagram:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/FG-Squircular.png)
#### Formula:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/FG-Squircular_formula.PNG)
<br />
Note: for FG-Squircular Mapping when u = 0 or v = 0 set (x,y) = (u,v)
<br />
<br />
**Axial linear mappings based off FG-Squircular:** 2-Squircular Mapping, 3-Squircular Mapping, Cornerific Tapered2 Mapping, and Tapered4 Mapping

**Axial non-linear mappings based off FG-Squircular:** Non-Axial 2-Pinch Mapping, Non-Axial ½-Punch Mapping, and Sham Quartic Mapping

**Blended mappings based off FG-Squircular:** Power2 Blend (0 = 2-Squircular, 1 = Cornerific Tapered2) and Power3 Blend (0 = 3-Squircular, 1 = Tapered4 Squircular)
<br />
<br />

------

<br />

### Elliptical Grid Mapping:
#### Mapping Diagram:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/Elliptical_Grid.png)
#### Formula:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/Elliptical_Grid_formula.png)
<br />
<br />
**Mappings based off Elliptical Grid Mapping:** Squelched Grid Open Mapping, Vertical Squelch Open Mapping, and Horizontal Squelch Open Mapping

**Blended mappings based off Elliptical Grid Mapping:** Blended E-Grid Mapping (0 = Squelched Grid, 1 = Elliptical Grid), Biased Squelch Blended Mapping (0 = Vertical Squelch, 1 = Horizontal Squelch), Biased Squelch Horizontal (0 = Horizontal Squelch, 1 = Squelched Grid), and Biased Squelch Vertical (0 = Vertical Squelch, 1 = Squelched Grid)
<br />
<br />

------

<br />

### Lamé-based Mapping:
#### Mapping Diagram:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/Lame_based.PNG)
#### Formula:
![](https://raw.githubusercontent.com/Kuuuube/Circular_Area/main/readme_img/Lame_based_formula.PNG)
<br />
<br />

------

<br />

Huge thanks to [X9VoiD](https://github.com/X9VoiD) for helping majorly with the code, Chamberlain Fong's great research papers: \([1](https://arxiv.org/abs/1509.06344), [2](https://arxiv.org/abs/1709.07875)\) and [blog](https://squircular.blogspot.com/) for explaining all these mapping transformations and for the mapping diagrams and [this](http://marc-b-reynolds.github.io/math/2017/01/08/SquareDisc.html) post by Marc B. Reynolds for 3 of the mappings.
