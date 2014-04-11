using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace IPC.MMF
{
    internal class Server : IDisposable
    {
        private ulong counter = 0;
        public void Dispose()
        {

        }

        public Server()
        {

        }

        public void Run()
        {            
            while (true)
            {
                try
                {
                    using (var map = MemoryMappedFile.CreateOrOpen(Config.MAPPED_FILE_NAME, Config.BufferSize))
                    {
                        bool mutexCreated;
                        var mutex = new Mutex(true, "mmfservermutex", out mutexCreated);

                        using (var stream = map.CreateViewStream())
                        {
                            var writer = new BinaryWriter(stream);
                            var message = GetMessage();
                            writer.Write(message);
                            Console.WriteLine(message);
                        }
                        mutex.ReleaseMutex();
                        mutex.WaitOne();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        private string GetMessage()
        {
            return String.Format("{0}: Ping! The time is: {1}",
                ++counter, DateTime.Now.ToLongTimeString());
        }
    }
}