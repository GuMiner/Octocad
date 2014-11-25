using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Octocad_2D
{
    class ProcessLink
    {
        private const String OCTOCAD_IN = "OctocadIn", OCTOCAD_OUT = "OctocadOut";
        private Thread pipeThread;

        private int BUFFER_SIZE = 4096;
        private NamedPipeServerStream inputCommunicationStream, outputCommunicationStream;

        private Thread readThread;
        private bool isInSetup;

        private Process octocadCpp;

        public ProcessLink()
        {
            inputCommunicationStream = null;
            outputCommunicationStream = null;
            isInSetup = true;

            // Start up the server
            pipeThread = new Thread(CreatePipeServer);
            pipeThread.Start();

            // Start up the C++ program. This comes with a slight delay to avoid the named pipe race communication connection issue.
            octocadCpp = new Process();
            octocadCpp.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Path.GetFullPath("..\\..\\..\\..\\Debug\\Octocad.exe"),
                WorkingDirectory = Path.GetFullPath("..\\..\\..\\")
            };
            Console.WriteLine(octocadCpp.StartInfo.WorkingDirectory);
            octocadCpp.Start();
        }

        /// <summary>
        /// Halts the interprocess link (cleanly). The steps involved are:
        /// 1. Send the C++ message. The C++ executable joins to the read thread (and the finished startup thread) and waits to exit.
        /// 2. Join our reading thread, which will quit when the client closes the connection.
        /// </summary>
        public void HaltCommunication()
        {
            WriteToOctocadCpp(MessageHandler.TranslateMessage(MessageHandler.MessageType.EXIT_MESSAGE));
            readThread.Join();
        }

        ~ProcessLink()
        {
            if (inputCommunicationStream != null)
            {
                inputCommunicationStream.Dispose();
            }

            if (outputCommunicationStream != null)
            {
                outputCommunicationStream.Dispose();
            }
        }

        private void CreatePipeServer()
        {
            inputCommunicationStream = new NamedPipeServerStream("OctocadIn", PipeDirection.In, 1, PipeTransmissionMode.Byte, PipeOptions.WriteThrough);
            outputCommunicationStream = new NamedPipeServerStream("OctocadOut", PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.WriteThrough);

            Console.WriteLine("Waiting for Octocad cpp...");
            
            inputCommunicationStream.WaitForConnection();
            Console.WriteLine("C++ --> C# connected");

            outputCommunicationStream.WaitForConnection();
            Console.WriteLine("C++ <-- C# connected");

            isInSetup = false;

            readThread = new Thread(ReadFromOctocadCpp);
            readThread.Start();
        }

        /// <summary>
        /// Reads communications from the Octocad CPP system.
        /// </summary>
        private void ReadFromOctocadCpp()
        {
            byte[] inputBuffer = new byte[BUFFER_SIZE];
            int readCount = 1;

            while (readCount > 0)
            {
                readCount = inputCommunicationStream.Read(inputBuffer, 0, BUFFER_SIZE);
                if (readCount > 0)
                {
                    Console.WriteLine((char)inputBuffer[0]);
                    inputCommunicationStream.Write(inputBuffer, 0, 1);
                }
            }
        }

        /// <summary>
        /// Writes communications to the Octocad CPP system.
        /// </summary>
        /// <param name="sendData"></param>
        /// <returns></returns>
        public bool WriteToOctocadCpp(byte[] sendData)
        {
            if (isInSetup)
            {
                return false;
            }

            Console.WriteLine("Writing bytes to the stream...");
            outputCommunicationStream.Write(sendData, 0, sendData.Length);
            outputCommunicationStream.Flush();
            return true;
        }
    }
}
