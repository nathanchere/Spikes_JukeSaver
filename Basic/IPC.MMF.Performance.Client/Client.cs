using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;

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

        public async void Run()
        {
            Console.WriteLine("Listening to mapped file " + Config.MAPPED_FILE_NAME);
            var mutexName = "mmfclientmutex" + (Guid.NewGuid());
            Console.Title = mutexName;

            using (var map = MemoryMappedFile.CreateOrOpen(Config.MAPPED_FILE_NAME, Config.BufferSize))
            using (var stream = map.CreateViewStream())
            while (true) await Task.Run(() => Read(stream));
        }        

        private void Read(MemoryMappedViewStream stream)
        {                        
            try
            {
                var serializer = new BinaryFormatter();

                stream.Position = 0;
                var timestamp = DateTime.Now;
                AudioDataDTO result = null;

                try {
                    result = serializer.Deserialize(stream) as AudioDataDTO;
                } catch (SerializationException)
                {
                    if (lastGuid != Guid.Empty) {
                        Console.WriteLine("Error deserializing; resetting listener...");
                        lastGuid = Guid.Empty;
                    }                    
                }

                if (result == null || result.Guid == lastGuid)
                {
                    Thread.Sleep(1); // Drops typical CPU usage while 'idle' from 18-23% to <1%
                    return;
                }

                lastGuid = result.Guid;                
                var diff = timestamp - result.Timestamp;                

                Console.WriteLine("Message in; delay: " + diff.TotalMilliseconds);
                Console.WriteLine(result.ToString());
                                
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }                        
        }
    }
}