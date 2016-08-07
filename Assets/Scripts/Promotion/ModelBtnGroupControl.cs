using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelBtnGroupControl : MonoBehaviour
{
    public ItemButtonControl[] AllBtnControl;  //  所有分组按钮的管理脚本
    public int ModelNummber;   //  模板编号
    public int ModelType;  //  模板类型编号
    public bool Used = true;
    public Dictionary<string, UiItem> Items;

    public int codeIndex;
    public const string CODE = "10";
	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RefreshData(Dictionary<string,UiItem> itemList)
    {
        UiItem btnData;
        if (Used && itemList != null)
        {
            //Debug.Log("Items.Count = " + Items.Count + ";  " + itemList.Count);
            foreach (string code in itemList.Keys)
            {
                //Debug.Log("code:" + code);
                if (Items.ContainsKey(code))
                {
                    //Debug.Log("Contains:" + code);
                    btnData = itemList[code].target as UiItem;
                    Items[code].leyoutCode = btnData.leyoutCode;
                    Items[code].leyoutID = btnData.leyoutID;
                    //Items[code]
                }
                else 
                {
                    Debug.Log("No Contains:" + code);
                    foreach (string item in Items.Keys)
                    {
                        Debug.Log(":" + item);
                    }
                }
            }
        }
    }

    public void Init()
    {
        Items = new Dictionary<string, UiItem>();
        codeIndex = 1;
        for (int i = 0; i < AllBtnControl.Length; i++)
        {
            for (int j = 0; j < AllBtnControl[i].Items.Length; j++, codeIndex++)
            {
                Items.Add(CODE + codeIndex.ToString(), 
                    AllBtnControl[i].Items[j]);
            }
        }
    }
}
