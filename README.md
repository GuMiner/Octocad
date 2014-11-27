Octocad
=======
An engineering-focused CAD software solution.

Introduction
------------
Octocad is an octree-based 3D CAD system. Objects are represented solely using cubes, with no boundary representation. This representation
enables a known output precision and makes CSG operations very straightforward.

Design
------
Octocad is split up into two parts:
* *Octocad 2D*: The C# 2D drawing and main control frontend.
* *Octocad*: The C++ object representation and 3D drawing backend.

These parts communicate over two named pipe connections, which are created upon process startup. Starting Octocad-2D will initialize
Octocad, and closing Octocad-2D will close Octocad.

Author Information
------------------
Gustave Granroth [gus.gran@gmail.com](mailto:gus.gran@gmail.com) [Website](http://helium24.net).

Included Libraries
------------------
* For OpenGL platform support: GLFW 3.0 - zlib\png - Marcus Geelnard|Camilla Berglund [website](http://www.glfw.org/)
* For OpenGL extension support: GLEW 1.1 - Modified BSD\MIT License - Milan Ikits|Marcelo Magallon|et al. [website](http://glew.sourceforge.net/)
* For general graphical mathematics: Generic Graphics Toolkit - LGPLv2 - Allen Bierbaum|Aron Bierbaum|Patrick Hartling [website](http://sourceforge.net/projects/ggt/)
* (GGT / GMTL has an addendum to the LGPLv2, which requires the additional statement in the documentation: "This software uses GMTL (http://ggt.sourceforge.net).")

 