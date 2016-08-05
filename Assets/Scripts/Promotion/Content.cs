using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void TurnPlay(BookInfomation.JumpPageParams param);

public class Content : MonoBehaviour {
    public static bool isLoaded;
    public static bool IsLoading;
    public static TurnPlay turnPlay;

    public Coroutine IEm;
    public Coroutine LoadBgImgCoroutine;     //  加载背景图协成句柄
    public class UpParam
    {
        public int PageTab;
        public bool isCatalogue;
        public Dictionary<string, RawImage> UploadData;

        public UpParam(int pPageTab, bool pIsCatalogue) 
        {
            PageTab = pPageTab;
            isCatalogue = pIsCatalogue;
            UploadData = new Dictionary<string, RawImage>();
        }

    }
    public bool isRight = false;
    public float moveSetUp;
    public GameObject ModelObj;
    public int ModleNum;  //  模板编号

    public List<GameObject> Elements;
    public Dictionary<string, UiItem> Items;

    public const string CODE = "100";
    public const string BTN_CODE = "10";

    public int codeIndex;

    private Vector3 Position;

    public virtual void Awake()
    {
        if (ModelObj != null)
        {
            Position = ModelObj.transform.position;
        }
        IsLoading = false;
    }

    void OnEnbale() 
    {
        if (isRight)
        {
            if (BookInfomation._isntance.LastIsLeft)
            {
                ModelObj.transform.position = Position;
            }
            else
            {
                ModelObj.transform.position = new Vector3(Position.x + moveSetUp, Position.y, Position.z);
            }
        }
        
    }
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public virtual void Init()
    {
        codeIndex = 1;
        //Debug.Log("your mather");
        Items = new Dictionary<string, UiItem>();
        Component com = null;
        ItemType type = ItemType.None;
        //Debug.Log(Elements.Count);

        for (int i = 0; i < Elements.Count; i++)
        {
            com = Elements[i].GetComponent<Text>();
            if (com != null)
            {
                type = ItemType.Text;
            }
            else 
            {
                com = Elements[i].GetComponent<RawImage>();
                if (com != null)
                {
                    type = ItemType.RawImage;
                }
                else 
                {
                    com = Elements[i].GetComponent<ModelBtnGroupControl>();
                    if (com != null)
                    {
                        type = ItemType.Link;
                    }
                }
            }
            //Debug.Log("Name = " + gameObject.name);
            //Debug.Log((CODE + codeIndex) + ": " + com.GetType().ToString());
            Items.Add(CODE + codeIndex, new UiItem(type, com, CODE + codeIndex,""));
            codeIndex++;
        }
    }

    /// <summary>
    /// 加载图片协成
    /// </summary>
    /// <param name="pUrl"></param>
    /// <returns></returns>
    public IEnumerator LoadCurtain(UpParam param) 
    {
        IsLoading = true;
        //Debug.Log("isLoaded = " + isLoaded);
        WWW www;
        Dictionary<string, RawImage>.Enumerator em = param.UploadData.GetEnumerator();
        while (em.MoveNext())
        {
            param.UploadData[em.Current.Key].texture = null;
        }

PagePool.Instance.GetPictureData(param);
        
        em = param.UploadData.GetEnumerator();
        //Debug.Log(param.Count);
        while (em.MoveNext() && param.UploadData[em.Current.Key].texture == null)
        {
            www = new WWW(GetXml.Instances.LoadUrl +  em.Current.Key);
            Debug.Log(GetXml.Instances.LoadUrl + em.Current.Key);
            yield return www;
            //Debug.Log("Count0 : " + param.Count);
            if (www.isDone && www.error == null)
            {
                param.UploadData[em.Current.Key].texture = www.texture;
            }
            else
            {
                //BookInfomation._isntance.TestText.text += "\n" + www.error + "->>  " + param[i].Url;
                Debug.Log(www.error);
                //Debug.Log(param[i].Url);
            }
        }
        IsLoading = false;
        //Debug.Log("LoadCurtain");
        if (isLoaded)
        {
            Debug.Log("isLoaded: turnPlay = null ? " + (CallBack.turnPlay == null));
            isLoaded = false;
            //CallBack.CurrentState = CurrentState;
            //CallBack.turnPlay = turnPlay;
            CallBack.maxPics = param.UploadData.Count / 2;
            CallBack.needCall = true;
        }
        IEm = null;
    }

    public virtual void SetContent(Page bp)
    {
        //Debug.Log("0");
        UpParam iip = null;
        //Debug.Log("1");
        if (bp is CataloguePage)
        {
            iip = new UpParam(bp.PageTab, true);
        }
        else
        {
            iip = new UpParam(bp.PageTab + BookInfomation._isntance.CatalogueLen,false);
        }

        Debug.Log(gameObject.name + ":SetContent");
        //Debug.Log(bp.PageTab + "; SetContent");
        Debug.Log(Items.Count + ";   param.items.count = " + bp.Items.Count);
        GameObject obj;
        foreach (string code in Items.Keys)
        {
            if (bp.Items.ContainsKey(code))
            {
                
                switch (Items[code].type)
                {
                    case ItemType.Text: (Items[code].target as Text).text = bp.Items[code].target as string;
                        //Debug.Log("code: " + code + "; text = " + bp.Items[code].target as string);
                        break;
                    case ItemType.RawImage:
                        if (bp.Items[code].target is string)
                        {
                            //Debug.Log();
                            iip.UploadData.Add(bp.Items[code].target as string, Items[code].target as RawImage);
                        }
                        
                        break;
                    case ItemType.Audio:
                        break;
                    case ItemType.Video:
                        break;
                    case ItemType.None:
                        break;
                }
            }
            else 
            {
                Debug.Log("Page not containsKey:" + code);
            }
        }
        
        if (IEm != null)
        {
            try
            {
                StopCoroutine(IEm);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally 
            {
                IEm = null;
            }
        }
        //Debug.Log("PageTable = " + bp.PageTab);
        //Debug.Log("iip.UploadData.Count = " + iip.UploadData.Count);
        gameObject.SetActive(true);
        //Debug.Log(gameObject.transform.parent.parent.gameObject.name + "-> " + gameObject.name);
        IEm = StartCoroutine(LoadCurtain(iip));
        //Debug.Log("wwwwwwwwwwwwwwwwww");
        //OnEnbale();
    }
}
