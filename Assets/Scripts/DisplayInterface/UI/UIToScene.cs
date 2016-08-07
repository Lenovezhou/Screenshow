using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIToScene : MonoBehaviour
{
    public string Path;

    public Toggle GameSearch;//更换完成图片或者是bundle之后，出现搜索图片覆盖//
    AssetManager Asset;
    public string filePath;
    public static UIToScene _instand;
    SceneManager Scene;
    private Vector3 vec3;
    private Vector3 pos;
    public bool isDrop;
    // Use this for initialization
    void Awake()
    {
        Scene = this.GetComponent<SceneManager>();
        Asset = Camera.main.GetComponent<AssetManager>();
        _instand = this;
        filePath = MsgCenter._instance.WWWURL + "LoadWindow.xml";
    }
    void Start()
    {
        GameSearch.group = transform.parent.GetComponent<ToggleGroup>();
        //Asset.StartInitWindow(MsgCenter._instance.WWWURL + "LoadWindow.xml");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        if (GameSearch.isOn&&!isDrop)
        {
            InitServerConfig._instance.m_downLoader.Clear();
            MsgCenter._instance.nowScene = this.GetComponent<SceneManager>();
            MsgCenter._instance.SceneID = (int)this.GetComponent<SceneManager>().ID;
            if (MsgCenter._instance.lookTarget == LookTarget.zhuangxiu)
            {
                MsgCenter._instance.StyleTarget = TargetStyle.Null;
                LoadDataFromList._Instand.DisableStyle();
            }
            else if (MsgCenter._instance.lookTarget == LookTarget.chuanglian)
            {
                MsgCenter._instance.Target = ProdKind.Null;
                LoadDataFromList._Instand.DisableCurtain();
            }


            MsgCenter._instance.LookIcon.SetParent(this.transform);
            MsgCenter._instance.LookIcon.anchoredPosition = Vector2.zero;
            MsgCenter._instance.LookIcon.localEulerAngles = Vector3.zero;
            string qingqiu = "scene_id=" + "\"" + MsgCenter._instance.nowHouse.ID + "\"" + " room_id=" + "\"" + MsgCenter._instance.nowScene.ID + "\"";
            MsgCenter._instance.start(MsgCenter._instance.strXML("3D404638", "roomDetail", qingqiu));
            //NewReadXml.ReadLoyoutXml(temp);
            //StartCoroutine( Asset.StartInitWindow());


            //ClickEven(Scene.Idx);
            //if (filePath == string.Empty || filePath == null) return;
            //StartCoroutine(LoadXML(filePath));
            //MsgCenter._instance.Target = TargetKind.Null;
        }
    }

    IEnumerator LoadXML(string filePath)
    {
        WWW www = new WWW(filePath);
        yield return www;
    }

    public void ClickEven(int ClickId)
    {
        //SceneManager temp = LoadDataFromList._Instand.SceneGameObjectPool[ClickId].GetComponent<SceneManager>();
        MsgCenter._instance.QiuURL = Scene.QiuURL;
        Asset.LoadQiuTexture(MsgCenter._instance.QiuURL);
        Debug.Log(Scene.WindowID[0]);
    }
    public void MoveObject()
    {
        isDrop = true;
        Vector3 off = Input.mousePosition - vec3;
        vec3 = Input.mousePosition;
        pos = pos + off;
        transform.GetComponent<RectTransform>().position = pos;
    }
    public void PointerDown()
    {
        vec3 = Input.mousePosition;
        pos = transform.GetComponent<RectTransform>().position;
    }
    public void PointerUp()
    {
        //isDrop = false;
    }
    void OnDisable()
    {
        //Debug.Log("禁用");
        GameSearch.isOn = false;
    }

}
