using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
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
                            var timestamp = DateTime.Now;

                            var serializer = new BinaryFormatter();
                            var result = serializer.Deserialize(stream) as AudioDataDTO;
                            if(result==null) continue;
                            
                            var diff = timestamp - result.Timestamp;
                            Console.WriteLine("Message in; delay: " + diff.TotalMilliseconds);
                            Console.WriteLine(result.ToString());
                            //stream.Flush();
                        }
                        mutex.ReleaseMutex();
                        mutex.WaitOne();
                        mutex.Close();
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