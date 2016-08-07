using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DataBase;
using System.Collections.Generic;
public class ClickPicture : MonoBehaviour, IPointerClickHandler
{
    public float Ratio;
    public RawImage SelfImage;
    public Texture SelfTexture;
    // public Color BlueColor = Color.blue;    //点击时的色调
    private Color SelfColor;
    //public string UerTextureUrl;
    public CurtainManager myCurtain;  //最好是一个公用的父类
    public AssetInfo myInfo;
    public NewInfomation styleInfo;
    private Texture ToTexture;
    private Image MyImage;

    public Toggle GameSearch;//更换完成图片或者是bundle之后，出现搜索图片覆盖//
    public static ClickPicture _instance;
    void Awake()
    {
        //isInit = true;
        _instance = this;
        //Camera.main.GetComponent<AssetManager>().textshow.text +="11111::::  "+ S_Request;
    }

    //装饰
    public void setValue(NewInfomation data)
    {
        styleInfo = data;
        myInfo = null;
        myCurtain = null;
        //Debug.Log((myInfo == null) + "222222222222222222222");
    }
    //单个替换
    public void setValue(CurtainManager data)
    {
        myCurtain = gameObject.AddComponent<CurtainManager>();
        myCurtain.InitCurtain(data);
        //myCurtain = data;
        myInfo = null;
        //Debug.Log((myInfo == null) + "333333333333333333333333");
    }
    //整体替换
    public void setValue(AssetInfo data)
    {
        //print("执行了没有啊");
        myInfo = data;
        myCurtain = null;
        //Debug.Log((myInfo == null) +"jjjjjjjjjjjjjjjjjjjj");
    }


    void Start()
    {
        // MyImage = GetComponent<Image>();
        //SelfColor = this.GetComponent<Image>().color;
        SelfImage = this.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
        GameSearch.group = transform.parent.GetComponent<ToggleGroup>();
    }

    #region Editor

    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(string.Format("执行的次数"));

        //SelfTexture = SelfImage.mainTexture;
        //ChangeCloth(SelfTexture);
    }

    //改变成toggle//
    public void ChangeToggle()
    {
        if (GameSearch.isOn)
        {
            Debug.Log(" dsdsds  " + this.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text);
            // MyImage.color = BlueColor;
            SelfTexture = SelfImage.mainTexture;
            ChangeCloth(SelfTexture);
            Debug.Log("内部执行");
        }
        Debug.Log("out  外部");
    }

    void OnDisable()
    {
        //Debug.Log("禁用");
        GameSearch.isOn = false;
    }

    IEnumerator LoadPicture(string path)
    {
        Debug.Log("oeoeoeoeoeo:" + path);
        WWW www = new WWW(path);
        yield return www;

        ToTexture = www.texture;
        //Mesh mesh = MsgCenter._instance.AssetMesh;
        //if (ToTexture != null)
            //MsgCenter._instance._changeTexture.ReceiveMessage(mesh, ToTexture);
    }
    
    void ChangeCloth(Texture texture)
    {

        //if (myCurtain == null && myInfo == null)
        {
            if (MsgCenter._instance.lookTarget == LookTarget.zhuangxiu)
            {
                if (MsgCenter._instance.StyleTarget != TargetStyle.Null)
                {
                    if (MsgCenter._instance.StyleTarget != TargetStyle.chuanghu)
                    {
                        MsgCenter._instance.ChangeStyle(texture);
                        //InitServerConfig._instance.m_downLoader.Clear();
                        MsgCenter._instance.QiuURL = new string[3];
                        for (int i = 0; i < MsgCenter._instance.QiuURL.Length; i++)
                        {
                            if (i == (int)MsgCenter._instance.StyleTarget)
                            {
                                MsgCenter._instance.QiuURL[i] = styleInfo.URL;
                            }
                            else
                            {
                                MsgCenter._instance.QiuURL[i] = "";
                            }
                            //LoadPictureManager._instance.ReceiveTextureUrl(MsgCenter._instance.WWWURL + UerTextureUrl);
                        }
                        AssetManager._instans.LoadQiuTexture(MsgCenter._instance.QiuURL);
                    }
                    else
                    {
                        MsgCenter._instance.ChangeStyle(texture);
                        StartCoroutine(AssetManager._instans.loadWindowPicture(MsgCenter._instance.WWWURL + styleInfo.URL, MsgCenter._instance.chuanghu.transform));
                        //AssetManager._instans.loadwindowpicture(styleInfo.URL, MsgCenter._instance.chuanghu.transform);
                        MsgCenter._instance.chuanghu.transform.localScale = new Vector3(float.Parse(styleInfo.L) + 0.6f, float.Parse(styleInfo.H) + 0.6f, float.Parse(styleInfo.W));
                    }
                }
            }
            else if (MsgCenter._instance.lookTarget == LookTarget.chuanglian)
            {
                if (myCurtain != null)
                {
                    //如果不是模型
                    if (myCurtain.IsModel == false)
                    {
                        if (MsgCenter._instance._changeTexture != null)
                        {
                            //StartCoroutine(LoadPicture(MsgCenter._instance.WWWURL + myCurtain.Icon));
                            //MsgCenter._instance.Go.GetComponent<CurtainManager>().InitCurtain(myCurtain);
                            //Texture t = SelfImage.mainTexture;
                        }
                        else
                        {
                            //if (MsgCenter._instance.StyleTarget != TargetStyle.Null)
                            //{
                            //    MsgCenter._instance.ChangeStyle(texture);
                            //}
                        }
                    }
                    //如果是模型
                    else
                    {
                        Debug.Log(myCurtain.IsModel+" d  d d d  d d d d ");
                        Camera.main.GetComponent<AssetManager>().LoadAsset(myCurtain);
                    }
                }
                if (myInfo != null)
                {
                    Debug.Log("111111");
                    if (myInfo.isModle)
                    {
                        Debug.Log("22222222");
                        SingleShow._instance.SetButtonState(true);
                        //MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("Plane").gameObject.SetActive(false);
                        //MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("GD_Middle01").gameObject.SetActive(true);
                        //MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("GD_UP01").gameObject.SetActive(true);
                        string qingqiu =  " prod_id=" + "\"" + myInfo.ProdId + "\"";
                        //Debug.Log("1213154687956413212657894651321");
                        MsgCenter._instance.Request(qingqiu,false);
                    }
                    else
                    {
                        Debug.Log("33333333333");
                        SingleShow._instance.SetButtonState(false);
                        MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("GD_UP01").gameObject.SetActive(false);
                        MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("GD_Middle01").gameObject.SetActive(false);
                        //AllPicture(myInfo.DefaultTexture);
                    }
                    //Debug.Log(myInfo.ModelPath.Count);
                }

            }
        }
    }

    void AllPicture(string path)
    {
            AssetBundle ab = Resources.Load(path) as AssetBundle;
            AssetBundleRequest abr = ab.LoadAssetAsync<Texture>("窗帘1111");
            Texture t = abr.asset as Texture;
            MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("Plane").GetComponent<Renderer>().material.mainTexture = t;
            MsgCenter._instance.nowWidow.transform.GetChild(1).FindChild("Plane").gameObject.SetActive(true);
            ab.Unload(false);
    }


    #endregion

    #region WebPlayer

    void JSConmunication(string message)
    {
        //TODO:解析传递的message
        Application.ExternalCall("ChangeCloth", message);
    }
    #endregion

}
