using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelBtnControll : MonoBehaviour {
    public ModelBtnGroupControl[] LeftGroupControlScs;  //  左侧分组管理按钮
    public ModelBtnGroupControl[] RightGroupControlScs;  //  右侧分组管理按钮
	// Use this for initialization
	void Start () {
        Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Initialize(Page pBookPage) 
    {
        if (pBookPage != null && !(pBookPage is CataloguePage))
        {
            ModelBtnGroupControl targetSc = null;
            if (pBookPage.PageTab % 2 != 0)
            {
                targetSc = GetLeftGroupSc(pBookPage.ModelNumber,0);
            }
            else 
            {
                targetSc = GetRightGroupSc(pBookPage.ModelNumber, 0);
            }
            if (targetSc != null)
            {
                Debug.Log("Initialize[button]:" + pBookPage.PageTab);
                targetSc.RefreshData(pBookPage.ButtonItems);
                targetSc.gameObject.SetActive(targetSc.Used);
                //Debug.Log(targetSc.ModelType);
            }
            
        }
    }
    /// <summary>
    /// 根据模板编号和类型编号，返回模板分组管理脚本
    /// </summary>
    /// <param name="pModeNumm"></param>
    /// <param name="pModelType"></param>
    /// <returns></returns>
    public ModelBtnGroupControl GetLeftGroupSc(int pModeNumm,int pModelType) 
    {
        ModelBtnGroupControl resSc = null;
        for (int i = 0; i < LeftGroupControlScs.Length; i++)
        {
            if (LeftGroupControlScs[i].ModelType == pModeNumm)
            {
                resSc = LeftGroupControlScs[i];
                //break;
            }
            LeftGroupControlScs[i].gameObject.SetActive(false);
        }
        return resSc;
    }
    /// <summary>
    /// 根据模板编号和类型编号，返回模板分组管理脚本
    /// </summary>
    /// <param name="pModeNumm"></param>
    /// <param name="pModelType"></param>
    /// <returns></returns>
    public ModelBtnGroupControl GetRightGroupSc(int pModeNumm, int pModelType)
    {
        ModelBtnGroupControl resSc = null;
        for (int i = 0; i < RightGroupControlScs.Length; i++)
        {
            if (RightGroupControlScs[i].ModelNummber == RightGroupControlScs[i].ModelType)
            {
                resSc = RightGroupControlScs[i];
                //break;
            }
            RightGroupControlScs[i].gameObject.SetActive(false);
        }
        return resSc;
    }
    /// <summary>
    /// 隐藏所有按钮
    /// </summary>
    public void HideAllBtn() 
    {
        for (int i = 0; i < RightGroupControlScs.Length; i++)
        {
            RightGroupControlScs[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < LeftGroupControlScs.Length; i++)
        {
            LeftGroupControlScs[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 隐藏左侧按钮
    /// </summary>
    public void HideLeftBtn()
    {
        for (int i = 0; i < LeftGroupControlScs.Length; i++)
        {
            LeftGroupControlScs[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 隐藏右侧按钮
    /// </summary>
    public void HideRightBtn()
    {
        for (int i = 0; i < RightGroupControlScs.Length; i++)
        {
            RightGroupControlScs[i].gameObject.SetActive(false);
        }
    }

    public void Init() 
    {
        for (int i = 0; i < LeftGroupControlScs.Length; i++)
        {
            LeftGroupControlScs[i].Init();
        }
        for (int i = 0; i < RightGroupControlScs.Length; i++)
        {
            RightGroupControlScs[i].Init();
        }
    }
}
