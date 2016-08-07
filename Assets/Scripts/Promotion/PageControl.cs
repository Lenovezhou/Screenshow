using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PageControl : MonoBehaviour {

    public GameObject   PageModel;  //  正面模板的物体
    public GameObject   CatalogueModel;  //  正面模板的物体

    public GameObject[] ButtonGroups;  //  正面模板的按钮
    //public Content[] ModelContents;   //  模板的容器
    public int NummberMax;

    private Model _pageModelSc;
    private Model _catalogueModelSc;

    public static PageControl CurrentPageControl;  // 当前选中的管理脚本
	void Awake ()
    {
        _pageModelSc = PageModel.GetComponent<Model>();
        if (CatalogueModel != null)
        {
            _catalogueModelSc = CatalogueModel.GetComponent<Model>();
        }
        
	}

    public virtual void Init(MsgCenter_h.InitModelParam param) 
    {
        _pageModelSc.Init(param.pageModels,null);
        _catalogueModelSc.Init(param.cataModels,null);
    }


    /// <summary>
    /// 根据参数换模板
    /// </summary>
    /// <param name="pIndex"></param>
    public void SetCurrentModel(Page pBookPage)
    {
        if (pBookPage != null)
        {
            CataloguePage caPage = pBookPage is CataloguePage ? (CataloguePage)pBookPage : null;
            if (CatalogueModel != null)
            {
                if (caPage == null)
                {
                    PageModel.SetActive(true);
                    CatalogueModel.SetActive(false);
                }
                else
                {
                    CatalogueModel.SetActive(true);
                    PageModel.SetActive(false);
                }
            }
            
            // 显示对应的项
            if (pBookPage.ModelNumber >= 0 && pBookPage.ModelNumber < NummberMax)
            {
                Model md = null;
                if (pBookPage.Catalogue)
                {
                    md = _catalogueModelSc;
                }
                else
                {
                    md = _pageModelSc;
                }
                Debug.Log("caPage == null ? " + (caPage == null));
                if(caPage == null)
                {
                    if (md.TestCode)
                    {
                        for (int i = 0; i < md.Contents.Count; i++)
                        {
                            if (md.Contents[i].ModleNum == pBookPage.ModelNumber)
                            {
                                md.Contents[i].gameObject.SetActive(true);
                            }
                            else
                            {
                                md.Contents[i].gameObject.SetActive(false);
                            }
                        }
                    }
                    else 
                    {
                        foreach (Content con in md.ModelNumDic.Values)
                        {
                            if (con.ModleNum == pBookPage.ModelNumber)
                            {
                                con.gameObject.SetActive(true);
                            }
                            else
                            {
                                con.gameObject.SetActive(false);
                            }
                        }
                    }
                }
                //  显示按钮
                DisplayButtons(pBookPage.ModelNumber, pBookPage.ModelNumber);
             
                // 刷新对应的模板数据
                md.RefreshData(pBookPage);
            }
            else 
            {
                Debug.Log("Numm:" + pBookPage.ModelNumber);
            }
        }
    }

    /// <summary>
    /// 设置显示下标小于pIndex的Button组
    /// </summary>
    /// <param name="pIndex"></param>
    public void DisplayButtons(int pIndex,int pType) 
    {

        for (int i = 0; i < ButtonGroups.Length; i++)
        {
            if (i == pIndex)
            {
                ButtonGroups[i].SetActive(true);
                for (int j = 0; j < ButtonGroups[i].transform.GetChildCount(); j++)
                {
                    if (j <= pType)
                    {
                        ButtonGroups[i].transform.GetChild(j).gameObject.SetActive(true);
                    }
                    else 
                    {
                        ButtonGroups[i].transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                ButtonGroups[i].SetActive(false);
            }
        }
    }   
}
