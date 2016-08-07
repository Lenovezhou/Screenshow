using UnityEngine;
using System.Collections;
using Data;
using UnityEngine.UI;
using System.Collections.Generic;
using DataBase;
public class SingleMainButton : MonoBehaviour
{
    private Button ClickButton;
    private string filePath;

    bool IsRun;

    bool IsTieTu, IsCaiZhi;
    bool IsEnebel;
    float T_temp,C_temp;
    private List<SingleCurtain> mydata = new List<SingleCurtain>();
    void Start()
    {
        //filePath = "http://ftp514893.host571.zhujiwu.cn/Loading5.xml";
    }
    void OnEnable()
    {
        Debug.Log("ins = null ? : " + (MsgCenter._instance == null));
        //filePath = MsgCenter._instance.WWWURL+"LoadSingle.xml";
        //if (filePath == string.Empty || filePath == null)
           // return;
        if(!IsEnebel)
        {
            string qingqiu = "corp_id=" + "\"" + MsgCenter._instance.nowHouse.Corp_ID + "\"";
            //InitCurtainTexture(qingqiu);
            IsEnebel = true;
            //StartCoroutine(LoadXMLNew(filePath));
        }
    }

    /// <summary>
    /// 外部调用，传递数据
    /// </summary>
    /// <param name="data"></param>
    void ReadInfomation(string data)
    {

    }

    private void ClickButtonEvent()
    {
    }

    IEnumerator LoadXMLNew(string path)
    {
        WWW www = new WWW(path);
        yield return www;

        if (www.isDone)
        {
            Debug.Log("加载XML！！");
            //SendMessage(ReadXml.SingleReadInfo(www.text));
        }
    }

    void OnDisable()
    {
        IsEnebel = false;
    }
}
