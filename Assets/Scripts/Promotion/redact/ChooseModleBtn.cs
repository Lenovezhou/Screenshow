using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChooseModleBtn : ModleBtn {
    public GameObject EditPage;

    private Model _pageModel;

    void Awake() 
    {
        
    }

    public override void Click()
    {
        if (selfToggle.isOn)
        {
            EditPage.SendMessage("AddPageMessage", new Page(RedactControll.Instance.Book.PageList.Count - RedactControll.Instance.Book.CatalogueLen,
                ModelNum, GetItems(ModelNum)));
        }
    }

    public Dictionary<string, UiItem> GetItems(int modelNum) 
    {
        Dictionary<string, UiItem> items = null;

        _pageModel = EditPage.GetComponent<PageControl>().PageModel.GetComponent<Model>();

        foreach (Content con in _pageModel.ModelNumDic.Values)
	    {
            if (modelNum == con.ModleNum)
            {
                items = con.Items;
                break;
            }
	    }
        return items;
    }
}
