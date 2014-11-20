#pragma once

// Performs basic message translation of byte arrays. The inverse of the C# message handler.
class MessageHandler
{
public:
    enum MessageType { PREFERENCES_UPDATE = 0, EXIT_MESSAGE, EXTRUDE_SETTINGS, REVOLVE_SETTINGS, PLANE_BIT_DATA};

    MessageHandler();
};

