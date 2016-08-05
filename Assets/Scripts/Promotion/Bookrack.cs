using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


public class Bookrack : MonoBehaviour {
    public static Bookrack Instance;

    public Texture DefaultCoverTexture1;  //  默认的封面图片
    public Texture DefaultCoverTexture2;  //  默认的封面图片
    public Texture DefaultCoverTexture3;  //  默认的封面图片
    public Promotion CurrentBook;

    public Button AddBtn;
    public Button DeleteBtn;
    public Button BrossBtn;
    public Button ReturnBtn;
    public Button EditBtn;

    public Model CoverModel;

    public GameObject RedactView;

    public float InvTiem = 4.0f;  //  切换书所用的时间
    public bool AutoPlay;   //  是否自动播放

    public RenderTexture rt;   //  显示封面的renderTexture

    public BackGroundModelControll BackGroundModelControllSc;   //  背景装饰管理脚本
    public List<BookProfebSc> BookProfebScs;   //  每本书身上的脚本 

    public GameObject BookRoot;

    private int _currentBookIndex;
    public int CurrentBookIndex 
    {
        set 
        {
            _currentBookIndex = value;
            CurrentBook = null;
            if(value >= 0 && value < _bookList.Count)
            {
                CurrentBook = _bookList[value];
                BackGroundModelControllSc.DisplayModel(CurrentBook.DecorativeNumber);
            }
        }
        get 
        {
            return _currentBookIndex;
        }
    }

    //  书书籍列表
    private List<Promotion> _bookList;
    public List<Promotion> BookList 
    {
        set 
        {
            _bookList = value;
            Initialize(value);
        }
        get 
        {
            return _bookList;
        }
    }
    public GameObject BookPrefab;  //  书籍的预设
    public GameObject BookContent;  //  书的容器--->书架

    public Dictionary<RawImage, string> Imgs;  //  要加载的图片列表
    public List<Image> AllBookImgs;

    private bool _loadImgFlg;
    private bool _openFlg;
    private float _dialTime;  //  积累的时间
    private int _index;  // 当前切换到的列表下标索引
    void Awake() 
    {
        Instance = this;
        _loadImgFlg = false;
        _openFlg = false;
        _index = 0;
        _dialTime = 0;
    }

	// Use this for initialization
	void Start () 
    {
        BookProfebScs = new List<BookProfebSc>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(AutoPlay)
        {
            _dialTime += Time.deltaTime;
            if (_openFlg && _dialTime >= InvTiem)
            {
                BrossBook();
                _openFlg = false;
                _dialTime = 0;
            }
            else 
            {
                if (_dialTime >= InvTiem)
                {
                    _dialTime = 0;
                    BookProfebScs[_index].Click();
                    _openFlg = true;
                    _index++;
                    if (_index >= BookProfebScs.Count)
                    {
                        _index = 0;
                    }
                }
            }
        }
        if (_loadImgFlg && BookProfebSc.LoadedCount == BookList.Count)
        {
            BrossBtn.interactable = true;
            AddBtn.interactable = true;   //  测试代码
            DeleteBtn.interactable = true;  //  测试代码
            EditBtn.interactable = true;
            _loadImgFlg = false;
            BookProfebSc.LoadedCount = 0;
        }

        if (!UpService.UpInstance.IsAddingBook())
        {
            AddBtn.interactable = true;
        }
	}

    public void OnDisable()
    {
        _dialTime = 0;
    }

    public void Initialize(List<Promotion> pBookList)
    {
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nInitialize:book_count = " + pBookList.Count;
        GameObject obj;
        RawImage img;
        BookProfebSc bpc;
        Imgs = new Dictionary<RawImage, string>();
        AllBookImgs = new List<Image>();
        for (int i = 0; i < pBookList.Count; i++)
        {
            obj = Instantiate(BookPrefab, BookPrefab.transform.position, BookPrefab.transform.rotation) as GameObject;
            //Debug.Log("bookrack:" + (CoverModel.ModelNumDic == null));
            Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nCoverModel.ModelNumDic.count = " + CoverModel.ModelNumDic.Count;

            if (CoverModel.ModelNumDic.ContainsKey(pBookList[i].SelfCover.ModelNumber))
            {
                BookProfebScs.Add(obj.GetComponent<BookProfebSc>());
                img = obj.transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
                img.texture = DefaultCoverTexture1;
                Imgs.Add(img, pBookList[i].SelfCover.PictureUrl);

                AllBookImgs.Add(obj.GetComponent<Image>());
                bpc = obj.GetComponent<BookProfebSc>();
                bpc.PicUrl = pBookList[i].SelfCover.PictureUrl;
                bpc.CurrentModelSc = CoverModel.ModelNumDic[pBookList[i].SelfCover.ModelNumber];
                bpc.Index = i;
                bpc.selfCover = pBookList[i].SelfCover;
                
                if(i == 0)
                {
                    bpc.Click();
                }
                //Imgs.Add(cm.ConverPicture, pBookList[i].SelfCover.PictureUrl);
                obj.transform.parent = BookContent.transform;
                obj.SetActive(true);
            }
            else 
            {
                string str = "";
                foreach (int key in CoverModel.ModelNumDic.Keys)
                {
                    str += "  " + key.ToString();
                }
                str += "\n";
                Camera.main.GetComponent<MsgCenter_h>().TestText.text += str;
            }
        }
        _loadImgFlg = true;
        StartCoroutine(LoadImgs());
        //Bookrack.Instance.BrossBook();
    }

    public IEnumerator LoadImgs()
    {
        WWW www;
        Dictionary<RawImage,string>.Enumerator en = Imgs.GetEnumerator();
        while(en.MoveNext())
        {
            www = new WWW(GetXml.Instances.LoadUrl + en.Current.Value);
            yield return www;
            if (www.error == null && www.isDone)
            {
                en.Current.Key.texture = www.texture;
                en.Current.Key.gameObject.transform.parent.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("Error:加载失败." + (www.error == null ? "www  is not done." : www.error + ":" + www.url));
            }
        }
        Camera.main.GetComponent<MsgCenter_h>().TestText.text += "\nLoadImgs over";
        //_loadImgFlg = false;
        //BrossBook();  //  测试
        //SetAutoPlay(true);
    }
    ///
#region         >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  接  口  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    ///  获取当前的书的封面图片
    /// 
    public byte[] GetCurBookCover() 
    {
        return GetPngByte();
    }
    /// <summary>
    /// 设置是否自动播放
    /// </summary>
    /// <param name="autoFlg"></param>
    public void SetAutoPlay(bool autoFlg)
    {
        AutoPlay = autoFlg;
    }

    /// <summary>
    /// 添加书
    /// </summary>
    public void AddBook()
    {
        if (!_loadImgFlg && !UpService.UpInstance.IsAddingBook())
        {
            BookCover SelfCover = new BookCover();
            List<Page> bop = new List<Page>();
            Promotion boo = new Promotion(SelfCover, bop, 0);
            _bookList.Add(boo);
            UpService.UpInstance.AddBook(boo,AddLastToBookrack(SelfCover));
        }
        else if (UpService.UpInstance.IsAddingBook())
        {
            AddBtn.interactable = false;
        }
    }
    /// <summary>
    /// 删除书
    /// </summary>
    public void DeleteBook()
    {
        if (!_loadImgFlg && _currentBookIndex >= 0 && _currentBookIndex < _bookList.Count)
        {
            _bookList.RemoveAt(_currentBookIndex);
            Dictionary<RawImage, string>.Enumerator enm = Imgs.GetEnumerator();
            int i = 0;
            GameObject obj;
            while (enm.MoveNext())
            {
                if (i == _currentBookIndex)
                {
                    obj = enm.Current.Key.gameObject.transform.parent.gameObject;
                    Imgs.Remove(enm.Current.Key);
                    Destroy(obj);
                    AllBookImgs.RemoveAt(_currentBookIndex);
                    break;
                }
                i++;
            }
        }
    }
    /// <summary>
    /// 浏览书
    /// </summary>
    public void BrossBook()
    {
        if (!_loadImgFlg && _currentBookIndex >= 0 && _currentBookIndex < _bookList.Count)
        {
            gameObject.SetActive(false);
            BookInfomation._isntance.promotion = CurrentBook;
            BookInfomation._isntance.CanTurn = true;
            BookInfomation._isntance.AutoFlg = AutoPlay;
            OperationBook.Instance.CreateCatalogue(CurrentBook);
            ReturnBtn.interactable = true;
            //Debug.Log(BookInfomation._isntance.promotion.PageList.Count);
        }
    }
    /// <summary>
    /// 编辑书
    /// </summary>
    public void EditBook()
    {
        if (!_loadImgFlg && _currentBookIndex >= 0 && _currentBookIndex < _bookList.Count)
        {
            gameObject.SetActive(false);
            RedactControll.Instance.Init(CurrentBook);
            RedactView.SetActive(true);
            BookRoot.SetActive(false);
            //Debug.Log(BookInfomation._isntance.promotion.PageList.Count);
        }
    }
    
#endregion        <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    /// <summary>
    /// 添加一本书
    /// </summary>
    /// <param name="cover"></param>
    public BookProfebSc AddLastToBookrack(BookCover cover)
    {
        GameObject obj = Instantiate(BookPrefab, BookPrefab.transform.position, BookPrefab.transform.rotation) as GameObject;
        BookProfebSc sc = obj.GetComponent<BookProfebSc>();
        sc.Index = _bookList.Count - 1;
        sc.DefaultTexture = DefaultCoverTexture1;
        AllBookImgs.Add(obj.GetComponent<Image>());
        sc.selfCover = cover;
        //Imgs.Add(ri, _bookList[_bookList.Count - 1].SelfCover.PictureUrl);
        obj.transform.parent = BookContent.transform;
        obj.SetActive(true);
        obj.transform.GetChild(0).gameObject.SetActive(true);
        return sc;
    }

    byte[] GetPngByte()
    {
        RenderTexture.active = rt;
        Texture2D tx = new Texture2D(rt.width,rt.height,TextureFormat.RGB24,false);
        tx.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);
        tx.Apply();
        return tx.EncodeToPNG();
    }

    public string GetAttribute(XmlNode node) 
    {
        int res = int.Parse(node.Attributes["sequ"].Value as string);
        if (res >= 0)
        {
            return "0";
        }
        else 
        {
            return  "";
        }
    }

    Texture2D GetTexture()
    {
        RenderTexture.active = rt;
        Texture2D tx = new Texture2D(rt.width, rt.height,TextureFormat.RGB24,false);
        tx.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tx.Apply();
        return tx;
    }
}
