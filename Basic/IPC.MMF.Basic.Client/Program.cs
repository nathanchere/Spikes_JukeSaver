using System;
using System.Threading;

namespace IPC.MMF
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

            var client = new Client();
            client.Run();

            Console.WriteLine("Memory mapped files client running; Ctrl+C to quit");
            quit.WaitOne();
        }
    }
}