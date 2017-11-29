using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using System.Text;
using Common.Base;
public enum EHTTPOptState
{
    eOptState_Ready,
    eOptState_Doing,
    eOptState_Complete,
    eOptState_Failed,
}

public class ALWWWRequest
{
    public uint             m_dwRequestId;
    public string           m_szRequestUrl;
    public WWW              m_www;
    public EHTTPOptState    m_eOptState;
    public uint m_dwErrorNum;
    virtual public void Update() { }
    virtual public bool IsDone() 
    {
        return m_eOptState == EHTTPOptState.eOptState_Failed || m_eOptState == EHTTPOptState.eOptState_Complete;
    }
}


public class ALWWWOprator : MonoBehaviour
{
    //public static ALWWWOprator                  m_Ins = null;
    private Dictionary<uint, ALWWWRequest>      m_dicRequest = null;
    private List<uint>                          m_lstReqComplete = null;
    private uint                                m_dwRequestId;

    private Dictionary<uint, ALWWWRequest>      m_dicRequestCache = null;
    private bool                                m_bUpdating = false;

    public string szUrl ="";        //url地址 内网地址：http://192.168.2.21:3000 外网地址：http://120.27.131.52:3000


    // Use this for initialization
    public void Start() 
    {
        if(String.IsNullOrEmpty(szUrl))
        {
            Debug.Log("http url is null");
            return;
        }
        //m_Ins = this;
        m_dicRequest = new Dictionary<uint, ALWWWRequest>();
        m_dicRequestCache = new Dictionary<uint, ALWWWRequest>();
        m_lstReqComplete = new List<uint>();
        m_dwRequestId = 0;
        Net.Instance.InitNet(this, szUrl);
        Debug.Log("Init Net");
        //DownFile();
        DontDestroyOnLoad(this);
	}


    private void DownFile()
    {
        StartCoroutine(DownFileAsyn());
    }

    IEnumerator DownFileAsyn()
    {
        string uri = "";
        Debug.Log("Start DownloadByWWWFile");

        WWW www = new WWW(uri);

        yield return www;

        if(www.error == null)
            File.WriteAllBytes(Application.persistentDataPath + "/test.txt", www.bytes);
        else
            Debug.Log("download completed& www.error: " + www.error); 
        www.Dispose();

        Debug.Log("write complete!");
    }

    

    /// <summary>
    /// 用WWW进行http请求
    /// </summary>
    /// <param name="_szUrl"></param>
    /// <param name="_ParamForm"></param>
    /// <param name="_delegateAck"></param>
    /// <param name="_bPost"></param>
    public bool HTTPRequest(string _szUrl, 
                            ref ALHttpParamForm _ParamForm, 
                            HTTPReqAckDelegate _delegateAck, 
                            HttpCallbackDelegate _httpCallback,
                            bool _bPost = true)
    {
        ALWWWHTTPRequest request = new ALWWWHTTPRequest();
        request.m_dwErrorNum = 0;
        request.m_eOptState = EHTTPOptState.eOptState_Ready;
        request.m_szRequestUrl = _szUrl;
        request.m_dwRequestId = ++m_dwRequestId;
        request.m_Param = new ALHttpParamForm();
        request.m_Param.CloneParam(ref _ParamForm);
        request.m_ReqDelegate = _delegateAck;
        request.m_ReqCallback = _httpCallback;
        request.m_bPost = _bPost;

        if (m_bUpdating)
        {
            m_dicRequestCache.Add(request.m_dwRequestId, request);
        }
        else
        {
            m_dicRequest.Add(request.m_dwRequestId, request);
        }
        return true;
    }

    /// <summary>
    /// 用WWW进行http下载 (暂无，扩展用)
    /// </summary>
    public void HTTPDownLoad()
    {

    }

    void ExchangeData()
    {
        if (m_dicRequestCache.Count <= 0)
            return;

        foreach (KeyValuePair<uint, ALWWWRequest> kvp in m_dicRequestCache)
        {
            m_dicRequest.Add(kvp.Key, kvp.Value);
        }
        m_dicRequestCache.Clear();
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (m_dicRequest == null || m_dicRequest.Count <= 0)
            return;

	    foreach (KeyValuePair<uint,ALWWWRequest> kvp in m_dicRequest)
	    {
            m_bUpdating = true;
            if (kvp.Value.IsDone())
            {
                m_lstReqComplete.Add(kvp.Key);
                continue;
            }
            kvp.Value.Update();
	    }

        for (int i = 0; i < m_lstReqComplete.Count; ++i)
        {
            m_dicRequest.Remove(m_lstReqComplete[i]);
        }

        ExchangeData();

        m_lstReqComplete.Clear();
        m_bUpdating = false;
	}
}


