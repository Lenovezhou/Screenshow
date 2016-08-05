using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EditContent : Content {
    public GameObject oPicButtonPrefab;   //  按钮的prefab
    public float fBtnWidth;
    public float fBtnHieght;

    public List<GameObject> BtnElenments;

    public GameObject oInputPrefab;   //  输入框的prefab
    public float fInWidth;
    public float fInHieght;

    public float fPageWidth;
    public float fPageHeight;

    public override void Init()
    {
        Items = new Dictionary<string, UiItem>();
        //Debug.Log("EditContent->Init");
        Component com = null;
        ItemType type = ItemType.None;
        codeIndex = 1;
        GameObject obj;
        RectTransform rect, rect1;
        for (int i = 0; i < Elements.Count; i++)
        {
            com = Elements[i].GetComponent<InputField>();
            if (com != null)
            {
                type = ItemType.Text;
            }
            else
            {
                com = Elements[i].GetComponent<RawImage>();
                if (com != null)
                {
                    type = ItemType.RawImage;
                    obj = Instantiate(oPicButtonPrefab) as GameObject;
                    obj.transform.parent = com.transform;
                    rect1 = com.GetComponent<RectTransform>();
                    rect = obj.GetComponent<RectTransform>();
                    rect.anchorMin = new Vector2(0.5f, 0.5f);
                    rect.anchorMax = new Vector2(0.5f,0.5f);
                    Debug.Log(rect1.sizeDelta);
                    rect.anchoredPosition = new Vector2(-rect1.sizeDelta.x / 2 + fBtnWidth / 2, rect1.sizeDelta.y / 2 - fBtnHieght / 2);
                    rect.sizeDelta = new Vector2(fBtnWidth,fBtnHieght);
                }
                else
                {
                    com = Elements[i].GetComponent<ModelBtnGroupControl>();
                    if (com != null)
                    {
                        type = ItemType.Link;
                    }
                }
            }
            Items.Add(CODE + codeIndex, new UiItem(type, com, CODE + codeIndex,""));
            codeIndex++;
        }
        BtnCodeInit();
    }

    public void BtnCodeInit()
    {
        for (int i = 0, codeIndex = 1; i < BtnElenments.Count; i++, codeIndex++)
        {
            Items.Add(BTN_CODE + codeIndex, new UiItem(ItemType.Link, BtnElenments[i], BTN_CODE + codeIndex, null));
        }
    }

    public override void SetContent(Page bp)
    {
        UpParam iip = null;

        if (bp is CataloguePage)
        {
            iip = new UpParam(bp.PageTab, true);
        }
        else
        {
            iip = new UpParam(bp.PageTab + BookInfomation._isntance.CatalogueLen, false);
        }

        InputField input;
        RawImage ri;
        UpdataEvent ue;
        GameObject obj;
        foreach (string code in Items.Keys)
        {
            Debug.Log("bp.Items.ContainsKey(" + code + ") = " + bp.Items.ContainsKey(code) );

            //Debug.Log("[原本]:" + Items[code].type.ToString() + " ；  [内页]:" + bp.Items[code].type.ToString());
            if (bp.Items.ContainsKey(code))
            {
                Debug.Log("[原本]:" + Items[code].type.ToString() + " ；  [内页]:" + bp.Items[code].type.ToString());
                switch (Items[code].type)
                {
                    case ItemType.Text:
                        if (Items[code].target is string)
                        {
                            input = Items[code].target as InputField;
                            input.text = bp.Items[code].target as string;
                            ue = input.gameObject.GetComponent<UpdataEvent>();
                            ue.LeyoutID = Items[code].leyoutID;
                            ue.selfType = ItemType.Text;
                        }
                        
                        //Debug.Log("code: " + code + "; text = " + bp.Items[code].target as string);
                        break;
                    case ItemType.RawImage:
                        if (bp.Items[code].target is string)
                        {
                            ri = Items[code].target as RawImage;
                            ue = ri.transform.GetChild(0).GetComponent<UpdataEvent>();
                            ue.LeyoutID = Items[code].leyoutID;
                            ue.selfType = ItemType.RawImage;
                            iip.UploadData.Add(bp.Items[code].target as string, ri);
                        }
                        
                        break;
                    case ItemType.Link:
                        if (Items[code].target is GameObject)
                        {
                            obj = Items[code].target as GameObject;
                            ItemButton btn = obj.GetComponent<ItemButton>();
                            ButtonItem btnData = bp.Items[code].target as ButtonItem;
                            btn.Url = btnData.url;
                            //btn.LeyoutID = btnData.leyoutID;
                        }
                        
                        break;
                    case ItemType.Audio:
                        break;
                    case ItemType.Video:
                        break;
                    case ItemType.None:
                        break;
                }
                
            }
            else
            {
                Debug.Log("leyout = " + code);
                Debug.Log("item.count = " + Items.Count);
                string str = "->\n";
                foreach (string key in Items.Keys)
                {
                    str += "; " + key;
                }
                Debug.Log(str);
                str = "-->\n";
                foreach (string key in bp.Items.Keys)
                {
                    str += "; " + key;
                }
                Debug.Log(str);
            }
        }

        if (IEm != null)
        {
            try
            {
                StopCoroutine(IEm);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally
            {
                IEm = null;
            }
        }
        //Debug.Log("PageTable = " + bp.PageTab);
        //Debug.Log("iip.UploadData.Count = " + iip.UploadData.Count);
        gameObject.SetActive(true);
        //Debug.Log(gameObject.transform.parent.parent.gameObject.name + "-> " + gameObject.name);
        IEm = StartCoroutine(LoadCurtain(iip));
    }
}
