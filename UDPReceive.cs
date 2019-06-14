using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


    public class UDPReceive : MonoBehaviour
    {
        [SerializeField]
        private int port = 4566;   //端口 

        [Tooltip("接收的包为文本")] public bool receiveAsText = true; //接收的包为文本
        [Tooltip("接收的包为十六进制")] public bool receiveAsHex = true; //接收的包为十六进制
        [Tooltip("在画面打印接收的包")] public bool showReceivePacket = true;    //在屏幕打印接受的数据包

        private string received = "";
        private UdpClient client;
        private Thread receiveThread;

        private Queue<byte[]> recieveQueue = new Queue<byte[]>();

        public Action<byte[]> ReceiveEventHandler = new Action<byte[]>((packet) => { });

        private string hexPrint = "", textPrint = "";   //用于GUI打印消息
        public static UDPReceive instance;

        public void Awake()
        {

            instance = this;
            DontDestroyOnLoad(gameObject);
            client = new UdpClient(port);
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }


        void Update()
        {
            while (recieveQueue.Count > 0)
            {
                ReceiveEventHandler(recieveQueue.Dequeue());
            }
        
        }


        public void ReceiveData()
        {
            while (true)
            {
                try
                {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = client.Receive(ref anyIP);
                    recieveQueue.Enqueue(data);
                    if (showReceivePacket)
                    {
                        hexPrint = BitConverter.ToString(data);
                        textPrint = Encoding.UTF8.GetString(data);
                    }                  
                }
                catch (Exception err)
                {
                    Debug.Log("错误:" + err.ToString());
                }
            }
        }

        public void OnDisable()
        {
            if (receiveThread != null)
            {
                receiveThread.Abort();
                receiveThread = null;
            }
            client.Close();
            Debug.Log("UDPClient退出");
        }

        private void OnGUI()
        {
            if (!showReceivePacket) return;
            GUI.Label(new Rect(0, 0, Screen.width, 500), "<size=30>Hex  : " + hexPrint + "</size>");
            GUI.Label(new Rect(0, 50, Screen.width, 500), "<size=30>Text : " + textPrint + "</size>");
        }


    }

