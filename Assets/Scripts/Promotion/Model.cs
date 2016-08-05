using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Model : MonoBehaviour {
    public Text PageTab;  //  页码

    public GameObject PageTabPar;

    public Catalogue CatalogueSc;

    public List<Content> Contents;

    public Dictionary<int, Content> ModelNumDic;   //  test

    public bool TestCode;

    public ModelType modelType;  //  自身管理的类型

    public GameObject ModelBtnContent;

    public GameObject oPicButtonPrefab;

    public GameObject oPageBtnPrefab;
    public GameObject oModelScrollContent;

    public float fPageWidth;
    public float fPageHeight;

    void Awake() 
    {
        
    }

    public void Init(List<GameObject> models, List<GameObject> btnModels) 
    {
        //Debug.Log("Model->Init.");
        ModelNumDic = new Dictionary<int, Content>();
        Content cont;
        GameObject obj;
        for (int i = 0; i < models.Count; i++)
        {
            //Debug.Log(i + " == null " + (models[i] == null));
            obj = Instantiate(models[i], models[i].transform.position, models[i].transform.rotation) as GameObject;
            
            cont = obj.GetComponent<Content>();
            if (cont != null)
            {
                switch (modelType)
                {
                    case ModelType.Cover:
                        break;
                    case ModelType.Page:
                        break;
                    case ModelType.Catalogue:
                        (cont as Catalogue).PageTab = PageTab;
                        break;
                }
                if(cont is EditCoverContent)
                {
                    EditCoverContent conte = cont as EditCoverContent;
                    conte.oPicButtonPrefab = oPicButtonPrefab;
                    conte.fPageWidth = fPageWidth;
                    conte.fPageHeight = fPageHeight;
                    
                }
                else if (cont is EditContent)
                {
                    EditContent conte = cont as EditContent;
                    conte.oPicButtonPrefab = oPicButtonPrefab;
                    conte.oPicButtonPrefab = oPicButtonPrefab;
                    conte.fPageWidth = fPageWidth;
                    conte.fPageHeight = fPageHeight;
                }
                Debug.Log(":-=>" + cont.ModleNum);
                
                ModelNumDic.Add(cont.ModleNum,cont);
            }
            else 
            {
                Exception ex = new Exception("Model：获取的Content脚本为空！");
                throw ex;
            }
        }

        transform.DetachChildren();
        if(TestCode)
        {
            for (int i = 0; i < Contents.Count; i++)
			{
                Contents[i].gameObject.transform.parent = transform;
                Contents[i].Init();
                RectTransform recf = Contents[i].gameObject.GetComponent<RectTransform>();
                recf.offsetMin = Vector2.zero;
                recf.offsetMax = Vector2.zero;
                recf.localScale = Vector3.one;
			}
        }
        else
        {
            foreach (Content con in ModelNumDic.Values)
            {
                con.gameObject.transform.parent = transform;
                con.Init();
                RectTransform recf = con.gameObject.GetComponent<RectTransform>();
                recf.offsetMin = Vector2.zero;
                recf.offsetMax = Vector2.zero;
                recf.localScale = Vector3.one;
            }
        }

        if (PageTabPar != null)
        {
            PageTabPar.transform.parent = transform;
            RectTransform rect = PageTabPar.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        CreateModelBtn(btnModels);
        
    }


    public virtual void CreateModelBtn(List<GameObject> btnModels) 
    {
        if (btnModels != null)
        {
            GameObject insObj, insobj1;
            ModleBtn btnData;
            ToggleGroup group1 = null, group2 = null;
            if (ModelBtnContent != null)
            {
                group1 = ModelBtnContent.GetComponent<ToggleGroup>();
            }
            if (oModelScrollContent != null)
            {
                group2 = oModelScrollContent.GetComponent<ToggleGroup>();
            }
            for (int i = 0; i < btnModels.Count; i++)
            {
                insObj = Instantiate(btnModels[i]);
                switch (modelType)
                {
                    case ModelType.Cover:
                        insObj.GetComponent<ModleBtn>().CoverModelSc = transform.GetComponent<CoverModelControll>();
                        insObj.GetComponent<Toggle>().group = group1;
                        break;
                    case ModelType.Page:
                        btnData = insObj.GetComponent<ModleBtn>();
                        btnData.pageControllSc = transform.parent.GetComponent<PageControl>();
                        insObj.GetComponent<Toggle>().group = group1;
                        insobj1 = Instantiate(oPageBtnPrefab) as GameObject;
                        insobj1.GetComponent<Toggle>().group = group2;
                        insobj1.GetComponent<ChooseModleBtn>().ModelNum = btnData.ModelNum;
                        insobj1.transform.GetChild(1).GetComponent<Image>().sprite = insObj.transform.GetChild(1).GetComponent<Image>().sprite;
                        insobj1.transform.parent = oModelScrollContent.transform;
                        insobj1.SetActive(true);
                        break;
                    case ModelType.Catalogue:
                        break;
                }
                insObj.transform.parent = ModelBtnContent.transform;
            }
        }
    }

	// Use this for initialization
	void Start () 
    {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    /// <summary>
    /// 初始化或者刷新页的显示
    /// </summary>
    /// <param name="pBookPage"></param>
    public void RefreshData(Page pBookPage)
    {
        Page pg;
        //Debug.Log("RefreshData");
        if (pBookPage is Page)
        {
            pg = (Page)pBookPage;
            if (pg.Catalogue)   //  如果是目录
            {
                CataloguePage clp = (CataloguePage)pg;
                if (TestCode)
                {
                    CatalogueSc = Contents[pBookPage.ModelNumber] as Catalogue;
                }
                else 
                {
                    CatalogueSc = ModelNumDic[pBookPage.ModelNumber] as Catalogue;
                }
                
                CatalogueSc.gameObject.SetActive(true);
                CatalogueSc.Initialize(clp);
                CatalogueSc.PageTab.text = (clp.PageTab + 1).ToString();
            }
            else 
            {
                PageTab.text = "- " + (pBookPage.PageTab + 1).ToString() + " -";
                //Debug.Log("RefreshData-> " + pg.Catalogue);
                if (TestCode)
                {
                    //Debug.Log("RefreshData-> is Test : contents array");
                    //Debug.Log(Contents[pBookPage.ModelNumber] == null);
                    Contents[pBookPage.ModelNumber].SetContent(pBookPage);
                }
                else
                {
                    ModelNumDic[pBookPage.ModelNumber].SetContent(pBookPage);
                }
            }
        }
    }
}
