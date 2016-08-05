using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum BookState
{
    NoRead,
    Read,
    None
}

public enum PageTabState
{
    One,
    Two,
    Three,
    Four,
    Left,
    Right,
    None
}

public class OperationBook : MonoBehaviour
{
    public Page CurrentPage;   //  当前操作的页

    public InputField NameInput;
    public InputField DInput;
    public Dropdown Models;
    public Button ReturnBtn;

    public PageControl pageControllSc;   //  模板管理脚本中的一个

    public GameObject CatalogueBtnPrefabs;  //  目录按钮的预设
    public CatalogueBtnContent CatalogueBtnContentSc;  // 容纳button的容器的button

    public GameObject BookrackUI;   
    
    public static OperationBook Instance;
    public PageTabState readState
    {
        set
        {
            _readState = value;
            switch (value)
            {
                case PageTabState.Left:
                    
                    CurrentPage = BookInfomation._isntance.LastSeletedPage;
                    //Debug.Log("Page: " + (CurrentPage.PageTab + 1));
                    break;
                case PageTabState.Right:
                    if (BookInfomation._isntance.LastIsLeft)
                    {
                        CurrentPage = BookInfomation._isntance.NextCurrentPage;
                        //if (CurrentPage != null)
                        //Debug.Log("NextCurrentPage: " + BookInfomation._isntance.NextCurrentPage.PageTab);
                    }
                    else 
                    {
                        CurrentPage = BookInfomation._isntance.CurrentSeletedPage;
                        //if (CurrentPage != null)
                        //Debug.Log("CurrentSeletedPage: " + BookInfomation._isntance.CurrentSeletedPage.PageTab);
                    }
                    //Debug.Log("Page: " + (CurrentPage.PageTab + 1));
                    break;
                default:
                    break;
            }
        }
        get
        {
            return _readState;
        }
    }
    //public Promotion SelfPromotion = new Promotion();
    private PageTabState _readState;

    public Promotion SelfPromotion = new Promotion();
    void Awake()
    {
        Instance = this;
    }

    void Update()
    {

    }

    public IEnumerator TestAddPage(string url) 
    {
        WWW www = new WWW(url);
        yield return www;
        if(www.error == null && www.isDone)
        {
            InsertToCurrentPage(www.text);
        }
    }

    public void Add_Page_click() 
    {
        StartCoroutine(TestAddPage("http://ftp514893.host571.zhujiwu.cn/PageConfig.xml"));
    }

    public IEnumerator CloseBook()
    {
        BookInfomation._isntance.CanTurn = false;
        if (BookInfomation._isntance.index <= BookInfomation._isntance.CatalogueLen) 
        {
            if (BookInfomation._isntance.index < BookInfomation._isntance.CatalogueLen)
            {
                for (int i = 0; i < BookInfomation._isntance.CatalogueLen - BookInfomation._isntance.index; i++)
                {
                    BookInfomation._isntance.TurnRight(new BookInfomation.JumpPageParams(true, 1));
                    while (BookInfomation._isntance.IsPlaying())
                    {
                        yield return new WaitForSeconds(0.02f);
                        BookInfomation._isntance.MoveBook();
                    }
                }
            }
            else 
            {
                for (int i = 0; i < BookInfomation._isntance.CatalogueLen + 1 - BookInfomation._isntance.index; i++)
                {
                    BookInfomation._isntance.TurnRight(new BookInfomation.JumpPageParams(true, 1));
                    while (BookInfomation._isntance.IsPlaying())
                    {
                        yield return new WaitForSeconds(0.02f);
                        BookInfomation._isntance.MoveBook();
                    }
                }
            }
        }
        else
        {
            BookInfomation._isntance.JumpPageTab(0);
            while (BookInfomation._isntance.IsPlaying())
            {
                yield return new WaitForSeconds(0);
            }

            BookInfomation._isntance.index = 2;
            BookInfomation._isntance.TurnRight(new BookInfomation.JumpPageParams(true, 1));
            while (BookInfomation._isntance.IsPlaying())
            {
                yield return new WaitForSeconds(0);
            }
            BookInfomation._isntance.TurnRight(new BookInfomation.JumpPageParams(true, 1));
            
            while (BookInfomation._isntance.IsPlaying())
            {
                yield return new WaitForSeconds(0.02f);
                BookInfomation._isntance.MoveBook();
            }
        }
        BookrackUI.SetActive(true);
        PageNControl.userState = UserState.None;
        OperationBook.Instance.CreateCatalogue(null);
    }


#region         >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  接  口  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    /// <summary>
    /// 返回到书架界面
    /// </summary>
    public void ReturnBookrack()
    {
        StartCoroutine(CloseBook());
//ReturnBtn.interactable = false;  // 测试
    }
    /// <summary>
    /// 从当前选中页删除
    /// </summary>
    public void DeleteCurrentPage()
    {
        if (CurrentPage != null && !(CurrentPage is CataloguePage))
        {
            DeleteCurrentPages(readState, CurrentPage.PageTab);
        }
    }
    /// <summary>
    /// 从任意一页删除
    /// </summary>
    /// <param name="targetPageTab">要删除的页码</param>
    public void DeleteTargetPage(int targetPageTab)
    {
        DeleteCurrentPages(PageTabState.None, targetPageTab);
    }
    /// <summary>
    /// 从当前页添加一页
    /// </summary>
    public void InsertToCurrentPage(string pageStr) 
    {
        Page newPage = BookManager.GetPageOfXml(pageStr);
        if (newPage != null)
        {
            if (CurrentPage != null)
            {
                if (pageStr != "")
                {
                    AddPage(newPage, CurrentPage.PageTab, true);
                }
            }
            else
            {
                AddPage(newPage, -1, true);
            }
        } 
    }
    /// <summary>
    /// 从指定页添加一页，不刷新当前数据
    /// </summary>
    /// <param name="pPageTab">指定的页码</param>
    public void InsertToTargetPage(string pageStr, int pPageTab)
    {
        Page newPage = BookManager.GetPageOfXml(pageStr);
        if (newPage != null)
        {
            AddPage(newPage,pPageTab, false);
        }
    }
    /// <summary>
    /// 获取当前页的xml字符串格式
    /// </summary>
    /// <returns></returns>
    public string GetCurrentPage() 
    {
        string res = "";
        if(CurrentPage != null)
        {
            res =  BookManager.GetPageXmlStr((Page)CurrentPage);
        }
        return res;
    }
    /// <summary>
    /// 刷新当前选中页
    /// </summary>
    /// <param name="pageStr"></param>
    public void RefreshPage(string pageStr) 
    {
        Page newPage = BookManager.GetPageOfXml(pageStr);
        if (newPage != null)
        {
            RefreshTargetPage(newPage);
        }
    }

#endregion      <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    /// <summary>
    /// 修改页
    /// </summary>
    /// <param name="pts"></param>
    /// <param name="pBookPage"></param>
    public void RefreshTargetPage(Page pBookPage)
    {
        switch (readState)
        {
            case PageTabState.Left:
                BookInfomation._isntance.ChangeLastRender(pBookPage);
                BookInfomation._isntance.AllModelBtnControllSc.HideLeftBtn();
                break;
            case PageTabState.Right:
                BookInfomation._isntance.AllModelBtnControllSc.HideRightBtn();
                if (BookInfomation._isntance.LastIsLeft && BookInfomation._isntance.promotion.PageList.Count != 1)
                {
                    BookInfomation._isntance.ChangeNextCurrentRender(pBookPage);
                }
                else
                {
                    BookInfomation._isntance.ChangeCurrentRender(pBookPage);
                }
                break;
        }
        BookInfomation._isntance.AllModelBtnControllSc.Initialize(pBookPage);
        RefreshBookData(pBookPage);
    }

    public void RefreshBookData(Page pBookPage)
    {
        List<Page> bps = BookInfomation._isntance.promotion.PageList;
        for (int i = 0; i < bps.Count; i++)
        {
            if (!(bps[i] is CataloguePage) && bps[i].PageTab == pBookPage.PageTab)
            {
                bps[i] = pBookPage;
                break;
            }
        }
    }

    public IEnumerator TestLoadXml(string xmlUrl) 
    {
        WWW www = new WWW(xmlUrl);
        yield return www;
        if (www.isDone && www.error == null)
        {
            Page page = BookManager.GetPageOfXml(www.text);
            if(page != null)
            {
                RefreshTargetPage(page);
                BookManager.GetPageXmlStr(page);
            }
        }
        else 
        {
            Debug.Log(www.error);
        }
    }

    public void ModifyPage_Click() 
    {
        if(CurrentPage != null)
        {
            StartCoroutine(TestLoadXml("http://127.0.0.1:8080/PageConfig.xml"));
        }
    }


    public void TestGetXmlPageStr()
    {
        //StartCoroutine(TestLoadXml(@"file://C:\Users\Administrator\Desktop\PageConfig.xml"));
    }
    /// <summary>
    /// 将传过来的book复制给自己
    /// </summary>
    /// <param name="pBook"></param>
    public void CreateCatalogue(Book pBook)
    {
        if (pBook != null)
        {
            for (int i = 0; i < pBook.PageList.Count; i++)
            {
                if (pBook.PageList[i] is CataloguePage)
                {
                    CreateCatalogueBtn((CataloguePage)pBook.PageList[i]);
                }
                else
                {
                    break;
                }
            }
        }
        else 
        {
            ClearCatalogueBtn();
        }
    }
    /// <summary>
    /// 清空目录按钮列表
    /// </summary>
    public void ClearCatalogueBtn() 
    {
        GameObject contentObj = CatalogueBtnContentSc.gameObject;
        GameObject obj;
        for (int i = 0; i < contentObj.transform.GetChildCount(); i++)
        {
            obj = contentObj.transform.GetChild(i).gameObject;
            if (contentObj.transform.GetChild(i).gameObject.activeSelf) 
            {
                Destroy(obj);
            }

        }
        CatalogueBtnContentSc.CatalogueBtns.Clear();
    }

    /// <summary>
    /// 创建目录按钮列表
    /// </summary>
    /// <param name="pCataloguePage"></param>
    public void CreateCatalogueBtn(CataloguePage pCataloguePage) 
    {
        GameObject obj;
        CatalogueBtn cb = null;
        for (int i = 0; i < pCataloguePage.CatalogueItems.Count; i++)
        {
            obj = Instantiate(CatalogueBtnPrefabs, 
                CatalogueBtnPrefabs.transform.position, CatalogueBtnPrefabs.transform.rotation) as GameObject;
            cb = obj.GetComponent<CatalogueBtn>();
            cb.SelfText.text = pCataloguePage.CatalogueItems[i].Head + "  " + pCataloguePage.CatalogueItems[i].PageTab;
            cb.TargetPageTab = pCataloguePage.CatalogueItems[i].IndexOfIDList;
            obj.transform.parent = CatalogueBtnContentSc.gameObject.transform;
            CatalogueBtnContentSc.CatalogueBtns.Add(cb);
            obj.transform.localScale = Vector3.one;
            obj.SetActive(true);
        }
        if(cb != null)
        {
            CatalogueBtn.CatalogueBtnContentSc = CatalogueBtnContentSc;
        }
    }
    /// <summary>
    /// 根据指定参数 刷新当前页
    /// </summary>
    /// <param name="pBookPage"></param>
    public void SetCurrentPage(Page pBookPage)
    {
        if (readState == PageTabState.Left)
        {
            BookInfomation._isntance.ChangeLastRender(pBookPage);
        }
        else if (readState == PageTabState.Right)
        {
            if (BookInfomation._isntance.LastIsLeft)
            {
                BookInfomation._isntance.ChangeNextCurrentRender(pBookPage);
            }
            else
            {
                BookInfomation._isntance.ChangeCurrentRender(pBookPage);
            }
        }
    }

    /// <summary>
    /// 从指定页插入一页
    /// </summary>
    /// <param name="currentPage"></param>
    /// <param name="readState"></param>
    private void AddPage(Page newPage,int targetPage,bool isCurrent)
    {
        BookInfomation bf = BookInfomation._isntance;
        int currIndex = -1;
        for (int i = bf.CatalogueLen; i < bf.promotion.PageList.Count; i++)
        {
            if (targetPage == bf.promotion.PageList[i].PageTab)
            {
                currIndex = i;
                break;
            }
        }
        if (currIndex != -1 || targetPage == -1)
        {
            if (targetPage == -1)
            {
                bf.promotion.PageList.Add(newPage);
            }
            else 
            {
                bf.promotion.PageList.Insert(currIndex, newPage);
                currIndex++;
                for (int i = currIndex; i < bf.promotion.PageList.Count; i++)
                {
                    bf.promotion.PageList[i].PageTab++;
                }
            }

            if (isCurrent && CurrentPage != null && !(CurrentPage is CataloguePage))
            {
                bf.AllModelBtnControllSc.HideAllBtn();
                if (readState == PageTabState.Left)
                {
                    if (bf.LastIsLeft)
                    {
                        bf.ChangeNextRender(newPage);
                        bf.ChangeNextCurrentRender(bf.promotion.PageList[currIndex]);
                    }
                    else
                    {
                        bf.ChangeLastRender(newPage);
                        bf.ChangeCurrentRender(bf.promotion.PageList[currIndex]);
                    }
                    bf.AllModelBtnControllSc.Initialize(newPage);
                    bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[currIndex]);
                }
                else
                {

                    if (bf.LastIsLeft)
                    {
                        bf.ChangeNextCurrentRender(newPage);
                    }
                    else
                    {
                        bf.ChangeCurrentRender(newPage);
                    }
                    bf.AllModelBtnControllSc.Initialize(newPage);
                }
            }
            else if (isCurrent && targetPage == -1)
            {
                if (bf.LastIsLeft)
                {
                    bf.ChangeNextCurrentRender(newPage);
                }
                else
                {
                    bf.ChangeCurrentRender(newPage);
                }
                bf.AllModelBtnControllSc.Initialize(newPage);
                if(readState == PageTabState.Left)
                {
                    bf.LastSeletedPage = newPage;
                }
                else if (readState == PageTabState.Right) 
                {
                    if (bf.LastIsLeft)
                    {
                        bf.NextCurrentPage = newPage;
                    }
                    else
                    {
                        bf.CurrentSeletedPage = newPage;
                    }
                }
            }
        }
        bf.PageNControl_A.HideAllHeightShine();
    }

    private void DeleteCurrentPages(PageTabState currentState,int pPageTab) 
    {
        BookInfomation bf = BookInfomation._isntance;
        bool changeFlg = false;
        switch (currentState)
        {
            case PageTabState.Left:
                for (int i = bf.CatalogueLen, j = 1; i < bf.promotion.PageList.Count; i++)
                {
                    if (changeFlg)
                    {
                        bf.promotion.PageList[i].PageTab = pPageTab + j;
                        j++;
                    }
                    if (!(bf.promotion.PageList[i] is CataloguePage) && bf.promotion.PageList[i].PageTab == pPageTab)
                    {
                        Debug.Log("Delete: " + bf.promotion.PageList[i].PageTab);
                        bf.promotion.PageList.RemoveAt(i);
                        Debug.Log("i: " + i + "; count: " + bf.promotion.PageList.Count);
                        bf.AllModelBtnControllSc.HideAllBtn();
                        if (i + 1 < bf.promotion.PageList.Count)
                        {
                            bf.promotion.PageList[i].PageTab = pPageTab;
                            bf.promotion.PageList[i + 1].PageTab = pPageTab + j;
                            changeFlg = true;
                            if (bf.LastIsLeft)
                            {
                                bf.NextCurrentPage = bf.promotion.PageList[i + 1];
                                bf.NextSeletedPage = bf.promotion.PageList[i + 1];
                                bf.ChangeNextRender(bf.promotion.PageList[i]);
                                bf.ChangeNextCurrentRender(bf.promotion.PageList[i + 1]);
                            }
                            else
                            {
                                bf.LastSeletedPage = bf.promotion.PageList[i];
                                bf.CurrentSeletedPage = bf.promotion.PageList[i + 1];
                                bf.ChangeLastRender(bf.promotion.PageList[i]);
                                bf.ChangeCurrentRender(bf.promotion.PageList[i + 1]);
                            }
                            bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i]);
                            bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i + 1]);
                        }
                        else if (i + 1 == bf.promotion.PageList.Count) 
                        {
                            bf.promotion.PageList[i].PageTab = pPageTab;
                            bf.CurrentSeletedPage = null;
                            bf.ChangeCurrentRender(null);
                            if (bf.LastIsLeft)
                            {
                                bf.NextCurrentPage = null;
                                bf.ChangeNextCurrentRender(null);
                                bf.NextSeletedPage = bf.promotion.PageList[i];
                                bf.ChangeNextRender(bf.promotion.PageList[i]);
                            }
                            else 
                            {
                                bf.CurrentSeletedPage = null;
                                bf.ChangeNextCurrentRender(null);
                                bf.LastSeletedPage = bf.promotion.PageList[i];
                                bf.ChangeLastRender(bf.promotion.PageList[i]);
                            }
                            bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i]);
                           // bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i + 1]);
                        }
                        else if (i == bf.promotion.PageList.Count)
                        {
                            if(i - 2 >= 0)
                            {
                                if (bf.LastIsLeft)
                                {
                                    bf.NextCurrentPage = bf.promotion.PageList[i - 1];
                                    bf.ChangeNextCurrentRender(bf.promotion.PageList[i - 1]);
                                    bf.NextCurrentPage = bf.promotion.PageList[i - 2];
                                    bf.ChangeNextRender(bf.promotion.PageList[i - 2]);
                                    bf.NextSeletedPage = bf.promotion.PageList[i - 2];
                                }
                                else
                                {
                                    bf.CurrentSeletedPage = bf.promotion.PageList[i - 1];
                                    bf.ChangeCurrentRender(bf.promotion.PageList[i - 1]);
                                    bf.CurrentSeletedPage = bf.promotion.PageList[i - 2];
                                    bf.ChangeLastRender(bf.promotion.PageList[i - 2]);
                                }
                                bf.index -= 2;
                                bf.LastSeletedPage = bf.promotion.PageList[i - 2];
                                bf.ChangeLastRender(bf.promotion.PageList[i - 2]);
                                bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i - 1]);
                                bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i - 2]);
                            }
                            else if (i - 1 >= 0)
                            {
                                if (bf.LastIsLeft)
                                {
                                    bf.ChangeNextCurrentRender(bf.promotion.PageList[i - 1]);
                                }
                                else
                                {
                                    bf.ChangeCurrentRender(bf.promotion.PageList[i - 1]);
                                }
                                bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i - 1]);
                            }
                            else
                            {
                                if (bf.LastIsLeft)
                                {
                                    bf.NextCurrentPage = null;
                                    bf.ChangeNextCurrentRender(null);
                                }
                                else
                                {
                                    bf.CurrentSeletedPage = null;
                                    bf.ChangeCurrentRender(null);
                                }
                            }
                            
                        }
                        else
                        {
                            if (bf.LastIsLeft)
                            {
                                bf.NextCurrentPage = null;
                                bf.ChangeNextCurrentRender(null);
                            }
                            else
                            {
                                bf.CurrentSeletedPage = null;
                                bf.ChangeCurrentRender(null);
                            }
                            break;
                        }
                    }
                }
                break;
            case PageTabState.Right:
                for (int i = bf.CatalogueLen, j = 1; i < bf.promotion.PageList.Count; i++)
                {
                    if (changeFlg)
                    {
                        bf.promotion.PageList[i].PageTab = pPageTab + j;
                        j++;
                    }
                    if (!changeFlg && !(bf.promotion.PageList[i] is CataloguePage) && bf.promotion.PageList[i].PageTab == pPageTab)
                    {
                        //Debug.Log("Delete is CataloguePage: " + bf.promotion.PageList[i] is CataloguePage);
                        bf.promotion.PageList.RemoveAt(i);
                        Debug.Log("i: " + i + "; Count: " + bf.promotion.PageList.Count);
                        bf.AllModelBtnControllSc.HideAllBtn();
                        if (i < bf.promotion.PageList.Count)
                        {
                            bf.promotion.PageList[i].PageTab = pPageTab;
                            changeFlg = true;
                            if (i + 1 != bf.promotion.PageList.Count)
                            {
                                if (bf.LastIsLeft)
                                {
                                    bf.NextCurrentPage = bf.promotion.PageList[i];
                                    bf.ChangeNextCurrentRender(bf.promotion.PageList[i]);
                                    Debug.Log("NextCurrentPage: " + bf.promotion.PageList[i].PageTab);
                                }
                                else
                                {
                                    Debug.Log("CurrentSeletedPage");
                                    bf.CurrentSeletedPage = bf.promotion.PageList[i];
                                    bf.ChangeCurrentRender(bf.promotion.PageList[i]);
                                }
                                bf.ChangeNextCurrentRender(bf.promotion.PageList[i]);
                                bf.NextCurrentPage = bf.promotion.PageList[i];
                                bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i]);
                            }
                            else 
                            {
                                bf.NextCurrentPage = null;
                                bf.ChangeNextCurrentRender(null);
                                if (!bf.LastIsLeft)
                                {
                                    bf.CurrentSeletedPage = null;
                                    bf.ChangeCurrentRender(null);
                                }
                            }
                            //bf.CurrentSeletedPage = bf.promotion.PageList[i];
                            
                        }
                        else if (i == bf.promotion.PageList.Count)
                        {
                            if (i - 1 >= 0)
                            {
                                Debug.Log(bf.promotion.PageList[i - 1].PageTab);
                                bf.ChangeCurrentRender(null);
                                bf.CurrentSeletedPage = null;
                                bf.NextCurrentPage = null;
                                bf.ChangeNextCurrentRender( null);
                                //bf.NextSeletedPage = bf.promotion.PageList[i - 1];
                                bf.AllModelBtnControllSc.Initialize(bf.promotion.PageList[i - 1]);
                            }
                            else
                            {
                                if (bf.LastIsLeft)
                                {
                                    bf.ChangeNextCurrentRender(null);
                                    bf.NextSeletedPage = null;
                                }
                                else
                                {
                                    bf.ChangeCurrentRender(null);
                                    bf.CurrentSeletedPage = null;
                                }
                            }
                        }
                        else   // 消除最后一页的显示信息
                        {
                            if (bf.LastIsLeft)
                            {
                                bf.ChangeNextCurrentRender(null);
                            }
                            else
                            {
                                bf.ChangeCurrentRender(null);
                            }
                        }
                    }
                }
                break;
            case PageTabState.None:
                for (int i = bf.CatalogueLen, j = 1; i < bf.promotion.PageList.Count; i++)
                {
                    if (changeFlg)
                    {
                        bf.promotion.PageList[i].PageTab = pPageTab + j;
                        j++;
                    }
                    if (!(bf.promotion.PageList[i] is CataloguePage) && bf.promotion.PageList[i].PageTab == pPageTab)
                    {
                        //Debug.Log("Delete: " + bf.promotion.PageList[i].PageTab);
                        bf.promotion.PageList.RemoveAt(i);
                        //Debug.Log("New target: " + bf.promotion.PageList[i].PageTab);
                        bf.promotion.PageList[i].PageTab = pPageTab;
                        bf.promotion.PageList[i + 1].PageTab = pPageTab + j;
                        changeFlg = true;
                    }
                }
                break;
            default:
                break;
        }
        bf.PageNControl_A.HideAllHeightShine();
    }

    public int GetRightKeepIndex() 
    {
        BookInfomation bf = BookInfomation._isntance;
        int res = -1;
        for (int i = 0; i < bf.promotion.PageList.Count; i++)
        {
            if (bf.promotion.PageList[i] == CurrentPage)
            {
                if (i - 1 >= 0)
                {
                    res = bf.promotion.PageList[i].PageTab;
                }
                else 
                {
                    break;
                }
            }
        }
        return res;
    }

    public int GetLefttKeepIndex()
    {
        BookInfomation bf = BookInfomation._isntance;
        int res = -1;
        for (int i = 0; i < bf.promotion.PageList.Count; i++)
        {
            if (bf.promotion.PageList[i] == CurrentPage)
            {
                if (i + 1 < bf.promotion.PageList.Count)
                {
                    res = bf.promotion.PageList[i + 1].PageTab;
                }
                else
                {
                    break;
                }
            }
        }
        return res;
    }

}
