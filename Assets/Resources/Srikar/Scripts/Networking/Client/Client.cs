using UnityEngine;
using System.Net.Sockets;
using System.IO;
using UnityEngine.UI;
using System;

public class Client : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject chatContainer;
    public GameObject messagePrefab;

    private bool socketReady = false;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    [Header("Connection Components")]
    [SerializeField] InputField hostField;
    [SerializeField] InputField portField;

    [Header("UI Componenets")]
    [SerializeField] InputField messageField;
    [SerializeField] InputField clientName;

    public void OnConnectedToServer()
    {
        //if already connected, ignore
        if (socketReady)
        {
            return;
        }

        string host = "127.0.0.1";
        int port = 6321;

        string h;
        int p;

        h = hostField.text;
        if (h != "")
            host = h;

        int.TryParse(portField.text, out p);
        if (p != 0)
            port = p;

        //Creating socket
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    private void OnIncomingData(string data)
    {
        if (data == "%NAME")
        {
            Send("&NAME |" + clientName.text);
            return;
        }

        GameObject temp = Instantiate(messagePrefab, chatContainer.transform);
        temp.GetComponentInChildren<Text>().text = data;
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }

    public void OnSendButton()
    {
        string message = messageField.text;
        Send(message);
    }

    private void CloseSocket()
    {
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }

    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }
}
