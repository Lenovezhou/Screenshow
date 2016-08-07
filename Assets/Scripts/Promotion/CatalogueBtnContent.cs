using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatalogueBtnContent : MonoBehaviour {
    public List<CatalogueBtn> CatalogueBtns = new List<CatalogueBtn>();
    public Color DefaultColor;  //  默认的颜色
    public Color SelectColor;  //  选中的颜色

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pageTab"></param>
    public void SetTargetBtnShine(int pageTab)
    {
        for (int i = 0; i < CatalogueBtns.Count; i++)
        {
            CatalogueBtns[i].selfImage.color = DefaultColor;
            if (CatalogueBtns[i].TargetPageTab == pageTab)
            {
                CatalogueBtns[i].selfImage.color = SelectColor;
            }
        }
    }
}
