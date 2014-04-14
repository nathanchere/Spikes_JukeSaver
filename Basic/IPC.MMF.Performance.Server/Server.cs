using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Timer = System.Timers.Timer;

namespace IPC.MMF
{
    internal class Server : IDisposable
    {
        private const int BROADCAST_INTERVAL = 1500; // 1.5 seconds

        private MemoryMappedFile _map;
        private Timer _timer;
        private Random _random;
        Mutex _mutex;

        public void Dispose()
        {
            Console.WriteLine("Server disposing...");
            if (_timer != null) {
                _timer.Stop();
                _timer = null;
            }

            if (_mutex != null)
            {                
                _mutex.Dispose();                
            }

            if (_map != null) {
                _map.Dispose();
                _map = null;
            }            
        }

        public Server()
        {
            _random = new Random();
            _timer = new Timer();
            _map = MemoryMappedFile.CreateOrOpen(Config.MAPPED_FILE_NAME, Config.BufferSize);
        }

        public void Run()
        {
            _timer.Interval = BROADCAST_INTERVAL;
            _timer.Elapsed += (sender, args) => Broadcast();
            _timer.Start();
        }

        private void Broadcast()
        {
            try {
                bool mutexCreated;
                _mutex = new Mutex(true, "mmfservermutex", out mutexCreated);                
                if (mutexCreated == false) {
                    Console.WriteLine("Could not create mutex; broadcast cancelled");
                    return;
                }

                using (var stream = _map.CreateViewStream()) {
                    var message = GetMessage();

                    var serializer = new BinaryFormatter();
                    serializer.Serialize(stream, message);

                    Config.RaiseBroadcastEvent(new BroadcastEventArgs(message.Guid));

                    Console.WriteLine(message.ToString());
                }                                       
                _mutex.WaitOne(0, true);
                _mutex.ReleaseMutex();
                _mutex.Close();
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private AudioDataDTO GetMessage()
        {
            var samples = new List<float>();
            for (int i = 0; i < 10; i++)
                samples.Add((float)_random.NextDouble());

            return new AudioDataDTO {
                Timestamp = DateTime.Now,
                Samples = samples,
                Message = "Tick!",
                Guid = Guid.NewGuid(),

            };
        }
    }
}