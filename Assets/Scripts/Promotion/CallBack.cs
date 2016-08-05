
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

public enum PageState 
{
    Last,
    Current,
    Next,
    NextCurrent
}

public class CallBack : MonoBehaviour {
    public PageState selfState;

    public static TurnPlay turnPlay;
    public static PageState CurrentState;
    public static bool needCall;
    public static int maxPics;
    void Awake() 
    {
        maxPics = 0;
    }
	// Use this for initialization
	void Start () {
	    //Debug.Log(gameObject.name);
	}
//End Start	
	// Update is called once per frame
	void LateUpdate () {
	    
	}//End Update

    void OnPostRender()
    {
        if (needCall && selfState == CurrentState)
        {
            StartCoroutine(CallPlay());
            needCall = false;
        }
    }

    public IEnumerator CallPlay() 
    {
        int i = 0;
        while(i < maxPics)
        {
            i++;
            yield return new WaitForEndOfFrame();
        }
        
        //Debug.Log("OnPostRender:" + turnPlay.Method.ToString());
        turnPlay(BookInfomation._isntance.turnParams);
    }
}
