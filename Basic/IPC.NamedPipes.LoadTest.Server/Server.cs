﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using Timer = System.Timers.Timer;

namespace IPC.NamedPipes
{
    internal class Server : IDisposable
    {
        private NamedPipeServerStream pipeServer;
        private long counter = 0;
        private Timer timer;
        private const bool THROTTLE_TIMER = false;
        private const int THROTTLE_DELAY = 2000;

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
                                Interval = THROTTLE_DELAY,
                            };
                            timer.Elapsed += (sender, args) => SendMessage();
                            timer.Start();

                            while(pipeServer.IsConnected)Thread.Sleep(1);
                            timer.Stop();
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
            if(!pipeServer.IsConnected) return;

            var message = counter++ + ": Ping! Time is " + DateTime.Now.ToLocalTime();
            var messageBytes = Encoding.Unicode.GetBytes(message);
            Console.WriteLine(message);            
            pipeServer.Write(messageBytes, 0, messageBytes.Length);
            pipeServer.WaitForPipeDrain();            
        }
    }
}