#pragma once

#include "Octree.h"
#include "ShapeManipulator.h"
#include "ProcessLink.h"

class Octocad
{
    // Opengl window and matrixes (CPU)
    GLFWwindow *pWindow;
    float aspect;
    gmtl::Matrix44f proj_matrix, lookAt;

    // Application drawing data
    GLuint boringProgram;
    GLuint vao;
    
    // Veretx information
    GLuint pointBuffer;
    GLsizei vertexCount;

    // Transfered to the shader program.
    GLint mv_location, proj_location;

    // All shape manipulation and storage.
    ShapeManipulator shapeManipulator;

    // Link to C#
    ProcessLink processLink;

    void SetupViewport();
    bool WindowInitialization();
    void Render(double);

public:
    static const char* NAME;

    Octocad();
    bool ApplicationSetup();
    bool RunApplication();
    ~Octocad();
};

