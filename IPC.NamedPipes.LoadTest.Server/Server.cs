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
            var security = new PipeSecurity();
            security.SetAccessRule(new PipeAccessRule("Administrators", PipeAccessRights.FullControl, AccessControlType.Allow)); // TODO: other options?
            
            pipeServer = new NamedPipeServerStream(
                Config.PipeName,
                PipeDirection.InOut, // TODO: single direction, benefits?
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Message, // TODO: investigate other options
                PipeOptions.None, // TODO: Async and writethrough, benefits?
                Config.BufferSize, Config.BufferSize,
                security,
                HandleInheritability.None
            );
        }

        public void Run()
        {
            Console.Write("Waiting...");
            pipeServer.WaitForConnection();
            Console.WriteLine("...Connected!");            

            timer = new Timer {
                Interval = 1,
            };

            timer.Elapsed += (sender, args) => { 
                var message = counter++ + ": Ping! Time is " + DateTime.Now.ToLocalTime();
                var messageBytes = Encoding.Unicode.GetBytes(message);
                Console.WriteLine(message);

                pipeServer.Write(messageBytes, 0, messageBytes.Length);
                pipeServer.WaitForPipeDrain();
            };

            timer.Start();                      
        }
    }
}