#include "stdafx.h"
#include "Vertex.h"

textureNormalVertex operator*(double value, const textureNormalVertex& other) {
  return textureNormalVertex(other.u*(float)value, other.v*(float)value, other.xn*(float)value, other.yn*(float)value, other.zn*(float)value);
}

textureNormalVertex& operator+=(textureNormalVertex& result, const textureNormalVertex& other) {
  result.u += other.u;
  result.v += other.v;
  result.xn += other.xn;
  result.yn += other.yn;
  result.zn += other.zn;
  return result;
}