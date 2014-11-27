#pragma once

#include "Octree.h"
#include "Vertex.h"
#include "MessageHandler.h"

struct OperationData
{
    // The information on the operation to perform.
    union
    {
        CsExtrudeSettings extrudeSettings;
        CsRevolveSettings revolveSettings;
    };

    CsBitPlane bitPlane;

    bool hasExtrudeSettings;
    bool hasRevolveSettings;

    bool hasBitPlane;

    OperationData() : hasExtrudeSettings (false), hasRevolveSettings(false), hasBitPlane(false)
    {
    }

    bool isReady()
    {
        return hasBitPlane && (hasExtrudeSettings || hasRevolveSettings);
    }
};

class ShapeManipulator
{
    // Designed shape.
    double length, resolution;
    Octree *pShapeOctree;
    bool needsGpuUpdate;

    // Next operation to be performed upon the shape.
    OperationData currentOperation;
    
    bool IsBitFilled(int xp, int yp, CsBitPlane &bitPlane);
    void ExecuteOperation();
public:

    ShapeManipulator();
    void Refresh(double length = 10, double resolution = 0.1);
    colorVertex* LoadTriangulatedShape(int &vertexCount);
    
    void AddExecute(CsExtrudeSettings extrudeSettings);
    void AddExecute(CsBitPlane bitPlane);
    ~ShapeManipulator();
};

