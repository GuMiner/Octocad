#pragma once
#include "stdafx.h"

struct MessageData
{
    char messageType;

    char* messageData;
    unsigned long long messageLength;
};

// Handles the details of communicating with the primary C# interface.
class ProcessLink
{
    HANDLE csInputHandle, csOutputHandle;
    std::thread connectionThread, readThread;
    bool connecting;

    MessageData newMessage;

    // Normally one would use a message queue. I'm using a message stack. Can anyone guess why?
    std::mutex stackModificationMutex;
    std::stack<MessageData> receivedMessages;

    void ConnectToCsServer();
    void ReadFromCsServer();
public:
    ProcessLink();
    //void WriteToOctocadCs(char [] data);               //  DWORD numWritten = 0
           //     WriteFile(csHandle, &data, 1, &numWritten, NULL);
    bool ReceiveMessages(MessageData *pNewMessage);
    ~ProcessLink();
};

