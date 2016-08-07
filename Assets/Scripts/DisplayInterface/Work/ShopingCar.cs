using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class ShopingCar : MonoBehaviour
{
    public InputField prodNum;

    public InputField window_L;
    public InputField window_H;

    public GameObject CarTip;

    bool isClick;
    bool isbuy;

    string path;
    AssetManager AssetManager;
    MsgCenter MsgCenter;
    string sKind, fWidth, fHeight, fDrape;
    void Start()
    {
        MsgCenter = MsgCenter._instance;
    }

    
    void Update()
    {
        if (isClick)
        {
            WriteXml._Instand.CreateAlterData(prodNum.text,float.Parse( window_L.text), float.Parse( window_H.text), float.Parse( fDrape));
            isClick = false;
            StartCoroutine(WaitXML());
        }
    }

    IEnumerator WaitXML()
    {
        yield return new WaitUntil(() => WriteXml._Instand.xmlStr != "");
        Debug.Log(WriteXml._Instand.xmlStr);
        //Camera.main.GetComponent<AssetManager>().textshow.text = " 购物车提交的数据：：：：   " + WriteXml._Instand.xmlStr;
        //向服务器发送数据
        if (WriteXml._Instand.xmlStr != "")
        {
            MsgCenter.start(WriteXml._Instand.xmlStr);
            StartCoroutine(LoadXML());
        }
        if(MsgCenter.nowWidow==null)
        {
            MsgCenter.shopingTip.parent.gameObject.SetActive(true);
            MsgCenter.shopingTip.FindChild("tipText").GetComponent<Text>().text = "请先选择您心怡的那一套！";
        }

        WriteXml._Instand.xmlStr = "";
        WriteXml._Instand.temps.Clear();
    }
    IEnumerator LoadXML()
    {
        yield return new WaitWhile(() => MsgCenter.xml == "");
        string erro = NewReadXml.Result(MsgCenter.xml);
        //Camera.main.GetComponent<AssetManager>().textshow.text += "   购物车返回的提交信息  " + MsgCenter.xml;
        MsgCenter.shopingTip.parent.gameObject.SetActive(true);
        Text erroText=MsgCenter.shopingTip.FindChild("tipText").GetComponent<Text>();
        //判断返回的值来确定是否上传成功
        if (erro!="")
        {
            erroText.text = erro;
        }
        else 
        {
            erroText.text = "未知错误，联系管理员！";
            Debug.Log("上传失败");
        }
    }
   
    //购物车结算按钮
    public void OnClickbuy()
    {
        Application.ExternalEval("window.open(" + "\'" + MsgCenter.CartURL + "\'" + ")");
    }
    //加入购物车按钮
    public void OnClickShop()
    {
        //如果id为空说明没登录
        if (MsgCenter.userID == "")
        {
            Application.ExternalCall("jzLogin");
        }
        else
            CarTip.SetActive(true);
    }
    //加
    public void ButtonPlus()
    {
        prodNum.text = ((int.Parse(prodNum.text)) + 1).ToString();
    }
    //减
    public void ButtonReduce()
    {
        if (int.Parse(prodNum.text)>1)
            prodNum.text = ((int.Parse(prodNum.text)) - 1).ToString();
    }

    public void ButtonSubmit()
    {
        isClick = true;
        CarTip.SetActive(false);
    }
    public void ButtonClossCarTip()
    {
        CarTip.SetActive(false);
    }
    public void ToggleChange(bool value)
    {
        if (value)
        {
            fDrape = "1";
        }
    }
    public void ToggleChange2(bool value)
    {
        if (value)
        {
            fDrape = "2";
        }
    }
    public void ToggleChange3(bool value)
    {
        if (value)
        {
            fDrape = "3";
        }
    }


    //关闭提示框
    public void OnClossTip()
    {

    }
}
