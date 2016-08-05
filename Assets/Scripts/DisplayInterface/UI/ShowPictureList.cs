using UnityEngine;
using System.Collections;
using Data;
using UnityEngine.UI;
using System.Collections.Generic;
using DataBase;
public class ShowPictureList : MonoBehaviour
{


    public ProdKind ChangeTarget;   //判断3D模型对象

    public string filePath;
    string path;
    //string beforPath = "http://192.168.1.3:8080/HostServer/";
    private FurnitureData furnitureData = new FurnitureData();       //（传递数据类型不确定）
    private List<Curtain> CurtainData = new List<Curtain>();
    private List<AssetInfo> AssetData = new List<AssetInfo>();

    public Toggle ClickButton;

    void Awake()
    {
        ClickButton = this.GetComponent<Toggle>();
    }

    void Start()
    {

        #region filePath
        //filePath = "http://192.168.1.5:8080/HostServer/Loa.xml";
        //filePath = "http://ftp514893.host571.zhujiwu.cn/Loading5.xml";
        #endregion

    }

    #region Editor测试
    public void ClickButtonEvent()
    {
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 222222 ";
        if (ClickButton.isOn)
        {
            //SingleAddButton._instance.ShuRu.SetActive(true);
            InitServerConfig._instance.m_iconLoader.Clear();
            //Debug.Log("S1111111" + ChangeTarget.ToString());
            if (filePath == string.Empty || filePath == null) return;
            //  加载整体的
            if (ChangeTarget == ProdKind.ChuangLian)
            {
                SingleShow._instance.ClearList();
                string qingqiu = "prod_kind=" + "\"" + EnumToolV2.GetDescription(ChangeTarget) + "\"";
                RequestAll(qingqiu);
            }
            // 加载单个
            else
            {
                //Debug.Log("222222" + ChangeTarget.ToString());
                //StartCoroutine(LoadXMLNew(MsgCenter._instance.WWWURL + filePath));
                string qingqiu = "prod_kind=" + "\"" + EnumToolV2.GetDescription(ChangeTarget) + "\"";
                RequestSingle(qingqiu);
            }
            //Camera.main.GetComponent<AssetManager>().textshow.text += " 2223332 ";
            if (ChangeTarget != ProdKind.ChuangLian)
                MsgCenter._instance.isDisplayList = true;
        }
        else
        {
            if (ChangeTarget != ProdKind.ChuangLian)
            {
                MsgCenter._instance.isDisplayList = false;
            }
            else
            {
            }
        }
        MsgCenter._instance.PictureListState(MsgCenter._instance.isDisplayList);
        MsgCenter._instance.ChangeTargetState(ChangeTarget, ClickButton.isOn);
    }
    public void RequestAll(string qingqiu)
    {
        string temp = MsgCenter._instance.start(MsgCenter._instance.strXML
            (EnumToolV2.GetDescription(FuncID.SingleCurtain), EnumToolV2.GetDescription(ActionID.SingleCurtain), qingqiu));
        Debug.Log(temp);
        StartCoroutine(LoadXMLAsset());
    }
    public void RequestSingle(string qingqiu)
    {
        string temp = MsgCenter._instance.start(MsgCenter._instance.strXML
            (EnumToolV2.GetDescription(FuncID.SingleCurtain), EnumToolV2.GetDescription(ActionID.SingleCurtain), qingqiu));
        //Debug.Log(temp);
        StartCoroutine(LoadXMLNew());
    }

    public void ChangeUIButton()
    {
        ClickButton.isOn = !ClickButton.isOn;
    }

    IEnumerator LoadXML(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        //furnitureData.PictureList = ReadXml.ReadInfo(www.text);

        SendMessage(furnitureData);
    }
    /// <summary>
    /// 加载单个的XML
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator LoadXMLNew()
    {

        yield return new WaitWhile(() => MsgCenter._instance.xml == "");

        List<CurtainManager> _Data = NewReadXml.ReplaceCurtainXml("prod_list", MsgCenter._instance.nowWidow.GetComponent<WindoManager>().GroupID, MsgCenter._instance.xml,MsgCenter._instance.isModle); 
        if (_Data != null)
            SendMessage(_Data);

        //SendMessage(CurtainData);
    }
    /// <summary>
    /// 整体XML加载
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    IEnumerator LoadXMLAsset()
    {
        yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        AssetData = NewReadXml.ReadAllAsset(MsgCenter._instance.xml);
        SendMessage(AssetData);
    }
    public void SendMessage(FurnitureData Data)
    {
        //MsgCenter._instance.ChangeTarget(ChooseTarget);
        //MsgCenter._instance.ReceiveMessage(Data);
    }
    /// <summary>
    /// 单个加载传递实例
    /// </summary>
    /// <param name="Data"></param>
    public void SendMessage(List<CurtainManager> Data)
    {
        MsgCenter._instance.ChangeTarget(ChangeTarget);
        MsgCenter._instance.ReceiveMessage(Data);

        string qingqiu1 = "corp_id=" + "\"" + MsgCenter._instance.nowHouse.Corp_ID + "\" " + "prod_kind=" + "\"" + EnumToolV2.GetDescription(ChangeTarget) + "\"";
        SingleShow._instance.InitCurtainTexture(qingqiu1);
    }

    /// <summary>
    /// 整体替换传递实例
    /// </summary>
    /// <param name="Data"></param>
    public void SendMessage(List<AssetInfo> Data)
    {
        MsgCenter._instance.ChangeTarget(ChangeTarget);
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
