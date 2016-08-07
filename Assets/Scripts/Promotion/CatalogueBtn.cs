using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CatalogueBtn : MonoBehaviour {
    public int TargetPageTab = -1;
    public BookInfomation bookInfomationSc;  //
    public Text SelfText;  // 显示内容
    public InputField targetInput;  //  跳转的输入框
    public static CatalogueBtnContent CatalogueBtnContentSc;
    public Image selfImage;

    private bool _isJump;
	// Use this for initialization
	void Start () {
        _isJump = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(_isJump)
        {
            TargetPageTab--;
            bookInfomationSc.JumpPageTab(TargetPageTab);
            //Debug.Log("JumpPageTab");
            CatalogueBtnContentSc.SetTargetBtnShine(TargetPageTab);
            _isJump = false;
        }
	}

    public void Click() 
    {
        int x = BookInfomation._isntance.index;
        if (!BookInfomation._isntance.IsPlaying() || BookInfomation._isntance.index == 0)
        {
            bookInfomationSc.JumpPageTab(TargetPageTab);
            Debug.Log("JumpPageTab");
            CatalogueBtnContentSc.SetTargetBtnShine(TargetPageTab);
            if(x == 0)
            {
                BookInfomation._isntance.BookPageA.SetActive(true);
            }
        }
        else 
        {
            Debug.Log("IsPlaying!");
        }
    }

    public void JumpBtn_Click() 
    {
        if (targetInput.text != "")
        {
            TargetPageTab = int.Parse(targetInput.text) - 1;
            Click();
        }
    }

    public void NextJump_click() 
    {
        if (targetInput.text != "")
        {
            bookInfomationSc.TurnLeft(new BookInfomation.JumpPageParams(true,1));
        }
    }

    public void BackJump_click()
    {
        if (targetInput.text != "")
        {
            bookInfomationSc.TurnRight(new BookInfomation.JumpPageParams(true, 1));
        }
    }


    public void After_click() 
    {
        TargetPageTab = BookInfomation._isntance.promotion.PageList.Count - BookInfomation._isntance.CatalogueLen;
        targetInput.text = TargetPageTab.ToString();
        Click();
    }

    public void First_click()
    {
        TargetPageTab = 0;
        targetInput.text = "1";
        Click();
    }
}
