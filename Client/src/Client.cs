﻿using System.Diagnostics;

namespace ServerTest
{
    public class Client
    {
        private const int BufferSize = 4096;
        public const int Port = 46551;
        public static Client Instance;
        private int _id = 0;
        public string ip = "127.0.0.1";
        private Tcp tcp;

        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new Client();
                Instance.Start();
            }
            else
            {
                Debug.WriteLine("Instance already exists, destroying Object!");
            }
        }

        private void Start() { tcp = new Tcp(); }

        public void ConnectToServer() { tcp.Connect(ip, Port); }
    }
}