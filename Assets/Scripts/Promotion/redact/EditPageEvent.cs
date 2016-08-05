using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EditPageEvent : MonoBehaviour {
    public bool OnAddPageMessage;
    public Page MsgPage;

    public GameObject ModelView;
    public PageControl pageControlSc;

    public Model PageModelSc;

	// Use this for initialization
	void Awake ()
    {
        OnAddPageMessage = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(OnAddPageMessage)
        {
            OnAddPageMessage = false;
            UpService.UpInstance.AddPage(MsgPage);
        }
	}

    public void AddPageMessage(Page pMsgPage) 
    {
        MsgPage = pMsgPage;
        OnAddPageMessage = true;
    }

    public void AddPageSuccess(MessageBox box, MessageParam msg) 
    {
        RedactControll.Instance.Book.PageList.Add(MsgPage);
        pageControlSc.SetCurrentModel(MsgPage);
        RedactControll.Instance.AddPageTog(MsgPage);
        ModelView.SetActive(false);
        box.ShowMessage(msg.param);
    }

    public void AddPageLoser(MessageBox box,MessageParam msg)
    {
        box.ShowMessage(msg.param);
    }

    public void AddBtn_Ckick() 
    {
        ModelView.SetActive(!ModelView.activeSelf);
    }

    public void DeleteBtn_Ckick()
    {
        UpService.UpInstance.DeletePage(RedactControll.Instance.CurrPage.ID);
        RedactControll.Instance.DeletePageTog();
    }

    public void Init(MsgCenter_h.InitModelParam param) 
    {
        PageModelSc.Init(param.editPages,param.editPageBtns);
    }
}
