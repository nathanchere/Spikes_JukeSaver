using System;
using System.IO;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Timers;

namespace IPC.NamedPipes
{
    internal class Server : IDisposable
    {
        private NamedPipeServerStream pipeServer;
        private long counter = 0;
        private Timer timer;
        private const bool THROTTLE_TIMER = false;

        public void Dispose()
        {            
            if (pipeServer != null)
            {
                pipeServer.Disconnect();
                pipeServer.Close();
                pipeServer = null;
            }
        }

        public Server()
        {            
        }

        public void Run()
        {
            while (true)
            {
                var security = new PipeSecurity();
                security.SetAccessRule(new PipeAccessRule("Administrators", PipeAccessRights.FullControl, AccessControlType.Allow));

                using (pipeServer = new NamedPipeServerStream(
                    Config.PipeName,
                    PipeDirection.InOut,
                    NamedPipeServerStream.MaxAllowedServerInstances,
                    PipeTransmissionMode.Message,
                    PipeOptions.None,
                    Config.BufferSize, Config.BufferSize,
                    security,
                    HandleInheritability.None
                    ))
                {
                    try
                    {
                        Console.Write("Waiting...");
                        pipeServer.WaitForConnection();
                        Console.WriteLine("...Connected!");

                        if (THROTTLE_TIMER)
                        {
                            timer = new Timer
                            {
                                Interval = 1,
                            };
                            timer.Elapsed += (sender, args) => SendMessage();
                            timer.Start();
                        }
                        else
                        {
                            while (true) SendMessage();
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Error: " + ex.Message);
                    }
                    finally
                    {
                        if (pipeServer != null)
                        {
                            Console.WriteLine("Cleaning up pipe server...");
                            pipeServer.Disconnect();
                            pipeServer.Close();
                            pipeServer = null;
                        }
                    }

                }

            }
        }      

        private void SendMessage()
        {
            var message = counter++ + ": Ping! Time is " + DateTime.Now.ToLocalTime();
            var messageBytes = Encoding.Unicode.GetBytes(message);
            Console.WriteLine(message);

            pipeServer.Write(messageBytes, 0, messageBytes.Length);
            pipeServer.WaitForPipeDrain();
        }
    }
}