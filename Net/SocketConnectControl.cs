using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using WebSocketSharp;
using PacketData;
using System.Reflection;
/// <summary>
/// 连接状态
/// </summary>
public enum SocketState
{
    connecting,//正在连接
    connected,//连接成功
    offline,//离线
}


public enum SocketBindingState
{
    none,//未绑定
    waiting,//已经发送绑定，等待返回
    success//已收到绑定返回
}


///// <summary>
///// socket包状态
///// </summary>
//public enum SocketPackState
//{
//    done,//完成
//    handle,//处理中
//    inQueue,//等待队列
//    fail//请求失败
//}

//public class SocketPack
//{
//    public int cmd = 0;
//    public Dictionary<string, object> requestData = new Dictionary<string, object>();
//    public SocketPackState packState = SocketPackState.inQueue;

//}


public class SocketConnectControl : MonoSingleton<SocketConnectControl>
{
    /// <summary>
    /// 连接状态
    /// </summary>
    //SocketState connectState = SocketState.offline;

    /// <summary>
    /// 绑定状态
    /// </summary>
    //SocketBindingState bindingState = SocketBindingState.none;
    private WebSocket ClientSocket;
    //public BallCharacter player;

    private PacketHandler packetHandler;
    public string sessionId;
    public bool isSync = false;
    //BallManager BallManager;
    SocketCallBack socketCallBack;

    string bindingID = "";

    bool isReconnecting = false;

    //30ms sync server data
    float gapTime = 0.03f;
    float time = 0;
    bool isSyncZero = false;

    //public ServerSetting serverSetting;

    public bool IsLocalNet = false;

    public string connectIp = "ws://120.27.131.52:8088";
    string outerNet = "ws://120.27.131.52:8088";
    string localNet = "ws://192.168.2.21:8088";

    void Awake()
    {
        //Debug.Log("ServerSetting : " + serverSetting.HttpServerIp);
        packetHandler = this.GetComponent<PacketHandler>();
        //BallManager = BallManager.instance;
        socketCallBack = new SocketCallBack(this);
        packetHandler.InitPacketHandler(socketCallBack);
    }

    public int PacketNum
    {
        get { return packetHandler.PacketNum; }
    }

    //ws://120.27.131.52:8088
    void Start()
    {
        Debug.Log("SocketConnectControl --------------------------");
        StartCoroutine(waitingMapLoading());
    }

    IEnumerator waitingMapLoading()
    {
        yield return new WaitForSeconds(1);
        //Connect ip
        connectIp = "ws://";
            //"ws://" + Global.BattleIp + ":" + Global.BattlePort;
        Dictionary<string, string> dic = new Dictionary<string, string>();
        ClientSocket = new WebSocket(connectIp);
        ClientSocket.WaitTime = new TimeSpan(0, 0, 4);
        InitMonitor();
        ClientSocket.ConnectAsync();

        while (!ClientSocket.IsConnected)
        {
            yield return null;
        }
        //ClientSocket.Connect();
        ClientSocket.OnMessage += OnMessage;
        if (ClientSocket.IsConnected)
        {
            Packet packet = new Packet();
            packet.cmd = Packet.EnmCmdType.CMD_CS_LOGIN;
            packet.packCSLogin = new PacketCSLogin();
            //packet.packCSLogin.account = PlayerData.Instance.USER_ID;// TestBaseInfo.instance.account;
            if (PlayerPrefs.GetInt("isdebug") == 0)
            {
                //packet.packCSLogin.md5 = PlayerData.Instance.MD5;
            }
            else
            {
                packet.packCSLogin.md5 = "";
            }
            // TestBaseInfo.instance.name;
            SendRequestData(packet);
            StartCoroutine(CsJoinGame());
        }
        else
        {
            ClientSocket.Close();
            Debug.Log("The connect is not successful");
        }
    }


    private void InitMonitor()
    {
        ClientSocket.OnOpen += (sender, e) =>
        {
            Debug.Log("The websocket is open!");
        };
        ClientSocket.OnClose += (sender, e) =>
        {
            //Debug.Log("The websocket is Close");
            packetHandler.AddCloseMessage();

        };
        ClientSocket.OnError += (sender, e) =>
        {
            Debug.Log("The connected is error : " + e.Message);
            packetHandler.AddErrorMessage();
        };
    }

    IEnumerator CsJoinGame()
    {
        while (string.IsNullOrEmpty(sessionId))
        {
            yield return null;
        }

        if (!string.IsNullOrEmpty(sessionId))
        {
            Packet csJoinPac = new Packet();
            csJoinPac.cmd = Packet.EnmCmdType.CMD_CS_JOIN;
            csJoinPac.packCSJoin = new PacketCSJoin();
            csJoinPac.packCSJoin.sessionid = sessionId;
            SendRequestData(csJoinPac);
        }
    }

    /// <summary>
    /// Send data to server
    /// </summary>
    /// <param name="packet"></param>
    public void SendRequestData(Packet packet)
    {
        MemoryStream memStream = new MemoryStream();
        ProtoBuf.Serializer.Serialize<Packet>(memStream, packet);
        byte[] requestData = memStream.ToArray();
        memStream.Close();
        ClientSocket.Send(requestData);
    }

    /// <summary>
    /// Listen to receive msg
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void OnMessage(object sender, MessageEventArgs e)
    {
        //Debug.Log("收到服务端信息");
        byte[] data = e.RawData;
        MemoryStream outStream = new MemoryStream(data);
        Packet packet = ProtoBuf.Serializer.Deserialize<Packet>(outStream);
        packetHandler.AddPacket(packet);
    }

    
    float accTime = 0;
    public float pingTime = 0;

    PACVector lastPacVector = new PACVector();

    float pingTimeGap = 0f;

    // Update is called once per frame
    void Update()
    {
        if(ClientSocket == null || !ClientSocket.IsConnected)
        {
            return;
        }
        pingTimeGap -= Time.deltaTime;
        if(pingTimeGap < 0)
        {
            pingTimeGap = 5f;
            Packet pingPacket = new Packet();
            pingPacket.cmd = Packet.EnmCmdType.CMD_CS_PING;
            pingPacket.packCSPing = new PacketCSPing();
            //pingPacket.packCSPing.sendtime = GlobalFunction.GetTimeStamp();
            
            SendRequestData(pingPacket);
        }
        //if (player != null)
        //{
        //    PACVector pacVector = CsVectorPacket();
        //    if (lastPacVector.spd != pacVector.spd || lastPacVector.dir != pacVector.dir)
        //    {
        //        Packet packet = CsMovePacket();
        //        SendRequestData(packet);
        //    }
        //    lastPacVector.spd = pacVector.spd;
        //    lastPacVector.dir = pacVector.dir;
        //}
    }

}
