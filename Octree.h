#pragma once

#include "Vertex.h"

// Holds node-specific data
struct OctreeData
{
    bool isFilled; // If this cube is filled or not.
    bool hasConsensus; // If the isFilled marker can be trusted (ie, all children are null)

    OctreeData(bool defaultFillStatus) : isFilled(defaultFillStatus), hasConsensus (true) { }
};

struct OctreeNode
{
    OctreeData nodeData;
    
    static const int OCTANT_COUNT = 8;
    OctreeNode *miniCubes[OCTANT_COUNT];
    
    OctreeNode (bool defaultFillStatus) : nodeData(defaultFillStatus) 
    {
        for (int i = 0; i < OCTANT_COUNT; i++)
        {
            miniCubes[i] = nullptr;
        }
    }
};

class Octree
{
    static const int OCTANT_COUNT = 8;
    static enum CubeOctant { XLYLZL = 0, XHYLZL, XLYHZL, XHYHZL, XLYLZH, XHYLZH, XLYHZH, XHYHZH };
    OctreeNode *pRootNode;
    double limits, minimumResolution;

    void RecursiveAddDeleteCube(double xL, double yL, double zL, double xH, double yH, double zH, double x, double y, double z, OctreeNode *pCurrentNode, bool isAdd);
    OctreeData* RecursiveQueryCube(double xL, double yL, double zL, double xH, double yH, double zH, double x, double y, double z, OctreeNode *pCurrentNode);
    void RecursiveDelete(OctreeNode *pCurrentNode);
    int RecursiveCubeCount(OctreeNode *pCurrentNode);
    int RecursiveTriangulateCube(double xL, double yL, double zL, double xH, double yH, double zH, OctreeNode *pCurrentNode, colorVertex **ppVertices, int currentCubeCount);

    inline double LowerX(double xL, double xH, CubeOctant octant);
    inline double LowerY(double yL, double yH, CubeOctant octant);
    inline double LowerZ(double zL, double zH, CubeOctant octant);
    inline double UpperX(double xL, double xH, CubeOctant octant);
    inline double UpperY(double yL, double yH, CubeOctant octant);
    inline double UpperZ(double zL, double zH, CubeOctant octant);
    inline CubeOctant GetOctant(double xL, double yL, double zL, double xH, double yH, double zH, double x, double y, double z);
    inline bool AtResolution(double xL, double yL, double zL, double xH, double yH, double zH);
    inline bool InLimits(double x, double y, double z);
public:
    Octree(double limits, double minResolution, bool isFull);

    bool AddCube(double x, double y, double z);
    OctreeData* QueryCube(double x, double y, double z);
    bool DeleteCube(double x, double y, double z);

    colorVertex* Triangulate(int& vertexCount);

    ~Octree(void);
};