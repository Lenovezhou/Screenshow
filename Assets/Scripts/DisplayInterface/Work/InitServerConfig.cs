#region Version Info
/**
	文件名：InitServerData
	Author: Kenny
	Time: 2015/5/23 星期六 下午 12:34:11
	Desctription: 
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Msg;
using Game;


public class InitServerConfig : MonoBehaviour
{
    public static InitServerConfig _instance;
    public Downloader m_downLoader;
    public AssetDownloader m_assetLoader;
    public IconDownloader m_iconLoader;
    public List<string> m_servers = new List<string>() {""};

    public Session Network
    {
        get { return Game.Session.Instance; }
    }
    private string m_Host = string.Empty;
    private int m_Port;

    #region Singleton
    //private static readonly InitServerConfig m_Instance = new InitServerConfig();
    public static InitServerConfig Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new InitServerConfig();
            }
            return _instance;
        }
    }
    static InitServerConfig()
    {
    }
    private InitServerConfig()
    {
    }
    #endregion

    public virtual void Awake()
    {
        _instance = this;
        m_downLoader = new Downloader();
        m_assetLoader = new AssetDownloader();
        m_iconLoader = new IconDownloader();
        m_downLoader.Init();
        m_assetLoader.Init();
        m_iconLoader.Init();

        MsgHandler.Instance.Init();
        //m_servers.Add(MsgCenter._instance.WWWURL);
    }

    public virtual void Update()
    {
       
        UpdateMsg();

        if (m_downLoader == null)
        {
            return;
        }
        m_downLoader.UpdateDownload();

        if (m_assetLoader == null)
        {
            return;
        }
        m_assetLoader.UpdateDownload();

        if (m_iconLoader == null)
        {
            return;
        }
        m_iconLoader.UpdateDownload();
    }

    private void ConnectToServer(string host, int port)
    {
        m_Host = host;
        m_Port = port;

        Network.Connect(host, port);
    }

    private void UpdateMsg()
    {
        while (true)
        {
            using (MsgDynamic msg = Network.GetMsg())
            {
                if (null != msg)
                {
                    Network.UpdateRecvMsgTime();
                    try
                    {
                        MsgManager.Instance.ProcessMsg(msg);
                    }
                    catch (Exception e)
                    {
                        string error = string.Format("Client UpdateMsg Exception: {0}", e.ToString());
                        UnityEngine.Debug.LogError(error);
                    }
                }
                else
                {
                    break;
                }
            }
        }
    }

    #region MyRegion
    //public void OnLoadUpdateZipComplete(object data)
    //{
    //    Texture t = data as Texture;
    //    Renderer render = m_Box1.GetComponent<Renderer>();
    //    render.material.mainTexture = t;
    //}
    //public void OnLoadUpdateZipComplete2(object data)
    //{
    //    Texture t = data as Texture;
    //    Renderer render = m_Box2.GetComponent<Renderer>();
    //    render.material.mainTexture = t;
    //}
    //void OnLoadFaile(object data)
    //{

    //}
    //private void StartDown1()
    //{
    //    m_downLoader.StartDownload(m_servers, "", "111.jpg", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
    //}
    //private void StartDown2()
    //{
    //    m_downLoader.StartDownload(m_servers, "", "222.jpeg", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete2, OnLoadFaile, true);
    //}
    //void OnGUI()
    //{
    //    if (GUILayout.Button("LoadAssetbundle"))
    //    {
    //        StartDown1();
    //    }
    //    if (GUILayout.Button("LoadAssetbundle2"))
    //    {
    //        StartDown2();
    //    }
    //}
    #endregion

}
