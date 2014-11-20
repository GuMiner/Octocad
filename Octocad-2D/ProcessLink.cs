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

            // Start up the C++ program. TODO this should really be gated on waiting until the server is up.
            octocadCpp = new Process();
            octocadCpp.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Path.GetFullPath("..\\..\\..\\..\\Release\\Octocad.exe"),
                WorkingDirectory = Path.GetFullPath("..\\..\\..\\")
            };
            Console.WriteLine(octocadCpp.StartInfo.WorkingDirectory);
            octocadCpp.Start();
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

            while (true)
            {
                Console.WriteLine("Reading bytes from the stream...");
                int readCount = inputCommunicationStream.Read(inputBuffer, 0, BUFFER_SIZE);
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
