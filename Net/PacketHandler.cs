using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PacketData;
using System.Reflection;

public class PacketHandler : MonoBehaviour {

    private Queue<Packet> packetQueue = new Queue<Packet>();

    public int PacketNum = 0;

    private bool isError = false;
    private bool isClose = false;

    SocketCallBack socketCallBack;
    private Transform parent;
    void Start()
    {
        parent = GameObject.Find("Canvas").transform;
    }
    public void InitPacketHandler(SocketCallBack scb)
    {
        socketCallBack = scb;
    }

    //handle packet
    void Update () {
        //if (BattleManager.instance.CurrentState != MatchState.End)
        //{
        //    if (isClose)
        //    {
        //        GameObject tip = (GameObject)Instantiate(ResourceManager.Load(Global.PREFAB_UI_PATH + "TipPanel"));
        //        tip.transform.SetParent(parent);
        //        tip.transform.localPosition = Vector3.zero;
        //        tip.GetComponent<TipPanel>().InitTipPanel("The websocket is Close！", null, null);
        //        isClose = false;
        //        return;
        //    }
        //}
        //if (isError)
        //{
        //    GameObject tip = (GameObject)Instantiate(ResourceManager.Load(Global.PREFAB_UI_PATH + "TipPanel"));
        //    tip.transform.SetParent(parent);
        //    tip.transform.localPosition = Vector3.zero;
        //    tip.GetComponent<TipPanel>().InitTipPanel("The connected is error！", null, null);
        //    isError = false;
        //    return;
        //}
        if (packetQueue.Count >0)
        {
            PacketNum = packetQueue.Count;
            for(int i = 0; i < PacketNum; i ++)
            {
                Packet packet = packetQueue.Dequeue();
                MethodInfo method = typeof(SocketCallBack).GetMethod(packet.cmd.ToString());
                method.Invoke(socketCallBack, new object[] { packet });
            }
        }
	}

    public void AddPacket(Packet packet)
    {
        packetQueue.Enqueue(packet);
    }

    public void AddErrorMessage()
    {
        isError = true;
    }

    public void AddCloseMessage()
    {
        isClose = true;
    }
}
