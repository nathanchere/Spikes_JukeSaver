using System;
using System.Threading;

namespace IPC.NamedPipes
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var quit = new ManualResetEvent(false);
            Console.CancelKeyPress += (s, a) =>
            {
                quit.Set();
                a.Cancel = true;
            };

            var server = new Server();
            server.Run();

            Console.WriteLine("Named pipe server running; Ctrl+C to quit");           
            quit.WaitOne();

            // TODO: Dispose server if needed
        }
    }
}