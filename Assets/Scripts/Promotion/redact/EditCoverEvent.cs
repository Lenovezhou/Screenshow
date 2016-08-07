
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

public class EditCoverEvent : MonoBehaviour {
    public Model CoverModelSc;

	// Use this for initialization
	void Start () {
	
	}
//End Start	
	// Update is called once per frame
	void Update () {
	
	}//End Update

    public void Init(MsgCenter_h.InitModelParam param) 
    {
        CoverModelSc.Init(param.editCovers,param.editCoverBtns);
    }
}
