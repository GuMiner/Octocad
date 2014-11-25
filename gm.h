/*--------------------------------------------------------------------------
    gm.h ('General Math library')
    Copyright (C) 2014 Gustave Granroth. (gus.gran@gmail.com)

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.
--------------------------------------------------------------------------*/
#pragma once

// General Math namespace
//  Contains integer and floating-point mathematical functions.
//  Naming follows GLSL specifications for vector and matrix names.
namespace gm
{
    // Prototypes to allow forward-declarations.
    template <typename T, size_t length> class vecX;
    template <typename T> class vecT2;
    template <typename T> class vecT3;
    template <typename T> class vecT4;
    template <typename T> class quaternionT;
    template <typename T, int width, int height> class matXY;
    template <typename T> class matT2;
    template <typename T> class matT4;

    // GLSL-named vector and matrix specializations.
    typedef vecX<float, 1> vec1;
    typedef vecX<int, 1> ivec1;

    typedef vecT2<float> vec2;
    typedef vecT2<int> ivec2;

    typedef vecT3<float> vec3;
    typedef vecT3<int> ivec3;

    typedef vecT4<float> vec4;
    typedef vecT4<int> ivec4;

    typedef quaternionT<float> quaternion;
    typedef quaternionT<int> iquaternion;
    
    typedef matT2<float> mat2;
    typedef matT2<int> imat2;
    
    typedef matT4<float> mat4;
    typedef matT4<int> imat4;

    // Matrix modification methods

    // Generates a perspective frustrum projection matrix
    mat4 Perspective(float angleYDeg, float aspect, float nearP, float farP);

    // Generates a look-at matrix
    mat4 Lookat(vec3 target, vec3 camera, vec3 up);

    // Generates a translation matrix
    mat4 Translate(vec3 translation);

    // Generates a scaling matrix from a vector
    mat4 Scale(vec3 scale);

    // Performs axis-angle rotation.
    mat4 Rotate(float angle, vec3 axis);

    // integer and float base template.
    template <typename T, size_t length> 
    class vecX
    {
    protected:
        T data[length];
    
    public:
        // Allow for direct access of the internal data.
        T& operator [] (int i) 
        {
            return data[i];
        }

        // Default, copy, and assignmentructors
        vecX()
        {}

        vecX(vecX& other)
        {
            for (size_t i = 0; i < length; i++)
            {
                data[i] = other[i];
            }
        }

        vecX& operator=(vecX& other)
        {
            for (size_t i = 0; i < length; i++)
            {
                data[i] = other[i];
            }
            return *this;
        }

        vecX(T value)
        {
            for (size_t i = 0; i < length; i++)
            {
                data[i] = value;
            }
        }

        // Dereferencing operator
        operator T* () 
        {
            return &data[0];
        }

        // Number of elements in the vector.
        static int Size()
        {
            return length;
        }

        // Non-size-specific vector operations.

        // Single-element multiplication / division / addition / subtraction
        vecX operator * (T value)
        {
            vecX<T, length> result;
            for (size_t i = 0; i < length; i++)
            {
                result[i] = data[i] * value;
            }

            return result;
        }
        
        vecX operator / (T value)
        {
            vecX<T, length> result;
            for (size_t i = 0; i < length; i++)
            {
                result[i] = data[i] / value;
            }

            return result;
        }

        vecX operator + (T other)
        {
            vecX<T, length> result;
            for (int i = 0; i < length; i++)
            {
                result[i] = data[i] + other;
            }

            return result;
        }

        vecX operator - (T value)
        {
            vecX<T, length> result;
            for (int i = 0; i < length; i++)
            {
                result[i] = data[i] - value;
            }

            return result;
        }

        // Memberwise multiplication / division / addition / subtraction
        vecX operator * (vecX other)
        {
            vecX<T, length> result;
            for (size_t i = 0; i < length; i++)
            {
                result[i] = data[i] * other[i];
            }

            return result;
        }
        
        vecX operator / (vecX other)
        {
            vecX<T, length> result;
            for (size_t i = 0; i < length; i++)
            {
                result[i] = data[i] / other[i];
            }

            return result;
        }

        vecX operator + (vecX other)
        {
            vecX<T, length> result;
            for (size_t i = 0; i < length; i++)
            {
                result[i] = data[i] + other[i];
            }

            return result;
        }

        vecX operator - (vecX other)
        {
            vecX<T, length> result;
            for (size_t i = 0; i < length; i++)
            {
                result[i] = data[i] - other[i];
            }

            return result;
        }
        
        // Vector negation
        vecX operator-()
        {
            vecX<T, length> result;
            for (int i = 0; i < length; i++)
            {
                result[i] = -data[i];
            }
            return result;
        }

        // Conjugate operations that are just plain useful.
        vecX& operator+=(vecX other)
        {
            for (size_t i = 0; i < length; i++)
            {
                data[i] += other[i];
            }

            return *this;
        }

        vecX& operator-=(vecX other)
        {
            for (size_t i = 0; i < length; i++)
            {
                data[i] -= other[i];
            }

            return *this;
        }

        // Dot product
        T Dot (vecX<T, length>& other)
        {
            T result;
            for (size_t i = 0; i < length; i++)
            {
                result += (data[i]*other[i]);
            }

            return result;
        }

        // Actual length of the vector.
        T Length()
        {
            T total = 0;
            for (size_t i = 0; i < length; i++)
            {
                total += data[i]*data[i];
            }

            return static_cast<T>(sqrt(total));
        }

        // Distance between two vectors
        T Distance(vecX<T, length>& other)
        {
            return (*this - other).len();
        }

        // Normalization
        vecX Normalize()
        {
            vecX<T, length> result;
            T totalLength = Length();

            for (int i = 0; i < length; i++)
            {
                result[i] = data[i]/totalLength;
            }

            return result;
        }

        // Angle between two vectors
        T Angle(vecX<T, length>& other)
        {
            return static_cast<T>(acos(dot(other)));
        }
    };

    // Two-element vector
    template <typename T>
    class vecT2 : public vecX<T, 2>
    {
    public:
        vecT2 ()
        { }
        vecT2 (vecX<T, 2>& other) : vecX<T, 2>(other)
        { }
        vecT2(T x, T y)
        {
            vecX<T, 2>::data[0] = x;
            vecX<T, 2>::data[1] = y;
        }
    };

    // Three-element vector
    template <typename T>
    class vecT3 : public vecX<T, 3>
    {
    public:
        vecT3 ()
        { }
        vecT3 (vecX<T, 3>& other) : vecX<T, 3>(other)
        { }
        vecT3(T x, T y, T z)
        {
            vecX<T, 3>::data[0] = x;
            vecX<T, 3>::data[1] = y;
            vecX<T, 3>::data[2] = z;
        }

        // Cross product (defined only for 3-element vectors)
        vecT3<T> Cross(vecX<T, 3>& other)
        {
            vecT3<T> result;
            result[0] = data[1]*other[2] - data[2]*other[1];
            result[1] = data[2]*other[0] - data[0]*other[2];
            result[2] = data[0]*other[1] - data[1]*other[0];

            return result;
        }
    };

    // Four-element vector
    template <typename T>
    class vecT4 : public vecX<T, 4>
    {
    public:
        vecT4 ()
        { }
        vecT4 (vecX<T, 4>& other) : vecX<T, 4>(other)
        { }
        vecT4(T x, T y, T z, T w)
        {
            vecX<T, 4>::data[0] = x;
            vecX<T, 4>::data[1] = y;
            vecX<T, 4>::data[2] = z;
            vecX<T, 4>::data[3] = w;
        }
    };

    // Four-element quaternion
    template <typename T>
    class quaternionT
    {
    protected:
        vecT4<T> data;
    public:
        

        // Default, copy, and vector-basedructors.
        quaternionT()
        { }
        quaternionT(T x, T y, T z)
        {
            data[0] = x;
            data[1] = y;
            data[2] = z;
            data[3] = 0;
        }
        quaternionT(T x, T y, T z, T w)
        {
            data[0] = x;
            data[1] = y;
            data[2] = z;
            data[3] = w;
        }
        quaternionT(quaternionT& other)
        {
            data = other.data;
        }
        quaternionT(vecT4<T>& other)
        {
            data = other;
        }

        // Data access operators
        T& operator [] (int n)
        {
            return data[i];
        }

        // Basic mathematical operators
        
        // Scalar multiplication
        quaternionT operator * (T value)
        {
            return quaternionT(data*value);
        }

        // Quaternion multiplication
        quaternionT operator * (quaternionT& other)
        {
            return quaternionT(data[3] * other[0] + data[0] * other[3] + data[1] * other[2] - data[2] * other[1],
                               data[3] * other[1] + data[1] * other[3] + data[2] * other[0] - data[0] * other[2],
                               data[3] * other[2] + data[2] * other[3] + data[0] * other[1] - data[1] * other[0],
                               data[3] * other[3] - data[0] * other[0] - data[1] * other[1] - data[2] * other[2]);
        }

        // Memberwise scalar division.
        quaternionT operator / (T value)
        {
            return quaternionT(data/value);
        }

        // Addition
        quaternionT operator + (quaternionT& other)
        {
            return quaternionT(data + other.data);
        }

        // Subtraction
        quaternionT operator - (quaternionT& other)
        {
            return quaternionT(data - other.data);
        }

        // Negation
        quaternionT operator- ()
        {
            return quaternionT(-data);
        }

        // Length calculation
        T Length()
        {
            return data.Length();
        }

        // Normalization
        quaternionT& Normalize()
        {
            return quaternionT(data.Normalize());
        }

        // Quaternion access as a rotation matrix.
        matXY<T, 4, 4> ToMatrix()
        {
            matXY<T,4,4> result;
            
            result[0][0] = T(1) - T(2) * (y*y + z*z);
            result[0][1] = T(2) * (x*y - z*w);
            result[0][2] = T(2) * (x*z + y*w);
            result[0][3] = T(0);

            result[1][0] = T(2) * (xy + z*w);
            result[1][1] = T(1) - T(2) * (x*x + z*z);
            result[1][2] = T(2) * (y*z - x*w);
            result[1][3] = T(0);

            result[2][0] = T(2) * (x*z - y*w);
            result[2][1] = T(2) * (y*z + x*w);
            result[2][2] = T(1) - T(2) * (x*x + y*y);
            result[2][3] = T(0);

            result[3][0] = T(0);
            result[3][1] = T(0);
            result[3][2] = T(0);
            result[3][3] = T(1);

            return result;
        }
    };

    // General multi-size matrix class
    template <typename T, int width, int height> 
    class matXY
    {
    protected:
        // An array of a vector form a matrix.
        vecX<T, height> data[width];

    public:

        vecX<T, height>& operator [] (int i)
        {
            return data[i];
        }

        //ructors
        matXY()
        { }

        matXY(T clear)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    data[i][j] = clear;
                }
            }
        }

        matXY(matXY& other)
        {
            for (int i = 0; i < width; i++)
            {
                data[i] = other.data[i];
            }
        }

        matXY& operator = (matXY<T, width, height>& other)
        {
            for (int i = 0; i < width; i++)
            {
                data[i] = other.data[i];
            }

            return *this;
        }

        // Dereferencing operator.
        operator T* ()
        {
            return &data[0][0];
        }

        static int Width()
        {
            return width;
        }
        static int Height()
        {
            return height;
        }

        // Basic mathematical operators

        // Addition
        matXY& operator + (matXY<T, width, height>& other)
        {
            matXY<T, w, n> result;
            for (int i = 0; n < width; i++)
            {
                result[i] = data[i] + other[i];
            }

            return result;
        }

        // Subtraction
        matXY& operator - (matXY<T, width, height>& other)
        {
            matXY<T, w, n> result;

            for (int i = 0; n < width; i++)
            {
                result[i] = data[i] - other[i];
            }

            return result;
        }

        // Memberwise scalar multiplication
        matXY& operator * (T other)
        {
            matXY<T, w, n> result;
            for (int i = 0; n < width; i++)
            {
                result[i] = data[i] * other;
            }

            return result;
        }

        // Matrix multiplication (Handles square matrixes only)
        matXY<T, width, height> operator * (matXY<T, width, height>& other)
        {
            matXY<T, width, height> result (0);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < width; k++)
                    {
                        result[i][j] += (data[k][j]*other[i][k]);
                    }
                }
            }

            return result;
        }

        // Matrix transpose
        matXY<T, height, width> Transpose()
        {
            matXY<T, height, width> result;

            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    result[x][y] = data[y][x];
                }
            }

            return result;
        }
    
        // Identity matrix
        matXY<T, width, height> Identity()
        {
            matXY<T, width, height> result (0);

            for (int i = 0; i < width; i++)
            {
                result[i][i] = 1;
            }

            return result;
        }
    };

    // Specialized 2x3 matrix.
    template <typename T>
    class matT2 : public matXY<T, 2, 2>
    {
    public: 
        matT2()
        { }
        matT2(T clear) : matXY<T, 2, 2>(clear)
        { }
        matT2(matXY<T, 2, 2>& other) : matXY<T, 2, 2>(other)
        { }
    };

    // Specialized 4x4 matrix.
    template <typename T>
    class matT4 : public matXY<T, 4, 4>
    {
    public: 
        matT4()
        { }
        matT4(T clear) : matXY<T, 4, 4>(clear)
        { }
        matT4(matXY<T, 4, 4>& other) : matXY<T,4, 4>(other)
        { }
    };
}