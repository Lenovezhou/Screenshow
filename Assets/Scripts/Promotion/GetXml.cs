using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TestUploadData;
using System.IO;
using System.Xml;
using System.Text;



public class GetXml : MonoBehaviour {

    public class XmlParam 
    {
        public string func_id;
        public string action_id;
        public string request;

        public XmlCallBack callBack;
        public string xmlStr;
        public Promotion book;
        public bool requestIsPage;
        public string PageID;
        public XmlParam(string pFunc_id, string pAction_id, string pRequest, XmlCallBack pcallBack,Promotion pbook) 
        {
            func_id = pFunc_id;
            action_id = pAction_id;
            request = pRequest;
            callBack = pcallBack;
            book = pbook;
        }

        public XmlParam(string pFunc_id, string pAction_id, string pRequest, XmlCallBack pcallBack, Promotion pbook,string pPageID)
        {
            func_id = pFunc_id;
            action_id = pAction_id;
            request = pRequest;
            callBack = pcallBack;
            book = pbook;
            PageID = pPageID;
        }

        public XmlParam(string pFunc_id, string pAction_id, string pRequest, XmlCallBack pcallBack)
        {
            func_id = pFunc_id;
            action_id = pAction_id;
            request = pRequest;
            callBack = pcallBack;
        }
    }
    public delegate void XmlCallBack(XmlParam param);
    public List<Promotion> Book;
    public List<Promotion> _book;

    public List<int> TemplateOfItems;  //  目录每页项的个数

    public string TextUrl;
    public string LoadUrl;
    public Text TextTest;
    public static GetXml Instances;

    public bool loadedFirst;
    
    public const string FRONT_CODE = "0";   //  前封底图片编码
    public const string BEHIND_CODE = "1";  //  后封底图片编码

    public string filePath;   //  

    public int maxCode;   //  宣传册最大编码

    public string[] files;

    private bool _loadingPageLeyout;
    private bool _loadingBookLeyout;
	// Use this for initialization
	void Awake () {
        Instances = this;
        Book = null;
        _book = null;
        loadedFirst = false;
        _loadingPageLeyout = false;
        _loadingBookLeyout = false;
        StartCoroutine(LoadBundle());
        
	}

    void Start() 
    {
        
    }

	// Update is called once per frame
	void Update () {
	    
	}

    public IEnumerator LoadBundle()
    {
        List<AssetBundle> bundles = new List<AssetBundle>();

        string request = GetTemplateRequest("3D404741");
        //  获取宣传册封面模板信息
        WWW www = new WWW(TextUrl,Encoding.UTF8.GetBytes(request));
        yield return www;
        
        if (www.isDone && www.error == null)
        {
            XmlElement xml = GetXmlElement(www.text);
            WWW ww;
            foreach (XmlNode node in xml["brochure_list"].ChildNodes)
            {
                if (node.Attributes["brochure_file_url"] != null)
                {
                    ww = new WWW(LoadUrl + node.Attributes["brochure_file_url"].Value);
                    yield return ww;
                    if (ww.isDone && ww.error == null)
                    {
                        //Debug.Log("1:" + ww.url + "; " + node.Attributes["brochure_file_url"].Value);
                        bundles.Add(ww.assetBundle);
                    }
                    else 
                    {
                        Debug.Log("ww err:" + (ww.error == null ? "ww is'nt Done!" : ww.error));
                    }
                }
            }
        }
        else 
        {
            Debug.Log("www err:" + (www.error == null ? "www is'nt Done!":www.error));
        }

        request = GetTemplateRequest("3D404743");
        //  获取宣传册目录模板信息
        www = new WWW(TextUrl, Encoding.UTF8.GetBytes(request));
        yield return www;
        
        if (www.isDone && www.error == null)
        {
            XmlElement xml = GetXmlElement(www.text);
            WWW ww;
            foreach (XmlNode node in xml["catalog_list"].ChildNodes)
            {
                if (node.Attributes["catalog_file_url"] != null)
                {
                    ww = new WWW(LoadUrl + node.Attributes["catalog_file_url"].Value);
                    yield return ww;
                    if (ww.isDone && ww.error == null)
                    {
                        //Debug.Log("2:" + ww.url + "; " + node.Attributes["catalog_file_url"].Value);
                        bundles.Add(ww.assetBundle);
                    }
                    else
                    {
                        Debug.Log("ww err:" + (ww.error == null ? "ww is'nt Done!" : ww.error));
                    }
                }
            }
        }
        else
        {
            Debug.Log("err:" + (www.error == null ? "www is'nt Done!" : www.error));
        }

        request = GetTemplateRequest("3D404745");
        //  获取宣传册封内页板信息
        www = new WWW(TextUrl, Encoding.UTF8.GetBytes(request));
        yield return www;
        //Debug.Log("LoadBundle->" + www.text);
        if (www.isDone && www.error == null)
        {
            XmlElement xml = GetXmlElement(www.text);
            foreach (XmlNode node in xml["page_list"].ChildNodes)
            {
                if (node.Attributes["page_file_url"] != null)
                {
                    WWW ww = new WWW(LoadUrl + node.Attributes["page_file_url"].Value);
                    yield return ww;
                    if (ww.isDone && ww.error == null)
                    {
                        //Debug.Log("3:" + ww.url + "; " + node.Attributes["page_file_url"].Value);
                        bundles.Add(ww.assetBundle);
                    }
                    else
                    {
                        Debug.Log("ww err:" + (ww.error == null ? "ww is'nt Done!" : ww.error));
                    }
                }
            }
        }
        else
        {
            Debug.Log("err:" + (www.error == null ? "www is'nt Done!" : www.error));
        }
        ParseBundle(bundles);
    }

    public void ParseBundle(List<AssetBundle> bundles) 
    {
        //  浏览模式下的模板列表
        List<GameObject> pageModels = new List<GameObject>();
        List<GameObject> cataModels = new List<GameObject>();
        List<GameObject> coverModels = new List<GameObject>();
        //  编辑模式下模板列表
        List<GameObject> editPages = new List<GameObject>();
        List<GameObject> editCatas = new List<GameObject>();
        List<GameObject> editCovers = new List<GameObject>();

        //  编辑模式下模板按钮列表
        List<GameObject> editPageBtns = new List<GameObject>();
        List<GameObject> editCataBtns = new List<GameObject>();
        List<GameObject> editCoverBtns = new List<GameObject>();

        UnityEngine.Object[] objs;
        for (int i = 0; i < bundles.Count; i++)
        {
            Debug.Log(bundles.Count);
            objs = bundles[i].LoadAllAssets();
            for (int j = 0; j < objs.Length; j++)
            {
                if (objs[j].name.Contains("epm"))
                {
                    editPages.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("bpm"))
                {
                    pageModels.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("ecm"))
                {
                    editCovers.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("bcm"))
                {
                    coverModels.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("bdm"))
                {
                    cataModels.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("edm"))
                {
                    editCatas.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("pvm"))
                {
                    editPageBtns.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("pcm"))
                {
                    editCoverBtns.Add(objs[j] as GameObject);
                }
                else if (objs[j].name.Contains("pdm"))
                {
                    editCoverBtns.Add(objs[j] as GameObject);
                }
                //Debug.Log(j + ":" + objs[j].name);
            }
		}

        //  给Camera MsgCenter_h 发消息，初始化所有模板
        MsgCenter_h.Instance.OnInitModelMsg(
            new MsgCenter_h.InitModelParam(ref pageModels, ref cataModels, ref coverModels,
                ref editPages,ref editCatas,ref editCovers,ref editPageBtns,ref editCataBtns,
                ref editCoverBtns)
            );
    }

    public string strXML(string func_id, string action_id, string qinqiu)
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
            func_id + "</func_id><action_id>" +
            action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " +
            " empower__=\"1\" "+qinqiu + "/></program>";
        //Debug.Log(strXMLModel);
        //Debug.Log(strXMLModel);
        return strXMLModel;
    }

    public string GetTemplateRequest(string func_id)
    {
        return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                        "<program>" +
                        "<func_id>" + func_id + "</func_id>" +
                        "<action_id>all</action_id>" +
                        "<parameter uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\"/>" +
                        "</program>";

    }

    public string btnSendXML_Click(string strXml,XmlParam xmlParam)//object sender, Event e
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
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "   strXml: " + strXml + "\n";
        //发送请求并处理结果

        //Debug.Log("sddsdsdwwwwww");
        //string strResponse = TestUploadData.HttpUtility.SendXML(TextUrl, strXml);
        byte[] sendBuff = Encoding.UTF8.GetBytes(strXml);

        start(TextUrl, sendBuff, xmlParam);
        //Debug.Log(TextUrl);
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "   TextUrl: " + TextUrl + "\n";
        return "";
    }

    public string xml = "";
    public string start(string path, byte[] sendBuff,XmlParam xmlParam)
    {
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "     startXML";
        StartCoroutine(LoadXML(path, sendBuff, xmlParam));
        return xml;
    }

    public IEnumerator LoadXML(string path, byte[] sendBuff,XmlParam xmlParam)
    {
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nLoadXML" + path;
        WWW www = new WWW(path, sendBuff);
        //Debug.Log("LoadXML:" + www.url);
        //while (www.progress <= 0.9f)
        {
            
        }
        
        yield return www;
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nprogroess:" + www.progress + "\n";
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "  www.errorfdfdfdfdfdfd      ";
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "  www.error      " + www.isDone; //Debug.Log( www.text);
        if (www.error != null)
        {
            Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nwww.error " + www.error;
        }
        if (www.error == null & www.isDone)
        {
            xmlParam.xmlStr = www.text;
            xmlParam.callBack(xmlParam);
            www.Dispose();
        }
        
        //Debug.Log(xml);
    }


    public void GetTarXml(XmlParam param)
    {
        //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nGetTarXml(XmlParam param)";
        btnSendXML_Click(strXML(param.func_id,param.action_id,param.request), param);
    }

    public void Init(XmlParam param) 
    {
        string xmlStr = param.xmlStr;
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nInit(XmlParam param)";
        
        if(xmlStr != null && xmlStr != string.Empty && xmlStr != "")
        {
            Debug.Log("Init : " + param.xmlStr);
            List<Promotion> pros = new List<Promotion>();
            XmlElement xmlRoot = GetXmlElement(param.xmlStr);
            BookCover cover;
            Promotion pro = null;
            maxCode = 0;
            int tmp = 0;
            XmlNode xmlNode = null;
            //Camera.main.GetComponent<MsgCenter_h>().TestText.text += param.xmlStr + "\n";
            if (xmlRoot["brochure_list"] != null)
            {
                //Debug.Log("if");
                foreach (XmlNode node in xmlRoot["brochure_list"].ChildNodes)
                {
                    pro = null;
                    cover = null;
                    if (node.Attributes["brochure_code"] != null)
                    {
                        tmp = int.Parse(node.Attributes["brochure_code"].Value);
                    }
                    else 
                    {
                        tmp = -1;
                    }
                    try
                    {
                        pro = new Promotion(node.Attributes["brochure_id"].Value,
                            node.Attributes["brochure_templet_id"].Value);

                        cover = new BookCover(node.Attributes["brochure_file_url"].Value);

                    }
                    catch (Exception exc)
                    {
                        Debug.Log(exc.Message);
                        //throw;
                    }
                    finally 
                    {
                        if (tmp > maxCode)
                        {
                            maxCode = tmp;
                        }
                        if(pro != null)
                        {
                            pros.Add(pro);
                            if (cover != null)
                            {
                                pro.SelfCover = cover;
                            }
                            else 
                            {
                                pro.SelfCover = new BookCover();
                            }
                        }
                        if (xmlNode == null) { xmlNode = node; }
                    }
                }
                //Debug.Log("if_after");
                
            }
            else 
            {
                Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nerr : xmlRoot[\"brochure_list\"] == null ！\n";
                Debug.Log("err : xmlRoot[\"brochure_list\"] == null");
            }
            Debug.Log("---------------->>>>>>>>>>>>>>>>>>>>" + maxCode);
            if(!string.IsNullOrEmpty(Bookrack.Instance.GetAttribute(xmlNode)))
            {
                _book = pros;
                Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nbook.Count = " + _book.Count.ToString() + "\n";
                XmlParam xmlParam;
                //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "     _book.Count" + _book.Count;//
                //Debug.Log(_book.Count);
                for (int i = 0; i < _book.Count; i++)
                {
                    xmlParam = new XmlParam("3D404767", "page", "brochure_id=\"" + _book[i].ID + "\" " + "layout_kind = \"1\"", AnalysisCoverLeyout, _book[i]);
                    GetTarXml(xmlParam);
                }
            }
        }

        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nOverInit(XmlParam param)";
    }

    public XmlElement GetXmlElement(string xmlStr) 
    {
        StringReader strRead = new StringReader(xmlStr);
        XmlReader reader = XmlReader.Create(strRead);


        System.IO.StringReader stringReader = new System.IO.StringReader(xmlStr);
        //stringReader.Read(); // 跳过 BOM 
        //System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());

        XmlDocument xml = new XmlDocument();
        xml.LoadXml(strRead.ReadToEnd());
        XmlElement xmlRoot = xml.DocumentElement;
        return xmlRoot;
    }

    public void AnalysisCoverLeyout(XmlParam param) 
    {
        XmlElement xmlRoot = GetXmlElement(param.xmlStr);
        bool isText = false;
        Debug.Log(param.xmlStr);
        foreach (XmlNode item in xmlRoot["rec_list"].ChildNodes)
        {
            isText = item.Attributes["res_file_url"] == null;
            //Debug.Log(item.Attributes["layout_code"].Value + ":\nresource_aside == null ? " +
            //    (item.Attributes["resource_aside"] == null) + ";\nres_file_url == null ?" + (item.Attributes["res_file_url"] == null));
            if (item.Attributes["layout_code"].Value != FRONT_CODE && item.Attributes["layout_code"].Value != BEHIND_CODE)
            {
                //Debug.Log(item.Attributes["layout_code"] == null ? "null" : item.Attributes["layout_code"].Value);
                //Debug.Log(item.Attributes["resource_aside"] == null ? "null" : item.Attributes["resource_aside"].Value);
                //Debug.Log(item.Attributes["res_file_url"] == null ? "null" : item.Attributes["res_file_url"].Value);
                if (item.Attributes["layout_code"].Value == "1001" && item.Attributes["res_file_url"] != null)
                {
                    //Debug.Log(param.book.SelfCover == null);
                    param.book.SelfCover.PictureUrl = item.Attributes["res_file_url"].Value;
                }
                param.book.SelfCover.Items.Add(item.Attributes["layout_code"].Value, new UiItem(isText ? ItemType.Text : ItemType.RawImage,
                     isText ? item.Attributes["resource_aside"] == null ? "" : item.Attributes["resource_aside"].Value
                     : item.Attributes["res_file_url"] == null ? "" : item.Attributes["res_file_url"].Value,
                     item.Attributes["layout_code"].Value, item.Attributes["layout_id"].Value));
            }
            else
            {
                try
                {
                    switch (item.Attributes["layout_code"].Value)
                    {
                        case FRONT_CODE:
                            param.book.SelfCover.FrontUrl = item.Attributes["res_file_url"].Value;
                            break;
                        case BEHIND_CODE: param.book.SelfCover.BehindUrl = item.Attributes["res_file_url"].Value;
                            break;
                    }
                }
                catch (Exception exc)
                {
                    //Debug.Log("err:" + exc.Message);
                }
                
            }
            //Debug.Log(book.SelfCover.Items[item.Attributes["layout_code"].Value].leyoutCode + " ; " + book.SelfCover.Items[item.Attributes["layout_code"].Value].target as string);
        }
        //  解析目录和内页信息（目录信息、内页ID）
        XmlParam xmlParam = new XmlParam("3D404762", "detail", "brochure_id=\"" + param.book.ID + "\"", AnalysisClgAndPageInfo, param.book);
        //Debug.Log("request:");
        GetTarXml(xmlParam);
        //  你把那个xml弄出来我看下
        loadedFirst = true;
    }

    /// <summary>
    /// 解析内页和目录信息
    /// </summary>
    /// <param name="param"></param>
    public void AnalysisClgAndPageInfo(XmlParam param) 
    {
        XmlElement xmlRoot = GetXmlElement(param.xmlStr);
        #region 目录信息
        int i = 0, pageTab = 0, tmpelateNum = 0;
        CataloguePage cp = null;
        List<CatalogueItem> ClgItems = null;
        Debug.Log("AnalysisClgAndPageInfo:" + param.xmlStr);
        if (xmlRoot["brochure_info"] != null && xmlRoot["brochure_info"].ChildNodes.Count > 0 &&
            xmlRoot["brochure_info"].ChildNodes[0].Attributes["brochure_file_url"] != null)
        {
            param.book.catalog_file_url = xmlRoot["brochure_info"].ChildNodes[0].Attributes["brochure_file_url"].Value;
        }
        //Debug.Log("ClgInfo:\n" + param.xmlStr);
        if (xmlRoot["catalog_list"] != null)
        {
            foreach (XmlNode firstNode in xmlRoot["catalog_list"].ChildNodes)
            {
                tmpelateNum = int.Parse(firstNode.Attributes["catalog_templet_id"].Value);
                if (ClgItems == null || i % TemplateOfItems[tmpelateNum] == 0)
                {
                    if (cp != null)
                    {
                        param.book.PageList.Add(cp);
                    }

                    ClgItems = new List<CatalogueItem>();
                    cp = new CataloguePage(tmpelateNum, pageTab, "", ClgItems);
                    cp.PictureUrl = param.book.catalog_file_url;
                    cp.Catalogue = true;
                    //cp.Items = new Dictionary<string, UiItem>();
                    pageTab++;
                }
                ClgItems.Add(new CatalogueItem(firstNode.Attributes["catalog_name"].Value, "", "", ClgRank.First, firstNode.Attributes["catalog_page_id"].Value));
                //Debug.Log(firstNode.Attributes["catalog_page_id"].Value);
                i++;
                foreach (XmlNode secondNode in firstNode.ChildNodes)
                {
                    tmpelateNum = int.Parse(secondNode.Attributes["catalog_templet_id"].Value);
                    if (i % TemplateOfItems[tmpelateNum] == 0)
                    {
                        if (cp != null)
                        {
                            param.book.PageList.Add(cp);
                        }

                        ClgItems = new List<CatalogueItem>();
                        cp = new CataloguePage(tmpelateNum, pageTab, "", ClgItems);
                        cp.PictureUrl = param.book.catalog_file_url;
                        cp.Catalogue = true;
                        pageTab++;
                    }
                    ClgItems.Add(new CatalogueItem(secondNode.Attributes["catalog_name"].Value, "", "", ClgRank.Second, secondNode.Attributes["catalog_page_id"].Value));
                    //Debug.Log(secondNode.Attributes["catalog_page_id"].Value);
                    i++;
                    foreach (XmlNode thirdlyNode in secondNode.ChildNodes)
                    {
                        tmpelateNum = int.Parse(firstNode.Attributes["catalog_templet_id"].Value);
                        if (i % TemplateOfItems[tmpelateNum] == 0)
                        {
                            if (cp != null)
                            {
                                param.book.PageList.Add(cp);
                            }

                            ClgItems = new List<CatalogueItem>();
                            cp = new CataloguePage(tmpelateNum, pageTab, "", ClgItems);
                            cp.PictureUrl = param.book.catalog_file_url;
                            cp.Catalogue = true;
                            pageTab++;
                        }
                        ClgItems.Add(new CatalogueItem(thirdlyNode.Attributes["catalog_name"].Value, "", "", ClgRank.Thirdly, thirdlyNode.Attributes["catalog_page_id"].Value));
                        //Debug.Log(thirdlyNode.Attributes["catalog_page_id"].Value);
                        i++;
                    }
                }
                param.book.CatalogueLen = pageTab;
            }
            if (cp != null)
            {
                param.book.PageList.Add(cp);
            }
            if (param.book.PageList.Count % 2 != 0)
            {
                ClgItems = new List<CatalogueItem>();
                cp = new CataloguePage(tmpelateNum, pageTab, (param.book.PageList[param.book.PageList.Count - 1]as CataloguePage).PictureUrl, ClgItems);
                param.book.PageList.Add(cp);
            }
        }
        
        #endregion

        #region 内页ID信息
        BookInfo bf = null;
        long maxCode = 0;
        //Debug.Log("Page_Info = " + param.xmlStr);
        if (xmlRoot["page_list"] != null)
        {
            foreach (XmlNode row in xmlRoot["page_list"].ChildNodes)
            {
                bf = new BookInfo(row.Attributes["page_id"].Value, 
                    row.Attributes["sequ"].Value, 
                    row.Attributes["page_templet_code"].Value,
                    row.Attributes["page_name"].Value,
                    row.Attributes["page_code"].Value);
                if(maxCode < bf.PageCode)
                {
                    maxCode = bf.PageCode;
                }
                if (row.Attributes["res_audio_url"] != null)
                {
                    bf.AudioUrl = row.Attributes["res_audio_url"].Value;
                }
                param.book.IDList.Add(bf);
                //Debug.Log("Getxml: id = " + bf.PageID + ";  Order = " + bf.Ordinal);
            }
            param.book.maxPageCode = maxCode;
            param.book.SortIDList();
            param.book.maxOrdinal = param.book.IDList[param.book.IDList.Count - 1].Ordinal;
            //Debug.Log("count = " + param.book.IDList.Count);
            // 对应目录和内页ID列表中的索引

            param.book.InitCataloguePage();
        }
        
        #endregion
        

        //  最后一本书加载完成
        if (_book[_book.Count - 1].ID == param.book.ID)
        {
            StartCoroutine(LoadBookLeyouts(_book));
            Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nBook = _book";
            Book = _book;
        }
    }

    public IEnumerator LoadBookLeyouts(List<Promotion> books)
    {
        for (int i = 0; i < books.Count; i++)
        {
            //  请求内页：
            foreach (string item in books[i].IdDictionary.Keys)
            {
                _loadingPageLeyout = true;
                //Debug.Log("LoadBookLeyouts");
                XmlParam xmlParam = new XmlParam("3D404767", "page", "page_id=\"" + item + "\" " + "layout_kind = \"3\"", 
                    AnalysisPageLeyout, books[i],item);
                GetTarXml(xmlParam);
                while (_loadingPageLeyout)
                {
                    yield return new WaitForSeconds(0);
                }
            }
            //books[i].FecthCatalogueLen();
        }
        
        TextTest.text += "\nLoadBookLeyouts was Loaded.\n";
        Resources.UnloadUnusedAssets();
//  测试代码
PagePool.Instance.Book = Book[0];
PagePool.Instance.SetCurrentIndex(0);
    }

    public void AnalysisPageLeyout(XmlParam param)
    {
        XmlElement xmlRoot = GetXmlElement(param.xmlStr);
        ItemType type = ItemType.None;
        UiItem item = null;
        Page page = new Page();
        Dictionary<string, UiItem> Items = new Dictionary<string,UiItem>();
        Dictionary<string, UiItem> BtnItems = new Dictionary<string, UiItem>();

        //Debug.Log("AnalysisPageLeyout:  " + param.xmlStr);
        if (xmlRoot["rec_list"] != null)
        {
            foreach (XmlNode leyout in xmlRoot["rec_list"].ChildNodes)
            {
                type = ItemType.None;
                if (leyout.Attributes["res_file_url"] != null)
                {
                    type = ItemType.RawImage;
                }
                else if (leyout.Attributes["resource_aside"] != null)
                {
                    type = ItemType.Text;
                }
                else if (leyout.Attributes["prod_id"] != null && leyout.Attributes["prod_id"].Value != "0")
                {
                    type = ItemType.Link;
                }
                //Debug.Log(leyout.Attributes["layout_code"].Value);
                switch (type)
                {
                    case ItemType.Text: item = new UiItem(type, leyout.Attributes["resource_aside"].Value,
                        leyout.Attributes["layout_code"].Value, leyout.Attributes["layout_id"].Value);
                        //Debug.Log(leyout.Attributes["layout_code"].Value + ": " + leyout.Attributes["resource_aside"].Value);
                        Items.Add(item.leyoutCode, item);
                        break;
                    case ItemType.RawImage: item = new UiItem(type,leyout.Attributes["res_file_url"].Value,
                        leyout.Attributes["layout_code"].Value, leyout.Attributes["layout_id"].Value);
                        
                        //Debug.Log(leyout.Attributes["layout_code"].Value + ": " + leyout.Attributes["res_file_url"].Value);
                        Items.Add(item.leyoutCode, item);
                        break;
                    case ItemType.Link: item = new UiItem(type, leyout.Attributes["prod_id"].Value,
                         leyout.Attributes["layout_code"].Value, leyout.Attributes["layout_id"].Value);
                        //Debug.Log("Link:" + leyout.Attributes["layout_code"].Value + " -> " + leyout.Attributes["prod_id"].Value);
                        BtnItems.Add(item.leyoutCode,item);
                        break;
                    case ItemType.Audio:
                        break;
                    case ItemType.Video:
                        break;
                    case ItemType.None:
                        break;
                }
            }
            
        }

        page.Items = Items;
        page.ButtonItems = BtnItems;
        page.ModelNumber = int.Parse(param.book.bookInfoDictionary[param.PageID].ModelNummber);
        //Debug.Log("Nummber = " + page.ModelNumber);
        page.PageTab = param.book.IdDictionary[param.PageID];
        page.ID = param.PageID;
        page.AudioUrl = param.book.IDList[param.book.IdDictionary[param.PageID]].AudioUrl;
        page.Name = param.book.IDList[param.book.IdDictionary[param.PageID]].PageName;
        page.Sequ = param.book.IDList[param.book.IdDictionary[param.PageID]].Ordinal;
//Debug.Log(page.PageTab + ":-> " + page.AudioUrl);
        param.book.PageList.Add(page);

        //Debug.Log(page.PageTab + "[BtnItemsCount]:" + page.ButtonItems.Count.ToString());
        _loadingPageLeyout = false;
    }
}
