using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// monobehavior singleton
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    private static T m_Instance = null;

    public static T instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("TempSingleton of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    //creation problem
                    if (m_Instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }
                }
            }
            return m_Instance;
        }
    }

    //private void Awake()
    //{

    //    if (m_Instance == null)
    //    {
    //        m_Instance = this as T;
    //        m_Instance.Init();
    //    }
    //}

    //初始化数据
    public virtual void Init() { }


    //public bool 

    //确保在程序退出时销毁实例。
    private void OnApplicationQuit()
    {
        m_Instance = null;
    }
}