using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DataBase;
using System.IO;

public class AssetManager : MonoBehaviour
{
    public Text textshow;
    public Transform QIU;

    public Transform ParentUp;
    public Transform ParentMiddle;
    public Transform Parent2D;

    public Material[] temp;
    public List<Window> windowData = new List<Window>();
    public Dictionary<string, GameObject> WindowList = new Dictionary<string, GameObject>();
    //List<Curtain> CurtainData = new List<Curtain>();
    List<string> server = new List<string>();

    bool isInit;
    List<CurtainManager> AssetInfo;//整体窗帘的信息
    CurtainManager CurtainData;//单组件窗帘信息

    public static AssetManager _instans;

    List<HouseManager> _Data;
    bool _isInit;
    WWW m_SaveWWW;
    string XmlPath = "";
    string[] AssetList;
    bool IsInit;
    MsgCenter MsgCenter;

    void Awake()
    {
        _instans = this;
    }

    void Start()
    {
        MsgCenter = MsgCenter._instance;
        XmlPath = MsgCenter.WWWURL + "LoadWindow.xml";
        //StartInitWindow(new string[] { "0" }, XmlPath);
        //StartCoroutine(InitHouse(MsgCenter.WWWURL + "House.xml"));
        //StartCoroutine(InitScene(MsgCenter.WWWURL + "Scene.xml", 0));
        _isInit = true;
        
    }

    private IEnumerator InitScene(string p, int ID)
    {
        WWW www = new WWW(p);
        yield return www;
        List<SceneManager> SceneData = ReadXml.ReadSceneXml(ID, www.text);

        LoadDataFromList._Instand.ReceiveMessage(SceneData);
    }

    IEnumerator InitHouse(string XML)
    {
        WWW www = new WWW(XML);
        yield return www;
        _Data = ReadXml.ReadHouseXml(www.text);
        StartCoroutine(LoadMap(MsgCenter._instance.WWWURL + _Data[0].Map));
    }

    public IEnumerator LoadMap(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        MsgCenter._instance.Map.texture = www.texture;
    }
    /// <summary>
    /// 初始化整个场景
    /// </summary>
    /// <param name="Path"></param>
    public void StartInitWindow(string xml)
    {
        List<WindoManager> item = NewReadXml.ReadLoyoutXml(xml);
        Debug.Log(MsgCenter._instance.xml);
        LoadQiuTexture(MsgCenter._instance.QiuIcon);
        LoadQiuTexture(MsgCenter._instance.QiuURL);
        LoadWindow(item);
    }
    //public IEnumerator StartInitWindow()
    //{
    //    yield return new WaitWhile(() => MsgCenter._instance.xml == "");
    //    List<WindoManager> item = NewReadXml.ReadLoyoutXml(MsgCenter.xml);
    //    Debug.Log(MsgCenter._instance.xml);
    //    LoadQiuTexture(MsgCenter._instance.QiuIcon);
    //    LoadQiuTexture(MsgCenter._instance.QiuURL);
    //    LoadWindow(item);
    //}
    /// <summary>
    /// 加载默认场景的XML
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>


    /// <summary>
    /// 动态创建窗户
    /// </summary>
    /// <param name="data"></param>
    void LoadWindow(List<WindoManager> data)
    {

        if (WindowList.Count != 0)
        {
            foreach (GameObject temp in WindowList.Values)
            {
                DestroyImmediate(temp);
            }
            WindowList.Clear();
        }

        IsInit = true;

        Debug.Log(data.Count);
        foreach (WindoManager item in data)
        {
            //根据信息生成window
            //如果是窗户
            MsgCenter.DefaultWindo = item;
            if (item.ModleType == 102)
            {
                GameObject WholeCurtain = (GameObject)Instantiate(Resources.Load("WholeCurtain"));
                WholeCurtain.transform.parent = QIU;
                WholeCurtain.name = item.GroupID;
                //WholeCurtain.transform.localScale = item.Scale;
                WindowList.Add(item.GroupID, WholeCurtain);
                Debug.Log(item.GroupID+"     wwwwwwwwwwwww");
                WholeCurtain.transform.localEulerAngles = new Vector3(item.Rotation.x, item.Rotation.y, item.Rotation.z);
                WholeCurtain.transform.localPosition = new Vector3(item.Position.x, item.Position.y, item.Position.z);
                Transform chuanghu = WholeCurtain.transform.FindChild("chaunghu");
                chuanghu.localScale = item.Scale;
                chuanghu.parent.localScale = item.OfferScale;
                string path = MsgCenter.WWWURL + item.WindowPictureUrl;
                WindoManager wm = WholeCurtain.GetComponent<WindoManager>();
                wm.InitWindow(item);
                Parent2D = wm.TwoD;
                ParentMiddle = wm.Middle;
                ParentUp = wm.Up;
                StartCoroutine(loadWindowPicture(path, chuanghu));
                foreach (WindoManager item2 in data)
                {
                    if (item.GroupID == item2.GroupID && item2.ModleType == 101)
                    {
                        WholeCurtain.transform.FindChild("guadian").localScale = item2.Scale;

                        //if (item.Sequ == "0" && MsgCenter.isSingleInit)
                        //{
                        //    item2.Prod_ID = MsgCenter.S_RProud;
                        //    MsgCenter.isSingleInit = false;
                        //}

                        //string qingqiu = "corp_id=" + "\"" + MsgCenter._instance.nowHouse.Corp_ID + "\"" + " prod_id=" + "\"" + item2.Prod_ID + "\"";
                        //string temp = MsgCenter._instance.start(MsgCenter._instance.strXML("MM404442", "detail", qingqiu));
                        LoadXML(item2, MsgCenter.ReadAndLoadXml("ProduceXML/"+item2.Prod_ID));
                    }
                }
            }
            //不是窗户（家具）
            else
            {
            }

        }
        //Debug.Log(MsgCenter.DefaultWindo.GroupID+"     "+  WindowList[MsgCenter.DefaultWindo.GroupID].transform.name + "     wwwwwwwwwwwww");
            if (WindowList.ContainsKey(MsgCenter.DefaultWindo.GroupID))
                Camera.main.GetComponent<UseCamareController>().Target1.LookAt(WindowList[MsgCenter.DefaultWindo.GroupID].transform);
    }
    void  LoadXML(WindoManager item2,string xml)
    {
        //yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        List<CurtainManager> tempw = NewReadXml.ReadCurtainXml(item2.GroupID, xml);
        Load(tempw);
    }
    //IEnumerator LoadXML(WindoManager item2)
    //{
    //    yield return new WaitWhile(() => MsgCenter._instance.xml == "");
    //    List<CurtainManager> tempw = NewReadXml.ReadCurtainXml(item2.GroupID, MsgCenter.xml);
    //    Load(tempw);
    //}

    /// <summary>
    /// 初始化窗户时替换贴图
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    /// <returns></returns>

    public void loadwindowpicture(string path, Transform go)
    {
        CurtainManager bss=new CurtainManager() ;

        InitServerConfig._instance.m_assetLoader.StartDownload(server,"",path,go,null,eDownloadType.Type_AssetBundle,OnLoadUpdateZipComplete4, OnLoadFaile4, true);
    }

    private void OnLoadUpdateZipComplete4(object data, object item)
    {
        Transform chuanghu = item as Transform;
        Debug.Log(chuanghu.name);
        //chuanghu.GetComponent<Renderer>().material.mainTexture = data.texture;
    }

    private void OnLoadFaile4(object data, object item)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator loadWindowPicture(string path, Transform go)
    {
        WWW www = new WWW(path);
        yield return www;
        if (www.texture != null && www.isDone)
        {
            //mater[0] = new Material((Shader.Find("StandardSSS")));
            //mater[0].mainTexture = www.texture;
            //Debug.Log(www.texture.height);
            go.GetComponent<Renderer>().material.mainTexture = www.texture;
        }
    }
    /// <summary>
    /// bundel替换的时候换贴图
    /// </summary>
    /// <param name="path"></param>
    /// <param name="go"></param>
    /// <returns></returns>
    IEnumerator loadCurtainPicture(string path, Transform go)
    {

        WWW www = new WWW(path);
        yield return www;
        if (www.texture != null && www.isDone)
        {
            //Debug.Log("Width" + www.texture.width + "   Height  " + www.texture.height);
            //float Ratio =(float)www.texture.width / (float)www.texture.height;
            //Debug.Log("Ration "+Ratio);
            if (go != null)
            {
                go.GetComponent<MeshRenderer>().material.mainTexture = www.texture;
                go.GetComponent<CurtainManager>().Material.mainTexture = www.texture;
            }
        }
    }

    public void LoadQiuTexture(string[] data)
    {
        server = InitServerConfig.Instance.m_servers;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] != "" && data[i]!=null)
            {
                InitServerConfig._instance.m_downLoader.StartDownload(server, "", data[i], i.ToString(), null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete0, OnLoadFaile0, true);
            }
        }
    }

    private void OnLoadUpdateZipComplete0(object data, string item)
    {
        Debug.Log("成功111" + item);
        Debug.Log(data);
        Texture t = data as Texture;
        if(data!=null)
            MsgCenter.Qiu[int.Parse(item)].GetComponent<Renderer>().material.mainTexture = t;
    }
    private void OnLoadFaile0(object data, string item)
    {

    }


    #region 换单个Assetbundle

    /// <summary>
    /// 换单个AssetBundle
    /// </summary>
    /// <param name="AssetPath">AssetBundle路径</param>
    public void LoadAsset(CurtainManager data)
    {
        CurtainData = data;
        server = InitServerConfig.Instance.m_servers;
        string item = data.ModuleName;
        //Debug.Log(data.ModelUrl);
        InitServerConfig._instance.m_downLoader.StartDownload(server, "", data.ModleURL, item, null, eDownloadType.Type_AssetBundle, OnLoadUpdateZipComplete, OnLoadFaile, true);
    }

    private void OnLoadUpdateZipComplete(object data, string item)
    {
        AssetBundle ab = data as AssetBundle;
       // GameObject obj = ab.LoadAsset(item) as GameObject;
        GameObject go=null;
        Object[] objs = ab.LoadAllAssets();
        foreach (var obj in objs)
        {
            if (obj is GameObject)
            {
                go = (GameObject)Instantiate(obj);
                break;
            }
        }
        go.layer = 11;
        foreach (Transform child in go.transform)
        {
            child.gameObject.layer = 11;
        }
        go.AddComponent<MeshCollider>();
        MsgCenter.Go = go;
        string Name = ((int)EnumToolV2.GetEnumName<ProdKind>(CurtainData.ModuleType)).ToString();
        go.name = Name;
        //清楚当前字典的该组件的信息
        MsgCenter.RemoveValue(Name);

        MsgCenter.AddInfomation(MsgCenter.nowWidow.name, Name, go);
        /*为窗帘赋值*/
        CurtainManager temp = null;
        if (go.GetComponent<CurtainManager>() == null)
        {
            temp = go.AddComponent<CurtainManager>();
            //temp.ModuleType = Name;
        }
        temp.Material = go.GetComponent<MeshRenderer>().material;
        temp.InitCurtain(CurtainData);
        go.GetComponent<MeshRenderer>().material = temp.Material;
        if (temp.TextureURL != "")
            StartCoroutine(loadCurtainPicture(InitServerConfig.Instance.m_servers[0] + temp.TextureURL, go.transform));
        /*         */
        if (item.Contains("UP"))
        {
            go.transform.parent = ParentUp;
        }
        else if (item.Contains("Middle"))
        {
            go.transform.parent = ParentMiddle;
        }
        if (go.name == "8" || go.name == "14"||go.name=="3")
        {
            Material[] Alpha_Material = new Material[1];
            Alpha_Material[0] = new Material(Resources.Load<Shader>("Alpha-Diffuse"));
            go.GetComponent<MeshRenderer>().materials = Alpha_Material;
            go.GetComponent<CurtainManager>().Material = Alpha_Material[0];
        }
        go.transform.localPosition = Vector3.zero;
        go.transform.localEulerAngles = Vector3.zero;
        go.transform.localScale = Vector3.one;
        if (go.GetComponent<ChangeTexture>() == null)
        {
            go.AddComponent<ChangeTexture>();
        }

        if (MsgCenter.Target != ProdKind.Null)
        {
            MsgCenter._changeTexture = go.GetComponent<ChangeTexture>();
        }
        RefreshWinCompoment(go, false);
        ab.Unload(false);
    }


    public void RefreshWinCompoment(GameObject go, bool waitAllLoad)
    {
        //  刷新Mesh列表
        switch (go.name)
        {
            case "2":
                if (go != null)
                {
                    SingleShow._instance.windowCompoments[0] = go;
                    SingleShow._instance.windowMeshs[0] = go.GetComponent<MeshFilter>().mesh;
                    if(go.transform.childCount>0)
                        SingleShow._instance.windowSubMeshs[0] = go.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                    if (!waitAllLoad) SingleShow._instance.SetCurComAcitev(SingleShow._instance._isClose);
                }

                break;
            case "11":
                if (go != null)
                {
                    SingleShow._instance.windowCompoments[2] = go;
                    Debug.Log("asdf");
                    SingleShow._instance.windowMeshs[2] = go.GetComponent<MeshFilter>().mesh;
                    if (go.transform.childCount > 0)
                    SingleShow._instance.windowSubMeshs[2] = go.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                    SingleShow._instance.SetCurComAcitev(SingleShow._instance._isClose);
                }

                break;
            case "8":
                if (go != null)
                {
                    SingleShow._instance.windowCompoments[1] = go;
                    SingleShow._instance.windowMeshs[1] = go.GetComponent<MeshFilter>().mesh;
                    if (go.transform.childCount>0)
                    SingleShow._instance.windowSubMeshs[1] = go.transform.GetChild(0).GetComponent<MeshFilter>().mesh;
                    if (!waitAllLoad) SingleShow._instance.SetCurComAcitev(SingleShow._instance._isClose);
                }


                break;
            default:
                break;
        }
    }

    private void OnLoadFaile(object obj, string item)
    {

    }

    #endregion


    #region 换多个Assetbundle

    /// <summary>
    ///  整体替换Assetbundle
    /// </summary>
    /// <param name="AssetArray">Assetbundle路径数组</param>
    public void LoadAsset(List<CurtainManager> AssetArray)
    {
        this.AssetInfo = AssetArray;
        MsgCenter.CleanAllList(MsgCenter.nowWidow);
        server = InitServerConfig.Instance.m_servers;

        foreach (CurtainManager curtain in AssetArray)
        {
            //if (curtain.IsModel == false)
            //{
            //    //如果不是模型，直接加载面片 还贴图

            //    continue;
            //}
            //else
            {
                //如果是模型，加载assetbundle 
                string AssetPath = curtain.ModleURL;
                InitServerConfig._instance.m_assetLoader.StartDownload(server, "", AssetPath, curtain, null,eDownloadType.Type_AssetBundle, OnLoadUpdateZipComplete2, OnLoadFaile2, true);
            }
        }
    }

    private void OnLoadUpdateZipComplete2(object obj, object item)
    {
        MsgCenter.mask.SetActive(true);
        if (item is CurtainManager)
        {
            //Debug.Log(obj.ToString());
            CurtainManager curtain = item as CurtainManager;
            string ModelPath = curtain.ModleURL;
            //Debug.Log(ModelPath);
            string ModelID = curtain.ModuleName;
            AssetBundle ab = obj as AssetBundle;
            //Debug.Log(ModelID);
            GameObject go = null;
            Object objc = ab.LoadAsset(curtain.ModuleName);
            //foreach (var temp in objc)
            //{
            //    if (temp is GameObject)
            //    {
            //        
            //        break;
            //    }
            //}
            go = (GameObject)Instantiate(objc);
            go.layer = 11;
            foreach (Transform child in go.transform)
            {
                child.gameObject.layer = 11;
            }
            //Debug.Log(go.name);
            string Name = ((int)EnumToolV2.GetEnumName<ProdKind>(curtain.ModuleType)).ToString();
            go.name = Name;

            if (curtain.IsModel)
            {
                if (curtain.TextureURL != "")
                {
                    StartCoroutine(loadCurtainPicture(InitServerConfig.Instance.m_servers[0] + curtain.TextureURL, go.transform));
                }
                if (ModelID.Contains("UP"))
                {
                    go.transform.parent = ParentUp;
                }
                else if (ModelID.Contains("Middle"))
                {
                    go.transform.parent = ParentMiddle;
                }
                go.transform.localPosition = Vector3.zero;
            }
            else
            {
                go.transform.parent = Parent2D;
                Debug.Log("Name:: "+go.name);
                if (go.name == "1")
                {
                    Debug.Log("Name:: " + go.name);
                    go.transform.localPosition = new Vector3(0,0.3f,0);
                }
                else if (go.name == "2")
                {
                    Debug.Log("Name:: " + go.name);
                    go.transform.localPosition = new Vector3(0, 0.2f, 0);
                }
                else if (go.name == "3")
                {
                    Debug.Log("Name:: " + go.name);
                    go.transform.localPosition = new Vector3(0, 0f, 0);
                }
            }

            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            curtain.Material = go.GetComponent<MeshRenderer>().material;
            //Debug.Log(curtain.TextureURL);

            int num = go.GetComponent<MeshRenderer>().materials.Length;
            Material[] m_Material = new Material[num];
            for (int i = 0; i < num; i++)
            {
                m_Material[i] = curtain.Material;

            }
            go.GetComponent<MeshRenderer>().materials = m_Material;

            go.AddComponent<MeshCollider>();

            if (go.GetComponent<CurtainManager>() == null)
            {
                CurtainManager temp = go.AddComponent<CurtainManager>();
                //curtain.ModuleType = Name;
            }
            //Debug.Log(curtain.Group_ID+"    d ddddddddddddddddddddddddddddddddd");
            //if (!MsgCenter.WindowList[MsgCenter.nowWidow.name].ContainsKey(Name))
            MsgCenter.AddInfomation(curtain.Group_ID, Name, go);//WindowList[MsgCenter.nowWidow.name].Add(Name, go);
            MsgCenter.TempDisctionary[curtain.Group_ID + curtain.ModleURL.Split('.')[0]] = true;
            //初始化组件上的窗帘组件信息
            go.GetComponent<CurtainManager>().InitCurtain(curtain);

            if (go.GetComponent<ChangeTexture>() == null)
            {
                go.AddComponent<ChangeTexture>();
            }

            RefreshWinCompoment(go, true);
            ab.Unload(false);
        }
    }

    private void OnLoadFaile2(object data, string item)
    {

    }

    #endregion


    #region 初始化该场景的窗户

    /// <summary>
    /// 初始化场景里所有窗户，包括窗户下的窗帘
    /// </summary>
    public void Load(List<CurtainManager> data)
    {
        server = InitServerConfig.Instance.m_servers;

        MsgCenter.OldData.Clear();  
        foreach (CurtainManager curtain in data)
        {
            //if (curtain.IsModel == false)
            //{
            //    //如果不是模型，直接加载面片 还贴图

            //    continue;
            //}
            //else
            {
                //如果是模型，加载assetbundle 
                string AssetPath = curtain.ModleURL;

                InitServerConfig._instance.m_assetLoader.StartDownload(server, "", AssetPath, curtain, null, eDownloadType.Type_AssetBundle, OnLoadUpdateZipComplete3, OnLoadFaile3, true);
            }
        }
    }



    private void OnLoadUpdateZipComplete3(object obj, object item)
    {
        //MsgCenter.CleanList();
        MsgCenter.mask.SetActive(true);
        if (item is CurtainManager)
        {
            //Debug.Log(obj.ToString());
            CurtainManager curtain = item as CurtainManager;
            //string ModelPath = curtain.ModleURL;
            //Debug.Log(ModelPath);
            string ModelID = curtain.ModuleName;
            AssetBundle ab = obj as AssetBundle;
            //Debug.Log(ModelID);
            GameObject go = null;
            //Debug.Log(curtain.ModuleName);
            Object objc = ab.LoadAsset(curtain.ModuleName);
            //foreach (var temp in objc)
            //{
            //    if (temp is GameObject)
            //    {
            //        
            //        break;
            //    }
            //}
            go = (GameObject)Instantiate(objc);
            go.layer = 11;
            foreach (Transform child in go.transform)
            {
                child.gameObject.layer = 11;
            }
            //Debug.Log(go.name);
            string Name = ((int)EnumToolV2.GetEnumName<ProdKind>(curtain.ModuleType)).ToString();
            go.name = Name;
            //Debug.Log(curtain.TextureURL);
            curtain.Material = go.GetComponent<MeshRenderer>().material;
            int num = go.GetComponent<MeshRenderer>().materials.Length;
            Material[] m_Material = new Material[num];
            for (int i = 0; i < num; i++)
            {
                m_Material[i] = curtain.Material;

            }
            go.GetComponent<MeshRenderer>().materials = m_Material;

            go.AddComponent<MeshCollider>();

            if (go.GetComponent<CurtainManager>() == null)
            {
                CurtainManager temp = go.AddComponent<CurtainManager>();
                //curtain.ModuleType = Name;
            }
            if (!MsgCenter.WindowList[curtain.Group_ID].ContainsKey(Name))
                MsgCenter.WindowList[curtain.Group_ID].Add(Name, go);
            //Debug.Log(" True::   "+curtain.Group_ID + curtain.ModleURL.Split('.')[0]);
            MsgCenter.TempDisctionary[curtain.Group_ID + curtain.ModleURL.Split('.')[0]] = true;
            //初始化组件上的窗帘组件信息
            go.GetComponent<CurtainManager>().InitCurtain(curtain);
            //MsgCenter.OldData.Add(go.GetComponent<CurtainManager>());
            if (curtain.IsModel)
            {
                if (curtain.TextureURL != "")
                {
                    StartCoroutine(loadCurtainPicture(InitServerConfig.Instance.m_servers[0] + curtain.TextureURL, go.transform));
                }
                if (ModelID.Contains("UP"))
                {
                    if (IsInit)
                        go.transform.parent = WindowList[curtain.Group_ID].GetComponent<WindoManager>().Up;
                    else
                        go.transform.parent = ParentUp;
                }
                else if (ModelID.Contains("Middle"))
                {
                    if (IsInit)
                        go.transform.parent = WindowList[curtain.Group_ID].GetComponent<WindoManager>().Middle;
                    else
                        go.transform.parent = ParentUp;
                }
                go.transform.localPosition = Vector3.zero;
            }
            else
            {
                go.transform.parent = Parent2D;
                Debug.Log("Name:: " + go.name);
                if (go.name == "1")
                {
                    Debug.Log("Name:: " + go.name);
                    go.transform.localPosition = new Vector3(0, 0.3f, 0);
                }
                else if (go.name == "2")
                {
                    Debug.Log("Name:: " + go.name);
                    go.transform.localPosition = new Vector3(0, 0.2f, 0);
                }
                else if (go.name == "3")
                {
                    Debug.Log("Name:: " + go.name);
                    go.transform.localPosition = new Vector3(0, 0f, 0);
                }
            }

            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localScale = Vector3.one;
            //if (ModelID.Contains("UP"))
            //{
            //    if (IsInit)
            //        go.transform.parent = WindowList[curtain.Group_ID].GetComponent<WindoManager>().Up;
            //    else
            //        go.transform.parent = ParentUp;
            //}
            //else if (ModelID.Contains("Middle"))
            //{
            //    if (IsInit)
            //        go.transform.parent = WindowList[curtain.Group_ID].GetComponent<WindoManager>().Middle;
            //    else
            //        go.transform.parent = ParentUp;
            //}
            //if (go.name == "8" || go.name == "14" || go.name == "3")
            //{
            //    Material[] Alpha_Material = new Material[1];
            //    Alpha_Material[0] = new Material(Resources.Load<Shader>("Alpha-Diffuse"));
            //    go.GetComponent<MeshRenderer>().materials = Alpha_Material;
            //    go.GetComponent<CurtainManager>().Material = Alpha_Material[0];
            //}
            if (go.GetComponent<ChangeTexture>() == null)
            {
                go.AddComponent<ChangeTexture>();
            }
            ab.Unload(false);
        }
    }


    private void OnLoadFaile3(object data, object item)
    {

    }

    #endregion



}
