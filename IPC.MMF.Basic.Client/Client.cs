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
            while (true)
            {                
                Console.WriteLine("Listening to mapped file " + Config.MAPPED_FILE_NAME);
                using (var map = MemoryMappedFile.CreateNew(Config.MAPPED_FILE_NAME, Config.BufferSize))
                {                    
                    bool mutexCreated;
                    var mutex = new Mutex(true, Config.MAPPED_FILE_NAME, out mutexCreated);

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
        }
    }
}