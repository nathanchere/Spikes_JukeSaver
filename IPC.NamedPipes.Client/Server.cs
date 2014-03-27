using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace IPC.NamedPipes
{
    internal class Client : IDisposable
    {
        private NamedPipeClientStream pipeClient;

        public void Dispose()
        {
            if (pipeClient != null)
            {
                pipeClient.Close();
                pipeClient = null;
            }
        }

        public Client()
        {            
            pipeClient = new NamedPipeClientStream(
                "localhost",
                Config.PipeName,
                PipeDirection.InOut,
                PipeOptions.None // TODO: options? async writethrough benefits?
                );            
        }

        public void Run()
        {
            Console.Write("Waiting...");
            pipeClient.Connect(5000);
            Console.WriteLine("...Connected!");

            pipeClient.ReadMode = PipeTransmissionMode.Message;

            while (true)
            {
                if (pipeClient.IsConnected) // && !pipeClient.IsMessageComplete
                {
                    byte[] bResponse = new byte[Config.BufferSize];
                    int cbResponse = bResponse.Length;

                    int cbRead = pipeClient.Read(bResponse, 0, cbResponse);

                    var message = Encoding.Unicode.GetString(bResponse).TrimEnd('\0');
                    Console.WriteLine(message);
                }
            }
        }
    }
}