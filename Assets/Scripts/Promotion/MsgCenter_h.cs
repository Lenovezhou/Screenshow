using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MsgCenter_h : MonoBehaviour {
    public class InitModelParam 
    {
        //  浏览模式下的模板列表
        public List<GameObject> pageModels;
        public List<GameObject> cataModels;
        public List<GameObject> coverModels;
        //  编辑模式下模板列表
        public List<GameObject> editPages;
        public List<GameObject> editCatas;
        public List<GameObject> editCovers;
        //  编辑模式下模板按钮列表
        public List<GameObject> editPageBtns;
        public List<GameObject> editCataBtns;
        public List<GameObject> editCoverBtns;
        public InitModelParam(ref List<GameObject> pPages, ref List<GameObject> pCatalogues,
            ref List<GameObject> pCovers, ref List<GameObject> pEditPages, ref List<GameObject> pEditCatas,
            ref List<GameObject> pEditCovers, ref List<GameObject> pEditPageBtns,ref List<GameObject> pEditCataBtns,
            ref List<GameObject> pEditCoverBtns) 
        {
            //Debug.Log("gouzao:" + pPageAssetBundles.Length);
            pageModels = pPages;
            cataModels = pCatalogues;
            coverModels = pCovers;

            editPages = pEditPages;
            editCatas = pEditCatas;
            editCovers = pEditCovers;

            editCataBtns = pEditCataBtns;
            editCoverBtns = pEditCoverBtns;
            editPageBtns = pEditPageBtns;
        }
    }

    public string btnUrl;

    public string prod_id;
    public string scense_id;
    public Text TestText;
    public static MsgCenter_h Instance;

    public GameObject[] RenderCanvas;   //  所有模板的Canvas
    public GameObject CoverControll;

    public EditCoverEvent editCoverSc;
    public EditPageEvent editPageSc;
    public EditCataEvent editCataSc;
    public RedactControll redactControll;

    public MessageBox messageBox;

    private Model _converModelSc;
    

	// Use this for initialization
	void Awake () {
        Instance = this;
        _converModelSc = CoverControll.GetComponent<CoverModelControll>();
	}

    void Start() 
    {
        
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void OnInitModelMsg(InitModelParam param) 
    {
        //Debug.Log("OnInitModelMsg: page.count = " + param.pageModels.Count + "\n"
        //    + "cata.count = " + param.cataModels.Count + "\n" +
        //    "cover.count = " + param.coverModels.Count + "\n" +
        //    "editPages.count = " + param.editPages.Count + "\n" +
        //    "editCatas.count = " + param.editCatas.Count + "\n" +
        //    "editCover.count = " + param.editCovers.Count);
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\n------------------------\n OnInitModelMsg: page.count = " + param.pageModels.Count + "\n" +
            "cata.count = " + param.cataModels.Count + "\n" +
            "cover.count = " + param.coverModels.Count + "\n" +
            "editPages.count = " + param.editPages.Count + "\n" +
            "editCatas.count = " + param.editCatas.Count + "\n" +
            "editCover.count = " + param.editCovers.Count + "\n-------------------\n";
        //  浏览模式下 内页和目录的模板初始化
        for (int i = 0; i < RenderCanvas.Length; i++)
        {
            RenderCanvas[i].GetComponent<PageControl>().Init(param);
        }
        //  浏览模式下 封面的模板初始化
        _converModelSc.Init(param.coverModels,null);
        //  编辑模式下 封面、目录、内页模板的初始化
        editCoverSc.Init(param);
        editCataSc.Init(param);
        editPageSc.Init(param);
        OnMessage("url=http://192.168.200.173:8080/jzwq/wbp&gourl=http://192.168.200.173:8080/jzwq/mo/curtain/JZ3D.jsp&scene_id=302151012&prod_id=1052555");
    }

    public void EditPageMsg(MessageParam msg) 
    {
        switch (msg.message)
        {
            case "addPageSuccess":
                editPageSc.AddPageSuccess(messageBox,msg);
                break;
            case "addPageLoser":
                editPageSc.AddPageLoser(messageBox,msg);
                break;
        }
    }

    public void ChooseGoodMsg(string goodID) 
    {
        
    }

    public void OnMessage(string msg) 
    {
        //Debug.Log("OnMessage");
        TestText.text += "OnMessage\n";
        Dictionary<string, string> dic = ToDic(msg);
        //if(dic.ContainsKey("url"))
        //{
        //    GetXml.Instances.TextUrl = dic["url"];
        //    int n = GetXml.Instances.TextUrl.LastIndexOf('/');
        //    GetXml.Instances.LoadUrl = GetXml.Instances.TextUrl.Substring(0,n);
        //}
        if (dic.ContainsKey("gourl"))
        {
            btnUrl = dic["gourl"];
        }
        else 
        {
            TestText.text += "not key gourl\n";
        }
        if (dic.ContainsKey("scene_id"))
        {
            scense_id = dic["scene_id"];
        }
        else 
        {
            scense_id = "";
        }
        if (dic.ContainsKey("prod_id"))
        {
            prod_id = dic["prod_id"];
        }
        else 
        {
            prod_id = "";
        }
        BookManager._isntance.MsgInit();
        TestText.text += "   OnMessage is Over\n";
    }

    public Dictionary<string, string> ToDic(string str)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        string[] ss = str.Split('&');
        string[] sa;
        if (ss.Length > 1)
        {
            for (int i = 0; i < ss.Length; i++)
            {
                //Debug.Log(i + ": " + ss[i]);

                sa = ss[i].Split('=');
                if (sa.Length == 2)
                {
                    dic.Add(sa[0], sa[1]);
                    //Debug.Log(sa[0] + ": " + sa[1]);
                }
                else if (sa.Length > 2)
                {
                    string url = "";
                    for (int j = 1; j < sa.Length; j++)
                    {
                        url += sa[j];
                        if(j < sa.Length - 1)
                        {
                            url += "=";
                        }
                    }
                    dic.Add(sa[0], url);
                    //Debug.Log(url);
                }
            }
        }
        else 
        {
            int n = str.IndexOf('=');
            dic.Add(str.Substring(0,n).Trim(),str.Substring(n,str.Length - n).Trim());
        }
        return dic;
    }
}
