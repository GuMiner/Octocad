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
        // TODO make this constantly mutate so that multiple copies of Octocad can run at once.
        public const String OctocadPipeName = "\\\\.\\pipe\\Octocad";
        private Thread pipeThread;

        private int BUFFER_SIZE = 4096;
        private NamedPipeServerStream communicationStream;

        private Thread readThread;
        private bool isInSetup;

        private Process octocadCpp;

        public ProcessLink()
        {
            communicationStream = null;
            isInSetup = true;

            // Start up the server
            pipeThread = new Thread(CreatePipeServer);
            // pipeThread.Start();

            // Start up the C++ program. TODO this should really be gated on waiting until the server is up.
            octocadCpp = new Process();
            octocadCpp.StartInfo = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = Path.GetFullPath("..\\..\\..\\..\\Release\\Octocad.exe"),
                WorkingDirectory = Path.GetFullPath("..\\..\\..\\")
            };
            Console.WriteLine(octocadCpp.StartInfo.WorkingDirectory);
           // octocadCpp.Start();
        }

        ~ProcessLink()
        {
            if (communicationStream != null)
            {
                communicationStream.Dispose();
            }
        }

        private void CreatePipeServer()
        {
            communicationStream = new NamedPipeServerStream("Octocad", PipeDirection.InOut);

            Console.WriteLine("Waiting for Octocad cpp...");
            communicationStream.WaitForConnection();

            Console.WriteLine("Connected to Octocad cpp.");
            isInSetup = false;

            readThread = new Thread(ReadFromOctocadCpp);
            readThread.Start();

            WriteToOctocadCpp(new byte[] { 65 }, 1);
        }

        /// <summary>
        /// Reads communications from the Octocad CPP system.
        /// </summary>
        private void ReadFromOctocadCpp()
        {
            byte[] inputBuffer = new byte[BUFFER_SIZE];

            while (true)
            {
                int readCount = communicationStream.Read(inputBuffer, 0, BUFFER_SIZE);
                if (readCount > 0)
                {
                    WriteToOctocadCpp(inputBuffer, 1);
                    Thread.Sleep(1000);
                    Console.WriteLine((char)inputBuffer[0]);
                }
            }
        }

        /// <summary>
        /// Writes communications to the Octocad CPP system.
        /// </summary>
        /// <param name="sendData"></param>
        /// <param name="dataLength"></param>
        /// <returns></returns>
        public bool WriteToOctocadCpp(byte[] sendData, uint dataLength)
        {
            if (isInSetup || communicationStream == null)
            {
                return false;
            }

            communicationStream.Write(sendData, 0, (int)dataLength);
            communicationStream.Flush();
            return true;
        }
    }
}
