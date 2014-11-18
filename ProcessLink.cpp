#include "stdafx.h"
#include "ProcessLink.h"

ProcessLink::ProcessLink(void)
{
    csHandle = nullptr;
    connectionThread = std::thread(&ProcessLink::ConnectToCsServer, this);
}

void ProcessLink::ConnectToCsServer()
{
    // Connects to the C# octocad instance.
    std::cout << "Connecting to Octocad C#..." << std::endl;
    csHandle = CreateFile("\\\\.\\pipe\\Octocad", GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
    if (csHandle == INVALID_HANDLE_VALUE)
    {
        std::cout << "Couldn't connect to C# Octocad through named pipes: " << GetLastError() << std::endl;
        return;
    }

    std::cout << "C# and C++ connection created." << std::endl;
    readThread = std::thread(&ProcessLink::ReadFromCsServer, this);
}

void ProcessLink::ReadFromCsServer()
{
    while (true)
    {
        char data[1024];
        DWORD numRead = 1;

        while (numRead >= 0)
        {
            ReadFile(csHandle, data, 1024, &numRead, NULL);

            if (numRead > 0)
            {
                std::cout << data[0] << std::endl;
                DWORD numWritten = 0;
                WriteFile(csHandle, &data, 1, &numWritten, NULL);
            }
        }
    }
}

// Join to the threads so that exiting won't crash.
ProcessLink::~ProcessLink(void)
{
    if (connectionThread.joinable())
    {
        connectionThread.join();
    }
    if (readThread.joinable())
    {
        readThread.join();
    }

    if (csHandle != nullptr)
    {
        CloseHandle(csHandle);
    }
}
