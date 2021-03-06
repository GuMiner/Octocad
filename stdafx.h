#pragma once
// Precompiled header file to speed up compilations

// For std::copy and the like.
#define _SCL_SECURE_NO_WARNINGS

// Standard C includes
#include <cstdlib>
#include <cmath>

// Data structures
#include <vector>
#include <map>
#include <stack>

// File and console IO
#include <iostream>
#include <fstream>

// String management
#include <sstream>
#include <string>

// Time and thread management
#include <chrono>
#include <thread>
#include <mutex>

// Miscellaneous
#include <limits>
#include <memory>

// Named pipe access -- and the rest of windows
#define WIN32_LEAN_AND_MEAN 1
#include <Windows.h>

// GLEW library
#include <GL/glew.h>

// GLFW library
#define GLFW_DLL 1
#include <GLFW/glfw3.h>

// GMTL mathematics library
// -- because a premade solution makes a lot of sense, and it's compatible with the MIT license.
#include <gmtl/gmtl.h>