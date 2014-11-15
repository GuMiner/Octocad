#include "stdafx.h"
#include "Octree.h"

Octree::Octree(double limit, double minResolution, bool isFull) : pRootNode (new OctreeNode(isFull)), limits (limit), minimumResolution (minResolution)
{
}

// Adds a cube at the specified position; true on success, false if out-of-bounds.
bool Octree::AddCube(double x, double y, double z)
{
    if (!InLimits(x, y, z))
    {
        return false;
    }

    RecursiveAddDeleteCube(-limits/2, -limits/2, -limits/2, limits/2, limits/2, limits/2, x, y, z, pRootNode, true);
    return true;
}

// Removes a cube from the specified position; true on success, false if out-of-bounds.
bool Octree::DeleteCube(double x, double y, double z)
{
    if (!InLimits(x, y, z))
    {
        return false;
    }

    RecursiveAddDeleteCube(-limits/2, -limits/2, -limits/2, limits/2, limits/2, limits/2, x, y, z, pRootNode, false);
    return true;
}

// Gets the cube data at the specified position. Returns a pointer to the data (valid until any write operations are performed), or null if out-of-bounds.
OctreeData* Octree::QueryCube(double x, double y, double z)
{
    if (!InLimits(x, y, z))
    {
        return nullptr;
    }

    return RecursiveQueryCube(-limits/2, -limits/2, -limits/2, limits/2, limits/2, limits/2, x, y, z, pRootNode);
}

// Very very temporary and inefficient way of drawing stuff to verify functionality.
static const GLfloat vertex_positions[] =
{
    -0.25f,  0.25f, -0.25f,
    -0.25f, -0.25f, -0.25f,
    0.25f, -0.25f, -0.25f,

    0.25f, -0.25f, -0.25f,
    0.25f,  0.25f, -0.25f,
    -0.25f,  0.25f, -0.25f,

    0.25f, -0.25f, -0.25f,
    0.25f, -0.25f,  0.25f,
    0.25f,  0.25f, -0.25f,

    0.25f, -0.25f,  0.25f,
    0.25f,  0.25f,  0.25f,
    0.25f,  0.25f, -0.25f,

    0.25f, -0.25f,  0.25f,
    -0.25f, -0.25f,  0.25f,
    0.25f,  0.25f,  0.25f,

    -0.25f, -0.25f,  0.25f,
    -0.25f,  0.25f,  0.25f,
    0.25f,  0.25f,  0.25f,

    -0.25f, -0.25f,  0.25f,
    -0.25f, -0.25f, -0.25f,
    -0.25f,  0.25f,  0.25f,

    -0.25f, -0.25f, -0.25f,
    -0.25f,  0.25f, -0.25f,
    -0.25f,  0.25f,  0.25f,

    -0.25f, -0.25f,  0.25f,
    0.25f, -0.25f,  0.25f,
    0.25f, -0.25f, -0.25f,

    0.25f, -0.25f, -0.25f,
    -0.25f, -0.25f, -0.25f,
    -0.25f, -0.25f,  0.25f,

    -0.25f,  0.25f, -0.25f,
    0.25f,  0.25f, -0.25f,
    0.25f,  0.25f,  0.25f,

    0.25f,  0.25f,  0.25f,
    -0.25f,  0.25f,  0.25f,
    -0.25f,  0.25f, -0.25f
};

// Returns the vertex list for all the mini cubes, along with the number of vertexes added.
colorVertex* Octree::Triangulate(int& vertexCount)
{
    // TODO perform next-to vertex optimization.
    int cubeCount = RecursiveCubeCount(pRootNode);
    
    vertexCount = 36*cubeCount;
    colorVertex *pVertices = new colorVertex[vertexCount];
    
    RecursiveTriangulateCube(-limits/2, -limits/2, -limits/2, limits/2, limits/2, limits/2, pRootNode, &pVertices, 0);

    return pVertices;
}

// Continues querying until we can't drill down the tree any more.
int Octree::RecursiveTriangulateCube(double xL, double yL, double zL, double xH, double yH, double zH, OctreeNode *pCurrentNode, colorVertex **ppVertices, int currentCubeCount)
{
    if (pCurrentNode->nodeData.hasConsensus && pCurrentNode->nodeData.isFilled)
    {
        // Current cube should be added.
        for (GLsizei j = 0; j < 36; j++)
        {
            (*ppVertices)[j + 36*currentCubeCount].Set(vertex_positions[j*3] < 0 ? xL : xH, vertex_positions[j*3 + 1] < 0 ? yL : yH, vertex_positions[j*3 + 2] < 0 ? zL : zH, (double)j/24, 0, 1.0 - (double)j/24);
        }
        return currentCubeCount + 1;
    }
    else if (pCurrentNode->nodeData.hasConsensus)
    {
        return currentCubeCount;
    }

    for (int i = 0; i < OCTANT_COUNT; i++)
    {
        bool nullIsFilled = pCurrentNode->nodeData.isFilled;
        if (pCurrentNode->miniCubes[i] == nullptr && nullIsFilled)
        {
            // Current cube should be added, but with a smaller size (as if it had been lowered)
            for (GLsizei j = 0; j < 36; j++)
            {
                (*ppVertices)[j + 36*currentCubeCount].Set(
                    vertex_positions[j*3] < 0 ? LowerX(xL, xH, (CubeOctant)i) : UpperX(xL, xH, (CubeOctant)i), 
                    vertex_positions[j*3 + 1] < 0 ? LowerY(yL, yH, (CubeOctant)i) : UpperY(yL, yH, (CubeOctant)i), 
                    vertex_positions[j*3 + 2] < 0 ? LowerZ(zL, zH, (CubeOctant)i) : UpperZ(zL, zH, (CubeOctant)i),
                    (double)j/24, 0, 1.0 - (double)j/24);
            }
            ++currentCubeCount;
        }
        else if (pCurrentNode->miniCubes[i] != nullptr)
        {
            currentCubeCount = RecursiveTriangulateCube(LowerX(xL, xH, (CubeOctant)i), LowerY(yL, yH, (CubeOctant)i), LowerZ(zL, zH, (CubeOctant)i), UpperX(xL, xH, (CubeOctant)i), UpperY(yL, yH, (CubeOctant)i), UpperZ(zL, zH, (CubeOctant)i), 
                pCurrentNode->miniCubes[i], ppVertices, currentCubeCount);
        }
    }
    
    return currentCubeCount;
}


// Counts the number of cubes we have (triangulation support)
int Octree::RecursiveCubeCount(OctreeNode *pCurrentNode)
{
    int subCount = 0;
    if (pCurrentNode->nodeData.hasConsensus && pCurrentNode->nodeData.isFilled)
    {
        // Completely filled space
        return 1;
    }
    else if (pCurrentNode->nodeData.hasConsensus)
    {
        // Empty space, no cubes here
        return 0;
    }

    for (int i = 0; i < OCTANT_COUNT; i++)
    {
        // Partially filled space
        bool nullIsFilled = pCurrentNode->nodeData.isFilled;
        if (pCurrentNode->miniCubes[i] == nullptr && nullIsFilled)
        {
            ++subCount;
        }
        else if (pCurrentNode->miniCubes[i] != nullptr)
        {
            subCount += RecursiveCubeCount(pCurrentNode->miniCubes[i]);
        }
    }

    return subCount;
}

// Drills down until the desired resolution is met in order to add or delete the cube to the octree.
void Octree::RecursiveAddDeleteCube(double xL, double yL, double zL, double xH, double yH, double zH, double x, double y, double z, OctreeNode *pCurrentNode, bool isAdd)
{
    if (AtResolution(xL, yL, zL, xH, yH, zH))
    {
        // At the level we can save this cube. Note that there can be no smaller cubes than this one.
        pCurrentNode->nodeData.isFilled = isAdd;
        pCurrentNode->nodeData.hasConsensus = true;
        return;
    }
    else if (pCurrentNode->nodeData.isFilled == isAdd && pCurrentNode->nodeData.hasConsensus)
    {
        // The supernode is the same as the individual one, so there's no point in repartitioning.
        return;
    }

    CubeOctant octant = GetOctant(xL, yL, zL, xH, yH, zH, x, y, z);

    if (pCurrentNode->miniCubes[octant] == nullptr)
    {
        // This location no longer has consensus, by virtue of us adding this node
        pCurrentNode->miniCubes[octant] = new OctreeNode(pCurrentNode->nodeData.isFilled);
        pCurrentNode->nodeData.hasConsensus = false;
    }
    RecursiveAddDeleteCube(LowerX(xL, xH, octant), LowerY(yL, yH, octant), LowerZ(zL, zH, octant), UpperX(xL, xH, octant), UpperY(yL, yH, octant), UpperZ(zL, zH, octant), 
        x, y, z, pCurrentNode->miniCubes[octant], isAdd);

    // Check to see if we can form consensus (and clean up the tree)
    if (!pCurrentNode->nodeData.hasConsensus)
    {
        bool allHaveConsensus = true;
        bool foundClear = false;
        bool foundFilled = false;
        for (int i = 0; i < OCTANT_COUNT; i++)
        {
            if (pCurrentNode->miniCubes[i] == nullptr || !pCurrentNode->miniCubes[i]->nodeData.hasConsensus)
            {
                allHaveConsensus = false;
                break;
            }
            if (pCurrentNode->miniCubes[i]->nodeData.isFilled)
            {
                foundFilled = true;
            }
            else
            {
                foundClear = true;
            }
        }

        // If they all have consensus and are the same, delete the subnodes and combine.
        if (allHaveConsensus && !(foundClear && foundFilled))
        {
            pCurrentNode->nodeData.hasConsensus = true;
            pCurrentNode->nodeData.isFilled = pCurrentNode->miniCubes[0]->nodeData.isFilled;
            for (int i = 0; i < OCTANT_COUNT; i++)
            {
                delete pCurrentNode->miniCubes[i];
                pCurrentNode->miniCubes[i] = nullptr;
            }
        }
    }
}

// Continues querying until we can't drill down the tree any more.
OctreeData* Octree::RecursiveQueryCube(double xL, double yL, double zL, double xH, double yH, double zH, double x, double y, double z, OctreeNode *pCurrentNode)
{
    CubeOctant octant = GetOctant(xL, yL, zL, xH, yH, zH, x, y, z);
    if (pCurrentNode->miniCubes[octant] == nullptr)
    {
        return &pCurrentNode->nodeData;
    }

    return RecursiveQueryCube(LowerX(xL, xH, octant), LowerY(yL, yH, octant), LowerZ(zL, zH, octant), UpperX(xL, xH, octant), UpperY(yL, yH, octant), UpperZ(zL, zH, octant), 
        x, y, z, pCurrentNode->miniCubes[octant]);
}

// A bunch of small functions to return the mini cube boundaries given the new octant
inline double Octree::LowerX(double xL, double xH, CubeOctant octant)
{
    switch (octant)
    {
    case XLYLZL:
    case XLYHZL:
    case XLYLZH:
    case XLYHZH:
        return xL;
    default:
        return (xH - xL) * 0.5 + xL;
    }
}
inline double Octree::LowerY(double yL, double yH, CubeOctant octant)
{
    switch (octant)
    {
    case XLYLZL:
    case XHYLZL:
    case XLYLZH:
    case XHYLZH:
        return yL;
    default:
        return (yH - yL) * 0.5 + yL;
    }
}
inline double Octree::LowerZ(double zL, double zH, CubeOctant octant)
{
    switch (octant)
    {
    case XLYLZL:
    case XHYLZL:
    case XLYHZL:
    case XHYHZL:
        return zL;
    default:
        return (zH - zL) * 0.5 + zL;
    }
}
inline double Octree::UpperX(double xL, double xH, CubeOctant octant)
{
    switch (octant)
    {
    case XLYLZL:
    case XLYHZL:
    case XLYLZH:
    case XLYHZH:
        return (xH - xL) * 0.5 + xL;
    default:
        return xH;
    }
}
inline double Octree::UpperY(double yL, double yH, CubeOctant octant)
{
    switch (octant)
    {
    case XLYLZL:
    case XHYLZL:
    case XLYLZH:
    case XHYLZH:
        return (yH - yL) * 0.5 + yL;
    default:
        return yH;
    }
}
inline double Octree::UpperZ(double zL, double zH, CubeOctant octant)
{
    switch (octant)
    {
    case XLYLZL:
    case XHYLZL:
    case XLYHZL:
    case XHYHZL:
        return (zH - zL) * 0.5 + zL;
    default:
        return zH;
    }
}

// Gets the octant the current position is in, given the high and low limits.
inline Octree::CubeOctant Octree::GetOctant(double xL, double yL, double zL, double xH, double yH, double zH, double x, double y, double z)
{
    bool isLowerX = (x < (xH - xL) * 0.5 + xL);
    bool isLowerY = (y < (yH - yL) * 0.5 + yL);
    bool isLowerZ = (z < (zH - zL) * 0.5 + zL);

    if (isLowerX)
    {
        if (isLowerY)
        {
            if (isLowerZ)
            {
                return XLYLZL;
            }
            return XLYLZH;
        }
        else
        {
            if (isLowerZ)
            {
                return XLYHZL;
            }
            return XLYHZH;
        }
    }
    else
    {
        if (isLowerY)
        {
            if (isLowerZ)
            {
                return XHYLZL;
            }
            return XHYLZH;
        }
        else
        {
            if (isLowerZ)
            {
                return XHYHZL;
            }
            return XHYHZH;
        }
    }
}

// Small function to determine if we have reached the desired octree resolution
inline bool Octree::AtResolution(double xL, double yL, double zL, double xH, double yH, double zH)
{
    return ((xH - xL) < minimumResolution && (yH - yL) < minimumResolution && (zH - zL) < minimumResolution);
}

// Small function to determine if we are in the current octree limits
inline bool Octree::InLimits(double x, double y, double z)
{
    return (x < limits * 0.5 && x > -limits * 0.5 && y < limits * 0.5 && y > -limits * 0.5 && z < limits * 0.5 && z > -limits * 0.5);
}

// Recursively deletes an octree node structure.
void Octree::RecursiveDelete(OctreeNode *pCurrentNode)
{
    if (pCurrentNode == nullptr)
    {
        return;
    }

    for (int i = 0; i < OCTANT_COUNT; i++)
    {
        RecursiveDelete(pCurrentNode->miniCubes[i]);
    }
}

Octree::~Octree(void)
{
    RecursiveDelete(pRootNode);
}