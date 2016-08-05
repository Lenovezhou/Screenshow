﻿using UnityEngine;
using System.Collections;
using LitJson;
using DataBase;
using Game;
using System.Collections.Generic;
using UnityEngine.UI;

public class changeCurtain : MonoBehaviour
{
    public delegate void ReCallBack(bool isRight);

    public ReCallBack Recall;
  
    private Material SelfMaterial;

    private Texture SelfTexture;

    public Text UItext;

    public Text Text222;

    void Start()
    {
        SelfMaterial = this.GetComponent<Renderer>().material;
        SelfTexture = SelfMaterial.mainTexture;
    }

    void ReceiveMessage(string data)
    {

        UItext.text = data;

        Curtain curtain = JsonMapper.ToObject<Curtain>(data);

        Text222.text = curtain.TextureUrl;

        List<string> m_servers= InitServerConfig._instance.m_servers;
        string path = curtain.TextureUrl;
       
        InitServerConfig._instance.m_downLoader.StartDownload(m_servers, "", path,null, null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);

    }


    public void OnLoadUpdateZipComplete(object data,string item )
    {
        Texture t = data as Texture;
        SelfMaterial.mainTexture = t;
        //Recall(true);
        Application.ExternalCall("showTip", "成功");
    }

    void OnLoadFaile(object data,string item)
    {
        //Recall(false);
        Application.ExternalCall("showTip", "失败");
    }

    IEnumerator LoadPicture(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.error!=null)
        {
            Recall(false);
        }
        else if (www.isDone && www.progress == 1)
        {
            Recall(true);
            SelfMaterial.mainTexture = www.texture;
        }

    }
}
