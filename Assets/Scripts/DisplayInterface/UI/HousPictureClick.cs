using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HousPictureClick : MonoBehaviour
{
    private HouseManager House;
    private List<SceneManager> _Data;
    public Toggle GameSearch;//更换完成图片或者是bundle之后，出现搜索图片覆盖//
    string filePath;
    public static bool IsFirst;
    AssetManager Asset;

    public static HousPictureClick _instance;
    void Awake()
    {
        _instance = this;
    }
    //private Image myImage;

    void Start()
    {
        //myImage = this.transform.GetChild(0).GetComponent<Image>();
        Asset=Camera.main.GetComponent<AssetManager>();
        House = this.GetComponent<HouseManager>();
        filePath = MsgCenter._instance.WWWURL + "Scene.xml";
        GameSearch.group = transform.parent.GetComponent<ToggleGroup>();
    }

    void Update()
    {

    }

    public void OnClick()
    {
        if (GameSearch.isOn)
        {
            Request();
            //MsgCenter._instance.Target = TargetKind.Null;
            //MsgCenter._instance.HouseID = House.ID;
            //StartCoroutine(Asset.LoadMap(MsgCenter._instance.WWWURL + this.GetComponent<HouseManager>().Map));
        }
        else
        {
            //myImage.color = Color.white;
        }
    }

    public void Request()
    {
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 111111 " + House.Temp_ID;
        MsgCenter._instance.nowHouse = House;
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 222222222222 ";
        Debug.Log(House.Temp_ID);
        MsgCenter._instance.start(MsgCenter._instance.strXML("3D404637", "page", "scene_id=" + "\"" + House.Temp_ID + "\""));
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 333333333 ";
        StartCoroutine(Asset.LoadMap(MsgCenter._instance.WWWURL + House.Map));

        //Camera.main.GetComponent<AssetManager>().textshow.text += " 44444444444444 ";
        StartCoroutine(LoadXML());
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 44444444444444 " + MsgCenter._instance.xml;
    }

    public IEnumerator LoadXML()
    {
        yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        //yield return new WaitForSeconds(5);
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 6666666666666666 " + MsgCenter._instance.xml;
        _Data = NewReadXml.ReadSceneXml(House.Temp_ID, MsgCenter._instance.xml);
        //_Data = ReadXml.ReadSceneXml(House.ID, www.text);
        if(_Data!=null)
            SendMessage(_Data);
    }
    public void SendMessage(List<SceneManager> Data)
    {
        MsgCenter._instance.ReceiveMessage(Data);
    }
    //void OnDisable()
    //{
    //    //Debug.Log("禁用");
    //    GameSearch.isOn = false;
    //}
}