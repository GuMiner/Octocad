#pragma once
// Includes common OpenGL constants and utility functions.

#include "stdafx.h"

class GLManager
{
    // Using a pointer instead of a STL unique_pointer to more delicately handle OpenGL memory.
    static GLManager *m_pManager;

public:
    // General constants.
    static const int OPENGL_MAJOR = 4, OPENGL_MINOR = 0;
    static const int ALIASING_LEVEL = 1;
    static const int FPS_TARGET = 60;
    static const int TEXTURE_WH = 1024; // Texture width and height (assumed).
    static float FOV_Y, NEAR_PLANE, FAR_PLANE;
    
    // Window and operational parameters
    bool running;
    bool fullscreen;
    int width, height;
    std::string title;

    GLuint CompileShaderProgram(const char rootName []);
    
    static gmtl::Matrix44f Lookat(gmtl::Vec3f target, gmtl::Vec3f camera, gmtl::Vec3f up);
    static gmtl::Matrix44f Rotate(float angle, gmtl::Vec3f axis);

    // Manager initialization.
    static bool Initialize(float yFov, float nearPlane, float farPlane, bool fullscreen, int width, int height, std::string title);
    static GLManager* GetManager();
    static bool Deinitialize();
};

