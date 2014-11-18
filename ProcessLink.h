#pragma once
#include "stdafx.h"

// Handles the details of communicating with the primary C# interface.
class ProcessLink
{
    HANDLE csHandle;
    std::thread connectionThread, readThread;
    bool connecting;

    void ConnectToCsServer();
    void ReadFromCsServer();
public:
    ProcessLink();
    ~ProcessLink();
};

