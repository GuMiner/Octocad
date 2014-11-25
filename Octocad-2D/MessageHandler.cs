using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Octocad_2D
{
    /// <summary>
    /// Converts incoming C++ messages to friendly C# forms and vice versa.
    /// </summary>
    class MessageHandler
    {
        // WARNING: This is passed as a byte, so you have 255 options only! This should also by synchronized with the C++ executable
        public enum MessageType { PREFERENCES_UPDATE = 0, EXIT_MESSAGE, EXTRUDE_SETTINGS, REVOLVE_SETTINGS, PLANE_BIT_DATA};
        private static uint HEADER_BYTE_LENGTH = 9;

        public static byte[] TranslateMessage(MessageType messageType, params object[] values)
        {
            byte[] returnBytes = null;

            switch (messageType)
            {
                case MessageType.PREFERENCES_UPDATE:
                    // Length and resolution passed in
                    byte[] length = BitConverter.GetBytes((double)values[0]);
                    byte[] resolution = BitConverter.GetBytes((double)values[1]);
                    returnBytes = ConcactenateAndReturn(messageType, length, resolution);
                    break;
                case MessageType.EXTRUDE_SETTINGS:
                    // Theta, phi, radius, distance, and isMirrored passed in.
                    byte[] theta = BitConverter.GetBytes((double)values[0]);
                    byte[] phi = BitConverter.GetBytes((double)values[1]);
                    byte[] radius = BitConverter.GetBytes((double)values[2]);
                    byte[] distance = BitConverter.GetBytes((double)values[3]);
                    byte[] isMirrored = BitConverter.GetBytes((bool)values[4]);
                    byte[] operationMode = BitConverter.GetBytes((Int32)values[5]);
                    returnBytes = ConcactenateAndReturn(messageType, theta, phi, radius, distance, isMirrored, operationMode);
                    break;
                case MessageType.PLANE_BIT_DATA:
                    Bitmap mainBitmap = (Bitmap)values[0];
                    BitmapData bitmapData = mainBitmap.LockBits(new Rectangle(0, 0, mainBitmap.Width, mainBitmap.Height), ImageLockMode.ReadOnly, mainBitmap.PixelFormat);
                    
                    byte[] width = BitConverter.GetBytes((Int32)bitmapData.Width);
                    byte[] height = BitConverter.GetBytes((Int32)bitmapData.Height);
                    byte[] stride = BitConverter.GetBytes((Int32)bitmapData.Stride);

                    int byteCount = bitmapData.Stride * bitmapData.Height;
                    byte[] image = new byte[byteCount];
                    Marshal.Copy(bitmapData.Scan0, image, 0, byteCount);

                    mainBitmap.UnlockBits(bitmapData);
                    returnBytes = ConcactenateAndReturn(messageType, width, height, stride, image);
                    break;
                case MessageType.EXIT_MESSAGE:
                    returnBytes = new byte[] { (byte)messageType, 0, 0, 0, 0, 0, 0, 0, 0 };
                    break;
                default:
                    break;
            }

            return returnBytes;
        }

        private static byte[] ConcactenateAndReturn(MessageType messageType, params byte[][] values)
        {
            ulong dataSize = HEADER_BYTE_LENGTH;
            foreach (byte[] dataArray in values)
            {
                dataSize += (ulong)dataArray.LongLength;
            }

            byte[] returnData = new byte[dataSize];
            
            // Prepare the header data. Note that we send the remaining message length, not the total message length
            returnData[0] = (byte)messageType;
            byte[] dataSizeBytes = BitConverter.GetBytes((ulong)(dataSize - HEADER_BYTE_LENGTH));
            for (int i = 0; i < dataSizeBytes.Length; i++)
            {
                returnData[i + 1] = dataSizeBytes[i];
            }

            // Concactenate the bytes.
            ulong dataOffset = 9;
            for (int i = 0; i < values.Length; i++)
            {
                Array.Copy(values[i], 0, returnData, (long)dataOffset, values[i].LongLength);
                dataOffset += (ulong)values[i].LongLength;
            }

            return returnData;
        }
    }
}
