#include "stdafx.h"
#include "Octocad.h"
#include "GLManager.h"
#include "InputSystem.h"
#include "Vertex.h"
#include "ProcessLink.h"
#include "MessageHandler.h"

// OpenGL libraries
#pragma comment(lib, "opengl32")
#pragma comment(lib, "lib/glfw3dll.lib")

#ifndef _DEBUG
#pragma comment(lib, "lib/glew32.lib")
#else
#pragma comment(lib, "lib/glew32d.lib")
#endif

const char* Octocad::NAME = "Octocad v1.0";

Octocad::Octocad(void) : processLink(), shapeManipulator()
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

    // -----------------------------------
    //   Non Open GL portions
    // -----------------------------------
    
    // Setup something in our shape manipulation system.
    shapeManipulator.Refresh();
    
    // -----------------------------------

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
    proj_matrix = gmtl::setPerspective(proj_matrix, GLManager::FOV_Y, aspect, GLManager::NEAR_PLANE, GLManager::FAR_PLANE);
    glViewport(0, 0, pM->width, pM->height);
}

// Draws the entire C++ display.
void Octocad::Render(double currentTime)
{
    // ---------- Draw the designed shape ----------
    colorVertex *pVertices = shapeManipulator.LoadTriangulatedShape(vertexCount);
    if (pVertices != nullptr)
    {
        // This was a reload operation, changes were performed that must be flushed to the GPU.
        glBufferData(GL_ARRAY_BUFFER, vertexCount*sizeof(colorVertex), pVertices, GL_STATIC_DRAW);
        delete [] pVertices;
    }

    lookAt = GLManager::Lookat(gmtl::Vec3f(0, 0, 0), gmtl::Vec3f(20, 20, 20), gmtl::Vec3f(0, 0, 1));
    glUseProgram(boringProgram);

    const GLfloat color[] = {0, 0, 0, 1};
    const GLfloat one = 1.0f;
    glClearBufferfv(GL_COLOR, 0, color);
    glClearBufferfv(GL_DEPTH, 0, &one);

    gmtl::Matrix44f result = proj_matrix*lookAt;
    glUniformMatrix4fv(proj_location, 1, GL_FALSE, result.getData());

    // Get the two vectors to rotate to-from by rotating the (1,1,1) src vector by the 
    gmtl::Matrix44f mv_matrix = GLManager::Rotate((float)currentTime*0.5f, gmtl::Vec3f(0.0f, 0.0f, 1.0f));
    glUniformMatrix4fv(mv_location, 1, GL_FALSE, mv_matrix.getData());
    glDrawArrays(GL_TRIANGLES, 0, vertexCount);

    // ---------------------------------------------
}

// Updates the entire C++ portion.
bool Octocad::RunApplication()
{
    double timeDelta = 1.0/(double)GLManager::FPS_TARGET;
    double lastTime = glfwGetTime();

    double lastDisplayTime = lastTime;
    long frameCounter = 0;

    while (GLManager::GetManager()->running)
    {
        // Draw and swap buffers
        Render(glfwGetTime());
        glfwSwapBuffers(pWindow);
       
        // Handle events.
        glfwPollEvents();
        
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

        char typedKey;
        if (InputSystem::KeyTypedEvent(typedKey))
        {
            switch(typedKey)
            {
            case GLFW_KEY_Q:
                std::cout << "Hit the ESCAPE key to exit, or better yet, close Octocad-2D" << std::endl;
            default:
                std::cout << "What?" << std::endl;
                break;
            }
        }

        // Check if we received any messages from the C# side
        MessageData newMessage;
        if (processLink.ReceiveMessages(&newMessage))
        {
            std::cout << "Recieved message of type " << (int)newMessage.messageType << std::endl;
            switch (newMessage.messageType)
            {
            case MessageHandler::EXIT_MESSAGE:
                GLManager::GetManager()->running = false;
                break;
            case MessageHandler::PREFERENCES_UPDATE:
                CsPreferences preferences;
                MessageHandler::DecodePreferences(preferences, &newMessage.messageData);
                std::cout << "Preferences: Length: " << preferences.length << ", Resolution: " << preferences.resolution << std::endl;
                
                // Recreate the octree area based on the new preferences data.
                shapeManipulator.Refresh(preferences.length, preferences.resolution);
                break;
            case MessageHandler::EXTRUDE_SETTINGS:
                CsExtrudeSettings extrudeSettings;
                MessageHandler::DecodeExtrudeSettings(extrudeSettings, &newMessage.messageData);
                std::cout << "Extrude data: " << extrudeSettings.theta << " " << extrudeSettings.phi << " " << extrudeSettings.radius << " " << extrudeSettings.distance << " " << extrudeSettings.isMirrored << " " << extrudeSettings.extrusionMode << std::endl;

                shapeManipulator.AddExecute(extrudeSettings);
                break;
            case MessageHandler::PLANE_BIT_DATA:
                CsBitPlane bitPlane;
                MessageHandler::DecodePlaneBitData(bitPlane, &newMessage.messageData);
                std::cout << "Bit plane data: " << bitPlane.width << " " << bitPlane.height << " " << bitPlane.stride << std::endl;

                shapeManipulator.AddExecute(bitPlane);
                break;
            default:
                std::cout << "ERROR: Unable to translate message!" << std::endl;
                break;
            }

            // At this point, recipients will have copied out the data that they are interested in.
            // Clean up the message data after transmission. 
            if (newMessage.messageLength != 0)
            {
                delete [] newMessage.messageData;
            }
        }

        // Update the FPS display timer
        if (glfwGetTime() - lastDisplayTime > 1)
        {
            std::cout << "FPS: " << frameCounter/(glfwGetTime() - lastDisplayTime) << ", " << vertexCount / 36 << " cubes." << std::endl;
            lastDisplayTime = glfwGetTime();
            frameCounter = 0;
        }

        // Update timer and try to sleep for the FPS Target.
        timeDelta = (double)glfwGetTime() - lastTime;
        lastTime  = (double)glfwGetTime();

        std::chrono::milliseconds sleepTime ((int)(1000/(double)GLManager::FPS_TARGET - 1000*timeDelta));
        if (sleepTime > std::chrono::milliseconds(0))
        {
            std::this_thread::sleep_for(sleepTime);
        }
        ++frameCounter;
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
            std::cout << std::endl << "Error initializing OctocadCpp!" << std::endl;
            break;
        }
        runStatus = Octocad->RunApplication();
    } while (false);

    // Wait before closing for display purposes.
    std::cout << std::endl << "End of application. " << std::endl;
    std::chrono::milliseconds sleepTime(2000);
    std::this_thread::sleep_for(sleepTime);

    return runStatus;
}