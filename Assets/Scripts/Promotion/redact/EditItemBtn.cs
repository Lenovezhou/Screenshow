
/*************************
 * Title: 工程名
 * Function:方法作用
 *      - 
 * Used By: 
 * Author: 001
 * Date:    2015.10
 * Version: 1.0
 * Record:  
 *      
 *************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditItemBtn : ItemButton {
    public GameObject GoodListScroll;
	// Use this for initialization
	void Start () {
	
	}
//End Start	
	// Update is called once per frame
	void Update () {
	
	}//End Update

    public override void Click()
    {
        RedactControll.Instance.CurrLeyoutID = prodID;
        GoodListScroll.SetActive(true);
    }
}
