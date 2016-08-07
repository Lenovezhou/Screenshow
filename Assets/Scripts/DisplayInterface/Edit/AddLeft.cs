using UnityEngine;
using System.Collections;

public class AddLeft : MonoBehaviour 
{
    LoadDataFromList LoadDataFromList;
    MsgCenter MsgCenter;
    private bool isClick;
	// Use this for initialization
	void Start ()
    {
        LoadDataFromList = LoadDataFromList._Instand;
        MsgCenter = MsgCenter._instance;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isClick)
        {
            isClick = false;
            switch (MsgCenter.lookTarget)
            {
                case LookTarget.huxing:
                    AddHuXing();
                    break;
                case LookTarget.zhuangxiu:
                    AddZhuangxiu();
                    break;
                case LookTarget.chuanglian:
                    AddChuangLian();
                    break;
                default:
                    break;
            }
        }
	}

    void AddHuXing()
    {
        MsgCenter._instance.insertType = "1";
        GameObject Go = (GameObject)Instantiate(LoadDataFromList.HousePrefab);
        Go.transform.parent = LoadDataFromList.houseParent.transform;
        Go.transform.localScale = Vector3.one;
        LoadDataFromList.HouseGameObjectPool.Add(Go);
        MsgCenter.UIShowStr = "2_7";
        MsgCenter.EditUI.SetActive(true);
    }
    void AddZhuangxiu()
    {
        MsgCenter._instance.insertType = "3";
        GameObject Go = (GameObject)Instantiate(LoadDataFromList.StylePrefab);
        Go.transform.parent = LoadDataFromList.styleParent.transform;
        Go.transform.localScale = Vector3.one;
        LoadDataFromList.StyleGameObjectPool.Add(Go);
        MsgCenter.UIShowStr = "";
        MsgCenter.EditUI.SetActive(true);
    }
    void AddChuangLian()
    {

    }

    public void ClickAdd()
    {
        isClick=true;
    }
    public void ClickSave()
    {

    }

}
