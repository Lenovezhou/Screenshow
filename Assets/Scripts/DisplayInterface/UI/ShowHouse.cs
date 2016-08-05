using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Data;
using System.Collections.Generic;
public class ShowHouse : MonoBehaviour {

    private Toggle ClickButton;
    private List<HouseManager> _Data;
    public TargetStyle ChooseTarget;
    public string filePath;
    public static ShowHouse _instance;
    void Awake()
    {
        _instance = this;
    }
	void Start () {

        ClickButton = this.GetComponent<Toggle>();

        _Data = new List<HouseManager>();

        filePath = MsgCenter._instance.WWWURL + filePath;

	}

    #region Editor测试
    public void ClickButtonEvent()
    {
        if (!MsgCenter._instance.isInit)
        {
            Request("corp_id=\"2015001\"");
            //Request("corp_id=" + "\"" + MsgCenter._instance.corpID + "\"");
        }
        //StartCoroutine(LoadXML(filePath));
        //MsgCenter._instance.Target = TargetKind.Null;
    }
    public void Request(string qingqiu)
    {
        if (filePath == string.Empty || filePath == null) return;
        //string strTemp = MsgCenter._instance.start(MsgCenter._instance.strXML("3D404641", "dict", ""));
        //SendMessage(NewReadXml.ReadHouseXml(strTemp));
//<?xml version="1.0" encoding="utf-8"?>
//<program>
//<func_id>3D404631</func_id>
//<action_id> dict </action_id>
//<parameter corp_id="2015001" uKey="gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM"/>
//</program>
        //Camera.main.GetComponent<AssetManager>().textshow.text += "qingqiu    " + qingqiu;
        if(!MsgCenter._instance.isEdit)
            MsgCenter._instance.start(MsgCenter._instance.strXML("3D404641 ", "page", qingqiu));
        else
            MsgCenter._instance.start(MsgCenter._instance.strXML("3D404631 ", "dict", qingqiu));

        StartCoroutine(LoadXML());
        MsgCenter._instance.Target = ProdKind.Null;
    }
    IEnumerator LoadXML()
    {
        yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        if(!MsgCenter._instance.isEdit)
            _Data = NewReadXml.ReadHouseXml(MsgCenter._instance.xml);
        else
            _Data = NewReadXml.ReadHouseTempXml(MsgCenter._instance.xml);
        if(_Data!=null)
            SendMessage(_Data);
    }
    public void SendMessage(List<HouseManager> Data)
    {
        //Debug.Log(Data.PictureList[0]);
        MsgCenter._instance.ChangeTarget(ChooseTarget);
        MsgCenter._instance.ReceiveMessage(Data);
    }
    #endregion


    #region Web版
    /// <summary>
    /// 与js通信，传递数据
    /// </summary>
    /// <param name="message">点击button的ID</param>
    void JsConmunication(string message)
    {
        Application.ExternalCall("ShowList", message);
    }
    #endregion

}
