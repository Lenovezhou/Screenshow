
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
using System.Collections.Generic;

public class EditCataEvent : MonoBehaviour {

    public Model ModelSc;

    public void Init(MsgCenter_h.InitModelParam param)
    {
        ModelSc.Init(param.editCatas,param.editCataBtns);
    }
}
