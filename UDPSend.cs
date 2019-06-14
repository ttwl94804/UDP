using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPSend : MonoBehaviour
{
    [SerializeField] [Tooltip("如果不填写Ip则广播")] private string ipSend = "";
    [SerializeField] private int sendPort = 4566; //发送到的端口

    private IPEndPoint remoteEndPoint;
    private UdpClient client;
    public static UDPSend instance;

    public void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        //如果不填写Ip则广播
        IPAddress ip;
        if (IPAddress.TryParse(ipSend, out ip))
            remoteEndPoint = new IPEndPoint(ip, sendPort);
        else
            remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, sendPort);
        client = new UdpClient();
    }
    private void Start()
    {

    }

    public void Send()
    {
        SendValue(System.Text.Encoding.Default.GetBytes("谭伟照傻傻嗨"));
    }
    //发送消息
    public void SendValue(byte[] msg)
    {
        try
        {
            if (msg != null)
            {
                client.Send(msg, msg.Length, remoteEndPoint);
            }
        }
        catch (Exception err)
        {
            Debug.LogError("Error udp send : " + err.Message);
        }
    }




}

