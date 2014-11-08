#pragma once
// Just a lot of different potential vertex types

struct colorVertex
{
    float x;
    float y;
    float z;
    float r;
    float g;
    float b;

    void Set(double xx, double yy, double zz, double rr, double gg, double bb)
    {
        x = (float)xx;
        y = (float)yy;
        z = (float)zz;
        r = (float)rr;
        g = (float)gg;
        b = (float)bb;
    }
};

struct textureNormalVertex
{
    float u;
    float v;
    float xn;
    float yn;
    float zn;

    textureNormalVertex() : u(0.0f), v(0.0f), xn(0.0f), yn(0.0f), zn(0.0f)
    { }

    textureNormalVertex(float uu, float vv, float xx, float yy, float zz)
    {
        u = uu;
        v = vv;
        xn = xx;
        yn = yy;
        zn = zz;
    }
};


struct textureNormalVertexIdx
{
    int t1, t2, t3, n1, n2, n3;

    textureNormalVertexIdx() {}

    textureNormalVertexIdx(int tt1, int tt2, int tt3, int nn1, int nn2, int nn3)
    {
        t1 = tt1;
        t2 = tt2;
        t3 = tt3;
        n1 = nn1;
        n2 = nn2;
        n3 = nn3;
    }
};

textureNormalVertex operator*(double value, const textureNormalVertex& other);
textureNormalVertex& operator+=(textureNormalVertex& result, const textureNormalVertex& other);

