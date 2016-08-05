
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
using System.Collections;

public class GoodBtnEvent : MonoBehaviour {
    public string GoodID;

    public void Click() 
    {
        MsgCenter_h.Instance.ChooseGoodMsg(GoodID);
    }
}
