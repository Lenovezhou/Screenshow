using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class PageBtn : MonoBehaviour {
    public Text NameTex;
    public Text PageTabText;
    public InputField NameInput;
    public GameObject ModleContent;
    public Page page { set; get; }
    public PageControl pageControlSc;

    public Dictionary<int, ModleBtn> ModelBtns;

    private bool _lastIsTrue;
	// Use this for initialization
	void OnEnable ()
    {
        NameTex.text = page.Name;
        NameInput.text = page.Name;
        PageTabText.text = (page.PageTab + 1).ToString();
        _lastIsTrue = false;
    }

    public virtual void Start() 
    {
        GameObject obj;
        ModleBtn modleBtn;
        ModelBtns = new Dictionary<int, ModleBtn>();
        for (int i = 0; i < ModleContent.transform.GetChildCount(); i++)
        {
            if (ModleContent.transform.GetChild(i).gameObject.activeSelf)
            {
                modleBtn = ModleContent.transform.GetChild(i).GetComponent<ModleBtn>();
                ModelBtns.Add(modleBtn.ModelNum, modleBtn);
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
	    
	}

    public virtual void Click(Toggle tog) 
    {
        if (page != null && tog.isOn)
        {
            //Debug.Log("Click");
            if (_lastIsTrue)
            {
                NameInput.gameObject.SetActive(!NameInput.gameObject.activeSelf);
            }
            else 
            {
                RedactControll.Instance.CurrPage = page;
                Debug.Log(RedactControll.Instance.CurrPage.ID + ":" + RedactControll.Instance.CurrPage.Name + ":" +
                    RedactControll.Instance.CurrPage.Sequ);
                if (ModelBtns.ContainsKey(page.ModelNumber))
                {
                    ModleBtn modleBtn = ModelBtns[page.ModelNumber];
                    modleBtn.GetComponent<Toggle>().isOn = true;
                    modleBtn.Click();
                }
                else 
                {
                    Exception ex = new Exception("不包含key为" + page.ModelNumber + "脚本");
                    throw ex;
                }
            }
        }
        else 
        {
            NameInput.gameObject.SetActive(false);
        }
        _lastIsTrue = tog.isOn;
    }
}
