--------------------------------------------------------------------
Qubicle Unite 
Version 1.0
Unity3D Plugin
Copyright (c) 2013 Tim Wesoly. All rights reserved.
--------------------------------------------------------------------



WELCOME!
--------------------------------------------------------------------

Thank you for your decision to use Qubicle Constructor and Qubicle
Unite to easily create 3d assets based on voxels.

Qubicle Unite extends Unity3d to import voxel models made with the
professional voxel editor Qubicle Constructor. Using it is as easy
as importing any other of Unity’s supported file formats: just drag
and drop the file on your project, the rest is done automatically
for you. Qubicle Unite reads the voxel data, creates a mesh and
optimizes it. The output mesh’s polycount is reduced by up to 90%
and uses a single draw call per imported object. Qubicle Unite
uses a vertex shader to guarantee crisp render results.


HOW TO USE QUBICLE UNITE
--------------------------------------------------------------------
- Select "Create New Project" from Unity's main menu
- In the "Import the following packages section" check
  QubicleUnite.unitypackage.
- Import a QEF or QB file by dragging it onto the project window or
  by copying it to the assets folder - just like you would do with
  any other supported asset type.
- Qubicle Unite analyzes and optimizes the voxel data found in the
  QEF/QB and creates a Collada file (*.dae) and a texture file
  (*-tex.png). The Collada file is stored in the same folder where
  you placed the QEF/QB. The texture is saved in the "Materials" 
  folder. 
- Unity3D automatically reimports the DAE file
- To use the texture: during the dae import Unity3D automatically
  created a material named "No Name" and placed it in the folder
  "Materials". Drag'n'drop the texture (*-tex.png) on this material.
  In the texture settings set Filter Mode to Point and Format to
  16bits or TrueColor
- To use the scene builder: after importing a Qubicle Binary file
  Qubicle Unite creates a qsbm file (Qubicle Scene Builder Metadata)
  with the same name in your project folder.
  Select "Assets > Qubicle Unite > Scene Builder" from the menu
  and select that qsbm file to let the Scene builder do the
  positioning and material creation/assignment for you automatically.
  This does only work with the new *.qb file format and not with the
  old *.qef file format.

Note that during the import process the QEF/QB file will be
deleted.



PREFERENCES
--------------------------------------------------------------------
Choose Assets > Qubicle Unite -> Import Settings from Unity's main
to adjust the import settings.

Here you can
- set the pivot alignment for all three axes
- adjust the texel side length. If you set this value to 8 for
  example each visible side of a voxel gets an 8x8 pixel square in
  the texture. This has direct influence on the texture size.
  If the texture gets bigger than 4096x4096 pixels Qubicle Unite
  automatically chooses the next smaller setting that would result
  in a 4k texture.
- set the amount of pixels used for edge bleeding



RELEASE NOTES
--------------------------------------------------------------------

version 1.4.8 21/6/2013
[*] New import setting "Blender compatible Collada DAE"

version 1.4.7 18/2/2013
[*] Qubicle Unite main menu entry moved to Assets
[+] Adjustable pivot alignment
[+] Import settings dialogue

version 1.4.5 26/11/2012
[+] Unity 4.0 support
[+] minor fixes

version 1.4.4 22/10/2012
[+] QB import supports matrix naming
[+] Scene builder

version 1.4.3 9/10/2012
[+] QB (Qubicle Binary) support 
[+] Improved mesh optimization
[+] Edge bleed

version 1.3.3 27/4/2012
[*] Fixed flipped axis

version 1.3.2 20/4/2012
[*] Fixed uv padding

version 1.3.1 17/4/2012
[+] Texture export

version 1.2 13/4/2012
[*] Fixed uv mapping

version 1.1 30/3/2012
[+] Collada file export

version 1.0 1/2/2012
- first release



CREDITS
--------------------------------------------------------------------
Qubicle Constructor developed by Tim Wesoly
Qubicle Unite developed by Tim Wesoly and Tobias Pott



CONTACT
--------------------------------------------------------------------
http://www.minddesk.com
info@minddesk.com
