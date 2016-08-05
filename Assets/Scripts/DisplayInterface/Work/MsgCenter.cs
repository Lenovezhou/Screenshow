using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
using System.Xml;

public class MsgCenter : MonoBehaviour
{
    public int HouseID;
    public int SceneID;
    public HouseManager nowHouse;
    public SceneManager nowScene;
    public Vector2 nowScenePoint;
    public GameObject EditUI;
    public string UIShowStr="";

    public LookTarget lookTarget;

    public bool isModle;

    public WindoManager DefaultWindo;
    public Transform shopingTip;
    public static MsgCenter _instance;
    public bool isSingleAuto;//单独展示界面的自动旋转选择框；
    public bool isEdit;//是否为编辑模式 
    public bool isChange;//是否改变了窗户
    public bool isChuangLian;
    public GameObject mask;
    public RawImage Map;
    public RectTransform LookIcon;
    public List<CurtainManager> OldData = new List<CurtainManager>();

    public GameObject Go;
    public ChangeTexture _changeTexture;
    public GameObject AssetPlane;

    //public Mesh AssetMesh;

    public string[] QiuURL = new string[3];
    public string[] QiuIcon = new string[3];

    public ProdKind Target;  //获取当前选中的是哪一个窗帘
    public TargetStyle StyleTarget;  //获取当前选中的是哪一个装饰风格

    public Dictionary<string, GameObject> GameManageList;  //用于存放窗帘组件
    public Dictionary<string, Dictionary<string, GameObject>> WindowList; //用于存放场景中的窗户
    public Dictionary<string, CompleteManager> Complete = new Dictionary<string, CompleteManager>();//商品信息
    public Dictionary<string, string> FengGe = new Dictionary<string, string>();
    public Dictionary<string, string> SceneStyle = new Dictionary<string, string>();

    public int ModuleCount = -1;
    public Dictionary<string, bool> TempDisctionary;//判断是否加载完，加载完清空;

    public bool IsOverLode;//结束加载

    private List<Curtain> CurtainDataList;

    public GameObject PictureList;   //图片列表（控制开关）
    private LoadDataFromList _LoadDataScript;  //显示ScrollView中的list

    private string FindName;
    public GameObject[] Qiu;
    public GameObject qiangmian;
    public GameObject diaoding;
    public GameObject diban;
    public GameObject chuanghu;
    public Camera windowCamera;
    public MainMenu m_MainMenu;
    public bool isDisplayList;//是否显示图片列表//

    public Material MaterialAlpha;//组件隐藏的材质
    public Material MaterialAlpha_;
    public string TextUrl;
    public string textRequestXML;
    public string WWWURL;
    public string CartURL;

    public string insertType;
    public bool isInit;
    public bool isSingleInit;
    public string userID="";
    public string corpID="";

    bool isone;
    bool isActiv;

    public string S_Request;
    public string sessionID;
    public string S_RProud;

    private Transform _nowWindow;
    private Transform _LastWindow;
    public Transform nowWidow
    {
        set
        {
            if (value != _LastWindow)
            {
                _nowWindow = value;
                if (value != null)
                {
                    for (int i = 0; i < SingleShow._instance.windowCompoments.Length; i++)
                    {
                        SingleShow._instance.windowCompoments[i] = null;
                        SingleShow._instance.windowMeshs[i] = null;
                        SingleShow._instance.windowSubMeshs[i] = null;
                    }
                    if (value.GetChild(1).GetChild(1).FindChild("2") != null)
                    {
                        SingleShow._instance.windowCompoments[0] = value.GetChild(1).GetChild(1).FindChild("2").gameObject;
                    }
                    if (value.GetChild(1).GetChild(1).FindChild("8") != null)
                    {
                        SingleShow._instance.windowCompoments[1] = value.GetChild(1).GetChild(1).FindChild("8").gameObject;
                    }
                    if (value.GetChild(1).GetChild(0).FindChild("11"))
                    {
                        SingleShow._instance.windowCompoments[2] = value.GetChild(1).GetChild(0).FindChild("11").gameObject;
                    }
                    //Debug.Log(value.GetChild(1).GetChild(0).FindChild("11").gameObject.name);
                    for (int i = 0; i < SingleShow._instance.windowCompoments.Length; i++)
                    {
                        if (SingleShow._instance.windowCompoments[i] != null)
                        {
                            SingleShow._instance.windowMeshs[i] = SingleShow._instance.windowCompoments[i].GetComponent<MeshFilter>().mesh;
                            SingleShow._instance.windowSubMeshs[i] = SingleShow._instance.windowCompoments[i].transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                        }

                    }
                    //MainMenu._instand.IniMemu();
                }

                _LastWindow = value;
            }

        }
        get
        {
            return _nowWindow;
        }
    }
    //登陆后传进来userid
    public void OnUserMessage(string temp)
    {
        string[] allArr = temp.Split('&');

        if (allArr.Length <= 1)
        {
            InitServerConfig._instance.m_servers[0] = WWWURL;
        }
        for (int i = 0; i < allArr.Length; i++)
        {
            string b = "";
            foreach (char c in allArr[i].ToCharArray(0, allArr[i].Length))
            {
                if (c != Convert.ToChar(" "))
                {
                    b += c;
                }
            }
            allArr[i] = b;

            string[] urlArr = allArr[i].Split('=');

            if (urlArr[0] == "user_id")
            {
                this.userID = urlArr[1];
            }
            else if (urlArr[0] == "corp_id")
            {
                this.corpID = urlArr[1];
            }
        }
    }
    //初始化信息
    public void OnMessageTreeD(string temp)
    {
        isInit = true;
        S_Request = "";
        S_RProud = "";
        string[] allArr = temp.Split('&');

        if (allArr.Length <= 1)
        {
            InitServerConfig._instance.m_servers[0] = WWWURL;
        }
        for (int i = 0; i < allArr.Length; i++)
        {
            string b = "";
            foreach (char c in allArr[i].ToCharArray(0, allArr[i].Length))
            {
                if (c != Convert.ToChar(" "))
                {
                    b += c;
                }
            }
            allArr[i] = b;


            string[] urlArr = allArr[i].Split('=');

            if (urlArr[0] == "scene_id")
            {
                S_Request = urlArr[0] + "=" + "\"" + urlArr[1] + "\"";
                //Camera.main.GetComponent<AssetManager>().textshow.text += "   请求：：  " + S_Request;
            }
            else if (urlArr[0] == "url")
            {
                WWWURL = urlArr[1].Substring(0, urlArr[1].Length - 4);
                TextUrl = urlArr[1];
                InitServerConfig._instance.m_servers[0] = WWWURL;
                //Camera.main.GetComponent<AssetManager>().textshow.text += "   资源地址URL：：  " + WWWURL;

                //Camera.main.GetComponent<AssetManager>().textshow.text += "   服务器地址TextUrl：：  " + TextUrl;
            }
            else if (urlArr[0] == "mycart")
            {
                CartURL = urlArr[1];
            }
            else if (urlArr[0] == "prod_id")
            {
                isSingleInit = true;
                S_RProud = urlArr[0] + "=" + "\"" + urlArr[1] + "\"";
            }
            else if (urlArr[0] == "user_id")
            {
                this.userID = urlArr[1];
            }
            else if (urlArr[0] == "corp_id")
            {
                this.corpID = urlArr[1];
            }
        }
        MainMenu._instand.ButtonType();
        mask.SetActive(true);
        ShowHouse._instance.Request(S_Request);
        //Camera.main.GetComponent<AssetManager>().textshow.text += "33333::::  " + S_Request;
        //"scene_id = \"20160531000141\""
        //Debug.Log(S_Request+"    "+TextUrl);
    }
    void Awake()
    {
        //isInit = true;
        _instance = this;
        TextUrl = "http://101.231.255.158:8780/jzcl/wbp";  // "http://192.168.200.173:8080/jzwq/wbp";
        CartURL = "";
    }

    void Start()
    {
        //Response
        Caching.CleanCache();
        PictureListState(false);
        _LoadDataScript = LoadDataFromList._Instand;
        GameManageList = new Dictionary<string, GameObject>();
        WindowList = new Dictionary<string, Dictionary<string, GameObject>>();
        TempDisctionary = new Dictionary<string, bool>();
        windowCamera.gameObject.SetActive(true);
        InitServerConfig._instance.m_servers.Add(WWWURL);

        Debug.Log(ReadAndLoadXml("LayoutXML/20160528000004"));
        //MsgCenter._instance.start(MsgCenter._instance.strXML("3D404638", "roomDetail", qingqiu));
        //NewReadXml.ReadLoyoutXml(temp);
        AssetManager._instans.StartInitWindow(ReadAndLoadXml("LayoutXML/20160528000005"));
    }

    public string xml = "";
    public string start(string sendBuff)
    {
        byte[] sendBuffs = Encoding.UTF8.GetBytes(sendBuff);
        StartCoroutine(MsgCenter._instance.LoadXML(sendBuffs));
        return xml;
    }
    public IEnumerator LoadXML(byte[] sendBuff)
    {
        xml = "";
        WWW www = new WWW(TextUrl, sendBuff);
        yield return www;
        xml = www.text;
        Debug.Log(xml);
        if (www.error == null)
        {
            if (www.text == "")
            {
                shopingTip.parent.gameObject.SetActive(true);
                shopingTip.FindChild("tipText").GetComponent<Text>().text = "数据访问失败！";
            }
        }
        // Camera.main.GetComponent<AssetManager>().textshow.text += "  ddddd   " + www.text; //Debug.Log( www.text);
        if (www.error != null)
        {
            shopingTip.parent.gameObject.SetActive(true);
            shopingTip.FindChild("tipText").GetComponent<Text>().text = "数据访问失败！\n" + www.error;
        }
    }

    public void Request(string qingqiu, bool isinit)
    {
        qingqiu += " corp_id=" + "\"" + MsgCenter._instance.nowHouse.Corp_ID + "\"";
        MsgCenter._instance.start(MsgCenter._instance.strXML("MM404442", "detail", qingqiu));
        StartCoroutine(LoadXMLAsset(isinit));
    }
    IEnumerator LoadXMLAsset(bool isinit)
    {
        yield return new WaitWhile(() => xml == "");



        List<CurtainManager> tempw = NewReadXml.ReplaceCurtainXml("bom_list", MsgCenter._instance.nowWidow.GetComponent<WindoManager>().GroupID, xml, true);
        if (tempw != null)
        {
            if (isinit)
            {
                Debug.Log("ddddddddddddddddddddddddddddddddddddd d" + isChuangLian);
                if (isChuangLian)
                {
                    Camera.main.GetComponent<AssetManager>().LoadAsset(tempw);
                }
                else
                {
                    Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa d" + isChuangLian);
                    Camera.main.GetComponent<AssetManager>().LoadAsset(tempw[0]);
                }
            }
            else
            {
                Camera.main.GetComponent<AssetManager>().LoadAsset(tempw);
            }
        }
    }

    public static string ReadAndLoadXml(string path)
    {
        XmlDocument doc = new XmlDocument();
        Debug.Log("加载xml文档");
        doc.Load(Application.dataPath + "/XML/"+path+".xml");
        return doc.InnerXml;
    }
    void Update()
    {
        windowCamera.fieldOfView = Camera.main.fieldOfView;
        if (TempDisctionary != null)
        {
            if (TempDisctionary.Keys.Count >= ModuleCount)
            {
                isone = false;
                int i = 0;
                foreach (bool temp in TempDisctionary.Values)
                {
                    if (!temp)
                    {
                        IsOverLode = false;
                    }
                    if (temp)
                    {
                        i++;
                    }
                }
                //Debug.Log("i = " + i + ";  ModuleCount = " + ModuleCount);
                if (i >= ModuleCount)
                {
                    IsOverLode = true;
                    UIToWindow._instance.ChangeWindow(DefaultWindo.GroupID);

                    TempDisctionary.Clear();
                    ModuleCount = 100000000;
                }
            }
            if (IsOverLode)
            {
                mask.SetActive(false);
                if (!isone)
                {
                    //MsgCenter._instance.isInit = true;
                    isone = true;
                    IsOverLode = false;
                    RefreashNowWindow();
                    if (S_RProud != "")
                    {
                        Request(S_RProud, true);
                        S_RProud = "";
                    }
                    //else
                        //MainMenu._instand.ButtonCurtains(true);
                }
            }
            if (TempDisctionary.Count == 0)
            {
                mask.SetActive(false);
            }

        }
    }
    #region Editor测试
    public void ReceiveMessage(List<NewInfomation> _Data)
    {
        PictureListState(true);
        _LoadDataScript.ReceiveMessage(_Data);
    }
    public void ReceiveMessage(List<SceneManager> _Data)
    {
        PictureListState(true);
        _LoadDataScript.ReceiveMessage(_Data);
    }
    public void ReceiveMessage(List<HouseManager> _Data)
    {
        PictureListState(true);
        _LoadDataScript.ReceiveMessage(_Data);
    }

    public void ReceiveMessage(List<CurtainManager> _Data)
    {
        PictureListState(true);
        _LoadDataScript.ReceiveMessage(_Data);
    }

    public void ReceiveMessage(List<AssetInfo> Data)
    {
        PictureListState(true);
        _LoadDataScript.ReceiveMessage(Data);
    }

    public void PictureListState(bool IsTrue)
    {
        PictureList.SetActive(IsTrue);
    }
    #endregion

    public string strXML(string func_id, string action_id, string qinqiu)
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
            func_id + "</func_id><action_id>" +
            action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" " +
            qinqiu + "/></program>";
        Debug.Log(strXMLModel);
        //Camera.main.GetComponent<AssetManager>().textshow.text += "strXml    "+strXMLModel;
        return strXMLModel;
    }
    public string btnSendXML_Click(string strXml)//object sender, Event e
    {
        //校验输入数据
        if (String.IsNullOrEmpty(TextUrl) || !TextUrl.StartsWith("http://"))
        {

            //Camera.main.GetComponent<AssetManager>().textshow.text += "     23452" + "请输入合法的地址！";
            return "0";
        }
        if (String.IsNullOrEmpty(strXml))
        {

            //Camera.main.GetComponent<AssetManager>().textshow.text += "     333" + "请输入请求XML";
            return "0";
        }
        //发送请求并处理结果

        //Debug.Log("sddsdsdwwwwww");
        string strResponse = TestUploadData.HttpUtility.SendXML(TextUrl, strXml);
        return "";
    }


    #region Web版
    public void ReceiveAllMessage(string data)
    {
        //TODO:解析XML数据(主要确定传递数据类型)
        //

    }

    /// <summary>
    /// 接受到js传递的message （窗帘信息）
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveCurtainMessage(string data)
    {
        //TODO:解析XML数据(主要确定传递数据类型)
        CurtainDataList = ReadXml.ReadInfo(1, data);
        PictureList.SetActive(true);
        //_LoadDataScript.ReceiveMessage(CurtainDataList);
    }

    /// <summary>
    /// 接受到js传递的message （整体信息）
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveWholeMessage(string data)
    {
        //TODO:解析XML数据(主要确定传递数据类型)

        //传递消息给_LoadDataScript
    }

    /// <summary>
    /// 接受到js传递的message （装饰风格信息）
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveStyleMessage(string data)
    {
        //TODO:解析XML数据(主要确定传递数据类型)

        //传递消息给_LoadDataScript
    }

    /// <summary>
    /// 接受到js传递的message （Assetbundle信息）
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveAssetMessage(string data)
    {
        //TODO:解析XML数据(主要确定传递数据类型)

        //传递消息给AssetManager(换单个bundle)
    }

    /// <summary>
    /// 接受到js传递的message （Assetbundles信息）
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveAssetsMessage(string data)
    {
        //TODO:解析XML数据(主要确定传递数据类型)

        //传递消息给AssetManager(换多个bundle)
    }

    #endregion


    #region 确定换装对象

    /// <summary>
    /// 改变窗帘对象
    /// </summary>
    /// <param name="target"></param>
    public void ChangeTarget(ProdKind target)
    {
        if (target == Target)
        {
            //return;
        }
        Target = target;

        if (target == ProdKind.ChuangLian)
        {
            if (!isActiv)
            {

            }
        }
        else
        {
            #region switch
            Debug.Log(target.ToString());
            switch (target)
            {
                case ProdKind.ChuangMan: FindName = "1";
                    break;
                case ProdKind.ChuangShen: FindName = "2";
                    break;
                case ProdKind.ChuangSha: FindName = "3";
                    break;
                case ProdKind.GuiDao: FindName = "4";
                    break;
                case ProdKind.ChuangYing: FindName = "5";
                    break;
                case ProdKind.ZhangTu: FindName = "6";
                    break;
                case ProdKind.DaiShi: FindName = "7";
                    break;
                case ProdKind.HuaBian: FindName = "8";
                    break;
                case ProdKind.ChuangJin: FindName = "9";
                    break;
                case ProdKind.CeGou: FindName = "10";
                    break;
                case ProdKind.BengDai: FindName = "11";
                    break;
                case ProdKind.ChuangGou: FindName = "12";
                    break;
                case ProdKind.ChuangDai: FindName = "13";
                    break;
                case ProdKind.PeiZhong: FindName = "14";
                    break;
                default: FindName = null;
                    break;
            }
            #endregion

            GameObject Go = null;
            GameManageList.TryGetValue(FindName, out Go);
            //Debug.Log("Go3");
            this.Go = Go;
            //Debug.Log("Go == null ? " + (Go == null) + ";   FindName: " + FindName);
            //Debug.Log("ddddddddddddd" + Go.name);
            if (Go != null)
            {
                _changeTexture = Go.GetComponent<ChangeTexture>();
            }
            else
            {
                _changeTexture = null;
            }
        }

    }

    /// <summary>
    ///窗帘中的组件状态
    /// </summary>
    /// <param name="target"></param>
    /// <param name="IsTrue"></param>
    public void ChangeTargetState(ProdKind target, bool IsTrue)
    {
        //ChangeMaterialRecord();
        if (target == Target)
        {
            //return;
        }
        Target = target;

        if (target != ProdKind.ChuangLian)
        {
            GetState(target, IsTrue);
        }
    }

    private void ChangeMaterialRecord()
    {
        foreach (KeyValuePair<string, GameObject> targetState in GameManageList)
        {
            GameObject Go;
            GameManageList.TryGetValue(targetState.Key, out Go);
            MaterialRecord(System.Int32.Parse(Go.name), Go);
            Debug.Log("Go1");
        }
    }

    private void MaterialRecord(int ID, GameObject Go)
    {
        int num = Go.GetComponent<MeshRenderer>().materials.Length;
        Debug.Log(string.Format("name is {0}   num is {1}", Go.name, num));
        Material[] m_Material = new Material[num];
        m_Material = Go.GetComponent<MeshRenderer>().materials;


        //   MaterialAlpha_[ID] = m_Material;

        //MaterialAlpha_[ID] = new Material[num];
        //for (int i = 0; i < num; i++)
        //{`        
        //    //MaterialAlpha_[ID][i] = m_Material[i];
        Debug.Log("执行的次数");
        //}
    }
    private void GetState(ProdKind target, bool IsTrue)
    {
        foreach (KeyValuePair<string, GameObject> targetState in GameManageList)
        {
            GameObject Go;
            GameManageList.TryGetValue(targetState.Key, out Go);
            //Debug.Log(targetState.Value.name + "      " + ((int)target).ToString());

            //this.Go = Go;
            //Debug.Log("Go2");

            //isOn==true
            if (IsTrue)
            {
                if (targetState.Value != null)
                {
                    if (targetState.Value.name == ((int)target).ToString())
                    {
                        //Debug.Log("恢复1");
                        //Debug.Log("targetState.Value.name == ((int)target).ToString()");
                        Go.GetComponent<MeshRenderer>().material = Go.GetComponent<CurtainManager>().Material;
                        // Go.GetComponent<MeshRenderer>().material = MaterialAlpha;
                    }
                    else
                    {
                        ///Debug.Log("恢复2");
                        //Debug.Log("targetState.Value.name != ((int)target).ToString()");
                        int num = Go.GetComponent<MeshRenderer>().materials.Length;
                        Material[] m_Material = new Material[num];
                        if (Go.GetComponent<CurtainManager>().IsModel)
                        {
                            for (int i = 0; i < num; i++)
                            {
                                m_Material[i] = MaterialAlpha;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < num; i++)
                            {
                                m_Material[i] = MaterialAlpha_;
                            }
                        }
                        Go.GetComponent<MeshRenderer>().materials = m_Material;
                        // Debug.Log(string.Format("   改变 ：   {0}", num));
                    }
                }
            }
            else//isOn==false
            {
                //Debug.Log("IsTrue==false");
                if (targetState.Value != null)
                {
                    if (targetState.Value.name == ((int)target).ToString())
                    {
                        //Debug.Log("恢复3");

                        Go.GetComponent<MeshRenderer>().material = Go.GetComponent<CurtainManager>().Material;
                    }
                    else
                    {
                        //Debug.Log("恢复4");
                        int num = Go.GetComponent<MeshRenderer>().materials.Length;
                        Material[] m_Material = new Material[num];
                        for (int i = 0; i < num; i++)
                        {
                            m_Material[i] = Go.GetComponent<CurtainManager>().Material;
                            //
                            //Debug.Log("ss"+Go.transform.parent.parent.parent.name);
                        }
                        //SingleShow._instance.UIButton.GetChild(0).GetComponent<ShowPictureList>().ChangeUIButton();
                        Go.GetComponent<MeshRenderer>().materials = m_Material;
                        // Debug.Log(string.Format(" wei    改变 ：   {0}", num));
                        // Go.GetComponent<MeshRenderer>().materials = MaterialAlpha_[System.Int32.Parse(Go.name)];
                    }
                }
            }
            //Debug.Log(string.Format("{0}  is  {1}",Go.name,Go.transform.parent.parent.parent.name ));

        }
    }

    //void OnGUI()
    //{
    //    if (GUI.Button(new Rect(100, 100, 100, 100), "sssssssss"))
    //    {
    //        StartCoroutine(show());
    //    }
    //}
    public IEnumerator show()
    {
        WWWForm header = new WWWForm();
        header.headers.Add("Cookie", sessionID);
        WWW www = new WWW(TextUrl, header);
        yield return www;
        Debug.Log(www.text);
        if (www.isDone && www.responseHeaders.Count > 0)
        {
            foreach (KeyValuePair<string, string> entry in www.responseHeaders)
            {
                Debug.Log(entry.Key + "     ::::::::     " + entry.Value);
            }
        }

    }

    public void ReceiveWebMessage(string msg)
    {
        //Camera.main.GetComponent<AssetManager>().textshow.text += "     " +msg;
    }


    public void RefreashNowWindow()
    {
        Transform tra = nowWidow;
        //nowWidow = null;
        //Debug.Log("RefreashNowWindow:" + tra == null);
        nowWidow = tra;
    }

    /// <summary>
    /// 改变装饰风格对象
    /// </summary>
    /// <param name="target"></param>
    public void ChangeTarget(TargetStyle target)
    {
        StyleTarget = target;
    }

    /// <summary>
    /// 给装饰风格换装
    /// </summary>
    /// <param name="texture"></param>
    public void ChangeStyle(Texture texture)
    {
        if (StyleTarget == null)
        {
            return;
        }
        switch (StyleTarget)
        {
            case TargetStyle.chuanghu:
                chuanghu.GetComponent<Renderer>().material.mainTexture = texture;
                break;
            case TargetStyle.qiangmian:
                qiangmian.GetComponent<Renderer>().material.mainTexture = texture;
                break;
            case TargetStyle.diban:
                diban.GetComponent<Renderer>().material.mainTexture = texture;
                break;
            case TargetStyle.diaoding:
                diaoding.GetComponent<Renderer>().material.mainTexture = texture;
                break;
            default:
                break;
        }
    }

    #endregion


    #region Dictionary控制
    public void AddInfomation(string KeyName, string Name, GameObject obj)
    {
        if (WindowList[KeyName].ContainsKey(Name)) return;
        //Debug.Log(KeyName+"  ccccccccccccccccccccccccc");
        WindowList[KeyName].Add(Name, obj);
    }


    public void CleanList()
    {
        if (WindowList != null)
        {
            foreach (Dictionary<string, GameObject> item in WindowList.Values)
            {
                //if (item != null)
                //DestroyImmediate(item[(i + 1).ToString()]);
            }
            WindowList.Clear();
        }
    }

    public void CleanAllList(Transform Window)
    {
        if (Window != null)
        {
            foreach (GameObject item in GameManageList.Values)
            {
                DestroyImmediate(item);
            }
            WindowList[Window.name].Clear();
        }
    }

    public bool RemoveValue(string Name)
    {
        if (GameManageList.ContainsKey(Name))
        {
            DestroyImmediate(GameManageList[Name].gameObject);
            GameManageList.Remove(Name);
            return true;
        }
        else
        {
            return false;
        }
    }


    public void AddWindowList(string Name, Dictionary<string, GameObject> CurtainList)
    {
        if (WindowList == null)
            Debug.Log("sssssss");
        if (WindowList.ContainsKey(Name)) return;

        WindowList.Add(Name, CurtainList);
        //Debug.Log(Name);
    }

    #endregion
}

public enum TargetKind
{
    Zhengti,
    Chuangman,
    Chuangshen,
    Chuangsha,
    Guidao,
    Chuagnying,
    Zhangtu,
    Daishi,
    Huabian,
    Chuanjin,
    Cegou,
    Bengdai,
    Chuanggou,
    Chuangdai,
    Peizhou,
    Null
}


public enum TargetStyle
{
    [Description("103")]
    diaoding = 0,
    [Description("101")]
    diban,
    [Description("102")]
    qiangmian,
    chuanghu,
    Null
}

public enum LookTarget
{
    huxing,
    zhuangxiu,
    chuanglian
}




