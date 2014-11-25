#include "stdafx.h"
#include "MessageHandler.h"

void MessageHandler::DecodePreferences(CsPreferences &preferences, char **ppData)
{
    std::copy(&(*ppData)[0], &(*ppData)[0] + sizeof(double), reinterpret_cast<char*>(&preferences.length));
    std::copy(&(*ppData)[sizeof(double)], &(*ppData)[sizeof(double)] + sizeof(double), reinterpret_cast<char*>(&preferences.resolution));
}

void MessageHandler::DecodeExtrudeSettings(CsExtrudeSettings &extrudeSettings, char **ppData)
{
    long offset = 0;
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(double), reinterpret_cast<char*>(&extrudeSettings.theta));
    offset += sizeof(double);

    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(double), reinterpret_cast<char*>(&extrudeSettings.phi));
    offset += sizeof(double);
    
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(double), reinterpret_cast<char*>(&extrudeSettings.radius));
    offset += sizeof(double);
    
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(double), reinterpret_cast<char*>(&extrudeSettings.distance));
    offset += sizeof(double);
    
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(bool), reinterpret_cast<char*>(&extrudeSettings.isMirrored));
    offset += sizeof(bool);

    int extrusionMode;
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(int), reinterpret_cast<char*>(&extrusionMode));
    
    extrudeSettings.extrusionMode = (CsExtrudeSettings::ExtrusionMode)extrusionMode;
}

void MessageHandler::DecodeRevolutionSettings(CsRevolveSettings &CsRevolveSettings, char **ppData)
{
    // TODO TODO
}

void MessageHandler::DecodePlaneBitData(CsBitPlane &bitPlane, char **ppData)
{
    long offset = 0;
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(int), reinterpret_cast<char*>(&bitPlane.width));
    offset += sizeof(int);

    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(int), reinterpret_cast<char*>(&bitPlane.height));
    offset += sizeof(int);
    
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(int), reinterpret_cast<char*>(&bitPlane.stride));
    offset += sizeof(int);

    // Copy in the image data.
    bitPlane.pImage = new char[bitPlane.height*bitPlane.stride];
    std::copy(&(*ppData)[offset], &(*ppData)[offset] + sizeof(int), &bitPlane.pImage[0]);
}