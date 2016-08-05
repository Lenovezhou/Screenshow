using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 目录
/// </summary>
public class Catalogue : Content {
    public static bool isLoaded;
    public static bool IsLoading; 
    public static TurnPlay turnPlay;
    public static PageState CurrentState;

    public GameObject HeadTab;
    public Text[] PageHeads;   //  名称
    public Text[] PageTabs;   // 页码
    public Text PageTab;
    public RawImage Picture;

    public int firstFontSize;
    public int secondFontSize;
    public int thirdlyFontSize;

    public static Texture r_tex;

    private float f_initX;
    
    private string s_url;
	// Use this for initialization
	void Awake () {
        //Debug.Log(gameObject.name + ":" + PageHeads.Length);
        f_initX = PageHeads[0].gameObject.transform.position.x;
        IsLoading = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public IEnumerator LoadPic(string url)
    {
        IsLoading = true;
        //Debug.Log("isLoaded = " + isLoaded);
        WWW www = new WWW(GetXml.Instances.LoadUrl + url);
        yield return www;
        if (www.isDone && www.error == null)
        {
            Picture.texture = www.texture;
            r_tex = www.texture;
            s_url = url;
        }
        else
        {
            Debug.Log("目录模板图片加载失败:" + GetXml.Instances.LoadUrl + url);
            if (r_tex != null)  
            {
                Picture.texture = r_tex;
                //Debug.Log("Picture.texture = r_tex");
            }
        }
        IsLoading = false;
        //Debug.Log("isLoaded : " + isLoaded);
        yield return new WaitForEndOfFrame();
        //yield return new WaitForEndOfFrame();
        if(isLoaded)
        {
            isLoaded = false;
            //CallBack.CurrentState = CurrentState;
            //CallBack.turnPlay = turnPlay;
            CallBack.maxPics = 1;
            CallBack.needCall = true;
            
            //turnPlay(BookInfomation._isntance.turnParams);
        }
    }

    public void Initialize(CataloguePage cp)
    {
        //Debug.Log(cp.PictureUrl);
        StartCoroutine(LoadPic(cp.PictureUrl));
        for (int i = 0; i < cp.CatalogueItems.Count; i++)
        {
            switch (cp.CatalogueItems[i].rank)
            {
                case ClgRank.First: PageHeads[i].fontSize = firstFontSize;
                    break;
                case ClgRank.Second: PageHeads[i].fontSize = secondFontSize;
                    break;
                case ClgRank.Thirdly: PageHeads[i].fontSize = thirdlyFontSize;
                    break;
            }
            //Debug.Log("(int)rank = " + cp.CatalogueItems[i].rank.ToString());
            PageHeads[i].gameObject.transform.position =
                            new Vector3(f_initX + 0.5f * (int)cp.CatalogueItems[i].rank, PageHeads[i].gameObject.transform.position.y,
                                PageHeads[i].gameObject.transform.position.z);
            PageHeads[i].text = cp.CatalogueItems[i].Head;
            PageTabs[i].text = (cp.CatalogueItems[i].IndexOfIDList + 1).ToString();
            PageHeads[i].gameObject.SetActive(true);
            PageTabs[i].gameObject.SetActive(true);
        }
        if (cp.CatalogueItems.Count < PageHeads.Length)
        {
            for (int i = cp.CatalogueItems.Count; i < PageHeads.Length; i++)
            {
                PageHeads[i].gameObject.SetActive(false);
                PageTabs[i].gameObject.SetActive(false);
            }
        }
        if (cp.PageTab  == 0)
        {
            HeadTab.SetActive(true);
        }
        else 
        {
            HeadTab.SetActive(false);
        }
    }
}
