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
    std::stack<MessageData> receivedMessages;

    void ConnectToCsServer();
    void ReadFromCsServer();
public:
    ProcessLink();
    //void WriteToOctocadCs(char [] data);               //  DWORD numWritten = 0
           //     WriteFile(csHandle, &data, 1, &numWritten, NULL);
    bool ReceiveMessages(std::vector<char>& newMessage);
    ~ProcessLink();
};

