# Plant3D-xml-import
test for importing xmplant format into Plant 3D, for testing purpose only, use at own risk..

This test was made for a mixed-metric xmplant input xml file (pipe nominal diameters in inch, coordinates in mm). It (kind of) worked on a mixed metric Plant 3D project.

How it works:

It collects all "PipingNetworkSegment" and loops through them. 
Attributes like Nominal Diameter and Spec are read from the "GenericAttributes" section of the segment.
It then collects all "Pipe" and "PipingComponent" elements within the segment and draws AutoCAD line representations for each, based on the following logic:
It collects all nodes from the elements and connects the first node (which seems to be always the center) with the other nodes as AutoCAD lines.
All lines created for a segment will then be converted by "linetopipe", using the "spec" and "size" option to provide the information from the "GenericAttributes".

From a quick check the following limitations apply (for sure not a complete list):
- no valves (and potential other not mentioned items that might be in this xml) can be created, because "lintopipe" cannot do it anyway
- reducers and tees and other pipe connections happen randomly and it will be hard to control them with the "linetopipe" approach
- line group not set by this script, but should be doable
- insulation not there, but that might be doable by assigning to the line group

Generally speaking the better approach to this import might be to translate this (xmplant) xml format into (Plant 3D) PCF format and then using the "PCF to pipe" function in Plant 3D, which allows to import several PCFs at a time.



