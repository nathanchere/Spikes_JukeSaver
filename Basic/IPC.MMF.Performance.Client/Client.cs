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
        private Guid lastGuid = Guid.Empty;

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

            var serializer = new BinaryFormatter();
            bool mutexCreated;
            var mutex = new Mutex(true, mutexName, out mutexCreated);

            using (var map = MemoryMappedFile.CreateOrOpen(Config.MAPPED_FILE_NAME, Config.BufferSize))
            using (var stream = map.CreateViewStream())
            while (true)            
            {                
                try
                {
                    stream.Position = 0;
                    var timestamp = DateTime.Now;
                    AudioDataDTO result;

                    try
                    {
                        result = serializer.Deserialize(stream) as AudioDataDTO;
                    }
                    catch (SerializationException)
                    {
                        if (lastGuid != Guid.Empty)
                        {
                            Console.WriteLine("Error deserializing; resetting listener...");
                            lastGuid = Guid.Empty;
                        }
                        continue;
                    }

                    if (result == null) continue;
                    if (result.Guid == lastGuid) continue;

                    var diff = timestamp - result.Timestamp;
                    lastGuid = result.Guid;

                    Console.WriteLine("Message in; delay: " + diff.TotalMilliseconds);
                    Console.WriteLine(result.ToString());
                    mutex.WaitOne();
                    mutex.ReleaseMutex();                                        
                    Thread.Sleep(16);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }                
            }
            mutex.Close();
            Console.WriteLine("Finished Run(), exiting...");
        }        
    }
}