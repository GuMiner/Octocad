#include "stdafx.h"
#include "Octocad.h"
#include "GLManager.h"
#include "InputSystem.h"
#include "Vertex.h"

// OpenGL libraries
#pragma comment(lib, "opengl32")
#pragma comment(lib, "lib/glfw3dll.lib")

#ifndef _DEBUG
#pragma comment(lib, "lib/glew32.lib")
#else
#pragma comment(lib, "lib/glew32d.lib")
#endif

const char* Octocad::NAME = "Octocad v1.0";

Octocad::Octocad(void)
{
}

// Performs OpenGL window initialization.
bool Octocad::WindowInitialization()
{
    if (!GLManager::Initialize(50.0f, 0.1f, 1000.0f, false, 1366, 768, Octocad::NAME))
    {
        std::cout << "Failed to initialize the OpenGL Manager!" << std::endl;
        return false;
    }
    GLManager *pM = GLManager::GetManager();
    
    // Initialize GLFW
    if (!glfwInit())
    {
        std::cout << "GLFW initialization failure!" << std::endl;
        return false;
    }

    // Setup GLFW

    // Window hints
    glfwDefaultWindowHints();
    glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, GLManager::OPENGL_MAJOR);
    glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, GLManager::OPENGL_MINOR);
    glfwWindowHint(GLFW_SAMPLES, GLManager::ALIASING_LEVEL);

    glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
    
    // Window creation.
    if (pM->fullscreen)
    {
        pWindow = glfwCreateWindow(pM->width, pM->height, pM->title.c_str(), glfwGetPrimaryMonitor(), NULL);
    }
    else
    {
        pWindow = glfwCreateWindow(pM->width, pM->height, pM->title.c_str(), NULL, NULL);
    }

    if (pWindow == NULL)
    {
        std::cout << "GLFW window creation failure: " << glewGetErrorString(glGetError()) << std::endl;
        return false;
    }
    glfwMakeContextCurrent(pWindow);
    
    // Callback initialization.
    InputSystem::Initialize();
    glfwSetCharCallback(pWindow, InputSystem::KeyTyped);
    glfwSetKeyCallback(pWindow, InputSystem::KeyEvent);
    glfwSetMouseButtonCallback(pWindow, InputSystem::MouseButtonEvent);
    glfwSetScrollCallback(pWindow, InputSystem::ScrollEvent);
    glfwSetCursorEnterCallback(pWindow, InputSystem::CursorTravel);
    glfwSetCursorPosCallback(pWindow, InputSystem::CursorMove);
    glfwSetWindowSizeCallback(pWindow, InputSystem::Resize);
    glfwSetErrorCallback(InputSystem::ErrorCallback);

    // GLEW initialization.
    glewExperimental = GL_TRUE;
    GLenum err = glewInit();
    if (err != GLEW_OK)
    {
        std::cout << "GLEW initialization failure: " << glewGetErrorString(err) << std::endl;
        return false;
    }
    
    // Display debug info and enable debug mode if specified.
    std::cout << "Using OpenGL vendor " << glGetString(GL_VENDOR) << ", version " << glGetString(GL_VERSION) << std::endl;
    std::cout << "  >> System uses OpenGL renderer " << glGetString(GL_RENDERER) << " <<" << std::endl;
    return true;
}

// Performs visual OpenGL setup,
bool Octocad::ApplicationSetup()
{
    if (!WindowInitialization())
    {
        std::cout << "Failed to initialize the OpenGL window!" << std::endl;
        return false;
    }

    // Only works if faces are positioned appropriately, 
    glDisable(GL_CULL_FACE);
    glFrontFace(GL_CW);

    glEnable(GL_DEPTH_TEST);
    glDepthFunc(GL_LEQUAL);

    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_R, GL_CLAMP_TO_EDGE);
    glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
    
    SetupViewport();

    // Now actually perform application-specific setup
    glGenVertexArrays(1, &vao);
    glBindVertexArray(vao);

    // Setup of vertex transfer (note we're using the "vertex" object in CodeGell)
    glGenBuffers(1, &pointBuffer);
    glBindBuffer(GL_ARRAY_BUFFER, pointBuffer);

    glVertexAttribPointer(0, 3, GL_FLOAT, GL_FALSE, sizeof(colorVertex), (GLvoid*)offsetof(colorVertex, x));
    glEnableVertexAttribArray(0);

    glVertexAttribPointer(1, 3, GL_FLOAT, GL_FALSE, sizeof(colorVertex), (GLvoid*)offsetof(colorVertex, r));
    glEnableVertexAttribArray(1);

    boringProgram = GLManager::GetManager()->CompileShaderProgram("render");
    mv_location = glGetUniformLocation(boringProgram, "mv_matrix");
    proj_location = glGetUniformLocation(boringProgram, "proj_matrix");

    return true;
}

Octocad::~Octocad()
{
    // Application shutdown.
    glDeleteVertexArrays(1, &vao);
    glDeleteBuffers(1, &pointBuffer);

    glDeleteProgram(boringProgram);

    // Close down GLFW
    glfwDestroyWindow(pWindow);
    glfwTerminate();

}

// Sets up the drawing viewport so we don't get a weird squished display.
void Octocad::SetupViewport()
{
    GLManager *pM = GLManager::GetManager();
    // Viewing aspect ratio and projection matrix.
    aspect = (float)pM->width/ (float)pM->height;
    proj_matrix = gm::Perspective(GLManager::FOV_Y, aspect, GLManager::NEAR_PLANE, GLManager::FAR_PLANE);
    glViewport(0, 0, pM->width, pM->height);
}

void Octocad::Render(double currentTime)
{
    // Load up a cube for simple testing
    vertexCount = 36;
    colorVertex *pVertices = new colorVertex[vertexCount];
    
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
    
    // Load up the cube
    for (GLsizei i = 0; i < vertexCount; i++)
    {
        pVertices[i].Set(vertex_positions[i*3], vertex_positions[i*3 + 1], vertex_positions[i*3 + 2], (double)i/12, (double)i/12, (double)i/12);
    }

    glBufferData(GL_ARRAY_BUFFER, vertexCount*sizeof(colorVertex), pVertices, GL_DYNAMIC_DRAW);

    delete [] pVertices;

    lookAt = gm::Lookat(gm::vec3(0, 0, 0), gm::vec3(0, 0, 6), gm::vec3(0, 1, 0));
    glUseProgram(boringProgram);

    const GLfloat color[] = {0, 0, 0, 1};
    const GLfloat one = 1.0f;
    glClearBufferfv(GL_COLOR, 0, color);
    glClearBufferfv(GL_DEPTH, 0, &one);

    gm::mat4 result = proj_matrix*lookAt;
    glUniformMatrix4fv(proj_location, 1, GL_FALSE, result);

    for (int i = 0; i < 1; i++)
    {
        for (int j = 0; j < 1; j++)
        {
            for (int k = 0; k < 1; k++)
            {
                gm::mat4 mv_matrix = gm::Rotate((float)currentTime/5.0f, gm::vec3(0.0f, 1.0f, 0.0f));
                glUniformMatrix4fv(mv_location, 1, GL_FALSE, mv_matrix);
                glDrawArraysInstanced(GL_TRIANGLES, 0, vertexCount, 100);
            }
        }
    }
}

bool Octocad::RunApplication()
{
    double timeDelta = 1.0f/(double)GLManager::FPS_TARGET;
    double lastTime = (double)glfwGetTime();
    while (GLManager::GetManager()->running)
    {
        // Draw and swap buffers
        Render(glfwGetTime());
        glfwSwapBuffers(pWindow);
       
        // Handle events.
        glfwPollEvents();
        
        //------------------
        // Escape event
        if (glfwGetKey(pWindow, GLFW_KEY_ESCAPE) == GLFW_PRESS || glfwWindowShouldClose(pWindow))
        {
            GLManager::GetManager()->running = false;
        }

        // Resize event
        if (InputSystem::ResizeEvent(GLManager::GetManager()->width, GLManager::GetManager()->height))
        {
            SetupViewport();
        }
        //------------------

        // Update timer and try to sleep for the FPS Target.
        timeDelta = (double)glfwGetTime() - lastTime;
        lastTime  = (double)glfwGetTime();

        std::chrono::milliseconds sleepTime ((int)(1.0/(double)GLManager::FPS_TARGET - 1000*timeDelta));
        if (sleepTime > std::chrono::milliseconds(0))
        {
            std::this_thread::sleep_for(sleepTime);
        }
    }

    return true;
}

// Runs the main application.
int main(int argc, char* argv [])
{
    int runStatus;
    do
    {
        std::unique_ptr<Octocad> Octocad(new Octocad());
        runStatus = Octocad->ApplicationSetup();
        if (!runStatus)
        {
            std::cout << std::endl << "Error initializing Rcsg-edit!" << std::endl;
            break;
        }
        runStatus = Octocad->RunApplication();
    } while (false);

    // Wait before closing for display purposes.
    std::cout << std::endl << "End of application. " << std::endl;
    std::chrono::milliseconds sleepTime(3000);
    std::this_thread::sleep_for(sleepTime);

    return runStatus;
}