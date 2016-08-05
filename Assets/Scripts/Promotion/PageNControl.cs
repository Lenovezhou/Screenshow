using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum UserState 
{
    Bross,
    Edit,
    None
}



public class PageNControl : MonoBehaviour
{
    public static UserState userState;   //  用户状态
    public string AudioUrl { set; get; }  //  音频
    public int PageTab { set; get; }   //  页码
    public int ItemType { set; get; }  //  模板类型
    public string Introduce { set; get; }
    public PageControl selfModelsSc;   //  自身的另一个模板管理脚本
    public GameObject[] HeightShines;   //  高亮物体
    public int ShineIndex;  //  高亮索引
    public Page currentPage;
    public int ModelNumber 
    {
        set 
        {
            _modelNum = value;
            //  刷新数据显示
            selfModelsSc.SetCurrentModel(currentPage);
        }
        get 
        {
            return _modelNum;
        }
    }  //模板编号

    public PageTabState readState;

    public GameObject[] ItemButtonGroups;  //  高亮显示的边框
    public OperationBook operationBookSc;   //  书的管理脚本

    private int _currItem;
    public int currItem 
    {
        set 
        {
            _currItem = value;
        }
        get 
        {
            return _currItem;
        }
    }  //  当前选中的

    private int _modelNum;

	// Use this for initialization
	void Start () {
        //setNull();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


    public void OnMouseDown()
    {
        if(userState == UserState.Edit)
        {
            OperationBook.Instance.readState = readState;
            for (int i = 0; i < HeightShines.Length; i++)
            {
                HeightShines[i].SetActive(false);
            }
            HeightShines[ShineIndex].SetActive(true);
        }
    }

    public void HideAllHeightShine() 
    {
        for (int i = 0; i < HeightShines.Length; i++)
        {
            HeightShines[i].SetActive(false);
        }
        OperationBook.Instance.CurrentPage = null;
    }

    public void SetCurrPage(Page page) 
    {
        currentPage = page;
    }
}
