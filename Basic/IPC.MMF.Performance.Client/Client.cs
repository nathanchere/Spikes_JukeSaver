using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net.Mail;
using System.Runtime.Serialization;
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

            var lastValidGuid = new Guid();

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
                            AudioDataDTO result;

                            try {
                                result = serializer.Deserialize(stream) as AudioDataDTO;                                
                            } catch (SerializationException) {
                                continue;
                            }
                            
                            if (result == null) continue;
                            if(result.Guid == lastValidGuid) continue;
                            
                            var diff = timestamp - result.Timestamp;
                            lastValidGuid = result.Guid;

                            Console.WriteLine("Message in; delay: " + diff.TotalMilliseconds);
                            Console.WriteLine(result.ToString());
                        }
                        mutex.ReleaseMutex();
                        mutex.WaitOne();
                        mutex.Close();
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}