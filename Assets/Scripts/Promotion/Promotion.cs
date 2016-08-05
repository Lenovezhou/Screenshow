using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;




public enum ClgRank
{
	First,
	Second,
	Thirdly
}

public enum ItemType 
{
    Text,
    RawImage,
    Link,
    Audio,
    Video,
    None
}

public class UiItem
{
    public ItemType type;
    public object target;
    public string leyoutCode;
    public string leyoutID;
    public UiItem(ItemType pType, object pTarget, string pLeyoutCode, string pLeyoutID)
    {
        type = pType;
        target = pTarget;
        leyoutCode = pLeyoutCode;
        leyoutID = pLeyoutID;
    }
}

public class ButtonItem 
{
    public string url;
    public string ID;

    public ButtonItem(string pUrl, string pID) 
    {
        url = pUrl;
        ID = pID;
    }
}

public class MessageParam
{
    public string message;
    public string param;
    public MessageParam(string pmessage, string pparam) 
    {
        message = pmessage;
        param = pparam;
    }
}

public class BookInfo 
{
    public string PageID;   //  内页ID
    public int Ordinal;  //  序号
    public string AudioUrl { set; get; }  //
    public string PageName { set; get; }
    public long PageCode { set; get; }

    //  模板编号
    private int _modelNummber; 
    public string ModelNummber 
    {
        set 
        {
            try
            {
                _modelNummber = int.Parse(value);
            }
            catch (System.Exception exc)
            {
                Debug.Log("ModelNummber Covert err:" + exc.Message);
            }
        }
        get 
        {
            return _modelNummber.ToString();
        }
    }
    public BookInfo(string pPageID, string pOrdinal, string pModelNummber, string pPageName,string pageCode) 
    {
        PageID = pPageID;
        try
        {
            Ordinal = int.Parse(pOrdinal);
        }
        catch (System.Exception exc)
        {
            Debug.Log("ModelNummber Covert err:" + exc.Message);
            throw exc;
        }
        ModelNummber = pModelNummber;
        PageName = pPageName;
        try
        {
            PageCode = long.Parse(pageCode);
        }
        catch (System.Exception exc)
        {
            Debug.Log(exc.Message);
            throw exc;
        }
    }
}

/// <summary>
/// 宣传册信息类
/// </summary>
public class Promotion : Book
{
    public string ID;
    public string str_ModelNumber;
    public int ModelNumber { set; get; }    //模板编号
    public int DecorativeNumber { set; get; }  //装饰元素编号
    public BookCover SelfCover { set; get; }
    public List<Page> PageList { set; get; }
    public List<BookInfo> IDList;
    public Dictionary<string, int> IdDictionary;
    public Dictionary<string, BookInfo> bookInfoDictionary;
    public string catalog_file_url;  //  目录图片
    public int CatalogueLen;

    public string CatalogueTemplateID;
    public int CataTempNum;

    public int maxOrdinal;   //  内页的最大序号值

    public long maxPageCode;

    public Promotion()
    {
        Init();
        IDList = new List<BookInfo>();
        CataTempNum = 0;
        IdDictionary = new Dictionary<string, int>();
    }

    public Promotion(string pID, string pModelNumber, BookCover pSelfCover)
    {
        ID = pID;
        str_ModelNumber = pModelNumber;
        SelfCover = pSelfCover;
        IDList = new List<BookInfo>();
        PageList = new List<Page>();
        IdDictionary = new Dictionary<string, int>();
        maxPageCode = 0;
    }

    public Promotion(string pID, string pModelNumber)
    {
        ID = pID;
        str_ModelNumber = pModelNumber;
        SelfCover = null;
        IDList = new List<BookInfo>();
        PageList = new List<Page>();
        IdDictionary = new Dictionary<string, int>();
        maxPageCode = 0;
    }

    public Promotion(BookCover pSelfCover, List<Page> pPageList, int pDecorativeNumber)
    {
        ModelNumber = 0;
        DecorativeNumber = 0;
        SelfCover = pSelfCover;
        PageList = pPageList;
        DecorativeNumber = pDecorativeNumber;
        IDList = new List<BookInfo>();
        IdDictionary = new Dictionary<string, int>();
        ID = null;
        CataTempNum = 0;
        maxPageCode = 0;
    }

    public Promotion(Promotion pTargetPromotion)
    {
        ModelNumber = pTargetPromotion.ModelNumber;
        DecorativeNumber = pTargetPromotion.DecorativeNumber;
        //SelfCover = new BookCover(pTargetPromotion.SelfCover.ModelNumber, pTargetPromotion.SelfCover.SelfName,
        //    pTargetPromotion.SelfCover.LogoUrl, pTargetPromotion.SelfCover.SubHead, pTargetPromotion.SelfCover.Description,
        //    pTargetPromotion.SelfCover.PictureUrl);
        PageList = pTargetPromotion.PageList;
        IDList = new List<BookInfo>();
        IdDictionary = new Dictionary<string, int>();
        maxPageCode = 0;
    }

    public void Init()
    {
        SelfCover = new BookCover();
        PageList = new List<Page>();
    }

    public void SortIDList() 
    {
        BookInfo bf1 = null, bf2 = null;
        //Debug.Log("SortIDList");
        // 将内页ID按照序号排序
        for (int i = 0; i < IDList.Count - 1; i++)
        {
            //Debug.Log(IDList[i].PageID + ";   " + IDList[i].Ordinal);
            for (int j = i + 1; j < IDList.Count; j++)
            {
                //Debug.Log(IDList[j].PageID + ";   " + IDList[j].Ordinal);
                if (IDList[i].Ordinal > IDList[j].Ordinal)
                {
                    bf1 = IDList[i];
                    bf2 = IDList[j];
                    IDList.RemoveAt(i);
                    IDList.Insert(i,bf2);
                    IDList[j] = bf1;
                    //Debug.Log("i[id]:" + IDList[i].PageID + "   j[id]:" + bf.PageID);
                }
            }
        }
        //  创建页码与内页ID的字典
        
        bookInfoDictionary = new Dictionary<string, BookInfo>();
        for (int i = 0; i < IDList.Count; i++)
        {
            //Debug.Log(IDList[i].PageID + " ; order  = " + i);
            IdDictionary.Add(IDList[i].PageID,i);
            bookInfoDictionary.Add(IDList[i].PageID, IDList[i]);
        }
        //Debug.Log(IdDictionary.Count);
    }
    /// <summary>
    /// 将目录中的内页id对应到内页ID的页码
    /// </summary>
    public void InitCataloguePage() 
    {
        CataloguePage cp;
        for (int i = 0; i < PageList.Count; i++)
        {
            if (PageList[i] is CataloguePage)
            {
                cp = PageList[i] as CataloguePage;
                //Debug.Log("id : " + cp.CatalogueItems[i].PageID);
                for (int j = 0; j < cp.CatalogueItems.Count; j++)
                {
                    //Debug.Log(j +  " : " + (cp.CatalogueItems[j] == null));
                    //Debug.Log("id = " + cp.CatalogueItems[j].PageID);
                    if (IdDictionary.ContainsKey(cp.CatalogueItems[j].PageID))
                    {
                        cp.CatalogueItems[j].IndexOfIDList = IdDictionary[cp.CatalogueItems[j].PageID];
                        //Debug.Log(cp.CatalogueItems[j].IndexOfIDList);
                    }
                }
            }
            else 
            {
                break;
            }
        }
    }
    /// <summary>
    /// 计算目录的长度
    /// </summary>
    public void FecthCatalogueLen() 
    {
        for (int i = 0; i < PageList.Count; i++)
        {
            if (!(PageList[i] is CataloguePage))
            {
                CatalogueLen = i;
                break;
            }
        }
    }
}



public interface Cover
{
    int ModelNumber { set; get; }  //模板编号

    string SelfName { set; get; }  //名称

    string LogoUrl { set; get; }   //Logo

    string SubHead { set; get; }   //副标题

    string Description { set; get; }  //描述

    string PictureUrl { set; get; }   //图
}

/// <summary>
/// 封面信息类
/// </summary>
public class BookCover:Cover
{
    public int ModelNumber { set; get; }  //模板编号

    public string SelfName { set; get; }  //名称

    public string LogoUrl { set; get; }   //Logo

    public string SubHead { set; get; }   //副标题

    public string Description { set; get; }  //描述

    public string PictureUrl { set; get; }   //图片

    public string FrontUrl;   //  封面前边图片

    public string BehindUrl;   //  封面后边图片

    public string leyoutCode;  //  编码

    public RawImage thumbnail;   //  缩略图

    public Dictionary<string, UiItem> Items;

    public BookCover()
    {
        ModelNumber = 0;
        SelfName = "";
        LogoUrl = "";
        SubHead = "";
        Description = "";
        PictureUrl = "";
        Items = new Dictionary<string, UiItem>();
    }

    public BookCover(int pModelNumber, Dictionary<string, UiItem> pItems) 
    {
        ModelNumber = pModelNumber;
        Items = pItems;
    }

    public BookCover(int pModelNumber, string pSelfName,
        string pLogoUrl, string pSubHead, string pDescription, string pPictureUrl,
        string pBehindUrl)
    {
        ModelNumber = pModelNumber;
        SelfName = pSelfName;
        LogoUrl = pLogoUrl;
        SubHead = pSubHead;
        Description = pDescription;
        PictureUrl = pPictureUrl;
        BehindUrl = pBehindUrl;
    }

    public BookCover(string pPictureUrl)
    {
        ModelNumber = 0;
        SelfName = "";
        LogoUrl = "";
        SubHead = "";
        Description = "";
        PictureUrl = pPictureUrl;
        Items = new Dictionary<string, UiItem>();
    }
}

public class Page
{
    public string Name { set; get; }

    public int PageTab {set;get;}   //  

    public int ModelNumber{ set;get; }

    public string AudioUrl { set; get; }

    public Dictionary<string, UiItem> Items;

    public Dictionary<string, UiItem> ButtonItems;

    public bool Catalogue;  //  是否为目录

    public int Sequ { set; get; }

    public string ID { set; get; }


    public Page() 
    {
        Name = "";
    }

    public Page(int pPageTab, int pModelNumber, Dictionary<string, UiItem> pItems) 
    {
        PageTab = pPageTab;
        ModelNumber = pModelNumber;
        Items = pItems;
        Catalogue = false;
        Name = "新建书页";
    }
}

public class  CatalogueItem
{
    public string Head;
    public string PageTab;
    public string SectionInfo;
    public string PageID;
    public int IndexOfIDList;
	public ClgRank rank;
    public CatalogueItem(string pHead, string pPageTab, string pSectionInfo, ClgRank pRank, string pPageID) 
    {
		rank = pRank;
        Head = pHead;
        PageTab = pPageTab;
        SectionInfo = pSectionInfo;
        PageID = pPageID;
    }
}

public class CataloguePage : Page 
{
    public List<CatalogueItem> CatalogueItems; // 目录的项
    public string PictureUrl;
    public CataloguePage(int pModelNum,int pPageTab,string PicUrl, List<CatalogueItem> pCatalogueItems) 
    {
        CatalogueItems = pCatalogueItems;
        PageTab = pPageTab;
        Catalogue = true;
        PictureUrl = PicUrl;
        ModelNumber = pModelNum;
    }
}

public interface Book 
{
    
    int ModelNumber { set; get; } //模板编号

    int DecorativeNumber { set; get; }//装饰元素编号

    BookCover SelfCover { set; get; }  //  封面

    List<Page> PageList { set; get; }  //  页列表
}
