using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Threading;

namespace IPC.MMF
{
    internal class Client : IDisposable
    {
        private string lastMessage = null;

        public void Dispose()
        {
            
        }

        public Client()
        {            
              
        }

        public void Run()
        {
            Console.WriteLine("Listening to mapped file " + Config.MAPPED_FILE_NAME);
            var mutexName = "mmfclientmutex" + (Guid.NewGuid());
            Console.Title = mutexName;
            while (true)
            {                
                try
                {                    
                    using (var map = MemoryMappedFile.CreateOrOpen(Config.MAPPED_FILE_NAME, Config.BufferSize))
                    {
                        bool mutexCreated;
                        var mutex = new Mutex(true, mutexName, out mutexCreated);

                        using (var stream = map.CreateViewStream())
                        {
                            var reader = new BinaryReader(stream);
                            var message = reader.ReadString();

                            if (!string.IsNullOrEmpty(message) &&
                                !string.Equals(message, lastMessage))
                            {
                                Console.WriteLine(message);
                                lastMessage = message;
                            }

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
    }
}