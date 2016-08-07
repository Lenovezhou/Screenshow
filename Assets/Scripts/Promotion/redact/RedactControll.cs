using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RedactControll : MonoBehaviour {
    

    public static RedactControll Instance;

    public Page CurrPage;
    public BookCover CurrCover;
    public CataloguePage CurrCata;
    public string CurrLeyoutID;

    public GameObject PagePrefab;
    public GameObject PageContent;
    public GameObject EditPage;

    public GameObject Menu;

    private Promotion _book;
    public Promotion Book 
    {
        set 
        {
            CurrCover = value.SelfCover;
            _book = value;
        }
        get 
        {
            return _book;
        }
    }

    public List<PageBtn> pageBtns;

    private PageControl _pageControll;
	// Use this for initialization
	void Awake () 
    {
        Instance = this;
        _pageControll = EditPage.GetComponent<PageControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(Promotion book) 
    {
        GameObject obj;
        PageBtn pageBtn;
        pageBtns = new List<PageBtn>();
        for (int i = 0,j = 0; i < book.PageList.Count; i++)
        {
            if (!(book.PageList[i] is CataloguePage))
            {
                obj = Instantiate(PagePrefab,PagePrefab.transform.position,PagePrefab.transform.rotation) as GameObject;
                pageBtn = obj.GetComponent<PageBtn>();
                pageBtn.page = book.PageList[i];
                pageBtns.Add(pageBtn);
                obj.transform.parent = PageContent.transform;
                obj.SetActive(true);
                j++;
            }
        }
        Menu.SetActive(true);
        Book = book;
    }

    public void AddPageTog(Page page) 
    {
        GameObject obj = Instantiate(PagePrefab, PagePrefab.transform.position, PagePrefab.transform.rotation) as GameObject;
        PageBtn pageBtn = obj.GetComponent<PageBtn>();
        pageBtn.page = page;
        obj.transform.parent = PageContent.transform;
        obj.SetActive(true);
        pageBtn.Click(obj.GetComponent<Toggle>());
    }

    public void DeletePageTog() 
    {
        for (int i = 0; i < pageBtns.Count; i++)
        {
            if (pageBtns[i].page.ID == CurrPage.ID)
            {
                Debug.Log("delete:--=> " + pageBtns[i].page.ID + "::" + pageBtns[i].page.Name + "::" + pageBtns[i].page.Sequ);
                Destroy(pageBtns[i].gameObject);
                pageBtns.RemoveAt(i);
                if (i < pageBtns.Count)
                {
                    _pageControll.SetCurrentModel(pageBtns[i].page);
                }
                else
                {
                    _pageControll.SetCurrentModel(pageBtns[pageBtns.Count - 1].page);
                }
                break;
            }
            else 
            {
                Debug.Log(pageBtns[i].page.Name + "----" + pageBtns[i].page.Sequ);
            }
        }
        for (int i = 0; i < Book.PageList.Count; i++)
        {
            if (Book.PageList[i] == CurrPage)
            {
                Book.PageList.RemoveAt(i);
                if (i < Book.PageList.Count)
                {
                    CurrPage = Book.PageList[i];
                }
                else 
                {
                    CurrPage = Book.PageList[Book.PageList.Count - 1];
                }
                break;
            }
        }

    }

}
