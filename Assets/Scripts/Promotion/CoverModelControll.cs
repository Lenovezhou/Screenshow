using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CoverModelControll : Model {
    public const string CODE = "100";
    public int codeIndex;
    public Texture DefualtCover2;
    public Texture DefaultCover3;

    public class BookCoverParams
    {
        public BookCover bookCover;
        public Content coverModelSc;
        public GameObject SetActiveObj;

        public Texture CoverImg;
        public Texture LogoImg;
        public Texture AsideImg;

        public BookCoverParams(BookCover pbookCover, Content pcoverModelSc) 
        {
            bookCover = pbookCover;
            coverModelSc = pcoverModelSc;
        }
        public BookCoverParams(BookCover pbookCover, Content pcoverModelSc, GameObject pSetActiveObj)
        {
            bookCover = pbookCover;
            coverModelSc = pcoverModelSc;
            SetActiveObj = pSetActiveObj;
        }

        public override bool Equals(object obj)
        {
            if (obj is BookCoverParams)
            {
                 BookCoverParams bcp = (BookCoverParams)obj;
                if(bcp.bookCover == this.bookCover)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
        }
    }
    public GameObject BookRoot;
    
    public Material[] CoverMats;

    public CoverContent[] Model;
    public RawImage rawImg;  
    public BookCover CurrentCover
    {
        set 
        {
            _currentCover = value;
            RefreshCoverModel(value,true);
        }
        get 
        {
            return _currentCover;
        }
    }
    
    public static CoverModelControll Instance;
    private BookCover _currentCover;
    private Dictionary<BookCover,BookCoverParams> _coverBuffer;
    private bool _loadFlg;
	// Use this for initialization
	void Awake () 
    {
        Instance = this;
        _loadFlg = false;
        _coverBuffer = new Dictionary<BookCover, BookCoverParams>();
	}

	// Update is called once per frame
	void Update () {
        //Debug.Log("InitializeModel : " + Models[0].Items == null);
	}
    /// <summary>
    /// 刷新封面模板的显示
    /// </summary>
    public void RefreshCoverModel(BookCover pCover,bool isDisplay) 
    {
        //Debug.Log((pCover == null) + ";" + ";");
        if (ModelNumDic.ContainsKey(pCover.ModelNumber))
        {
            foreach (int key in ModelNumDic.Keys)
            {
                if (pCover.ModelNumber != key)
                {
                    ModelNumDic[key].gameObject.SetActive(false);
                }
                else 
                {
                    ModelNumDic[key].gameObject.SetActive(true);
                }
            }
            BookCoverParams bcp = new BookCoverParams(pCover, ModelNumDic[pCover.ModelNumber], isDisplay ? BookRoot : null);
            //Debug.Log("bcp.items == null ? " + bcp.Items == null);
            InitializeModel(bcp);
        }
    }

    public void InitializeModel(BookCoverParams pBook)
    {
        if (pBook.SetActiveObj != null)
        {
            pBook.SetActiveObj.SetActive(false);
        }
        
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nCoverModelControll->InitializeModel";
        foreach (string item in pBook.bookCover.Items.Keys)
        {
            //Debug.Log(pBook.coverModelSc == null);
            if (pBook.coverModelSc.Items.ContainsKey(item))
            {
                if (pBook.coverModelSc.Items[item].target is Text)
                {
                    (pBook.coverModelSc.Items[item].target as Text).gameObject.SetActive(false);
                }
                else if (pBook.coverModelSc.Items[item].target is RawImage)
                {
                    (pBook.coverModelSc.Items[item].target as RawImage).gameObject.SetActive(false);
                }
                else 
                {
                    Debug.Log(pBook.coverModelSc.Items[item].target.GetType().ToString());
                }
            }
        }
        
        StartCoroutine(LoadPicture(pBook));
        foreach (string item in pBook.bookCover.Items.Keys)
        {
            if (pBook.coverModelSc.Items.ContainsKey(item) && pBook.coverModelSc.Items[item].type == ItemType.Text)
            {
                if (pBook.coverModelSc.Items[item].target is Text)
                {
                    Text tx = pBook.coverModelSc.Items[item].target as Text;
                    tx.text = pBook.bookCover.Items[item].target as string;
                    //Debug.Log("text : " + tx.text);
                    tx.gameObject.SetActive(true);
                }
                else if (pBook.coverModelSc.Items[item].target is InputField)
                {
                    InputField tx = pBook.coverModelSc.Items[item].target as InputField;
                    tx.text = pBook.bookCover.Items[item].target as string;
                    tx.GetComponent<UpdataEvent>().LeyoutID = pBook.bookCover.Items[item].leyoutID;
                    //Debug.Log("text : " + tx.text);
                    tx.gameObject.SetActive(true);
                }
            }
        }
        
    }

    public IEnumerator LoadPicture(BookCoverParams pBook)
    {
        WWW www;
        bool addFlg = true;
        RawImage img;
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nLoadPicture[Start]:";
        foreach (string item in pBook.bookCover.Items.Keys)
        {
            if (pBook.coverModelSc.Items.ContainsKey(item) && pBook.coverModelSc.Items[item].type == ItemType.RawImage)
            {
                
                www = new WWW(GetXml.Instances.LoadUrl + pBook.bookCover.Items[item].target as string);
                yield return www;
                //Debug.Log(www.url); 
                if (www.error == null && www.isDone)
                {
                    img = pBook.coverModelSc.Items[item].target as RawImage;
                    img.texture = www.texture;
                    img.gameObject.SetActive(true);
                }
                else 
                {
                    Debug.Log("err:" + www.error);
                    addFlg = false;
                }
                www.Dispose();
            }
        }

        if (string.IsNullOrEmpty(pBook.bookCover.FrontUrl))
        {
            for (int i = 0; i < 2; i++)
            {
                if (pBook.coverModelSc is CoverContent)
                {
                    (pBook.coverModelSc as CoverContent ).CoverMats[i].mainTexture = DefualtCover2;
                }
                else if (pBook.coverModelSc is EditCoverContent)
                {
                    (pBook.coverModelSc as EditCoverContent).CoverMats[i].mainTexture = DefualtCover2;
                }
            }
        }
        else 
        {
            www = new WWW(GetXml.Instances.LoadUrl + pBook.bookCover.FrontUrl);
            yield return www;
            if (www.error == null && www.isDone)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (pBook.coverModelSc is CoverContent)
                    {
                        (pBook.coverModelSc as CoverContent).CoverMats[i].mainTexture = www.texture;
                    }
                    else if (pBook.coverModelSc is EditCoverContent)
                    {
                        (pBook.coverModelSc as EditCoverContent).CoverMats[i].mainTexture = www.texture;
                    }
                }
                pBook.AsideImg = www.texture;
            }
            else
            {
                addFlg = false;
            }
            www.Dispose();
        }

        if (string.IsNullOrEmpty(pBook.bookCover.BehindUrl))
        {
            if (pBook.coverModelSc is CoverContent)
            {
                CoverContent cc = (pBook.coverModelSc as CoverContent);
                for (int i = 2; i < cc.CoverMats.Length; i++)
                {
                    cc.CoverMats[i].mainTexture = DefaultCover3;
                }
            }
            else if (pBook.coverModelSc is EditCoverContent)
            {
                EditCoverContent cc = (pBook.coverModelSc as EditCoverContent);
                for (int i = 2; i < cc.CoverMats.Length; i++)
                {
                    cc.CoverMats[i].mainTexture = DefaultCover3;
                }
            }
            
        }
        else
        {
            www = new WWW(GetXml.Instances.LoadUrl + pBook.bookCover.BehindUrl);
            yield return www;
            if (www.error == null && www.isDone)
            {
                if (pBook.coverModelSc is CoverContent)
                {
                    CoverContent cc = (pBook.coverModelSc as CoverContent);
                    for (int i = 2; i < cc.CoverMats.Length; i++)
                    {
                        cc.CoverMats[i].mainTexture = www.texture;
                    }
                }
                else if (pBook.coverModelSc is EditCoverContent)
                {
                    EditCoverContent cc = (pBook.coverModelSc as EditCoverContent);
                    for (int i = 2; i < cc.CoverMats.Length; i++)
                    {
                        cc.CoverMats[i].mainTexture = www.texture;
                    }
                }
                pBook.AsideImg = www.texture;
            }
            else
            {
                addFlg = false;
            }
            www.Dispose();
        }
        

        if (addFlg && !_coverBuffer.ContainsKey(pBook.bookCover))
        {
            _coverBuffer.Add(pBook.bookCover, pBook);
        }
        
        pBook.coverModelSc.gameObject.SetActive(true);
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nLoadPicture[Cover]:over; pBook.SetActiveObj != null == " + (pBook.SetActiveObj != null);
        if (pBook.SetActiveObj != null)
        {
            pBook.SetActiveObj.SetActive(true);
        }
        pBook.SetActiveObj = BookRoot;
        _loadFlg = false;
    }
    /// <summary>
    /// 从缓冲区中读取书封面并刷新
    /// </summary>
    /// <param name="pCover"></param>
    public void RefreshPictures(BookCoverParams pCover)
    {
        if (pCover.coverModelSc is CoverContent)
        {
            CoverContent cc = (pCover.coverModelSc as CoverContent);
            cc.ConverPicture.texture = pCover.CoverImg;
            cc.LogoPicture.texture = pCover.LogoImg;
            //rawImg.texture = pCover.LogoImg;
            //Debug.Log(pCover.coverModelSc.LogoPicture.transform.parent.parent.gameObject.name + "->" + pCover.coverModelSc.LogoPicture.transform.parent.gameObject.name + ":" + pCover.coverModelSc.LogoPicture.gameObject.name);
            for (int i = 0; i < cc.CoverMats.Length; i++)
            {
                cc.CoverMats[i].mainTexture = pCover.AsideImg;
            }
        }
        else if (pCover.coverModelSc is EditCoverContent)
        {
            EditCoverContent cc = (pCover.coverModelSc as EditCoverContent);
            cc.ConverPicture.texture = pCover.CoverImg;
            cc.LogoPicture.texture = pCover.LogoImg;
            //rawImg.texture = pCover.LogoImg;
            //Debug.Log(pCover.coverModelSc.LogoPicture.transform.parent.parent.gameObject.name + "->" + pCover.coverModelSc.LogoPicture.transform.parent.gameObject.name + ":" + pCover.coverModelSc.LogoPicture.gameObject.name);
            for (int i = 0; i < cc.CoverMats.Length; i++)
            {
                cc.CoverMats[i].mainTexture = pCover.AsideImg;
            }
        }
        pCover.coverModelSc.gameObject.SetActive(true);
        pCover.SetActiveObj.SetActive(true);
    }
}
