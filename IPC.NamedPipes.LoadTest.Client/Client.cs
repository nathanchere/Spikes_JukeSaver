using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;

namespace IPC.NamedPipes
{
    /// <summary>
    /// Testing performance under mild load and handling dropped connections
    /// </summary>
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
        }

        public void Run()
        {
            using (pipeClient = new NamedPipeClientStream(
                "localhost",
                Config.PipeName,
                PipeDirection.InOut,
                PipeOptions.None
                ))
            {

                try
                {
                    Console.Write("Waiting for new connection...");
                    pipeClient.Connect(5000);
                    Console.WriteLine("...Connected!");
                    pipeClient.ReadMode = PipeTransmissionMode.Message;

                }
                catch (Exception ex)
                {
                    Console.WriteLine("...timed out :(");
                }

                try
                {
                    while (pipeClient.IsConnected)
                    {

                        byte[] bResponse = new byte[Config.BufferSize];
                        int cbResponse = bResponse.Length;

                        int cbRead = pipeClient.Read(bResponse, 0, cbResponse);

                        var message = Encoding.Unicode.GetString(bResponse).TrimEnd('\0');
                        Console.WriteLine(message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    Console.WriteLine("Connection lost");
                    if (pipeClient != null)
                    {
                        Console.WriteLine("Cleaning up pipe connection...");
                        pipeClient.Close();
                        pipeClient = null;
                    }
                }
            }

        }
    }

}