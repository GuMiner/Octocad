#include "stdafx.h"
#include "ProcessLink.h"

ProcessLink::ProcessLink(void) : csInputHandle (nullptr), csOutputHandle(nullptr)
{
    connectionThread = std::thread(&ProcessLink::ConnectToCsServer, this);
}

void ProcessLink::ConnectToCsServer()
{
    // Wait a bit for the console to appear for ease of debugging.
    std::this_thread::sleep_for(std::chrono::seconds(2));

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
bool ProcessLink::ReceiveMessages(MessageData *pNewMessage)
{
    // There's only one reader, so we can do the empty check before locking/unlocking.
    if (!receivedMessages.empty())
    {
        stackModificationMutex.lock();
        MessageData topMessage = receivedMessages.top();
        pNewMessage->messageData = topMessage.messageData;
        pNewMessage->messageLength = topMessage.messageLength;
        pNewMessage->messageType = topMessage.messageType;

        receivedMessages.pop();
        stackModificationMutex.unlock();
        return true;
    }
    return false;
}

// Continually reads a mesage in 1024 byte chunks from OctocadCs, and adds it to the message queue
//  when the message has finished being read.
//  WARNING: Messages are limited to the size of an unsigned Int64. I'm fairly certain other things will break before I hit that limit.
//  WARNING2: This only works if messages don't get squished together. That *should* not occur with the server write structure.
void ProcessLink::ReadFromCsServer()
{
    const int BUFFER_LENGTH = 2048;
    char data[BUFFER_LENGTH];
    DWORD numRead = 1;

    unsigned long long remainingMessageLength = 0;
    bool receivingMessage = false;
    while (numRead > 0)
    {
        ReadFile(csOutputHandle, data, BUFFER_LENGTH, &numRead, NULL);

        if (numRead > 0)
        {
            if (receivingMessage)
            {
                // Add this onto the amount we have currently read for this message.
                unsigned long long offset = newMessage.messageLength - remainingMessageLength;
                std::copy(&data[0], &data[0] + numRead, &newMessage.messageData[offset]);

                if (numRead > remainingMessageLength)
                {
                    std::cout << "ERROR: Squishing detected! Rewrite the message receiving backend!" << std::endl;
                    remainingMessageLength = 0;
                }
                else
                {
                    remainingMessageLength -= numRead;
                }
            }
            else
            {
                if (numRead < sizeof(unsigned long long))
                {
                    std::cout << "Read in insufficient data for the start of a message!" << std::endl;
                    return;
                }

                // Extract the message length and type from the stream
                newMessage.messageType = data[0];
                std::copy(&data[1], &data[1] + sizeof(unsigned long long), reinterpret_cast<char*>(&remainingMessageLength));
                newMessage.messageLength = remainingMessageLength;
                
                // Create and copy in the rest of the message that was transmitted
                if (remainingMessageLength != 0)
                {
                    newMessage.messageData = new char[(std::size_t)remainingMessageLength]; // Avoid auto-conversion to an int.
                    std::copy(&data[9], &data[9] + numRead - 9, &newMessage.messageData[0]);
                    remainingMessageLength = newMessage.messageLength + 9 - numRead;
                }
                else
                {
                    newMessage.messageData = nullptr;
                }

                receivingMessage = true;
            }

            // Finished receiving all the bytes we want.
            if (receivingMessage && remainingMessageLength == 0)
            {
                stackModificationMutex.lock();
                receivedMessages.push(newMessage);
                stackModificationMutex.unlock();
                receivingMessage = false;
            }
        }
    }
}

// Join to the threads so that exiting won't crash.
ProcessLink::~ProcessLink(void)
{
    // This must be closed first so that the C# application's read thread terminates...
    if (csInputHandle != nullptr)
    {
        CloseHandle(csInputHandle);
    }

    if (connectionThread.joinable())
    {
        connectionThread.join();
    }
    if (readThread.joinable())
    {
        readThread.join();
    }

    // ... which allows our read thread to terminate in the join phase.
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
