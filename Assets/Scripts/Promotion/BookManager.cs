using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class BookManager : MonoBehaviour
{
    public static BookManager _isntance;
    public PageTabState currPageTab;  //  当前页面的状态
    public OperationBook operationBookSc;  //  书本管理脚本
    public BookInfomation bookInfomationSc;  //  
    public GameObject TestBook; 
    public GameObject BookPrafeb;  //生成的Book的prafeb

    public string ServerHost;

    public Dictionary<string, Book> Books = new Dictionary<string, Book>();

    public GameObject CurrentSeletedBook;
    public GameObject CurrentSeletedCover;
    public GameObject CurrentSelectModelLeft;  //  当前使用的模板
    public GameObject CurrentSelectModelRight;  //  当前使用的模板
    public Page CurrentSeletedPage;

    public Text testText;

    private int _nummber;
    private bool _loadFlg;
    void Awake()
    {
        _isntance = this;
        _nummber = 0;
        _loadFlg = true;
        //Initialize(ServerHost + "/BookConfig.xml");
        
    }

    public void MsgInit()
    {
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "   Befor GetTarXml(XmlParam param)";
        GetXml.Instances.GetTarXml(new GetXml.XmlParam("3D404751", "query", "corp_id=\"2015001\"", GetXml.Instances.Init));
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "   after GetTarXml(XmlParam param)";
        StartCoroutine(Init());
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "   Msg Init";
    }

    public void CheckErr(WWW www)
    {
        if (www.isDone)
        {
            testText.text = www.error;
        }
        else 
        {
            testText.text = "www.isDone = false";
            Debug.Log("www 没有加载完.");
        }
    }

    public IEnumerator LoadXml(string Url)
    {
        WWW www = new WWW(Url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            //List<Promotion> books = ConvertToBookList(www.text);
            //Bookrack.Instance.BookList = books;
        }
        else 
        {
            CheckErr(www);
        }
    }
#region  >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  接 口   <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    /// <summary>
    /// 从 Url 初始化所有书
    /// </summary>
    /// <param name="xmlUrl"></param>
    public void Initialize(string xmlUrl)
    {
        StartCoroutine(LoadXml(xmlUrl));
    }
    /// <summary>
    /// 从 xml 字符串 初始化所有书
    /// </summary>
    /// <param name="data"></param>
    public void CreateBooks(string data)
    {
        //List<Promotion> books = ConvertToBookList(data);
        //Bookrack.Instance.BookList = books;
    }
    /// <summary>
    /// 根据传过来的 xml字符串 刷新 当前页
    /// </summary>
    /// <param name="data"></param>
    public void RefreshCurrentPage(string data) 
    {
        Page page = GetPageOfXml(data);
        if(page != null)
        {
            OperationBook.Instance.RefreshTargetPage(page);
        }
    }

#endregion   <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    /// <summary>
    /// 加载所有宣传册缩略图
    /// </summary>
    /// <returns></returns>
    public IEnumerator Init() 
    {
        while (GetXml.Instances.Book == null)
        {
            //Camera.main.GetComponent<MsgCenter_h>().TestText.text += "   GetXml.Instances.Book == null";
            yield return null;
        }
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nInit";
        Bookrack.Instance.BookList = GetXml.Instances.Book;
    }

    /// <summary>
    /// 从XmlString到BookList
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    //public List<Promotion> ConvertToBookList(string data) 
    //{
    //    Promotion book = null;
    //    List<Promotion> resList = new List<Promotion>();
    //    Page bp;
    //    Item bp1;
    //    List<Page> bpList;
    //    List<CatalogueItem> ciList;
    //    BookCover cover;
    //    StringReader strStream = new StringReader(data);
    //    strStream.Read();
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.LoadXml(strStream.ReadToEnd());
    //    XmlElement xmlRoot = xmlDoc.DocumentElement;
    //    XmlNode coverNode;
    //    ClgRank rank;
    //    ///  <Book>
    //    foreach (XmlNode bookNode in xmlRoot.ChildNodes)
    //    {
    //        bpList = new List<Page>();
    //        coverNode = bookNode["Cover"];




    //        cover = new BookCover(int.Parse(coverNode.Attributes["ModelNumber"].Value), coverNode.Attributes["Name"].Value,
    //            coverNode.Attributes["LogoUrl"].Value, coverNode.Attributes["SubHead"].Value, coverNode.Attributes["Description"].Value,
    //            coverNode.Attributes["PictureUrl"].Value, coverNode.Attributes["SideCoverUrl"].Value);
    //        //Debug.Log(string.Format("bookNode.Count = {0}", bookNode["PageList"].ChildNodes.Count));

    //        //Debug.Log(bookNode["CatalogueList"].ChildNodes.Count);
    //        foreach (XmlNode catalogueNode in bookNode["CatalogueList"].ChildNodes)
    //        {
    //            ciList = new List<CatalogueItem>();
                
    //            foreach (XmlNode catalogueItem in catalogueNode.ChildNodes)
    //            {

    //                //ciList.Add(new CatalogueItem(catalogueItem.Attributes["Head"].Value, catalogueItem.Attributes["PageTab"].Value,
    //                    //catalogueItem.Attributes["SectionInof"].Value, bool.Parse(catalogueItem.Attributes["isParent"].Value)));
    //            }
    //            bp = new CataloguePage(int.Parse(catalogueNode.Attributes["ModelNumber"].Value),
    //                int.Parse(catalogueNode.Attributes["PageTab"].Value), catalogueNode.Attributes["PicUrl"].Value, ciList);
    //            //Debug.Log("PicUrl = " + catalogueNode.Attributes["PicUrl"].Value);
    //            bpList.Add(bp);
    //        }
    //        if (bpList.Count % 2 != 0)
    //        {
    //            bpList.Add(new CataloguePage(0, bpList.Count,"",new List<CatalogueItem>()));
    //        }
    //        //   <Page>
    //        foreach (XmlNode pageNode in bookNode["PageList"].ChildNodes)
    //        {
    //            bp = new Page(int.Parse(pageNode.Attributes["PageTab"].Value), int.Parse(pageNode.Attributes["ModelNumber"].Value),
    //                0, new List<PageItem>(), false, pageNode.Attributes["BgImg"].Value, pageNode.Attributes["Logo"].Value);
    //            bp.Heading = pageNode.Attributes["Heading"].Value;
    // //Debug.Log(OperationBook.Instance.name);
    //            bp1 = OperationBook.Instance.pageControllSc.GetModelType(bp.ModelNumber);
    //            bp.ModelNumber = bp1.ModelNummber;
    //            bp.ItemType = bp1.ModelType;
    //            //Debug.Log(pageNode.ChildNodes.Count);
    //            //  <PageItem>
    //            foreach (XmlNode itemNode in pageNode.ChildNodes)
    //            {
    //                //Debug.Log(bp.PageTab + ":" + itemNode.Attributes["Heading"].Value);
    //                bp.PageItems.Add(new Item(itemNode.Attributes["Show3DUrl"].Value, itemNode.Attributes["AudioUrl"].Value,
    //                    itemNode.Attributes["MovieUrl"].Value, itemNode.Attributes["CurtainImgUrl"].Value,
    //                    itemNode.Attributes["Introduce"].Value, itemNode.Attributes["Heading"].Value));
    //                //Debug.Log(items[items.Count - 1].CurtainImgUrl);
    //            }
    //            //Debug.Log("items == null ? " + items == null);
    //            bpList.Add(bp);
    //        }
    //        book = new Promotion(cover, bpList, int.Parse(bookNode.Attributes["DecorativeNumber"].Value));
    //        //book.ModelNumber = int.Parse(bookNode.Attributes["ModelNumber"].Value);
    //        //CreateCatalogue(book);
    //        resList.Add(book);
    //    }
        
    //    return resList;
    //}
    //
    public static string GetBooksXmlStr(List<Promotion> pBooks)
    {
        string res = "";
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0","utf-8","yes");
        xmlDoc.AppendChild(declaration);
        XmlElement Books = xmlDoc.CreateElement("BOOKS");
        xmlDoc.AppendChild(Books);
        XmlElement Book_Element;
        XmlElement book_cover;
        XmlElement pagelist;
        XmlElement page_page;
        XmlElement page_item;
        XmlAttribute Attribute;
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "     GetBooksXmlStr";
        for (int i = 0; i < pBooks.Count; i++)
        {
            Book_Element = xmlDoc.CreateElement("Book");
            Books.AppendChild(Book_Element);

            Attribute = xmlDoc.CreateAttribute("ID");
            Attribute.Value = i.ToString();
            Book_Element.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("ModelNumber");
            Attribute.Value = pBooks[i].ModelNumber.ToString();
            Book_Element.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("DecorativeNumber");
            Attribute.Value = pBooks[i].DecorativeNumber.ToString();
            Book_Element.Attributes.Append(Attribute);
            //  保存 Cover
            book_cover = xmlDoc.CreateElement("Cover");
            Book_Element.AppendChild(book_cover);
            //  添加 Cover的属性
            Attribute = xmlDoc.CreateAttribute("ModelNumber");
            Attribute.Value = pBooks[i].SelfCover.ModelNumber.ToString();
            book_cover.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("Name");
            Attribute.Value = pBooks[i].SelfCover.SelfName;
            book_cover.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("LogoUrl");
            Attribute.Value = pBooks[i].SelfCover.LogoUrl;
            book_cover.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("SubHead");
            Attribute.Value = pBooks[i].SelfCover.SubHead;
            book_cover.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("PictureUrl");
            Attribute.Value = pBooks[i].SelfCover.PictureUrl;
            book_cover.Attributes.Append(Attribute);

            Attribute = xmlDoc.CreateAttribute("SideCoverUrl");
            Attribute.Value = pBooks[i].SelfCover.FrontUrl;
            book_cover.Attributes.Append(Attribute);

            //  添加 PageList 节点
            pagelist = xmlDoc.CreateElement("PageList");
            Book_Element.AppendChild(pagelist);

            Camera.main.GetComponent<MsgCenter_h>().TestText.text += "     for";
            for (int j = 0; j < pBooks[i].PageList.Count; j++)
            {
                if (!(pBooks[i].PageList[j] is CataloguePage))
                {
                    page_page = xmlDoc.CreateElement("Page");
                    pagelist.AppendChild(page_page);

                    Attribute = xmlDoc.CreateAttribute("ModelNumber");
                    Attribute.Value = pBooks[i].PageList[j].ModelNumber.ToString();
                    page_page.Attributes.Append(Attribute);

                    Attribute = xmlDoc.CreateAttribute("Heading");
                    //Attribute.Value = pBooks[i].PageList[j].Heading;
                    page_page.Attributes.Append(Attribute);

                    Attribute = xmlDoc.CreateAttribute("PageTab");
                    Attribute.Value = pBooks[i].PageList[j].PageTab.ToString();
                    page_page.Attributes.Append(Attribute);

                    Attribute = xmlDoc.CreateAttribute("AudioUrl");
                    //Attribute.Value = pBooks[i].PageList[j].AudioUrl;
                    page_page.Attributes.Append(Attribute);

                    Attribute = xmlDoc.CreateAttribute("Introduce");
                    //Attribute.Value = pBooks[i].PageList[j].Introduce;
                    page_page.Attributes.Append(Attribute);
                    //Debug.Log(i + ":" + (pBooks[i].PageList[j].PageItems == null));
                    //for (int k = 0; k < pBooks[i].PageList[j].PageItems.Count; k++)
                    //{
                    //    page_item = xmlDoc.CreateElement("PageItem");
                    //    page_page.AppendChild(page_item);

                    //    Attribute = xmlDoc.CreateAttribute("AudioUrl");
                    //    Attribute.Value = pBooks[i].PageList[j].PageItems[k].AudioUrl;
                    //    page_item.Attributes.Append(Attribute);

                    //    Attribute = xmlDoc.CreateAttribute("MovieUrl");
                    //    Attribute.Value = pBooks[i].PageList[j].PageItems[k].MovieUrl;
                    //    page_item.Attributes.Append(Attribute);

                    //    Attribute = xmlDoc.CreateAttribute("Show3DUrl");
                    //    Attribute.Value = pBooks[i].PageList[j].PageItems[k].Show3DUrl;
                    //    page_item.Attributes.Append(Attribute);

                    //    Attribute = xmlDoc.CreateAttribute("CurtainImgUrl");
                    //    Attribute.Value = pBooks[i].PageList[j].PageItems[k].CurtainImgUrl;
                    //    page_item.Attributes.Append(Attribute);
                    //}
                }
                
            }
        }

        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "     Over";
        //  将 xml 转换为字符串
        MemoryStream ms = new MemoryStream();
        XmlTextWriter writer = new XmlTextWriter(ms,null);
        writer.Formatting = Formatting.Indented;
        xmlDoc.Save(writer);
        StreamReader sr = new StreamReader(ms,System.Text.Encoding.UTF8);
        ms.Position = 0;
        res = sr.ReadToEnd();
        ms.Close();
        sr.Close();
        return res;
    }
    /// <summary>
    /// 将 page 转换为 xml 字符串
    /// </summary>
    /// <param name="pPage"></param>
    /// <returns></returns>
    public static string GetPageXmlStr(Page pPage)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0","utf-8","yes");
        xmlDoc.AppendChild(declaration);
        XmlElement page = xmlDoc.CreateElement("Page");
        xmlDoc.AppendChild(page);

        XmlAttribute attribute = xmlDoc.CreateAttribute("ModelNumber");
        attribute.Value = pPage.ModelNumber.ToString();
        page.Attributes.Append(attribute);

        attribute = xmlDoc.CreateAttribute("Heading");
        //attribute.Value = pPage.Heading;
        page.Attributes.Append(attribute);

        attribute = xmlDoc.CreateAttribute("PageTab");
        attribute.Value = pPage.PageTab.ToString();
        page.Attributes.Append(attribute);

        attribute = xmlDoc.CreateAttribute("AudioUrl");
        //attribute.Value = pPage.AudioUrl;
        page.Attributes.Append(attribute);

        attribute = xmlDoc.CreateAttribute("Introduce");
        //attribute.Value = pPage.Introduce;
        page.Attributes.Append(attribute);

        XmlElement page_item;
        //for (int i = 0; i < pPage.PageItems.Count; i++)
        //{
        //    page_item = xmlDoc.CreateElement("PageItem");
        //    page.AppendChild(page_item);

        //    attribute = xmlDoc.CreateAttribute("AudioUrl");
        //    attribute.Value = pPage.PageItems[i].AudioUrl;
        //    page_item.Attributes.Append(attribute);

        //    attribute = xmlDoc.CreateAttribute("MovieUrl");
        //    attribute.Value = pPage.PageItems[i].MovieUrl;
        //    page_item.Attributes.Append(attribute);

        //    attribute = xmlDoc.CreateAttribute("Show3DUrl");
        //    attribute.Value = pPage.PageItems[i].Show3DUrl;
        //    page_item.Attributes.Append(attribute);

        //    attribute = xmlDoc.CreateAttribute("CurtainImgUrl");
        //    attribute.Value = pPage.PageItems[i].CurtainImgUrl;
        //    page_item.Attributes.Append(attribute);
        //}

        MemoryStream ms = new MemoryStream();
        XmlTextWriter write = new XmlTextWriter(ms,null);
        write.Formatting = Formatting.Indented;
        xmlDoc.Save(write);
        StreamReader sr = new StreamReader(ms,System.Text.Encoding.UTF8);
        string res = "";
        res = sr.ReadToEnd();
        sr.Close();
        ms.Close();
        //xmlDoc.Save("http://ftp514893.host571.zhujiwu.cn/1.xml");
        return res;
    }

    /// <summary>
    /// 根据 页的 xml字符串 返回页
    /// </summary>
    /// <param name="pData"></param>
    /// <returns></returns>
    public static Page GetPageOfXml(string pData) 
    {
        Page res = null;
        //StringReader sr = new StringReader(pData);
        //sr.Read();
        //XmlDocument xmlDoc = new XmlDocument();
        //try
        //{
        //    xmlDoc.LoadXml(sr.ReadToEnd());
        //    XmlElement PageRoot = xmlDoc.DocumentElement;
        //    List<PageItem> pis = new List<PageItem>();
        //    foreach (XmlNode itemNode in PageRoot)
        //    {
        //        pis.Add(new Item(itemNode.Attributes["Show3DUrl"].Value, itemNode.Attributes["AudioUrl"].Value,
        //            itemNode.Attributes["MovieUrl"].Value, itemNode.Attributes["CurtainImgUrl"].Value,
        //            itemNode.Attributes["Introduce"].Value, itemNode.Attributes["Heading"].Value));
        //    }
        //    int tab = int.Parse(PageRoot.Attributes["ModelNumber"].Value);
        //    Item item = OperationBook.Instance.pageControllSc.GetModelType(tab);
        //    res = new Page(PageRoot.Attributes["Heading"].Value, PageRoot.Attributes["Introduce"].Value, PageRoot.Attributes["AudioUrl"].Value,
        //        int.Parse(PageRoot.Attributes["PageTab"].Value), item.ModelNummber, item.ModelType, pis);
        //}
        //catch (System.Exception exc)
        //{
        //    Debug.Log(exc.Message);
        //}
        
        return res;
    }

}
