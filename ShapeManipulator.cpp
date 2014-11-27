#include "stdafx.h"
#include "ShapeManipulator.h"

ShapeManipulator::ShapeManipulator() : pShapeOctree(nullptr), currentOperation(), needsGpuUpdate(true)
{
}

// Recreates the interior data structure with the following side length and resolution.
void ShapeManipulator::Refresh(double length, double resolution)
{
    if (pShapeOctree != nullptr)
    {
        delete pShapeOctree;
    }

    this->length = length;
    this->resolution = resolution;
    pShapeOctree = new Octree(length, resolution, false);
}

// Sends the triangulated shape data to the GPU, only if necessary.
colorVertex* ShapeManipulator::LoadTriangulatedShape(int &vertexCount)
{
    if (!needsGpuUpdate || pShapeOctree == nullptr)
    {
        return nullptr;
    }

    colorVertex *pTriangles = pShapeOctree->Triangulate(vertexCount);
    needsGpuUpdate = false;

    return pTriangles;
}

// Adds the extrude settings to the current operation, and if ready, executes the instructions.
void ShapeManipulator::AddExecute(CsExtrudeSettings extrudeSettings)
{
    currentOperation.extrudeSettings = extrudeSettings;
    currentOperation.hasExtrudeSettings = true;
    if (currentOperation.isReady())
    {
        ExecuteOperation();
    }
}

// Adds the bit plane to the current operation, and if ready, executes the instructions.
void ShapeManipulator::AddExecute(CsBitPlane bitPlane)
{
    currentOperation.bitPlane = bitPlane;
    currentOperation.hasBitPlane = true;
    if (currentOperation.isReady())
    {
        ExecuteOperation();
    }
}

// True if the specified point is filled, false otherwise.
bool ShapeManipulator::IsBitFilled(int xp, int yp, CsBitPlane &bitPlane)
{
    char* row = &bitPlane.pImage[0] + (yp * bitPlane.stride) + (xp / 8);
    switch (xp % 8)
    {
        case 0:
            return ((*row) & 0x80) == 0;
        case 1:
            return ((*row) & 0x40) == 0;
        case 2:
            return ((*row) & 0x20) == 0;
        case 3:
            return ((*row) & 0x10) == 0;
        case 4:
            return ((*row) & 0x08) == 0;
        case 5:
            return ((*row) & 0x04) == 0;
        case 6:
            return ((*row) & 0x02) == 0;
        case 7:
            return ((*row) & 0x01) == 0;
    }
    return false;
}

// Executes the current operation.
void ShapeManipulator::ExecuteOperation()
{
    // TODO remove duplicate code here, when possible
    if (currentOperation.hasExtrudeSettings)
    {
        // Extrusion
        // TODO Figure out current direction and iteration directions.
        gmtl::Vec3d startPosition (-length/2, -length/2, -length/2);

        gmtl::Vec3d xBoard (resolution/2, 0, 0), yBoard (0, resolution/2, 0), zBoard(0, 0, resolution/2); // Movement to go when moving x, y, and z in terms of the bit plane.

        // Note that we supersample the image
        for (int i = 0; i < currentOperation.bitPlane.width*2; i++)
        {
            for (int j = 0; j < currentOperation.bitPlane.height*2; j++)
            {
                bool isFilled = IsBitFilled(i/2, j/2, currentOperation.bitPlane);

                // TODO handle negative distances and negation and other operations and supersampling of the z axis
                for (int k = 0; k < 2*currentOperation.extrudeSettings.distance/resolution; k++)
                {
                    if (isFilled)
                    {
                        gmtl::Vec3d currentPos = startPosition + xBoard * (double)i + yBoard * (double)j + zBoard * (double)k;
                        pShapeOctree->AddCube(currentPos[0], currentPos[1], currentPos[2]);
                    }
                }
            }
        }

        currentOperation.hasExtrudeSettings = false;
    }

    // Clean up the no-longer needed bit plane.
    delete [] currentOperation.bitPlane.pImage;
    currentOperation.hasBitPlane = false;

    // Invalidate for drawing
    needsGpuUpdate = true;
}

ShapeManipulator::~ShapeManipulator()
{
    if (pShapeOctree != nullptr)
    {
        delete pShapeOctree;
    }
}
