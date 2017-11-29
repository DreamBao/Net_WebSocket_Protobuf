using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GlobalFunction {

    /// <summary>
    ///  获取时间戳  
    /// </summary>
    /// <returns></returns>
    public static ulong GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return Convert.ToUInt64(ts.TotalMilliseconds);
    }


    /// <summary>
    /// FromUnixTime 如果要获得当地时间（ToLocalTime）
    /// </summary>
    /// <param name="unixTime"></param>
    /// <returns></returns>
    public static DateTime FromUnixTime(ulong unixTime)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return epoch.AddMilliseconds(unixTime);
    }

    public static ulong ToUnixTime(DateTime date)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Convert.ToUInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
    }

    /// <summary>
    /// To Hex String
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToHexString(byte[] bytes)
    {
        string hexString = string.Empty;
        if (bytes != null)
        {
            StringBuilder strB = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                strB.Append(bytes[i].ToString("X2"));
            }
            hexString = strB.ToString();
        }
        return hexString;
    }


    public static byte[] ToHexByte(string hexString)
    {
        hexString = hexString.Replace(" ", "");
        if ((hexString.Length % 2) != 0)
            hexString += " ";
        byte[] returnBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < returnBytes.Length; i++)
            returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        return returnBytes;
    }

    /*public static TipPanel InstantiateTip(string noticeText, EventTriggerListener.VoidDelegate click)
    {
        GameObject go = GameObject.Instantiate(ResourceManager.Load<GameObject>(Global.PREFAB_PATH + "TipPanel"));
        TipPanel tip = go.GetComponent<TipPanel>();
        tip.InitTipPanel(noticeText, click);
        return tip;
    }*/
}
