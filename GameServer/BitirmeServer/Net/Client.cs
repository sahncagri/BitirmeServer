using GameServer.Model;
using GameServer.Servers;
using GameServer.Tool;
using MySql.Data.MySqlClient;
using System;
using System.Net.Sockets;
public class Client {
    private Socket clientSocket;
    private Server server;
    private Message msg = new Message();
    private User user;
    private Result result;
    public int HP { get; set; }
    public Client() { }
   
    private MySqlConnection mysqlConn;
    public MySqlConnection MysqlConn
    {
        get { return mysqlConn; }
    }
    public Client(Socket clientSocket, Server server)
    {
        this.clientSocket = clientSocket;
        this.server = server;
        mysqlConn = ConnHelper.Connect();
    }
    public void Start()
    {
        if (clientSocket == null || clientSocket.Connected == false) return;
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
    }
    public void Send(ActionCode actionCode, string data)
    {

        byte[] bytes = Message.PackData(actionCode, data);
        if (clientSocket != null && bytes != null)
            clientSocket.Send(bytes);

    }
    private void ReceiveCallBack(IAsyncResult ar)
    {

        int count = clientSocket.EndReceive(ar);
        if (count == 0)
        {
            Close();
        }
        msg.ReadMessage(count, OnProcessMessage);
        Start();

    }
    private void Close()
    {
        ConnHelper.CloseConnection(mysqlConn);
        if (clientSocket != null)
        {
            clientSocket.Close();
        }
        server.RemoveClient(this);
    }
    private void OnProcessMessage(RequestCode requestCode, ActionCode actionCode, string data)
    {
        server.HandleRequest(requestCode, actionCode, data, this);
    }
    public void SetUserData(User user, Result result)
    {
        this.user = user;
        this.result = result;
    }
    public string GetUserData()
    {
        return user.Id + "," + user.Username + "," + result.TotalCount + "," + result.WinCount;
    }
    public int GetUserId()
    {
        return user.Id;
    }
  
   
}