using UnityEngine;
using System.Collections;

public class CatalogueBtnControl : MonoBehaviour {
    public BookInfomation bookInfomationSc;  //  
    public CatalogueBtn[] CatalogueBtnScs;   //  所有按钮身上的脚本
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Refresh(CataloguePage pCataloguePage) 
    {
        for (int i = 0; i < CatalogueBtnScs.Length; i++)
        {
            CatalogueBtnScs[i].gameObject.SetActive(false);
            CatalogueBtnScs[i].bookInfomationSc = null;
        }
        if (pCataloguePage != null)
        {
            //Debug.Log("currTab = " + pCataloguePage.PageTab);
            for (int i = 0; i < pCataloguePage.CatalogueItems.Count && i < CatalogueBtnScs.Length; i++)
            {
                CatalogueBtnScs[i].TargetPageTab = pCataloguePage.CatalogueItems[i].IndexOfIDList;
                CatalogueBtnScs[i].gameObject.SetActive(true);
                CatalogueBtnScs[i].bookInfomationSc = bookInfomationSc;
            }
        }
        
    }
}
