using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class UpService : GetXml {
    public Dictionary<int, string> CoverTemplate;
    public Dictionary<int, string> CatalogueTemplate;
    public Dictionary<int, string> PageTemplate;

    public delegate void RequestCallback(RequestParam param);

    public class RequestParam
    {
        public byte[] Request;   //  请求的字节数组
        public RequestCallback callBack;
        public Promotion book;

        public Page page;

        public string Response;   //  响应的字符串

        public UnityEngine.Object Param;

        public RequestParam(string pRequestStr, RequestCallback pCallBack)
        {
            callBack = pCallBack;
            Request = Encoding.UTF8.GetBytes(pRequestStr); ;
        }
        /// <summary>
        /// 添加书的构造
        /// </summary>
        /// <param name="pRequestStr"></param>
        /// <param name="pCallBack"></param>
        public RequestParam(string pRequestStr, RequestCallback pCallBack,Promotion pbook,BookProfebSc psc)
        {
            callBack = pCallBack;
            Request = Encoding.UTF8.GetBytes(pRequestStr); ;
            book = pbook;
            Param = psc;
        }
        /// <summary>
        /// 添加内页构造
        /// </summary>
        /// <param name="pRequestStr"></param>
        /// <param name="pCallBack"></param>
        /// <param name="pPage"></param>
        public RequestParam(string pRequestStr, RequestCallback pCallBack, Page pPage)
        {
            callBack = pCallBack;
            Request = Encoding.UTF8.GetBytes(pRequestStr); ;
            page = pPage;
        }
    }


    public static UpService UpInstance;

    private bool _addingBook;  //  正在添加书
    private bool _addingPage;  //  正在添加内页 或 目录

    private XmlDocument _newBookRequestXml;
    private XmlDocument _newPageRequestXml;
    private XmlDocument _newLeyoutReqXml;
	// Use this for initialization
	void Awake ()
    {
        _addingBook = false;
        _addingPage = false;
	    UpInstance = this;
        Debug.Log("_newBookRequestXml == null ?" + (_newBookRequestXml == null));

        _newBookRequestXml = GetXmlRequest("NewBrochureRequest");
        _newPageRequestXml = GetXmlRequest("NewPageRequest");
        _newLeyoutReqXml = GetXmlRequest("NewLeyoutTemplate");
        Init();
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public  XmlDocument GetXmlRequest(string xmlName) 
    {
        string data = Resources.Load("Xml/" + xmlName).ToString();
        //Debug.Log("GetXmlRequest:" + data);
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(data);
        return xml;
    }

    public void Init() 
    {
        //  初始化封面模板字典：
        string reqStr = GetTemplateRequest("3D404741");

        StartCoroutine(Request(new RequestParam(reqStr, LoadCovTmpCallback)));
    }

    public void LoadCovTmpCallback(RequestParam param) 
    {
        XmlElement xmlRoot = GetXmlElement(param.Response);
        CoverTemplate = new Dictionary<int, string>();
        //Debug.Log(Encoding.UTF8.GetString(param.Request));
        Debug.Log(":->>>>>>>>>>>1");
        foreach (XmlNode node in xmlRoot["brochure_list"].ChildNodes)
        {
            CoverTemplate.Add(int.Parse(node.Attributes["brochure_code"].Value),
                node.Attributes["brochure_id"].Value);
        }
        //  初始化【内页】模板字典：
        string reqStr = GetTemplateRequest("3D404745");
        StartCoroutine(Request(new RequestParam(reqStr,LoadPageTmpCallback)));
    }

    public void LoadPageTmpCallback(RequestParam param)
    {
        XmlElement xmlRoot = GetXmlElement(param.Response);
        PageTemplate = new Dictionary<int, string>();
        Debug.Log(":->>>>>>>>>>>2");
        foreach (XmlNode node in xmlRoot["page_list"].ChildNodes)
        {
            PageTemplate.Add(int.Parse(node.Attributes["page_code"].Value),
                node.Attributes["page_id"].Value);
        }
        //  初始化【目录】模板字典：
        string reqStr = GetTemplateRequest("3D404743");
        StartCoroutine(Request(new RequestParam(reqStr, LoadCataTmpCallback)));
    }

    public void LoadCataTmpCallback(RequestParam param)
    {
        XmlElement xmlRoot = GetXmlElement(param.Response);
        CatalogueTemplate = new Dictionary<int, string>();
        Debug.Log(":->>>>>>>>>>>3");
        foreach (XmlNode node in xmlRoot["catalog_list"].ChildNodes)
        {
            CatalogueTemplate.Add(int.Parse(node.Attributes["catalog_code"].Value),
                node.Attributes["catalog_code"].Value);
            Debug.Log(node.Attributes["catalog_code"].Value + ":" + node.Attributes["catalog_code"].Value);
        }
    }
    /// <summary>
    /// 添加书请求的xml参数处理
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="book"></param>
    /// <returns></returns>
    public string AddBookRequest(XmlDocument xml,Promotion book) 
    {
        XmlElement xmlRoot = xml.DocumentElement;
        Debug.Log(CoverTemplate == null);
        xmlRoot["parameter"].Attributes["brochure_templet_id"].Value = CoverTemplate[book.SelfCover.ModelNumber];
        xmlRoot["parameter"].Attributes["brochure_code"].Value = (++GetXml.Instances.maxCode).ToString();
        //Debug.Log(GetXml.Instances.maxCode);
        Debug.Log(":=->" + book.CataTempNum);
        xmlRoot["parameter"].Attributes["catalog_templet_id"].Value = CatalogueTemplate[book.CataTempNum];
        return xml.InnerXml;
    }
    /// <summary>
    /// 添加内页请求的xml参数处理
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    public string AddPageRequest(XmlDocument xml, Page page)
    {
        XmlElement xmlRoot = xml.DocumentElement;
        xmlRoot["parameter"].Attributes["brochure_id"].Value = RedactControll.Instance.Book.ID;
        xmlRoot["parameter"].Attributes["page_name"].Value = page.Name;
        xmlRoot["parameter"].Attributes["page_templet_id"].Value = PageTemplate[page.ModelNumber];
        xmlRoot["parameter"].Attributes["page_code"].Value = (++RedactControll.Instance.Book.maxPageCode).ToString();
        //xmlRoot["parameter"].Attributes["page_code"].Value = "1031";
        xmlRoot["parameter"].Attributes["sequ"].Value = (RedactControll.Instance.Book.PageList[RedactControll.Instance.Book.PageList.Count - 1].Sequ + 1).ToString();
        //Debug.Log(":--==>>" + xmlRoot["parameter"].Attributes["page_code"].Value + "\n---=>" +
        //    RedactControll.Instance.Book.ID);
        return xml.InnerXml;
    }

    public void UpdataLeyoutPicture(string leyoutId) 
    {
        Debug.Log("UpdataPagePicture");
    }

    public void UpdataTextLeyout(string leyoutId,string newText)
    {
        Debug.Log("UpdataTextLeyout");
    }

    public void UpdataPageText(string leyoutId) 
    {
        Debug.Log("UpdataPageText");
    }

    public void UpdataPageModle(int modleNum)
    {

    }

    public void UpdataPageAudio(string pageId) 
    {
        
    }

    public void UpdataPageTab(int newSequ) 
    {
        
    }

    public void UpdataPageName(string pageId,string newName) 
    {
        
    }

    public void AddPage(Page page) 
    {
        string xmlStr = AddPageRequest(_newPageRequestXml,page);
        RequestParam param = new RequestParam(xmlStr,AddPageCallback,page);
        _addingPage = true;
        StartCoroutine(Request(param));
    }

    public void AddBook(Promotion book,BookProfebSc sc)
    {
        string request = AddBookRequest(_newBookRequestXml,book);
        Debug.Log(request);
        RequestParam param = new RequestParam(request,AddBookCallback,book,sc);
        _addingBook = true;
        StartCoroutine(Request(param));
    }

    public IEnumerator Request(RequestParam param)
    {
        WWW www = new WWW(TextUrl,param.Request);
        yield return www;
        if (www.isDone && www.error == null)
        {
            param.Response = www.text;
            //Debug.Log(TextUrl);
            //Debug.Log(www.text);
            param.callBack(param);
        }
        else
        {
            Debug.Log("err:" + www.error == null ? "www not done.":www.error);
        }
    }

    public bool IsAddingBook() { return _addingBook; }
    /// <summary>
    /// 添加书的请求的回调
    /// </summary>
    /// <param name="param"></param>
    public void AddBookCallback(RequestParam param) 
    {
        XmlElement xmlRoot = GetXmlElement(param.Response);
        Debug.Log(param.Response);
        param.book.ID = xmlRoot["data"].Attributes["brochure_id"].Value;
        (param.Param as BookProfebSc).ID = param.book.ID;
        _addingBook = false;
    }
    /// <summary>
    /// 添加内页回调
    /// </summary>
    /// <param name="param"></param>
    public void AddPageCallback(RequestParam param)
    {
        XmlElement xmlRoot = GetXmlElement(param.Response);
         Debug.Log(param.Response);
         if (xmlRoot["err_flag"].InnerText == "1")
         {
             //Debug.Log(xmlRoot["err_flag"].Value + ":" + xmlRoot["err_msg"].Value);
             param.page.ID = xmlRoot["page_id"].Attributes["page_id"].Value;
             MsgCenter_h.Instance.EditPageMsg(new MessageParam("addPageSuccess", xmlRoot["err_msg"].InnerText));
         }
         else
         {
             //Debug.Log(xmlRoot["err_flag"].Value + ":" + xmlRoot["err_msg"].Value);
             MsgCenter_h.Instance.EditPageMsg(new MessageParam("addPageLoser", xmlRoot["err_msg"].InnerText));
         }
        _addingPage = false;
    }

    public void AddLeyouts(Dictionary<string,UiItem> itemList) 
    {
        
    }

    public void DeletePage(string pageId)
    {
        Debug.Log("删除：" + pageId);
    }

    public void UpdataCoverModel(int newModelNum) 
    {
        
    }

    public void AddLeyout() 
    {
        
    }
}
