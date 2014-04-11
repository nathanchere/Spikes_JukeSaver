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

            var client = new Client();
            client.Run();

            Console.WriteLine("Named pipe client running; Ctrl+C to quit");
            quit.WaitOne();
        }
    }
}