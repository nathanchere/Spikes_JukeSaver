using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace IPC.MMF
{
    internal class Server : IDisposable
    {
        public void Dispose()
        {

        }

        public Server()
        {

        }

        public void Run()
        {
            ulong counter = 0;
            while (true)
            {
                using (var map = MemoryMappedFile.CreateNew(Config.MAPPED_FILE_NAME, Config.BufferSize))
                {
                    bool mutexCreated;
                    Mutex mutex = new Mutex(true, Config.MAPPED_FILE_NAME, out mutexCreated);

                    using (var stream = map.CreateViewStream())
                    {
                        var writer = new BinaryWriter(stream);
                        var message = String.Format("{0}: Ping! The time is: {1}",
                            ++counter, DateTime.Now.ToLongTimeString());
                        writer.Write(message);
                        Console.WriteLine(message);
                    }
                    mutex.ReleaseMutex();
                    mutex.WaitOne();                    
                }
            }
        }
    }
}