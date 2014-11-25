#pragma once

// Editting area preferences.
struct CsPreferences
{
    // The side distance of the drawing space, and the smallest resolvable resolution.
    double length, resolution;
};

// Extrusion settings. 
struct CsExtrudeSettings
{
    // Spherical coordinates denoting where the drawing board was at. Oriented using y+ as up, or x+ if phi is pi or -pi.
    double theta, phi, radius;
    double distance; // Range to extrude to, from the radius. + = away from center, - = towards center
    bool isMirrored;  // Whether or not to extrude on both sides of the radius, ignoring the distance sign.

    enum ExtrusionMode { ADD, SUBTRACT, INTERSECT };
    ExtrusionMode extrusionMode;
};

// Revolution settings.
// If not specified, everything is the same as in the extrusion settings.
struct CsRevolveSettings
{
    double theta, phi, radius;
    double angle;
    bool isMirrored, isCWRotation;

    enum RevolutionMode { ADD, SUBTRACT, INTERSECT };
    RevolutionMode RevolutionMode;
};

// The extrusion / rotation image.
struct CsBitPlane
{
    int width, height, stride;
    char *pImage;
};

// Performs basic message translation of byte arrays. The inverse of the C# message handler.
class MessageHandler
{
public:
    enum MessageType { PREFERENCES_UPDATE = 0, EXIT_MESSAGE, EXTRUDE_SETTINGS, REVOLVE_SETTINGS, PLANE_BIT_DATA};

    static void DecodePreferences(CsPreferences &preferences, char **ppData);
    static void DecodeExtrudeSettings(CsExtrudeSettings &extrudeSettings, char **ppData);
    static void DecodeRevolutionSettings(CsRevolveSettings &CsRevolveSettings, char **ppData);
    static void DecodePlaneBitData(CsBitPlane &bitPlane, char **ppData);
};

