using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class EditUIControl : MonoBehaviour {

    public InputField I_Name;
    public InputField I_Code;
    public InputField I_Group;
    public InputField I_Sequ;
    public Text T_Picture;
    public Text T_Spicture;
    public Text T_Bpicture;
    public Text T_Audio;
    public Dropdown D_FengGe;
    public Dropdown D_SceneStyle;

    public List<GameObject> UIControl;
    int caseid;
    MsgCenter MsgCenter;
    private bool isClick;
    private string xmlStr="";
	// Use this for initialization
    void OnEnable()
    {
        if (MsgCenter.UIShowStr != "")
        {
            string[] iii = MsgCenter.UIShowStr.Split('_');

            for (int i = 0; i < iii.Length; i++)
            {
                UIControl[int.Parse(iii[i])].SetActive(false);
            }
            MsgCenter.UIShowStr = "";
        }
    }
    void Awake()
    {
        MsgCenter = Camera.main.GetComponent<MsgCenter>();

        T_Spicture.text = "";
        T_Picture.text = "";
        T_Bpicture.text = "";
        T_Audio.text = "";
    }
	void Start ()
    {

        foreach (string temp in this.MsgCenter.FengGe.Keys)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = temp;

            D_FengGe.options.Add(op);
        } 
        foreach (string temp in this.MsgCenter.SceneStyle.Keys)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = temp;

            D_SceneStyle.options.Add(op);
        }
	}
	// Update is called once per frame
	void Update ()
    {
        if (isClick)
        {
            switch (MsgCenter.insertType)
            {
                case "1":
                    if (D_FengGe.value == 0 || D_SceneStyle.value == 0)
                        break;
                    xmlStr = InsertData.AddScene(I_Name.text,I_Code.text,I_Sequ.text,MsgCenter.corpID,MsgCenter.FengGe[D_FengGe.captionText.text],MsgCenter.SceneStyle[D_SceneStyle.captionText.text],T_Picture.text,T_Spicture.text,T_Audio.text);
                    break;
                case "2":
                    if ( MsgCenter.nowHouse == null || MsgCenter.nowScenePoint==null)
                        break;
                    xmlStr = InsertData.AddRoom(I_Name.text, I_Code.text, MsgCenter.nowHouse.ID.ToString(), MsgCenter.corpID, MsgCenter.nowScenePoint.x.ToString(), MsgCenter.nowScenePoint.y.ToString(), T_Picture.text, T_Spicture.text);
                    break;
                case "3":
                    if (D_FengGe.value == 0 || D_SceneStyle.value == 0 || MsgCenter.nowScene == null || MsgCenter.nowHouse ==null || MsgCenter.StyleTarget == TargetStyle.chuanghu || MsgCenter.StyleTarget == TargetStyle.Null)
                        break;
                    xmlStr = InsertData.AddPanorama(I_Name.text, I_Code.text, I_Sequ.text, I_Group.text, MsgCenter.FengGe[D_FengGe.captionText.text], EnumToolV2.GetDescription(MsgCenter.StyleTarget), MsgCenter.nowHouse.ID.ToString(),MsgCenter.nowScene.ID.ToString(), MsgCenter.corpID, T_Picture.text, T_Spicture.text, T_Bpicture.text);
                    break;
                case "4":

                    break;
                case "5":

                    break;
                case "6":

                    break;
                case "7":

                    break;
                default:
                    break;
            }
            if(xmlStr==""||xmlStr==null)
                this.gameObject.SetActive(false);
            else
                StartCoroutine(WaitXML());
            isClick = false;
        }
	}
    /// <summary>
    /// 接收html消息
    /// </summary>
    /// <param name="url"></param>
    public void OnMessegeURL(string url)
    {
        // NewReadXml.ReadFile(url);
        AssetManager._instans.textshow.text += "" + url;
        switch (caseid)
        {
            case 1:
                T_Spicture.text = url;
                break;
            case 2:
                T_Picture.text = url;
                break;
            case 3:
                T_Bpicture.text = url;
                break;
            case 4:
                T_Audio.text = url;
                break;
            default:
                break;
        }
    }
    //解析返回的xml
    IEnumerator WaitXML()
    {
        yield return new WaitUntil(() => xmlStr != "");
        Debug.Log(xmlStr);
        //Camera.main.GetComponent<AssetManager>().textshow.text = " 购物车提交的数据：：：：   " + WriteXml._Instand.xmlStr;
        //向服务器发送数据
        if (xmlStr != "" && xmlStr != null)
        {
            MsgCenter.start(xmlStr);
            StartCoroutine(LoadXML());
        }
        if (MsgCenter.nowWidow == null)
        {
            MsgCenter.shopingTip.parent.gameObject.SetActive(true);
            MsgCenter.shopingTip.FindChild("tipText").GetComponent<Text>().text = "请先选择您心怡的那一套！";
        }

        xmlStr = "";
    }
    IEnumerator LoadXML()
    {
        yield return new WaitWhile(() => MsgCenter.xml == "");
        string erro = NewReadXml.Result(MsgCenter.xml);
        //Camera.main.GetComponent<AssetManager>().textshow.text += "   购物车返回的提交信息  " + MsgCenter.xml;
        MsgCenter.shopingTip.parent.gameObject.SetActive(true);
        Text erroText = MsgCenter.shopingTip.FindChild("tipText").GetComponent<Text>();
        //判断返回的值来确定是否上传成功
        if (erro != "")
        {
            erroText.text = erro;
        }
        else
        {
            erroText.text = "未知错误，联系管理员！";
            Debug.Log("上传失败");
        }
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 1-缩略图、2-资源文件、3-大图、4-音频文件
    /// </summary>
    /// <param name="i"></param>
    public void ClickBrowes(int i)
    {
        caseid = i;
        //调用JS
        Application.ExternalCall("fucShowFileDlg");
    }


    //提交
    public void ClickSumbit()
    {
        isClick = true;
    }
    //关闭
    public void ClickCloss()
    {
        this.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        foreach (GameObject item in UIControl)
        {
            item.SetActive(true);
        }
    }
}
