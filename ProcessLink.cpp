#include "stdafx.h"
#include "ProcessLink.h"

ProcessLink::ProcessLink(void) : csInputHandle (nullptr), csOutputHandle(nullptr)
{
    connectionThread = std::thread(&ProcessLink::ConnectToCsServer, this);
}

void ProcessLink::ConnectToCsServer()
{
    // Wait a bit for the console to appear for ease of debugging.
    std::this_thread::sleep_for(std::chrono::seconds(3));

    // Connects to the C# octocad instance.
    std::cout << "Connecting to Octocad C#..." << std::endl;
    csInputHandle = CreateFile("\\\\.\\pipe\\OctocadIn", GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
    if (csInputHandle == INVALID_HANDLE_VALUE)
    {
        std::cout << "Couldn't connect C++ --> C# named pipe: " << GetLastError() << std::endl;
        return;
    }
    std::cout << "C++ --> C# connection created." << std::endl;

    csOutputHandle = CreateFile("\\\\.\\pipe\\OctocadOut", GENERIC_READ, 0, NULL, OPEN_EXISTING, 0, NULL);
    if (csOutputHandle == INVALID_HANDLE_VALUE)
    {
        std::cout << "Couldn't connect C++ <-- C# named pipe: " << GetLastError() << std::endl;
        return;
    }
    std::cout << "C++ <-- C# connection created." << std::endl;

    readThread = std::thread(&ProcessLink::ReadFromCsServer, this);
}

// Attempts to receive a message from the message stack. Returns true (and fills in the message)
//  if there is a message, false otherwise.
bool ProcessLink::ReceiveMessages(std::vector<char>& newMessage)
{
}

// Continually reads a mesage in 1024 byte chunks from OctocadCs, and adds it to the message queue
//  when the message has finished being read.
//  WARNING: Messages are limited to the size of an Int62 -- which is really insane.
void ProcessLink::ReadFromCsServer()
{
    while (true)
    {
        const int BUFFER_LENGTH = 2048;
        char data[BUFFER_LENGTH];
        DWORD numRead = 1;

        unsigned long long remainingMessageLength = 0;
        bool receivingMessage = false;
        while (numRead >= 0)
        {
            std::cout << "Reading bytes from the stream...";
            ReadFile(csOutputHandle, data, BUFFER_LENGTH, &numRead, NULL);

            if (numRead > 0)
            {
                std::cout << "Got data" << std::endl;
                continue;
                if (receivingMessage)
                {
                    // Add this onto the amount we have currently read for this message.
                }
                else
                {
                    //pNewMessage = new char[messageLength];
                    if (numRead < sizeof(unsigned long long))
                    {
                        std::cout << "Read in insufficient data for the start of a message!" << std::endl;
                    }

                    newMessage.messageType = data[0];
                    // Extract the message length from the stream
                    std::copy(&data[1], &data[1] + sizeof(unsigned long long), reinterpret_cast<char*>(&remainingMessageLength));
                    newMessage.messageLength = remainingMessageLength;

                    std::cout << remainingMessageLength << std::endl;
                }
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

    if (csInputHandle != nullptr)
    {
        CloseHandle(csInputHandle);
    }
    if (csOutputHandle != nullptr)
    {
        CloseHandle(csOutputHandle);
    }

    // Delete all the message that haven't been read
    while (!receivedMessages.empty())
    {
        MessageData message = receivedMessages.top();
        receivedMessages.pop();
        delete [] message.messageData;
    }
}
