#pragma once

#include "Octree.h"

class Octocad
{
    // Opengl window and matrixes (CPU)
    GLFWwindow *pWindow;
    float aspect;
    gm::mat4 proj_matrix, lookAt;

    // Application drawing data
    GLuint boringProgram;
    GLuint vao;
    
    // Veretx information
    GLuint pointBuffer;
    GLsizei vertexCount;

    // Transfered to the shader program.
    GLint mv_location, proj_location;

    // Test Octree
    Octree *testTree;

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

