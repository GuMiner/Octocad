#include "stdafx.h"

// Matrix construction methods.
namespace gm
{
    // Generates a perspective frustrum projection matrix
    mat4 Perspective(float angleYDeg, float aspect, float nearP, float farP)
    {
        mat4 result;
        static float PI = 3.141592653589f;

        result[0] = vec4(1.0f/(aspect*tan(0.5f*angleYDeg*PI/180.0f)), 0.0f, 0.0f, 0.0f);
        result[1] = vec4(0.0f, 1.0f/tan(0.5f*angleYDeg*PI/180.0f), 0.0f, 0.0f);
        result[2] = vec4(0.0f, 0.0f, (nearP + farP)/(nearP - farP), -1.0f);
        result[3] = vec4(0.0f, 0.0f, (2.0f * nearP * farP) / (nearP - farP), 0.0f);

        return result;
    }

    // Generates a look-at matrix
    mat4 Lookat(vec3 target, vec3 camera, vec3 up)
    {
         vec3 forward = (target - camera).Normalize();
        vec3 upNorm = up.Normalize();

        vec3 side = forward.Cross(upNorm);
        vec3 upNew = side.Cross(forward);

        mat4 result;

        result[0] = vec4(side[0], side[1], side[2], 0.0f);
        result[1] = vec4(upNew[0], upNew[1], upNew[2], 0.0f);
        result[2] = vec4(forward[0], forward[1], forward[2], 0.0f);
        result[3] = vec4(-camera[0], -camera[1], -camera[2], 1.0f);

        return result;
    }

    // Generates a translation matrix
    mat4 Translate(vec3 translation)
    {
        mat4 result;

        result[0] = vec4(1.0f, 0.0f, 0.0f, 0.0f);
        result[1] = vec4(0.0f, 1.0f, 0.0f, 0.0f);
        result[2] = vec4(0.0f, 0.0f, 1.0f, 0.0f);
        result[3] = vec4(translation[0], translation[1], translation[2], 1.0f);

        return result;
    }

    // Generates a scaling matrix from a vector
    mat4 Scale(vec3 scale)
    {
        mat4 result;

        result[0] = vec4(scale[0], 0.0f, 0.0f, 0.0f);
        result[1] = vec4(0.0f, scale[1], 0.0f, 0.0f);
        result[2] = vec4(0.0f, 0.0f, scale[2], 0.0f);
        result[3] = vec4(0.0f, 0.0f, 0.0f, 1.0f);

        return result;
    }

    // Performs axis-angle rotation.
    mat4 Rotate(float angle, vec3 axis)
    {
        mat4 result;

        float c = cosf(angle);
        float s = sinf(angle);
        float omc = 1.0f - c;

        result[0] = vec4(( axis[0] * axis[0] * omc + c), (axis[1] * axis[0] * omc + axis[2] * s), (axis[0] * axis[2] * omc - axis[1] * s), 0.0f);
        result[1] = vec4((axis[0] * axis[1] * omc - axis[2] * s), (axis[1] * axis[1] * omc + c), (axis[1] * axis[2] * omc + axis[0] * s), 0.0f);
        result[2] = vec4((axis[0] * axis[2] * omc + axis[1] * s), (axis[1] * axis[2] * omc - axis[0] * s), (axis[2]* axis[2] * omc + c), 0.0f);
        result[3] = vec4(0.0f, 0.0f, 0.0f, 1.0f);

        return result;
    }
}