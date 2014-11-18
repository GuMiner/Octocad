#include "stdafx.h"
#include "GLManager.h"
#include "StringUtility.h"

GLManager *GLManager::m_pManager;
float GLManager::FOV_Y;
float GLManager::NEAR_PLANE;
float GLManager::FAR_PLANE;

// Compiles a combination vertex and fragment shader into a single shader program.
GLuint GLManager::CompileShaderProgram(const char rootName [])
{
    GLuint program;
    GLuint vertexShader;
    GLuint fragmentShader;

    std::string vsShader, fsShader;
    std::stringstream vsFilenameStream, fsFilenameStream;
    vsFilenameStream << "shaders/" << rootName << ".vs";
    fsFilenameStream << "shaders/" << rootName << ".fs";

    if (!StringUtility::LoadStringFromFile(vsFilenameStream.str().c_str(), vsShader))
    {
        std::cout << "Could not load " << vsShader << "!" << std::endl;
    }

    if (!StringUtility::LoadStringFromFile(fsFilenameStream.str().c_str(), fsShader))
    {
        std::cout << "Could not load fragment shader" << fsShader << "!" << std::endl;
    }

    const char* vss = vsShader.c_str();
    const char* fss = fsShader.c_str();

    char buffer[1024];
    GLint len;

    GLint compileStatus;
    vertexShader = glCreateShader(GL_VERTEX_SHADER);
    glShaderSource(vertexShader, 1, &vss, NULL);
    glCompileShader(vertexShader);
    glGetShaderiv(vertexShader, GL_COMPILE_STATUS, &compileStatus);
    if (!compileStatus)
    {
        glGetShaderInfoLog(vertexShader, 1024, &len, buffer);
        std::cout << std::endl << "Error: " << glewGetErrorString(glGetError()) << " " << buffer << std::endl;
    }

    fragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
    glShaderSource(fragmentShader, 1, &fss, NULL);
    glCompileShader(fragmentShader);
    glGetShaderiv(fragmentShader, GL_COMPILE_STATUS, &compileStatus);
    if (!compileStatus)
    {
        glGetShaderInfoLog(fragmentShader, 1024, &len, buffer);
        std::cout << "Error: " << glewGetErrorString(glGetError()) << " " << buffer << std::endl;
    }

    // Create program
    program = glCreateProgram();
    glAttachShader(program, vertexShader);
    glAttachShader(program, fragmentShader);
    glLinkProgram(program);
    glGetProgramiv(program, GL_LINK_STATUS, &compileStatus);
    if (!compileStatus)
    {
        glGetProgramInfoLog(program, 1024, &len, buffer);
        std::cout << "Error: " << glewGetErrorString(glGetError()) << " " << buffer << std::endl;
    }
    
    glDeleteShader(vertexShader);
    glDeleteShader(fragmentShader);

    return program;
}

bool GLManager::Initialize(float yFov, float nearPlane, float farPlane, bool fullscreen, int width, int height, std::string title)
{
    m_pManager = new GLManager();
    m_pManager->running = true;
    m_pManager->fullscreen = fullscreen;
    m_pManager->width = width;
    m_pManager->height = height;
    m_pManager->title = title;

    FOV_Y = yFov;
    NEAR_PLANE = nearPlane;
    FAR_PLANE = farPlane;

    return true;
}

GLManager* GLManager::GetManager()
{
    return m_pManager;
}

bool GLManager::Deinitialize()
{
    delete m_pManager;
    return true;
}