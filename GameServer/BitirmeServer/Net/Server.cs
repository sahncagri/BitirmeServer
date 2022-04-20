using System.Net.Sockets;
using System.Net;
using System.Collections.Generic;
using System;
using GameServer.Controller;

public class Server {
    private IPEndPoint ipEndPoint;
    private Socket serverSocket;
    ControllerManager controllerManager;
    private List<Client> clientList = new List<Client>();
    public Server() { }

    public Server(string ipStr, int port)
    {
        SetIpAndPort(ipStr, port);
        controllerManager = new ControllerManager(this);
    }

    private void SetIpAndPort(string ipStr, int port)
    {
        ipEndPoint = new IPEndPoint(IPAddress.Parse(ipStr), port);
    }
    public void Start()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(ipEndPoint);
        serverSocket.Listen(0);
        serverSocket.BeginAccept(AcceptCallBack, null);
    }
    private void AcceptCallBack(IAsyncResult ar)
    {
        Socket clientSocket = serverSocket.EndAccept(ar);
        Client client = new Client(clientSocket, this);
        client.Start();
        clientList.Add(client);
        serverSocket.BeginAccept(AcceptCallBack, null);
    }
    public void RemoveClient(Client client)
    {
        lock (clientList)
        {
            clientList.Remove(client);
        }
    }
    public void SendResponse(Client client, ActionCode actionCode, string data)
    {
        client.Send(actionCode, data);
    }
    public void HandleRequest(RequestCode requestCode, ActionCode actionCode, string data, Client client)
    {
        controllerManager.HandleRequest(requestCode, actionCode, data, client);
    }
  
  
   
  
}