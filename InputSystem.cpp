#include "stdafx.h"
#include "InputSystem.h"

InputSystem::ResizeEventData InputSystem::resizeEvent;
InputSystem::KeyTypedData InputSystem::keyTypedEvent;
bool InputSystem::addCubeButton;
bool InputSystem::removeCubeButton;

void InputSystem::Initialize(void)
{
    // Setup all the event handlers to indicate that nothing occurred.
    resizeEvent.resizeEvent = false;
    keyTypedEvent.charEvent = false;
    addCubeButton = false;
    removeCubeButton = false;
}

void InputSystem::KeyTyped(GLFWwindow *pWindow, unsigned int character)
{
    keyTypedEvent.newChar = static_cast<char>(character);
    keyTypedEvent.charEvent = true;
}
bool InputSystem::KeyTypedEvent(char& character)
{
    if (keyTypedEvent.charEvent)
    {
        character = keyTypedEvent.newChar;
        keyTypedEvent.charEvent = false;
        return true;
    }
    return false;
}

void InputSystem::KeyEvent(GLFWwindow *pWindow, int key, int scancode, int action, int mods)
{
    switch (key)
    {
    case GLFW_KEY_A:
        if (action == GLFW_PRESS)
        {
            addCubeButton = true;
        }
        else if (action == GLFW_RELEASE)
        {
            addCubeButton = false;
        }
        break;
    case GLFW_KEY_R:
        if (action == GLFW_PRESS)
        {
            removeCubeButton = true;
        }
        else if (action == GLFW_RELEASE)
        {
            removeCubeButton = false;
        }
        break;
    }
}

void InputSystem::MouseButtonEvent(GLFWwindow *pWindow, int button, int action, int mods)
{
}

void InputSystem::ScrollEvent(GLFWwindow *pWindow, double xDelta, double yDelta)
{
}

void InputSystem::CursorTravel(GLFWwindow *pWindow, int action)
{
}

void InputSystem::CursorMove(GLFWwindow *pWindow, double xNew, double yNew)
{
}

// Simple resize handling.
void InputSystem::Resize(GLFWwindow *pWindow, int widthNew, int heightHew)
{
    resizeEvent.newWidth = widthNew;
    resizeEvent.newHeight = heightHew;
    resizeEvent.resizeEvent = true;
}
bool InputSystem::ResizeEvent(int& width, int& height)
{
    if (resizeEvent.resizeEvent)
    {
        width = resizeEvent.newWidth;
        height = resizeEvent.newHeight;
        resizeEvent.resizeEvent = false;
        return true;
    }

    return false;
}

// Very simple error callbacks
void InputSystem::ErrorCallback(int errCode, const char *pError)
{
    std::cout << "GLFW error " << errCode << ": " << pError << std::endl;
}
void APIENTRY InputSystem::GLCallback(GLenum source, GLenum type, GLuint id, GLenum severity, GLsizei length, const GLchar *pMessage, void *userParam)
{
    std::cout << "OpenGL debug error (" << source << ", " << type << ", " << id << ", " << severity << ", " << length << "): " << pMessage << std::endl; 
}