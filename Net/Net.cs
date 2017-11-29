using UnityEngine;
using Common.Base;
using System.Text;
using PacketData;
using System.IO;
using System.Reflection;
public class Net : Singleton<Net>
{
    //Dictionary<string, JSONObject> mapSendJsonList = new Dictionary<string, JSONObject>();
    JSONObject sendJson=new JSONObject();
    string szUrl = "";

    HttpCallBack httpCallBack;

    bool isInitHead = false;

    //private 
    private PacketCLCheckHead headData;

    public ALWWWOprator www = null;
    public void InitLogic()
    {
        //string sql = string.Format("insert into 你的表(姓名, 标题, 内容) values(\"{0}\", \"{1}\", \"{2}\")", textBox1.Text, textBox2.Text, textBox3.Text);
        //SqlConnection conn = new SqlConnection("连接字符串");
        //SqlCommand cmd = new SqlCommand(sql, conn);
        //cmd.ExecuteScalar();

        //ALWWWOprator.Instance.InitLogic();          //初使化WWW网络模块
    }

    public void InitNet(ALWWWOprator _www, string _url)
    {
        www = _www;
        szUrl = _url;

        httpCallBack = new HttpCallBack();
    }

    /// <summary>
    /// *** Main Send Message
    /// </summary>
    /// <param name="packet"></param>
    void SendMsg(Packet packet, bool isCheckHead = true)
    {
        if (null == www)
            return;

        MemoryStream memStream = new MemoryStream();
        ProtoBuf.Serializer.Serialize<Packet>(memStream, packet);
        byte[] data = memStream.ToArray();
        string dataStr = GlobalFunction.ToHexString(data); //ToHex
        memStream.Close();
        ALHttpParamForm paramForm = new ALHttpParamForm();       
        paramForm.AddParam("data", dataStr);    

        if(isCheckHead)
        {
            if(headData != null)
            {
                Packet headPacket = new Packet();
                headPacket.cmd = Packet.EnmCmdType.CMD_CL_CHECKHEAD;
                headPacket.packCLCheckHead = new PacketCLCheckHead();
                headPacket.packCLCheckHead.accountid = headData.accountid;
                headPacket.packCLCheckHead.md5 = headData.md5;
                MemoryStream headStream = new MemoryStream();
                ProtoBuf.Serializer.Serialize<Packet>(headStream, headPacket);
                byte[] headByte = headStream.ToArray();
                string headStr = GlobalFunction.ToHexString(headByte);
                paramForm.AddParam("head", headStr);
            }
        }

        www.HTTPRequest(szUrl, ref paramForm, OnReceive_Msg,null, false);
    }

    private  byte HexToByte(string hex)
    {
        byte tt = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        return tt;
    }

  

    public void SendMsgWithCallBack(Packet packet, HttpCallbackDelegate callback, bool isCheckHead = true)
    {
        if (null == www)
            return;

        MemoryStream memStream = new MemoryStream();
        ProtoBuf.Serializer.Serialize<Packet>(memStream, packet);
        byte[] data = memStream.ToArray();
        string dataStr = GlobalFunction.ToHexString(data); //ToHex
        ALHttpParamForm paramForm = new ALHttpParamForm();
        paramForm.AddParam("data", dataStr);
        if (isCheckHead)
        {
            if (headData != null)
            {

                Packet headPacket = new Packet();
                headPacket.cmd = Packet.EnmCmdType.CMD_CL_CHECKHEAD;
                headPacket.packCLCheckHead = new PacketCLCheckHead();
                headPacket.packCLCheckHead.accountid = headData.accountid;
                headPacket.packCLCheckHead.md5 = headData.md5;
                MemoryStream headStream = new MemoryStream();
                ProtoBuf.Serializer.Serialize<Packet>(headStream, headPacket);
                byte[] headByte = headStream.ToArray();
                string headStr = GlobalFunction.ToHexString(headByte);
                paramForm.AddParam("head", headStr);
            }
        }
        www.HTTPRequest(szUrl, ref paramForm, OnReceive_Msg, callback, false);
    }

    //初使化消息头
    public void InitMsgHead(PacketCLCheckHead head)
    {
        headData = head;
    }

    public void AddMsg(string _cmd, JSONObject _msg)
    {
        JSONObject tempjson = new JSONObject();
        tempjson.AddField("cmd", _cmd);
        tempjson.AddField("data", _msg);
        sendJson.Add(tempjson);
    }
    
    void OnReceive_Msg(string data, string error, HttpCallbackDelegate callback)
    {
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogError(error);
            return;
        }
        try 
        {
            string dataStr = data.Substring(2, data.Length - 4);    //remove [""]
            byte[] bData = GlobalFunction.ToHexByte(dataStr);
            MemoryStream outStream = new MemoryStream(bData);
            Packet packet = ProtoBuf.Serializer.Deserialize<Packet>(outStream);

            if (callback != null)
            {
                callback(packet);
            }
            else
            {
                MethodInfo method = typeof(HttpCallBack).GetMethod(packet.cmd.ToString());
                method.Invoke(httpCallBack, new object[] { packet });
            }
        }
        catch
        {          
            throw;
        }
    }

    //网络错误
    public void Net_Error(string szError)
    {
        //Debug.LogError("[Error]我去 error!!!: " + szError);
        //EventManager.Instance.TriggerEvent<string>("UI_Net_Error", szError);
    }


    public bool IsConnect()
    {
        if (www == null)
            return false;
        else
            return true;
    }
}