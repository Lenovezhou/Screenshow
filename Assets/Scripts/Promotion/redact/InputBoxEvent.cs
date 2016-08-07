
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

public class InputBoxEvent : MonoBehaviour {
    public GameObject sender;  //  发送者
    public InputField input;
    public GameObject parent;
    public const string MESSAGE_NAME = "InputMessage";
	// Use this for initialization
	void Start () {
	
	}
//End Start	
	// Update is called once per frame
	void Update () {
	
	}//End Update

    public void Ok_Click() 
    {
        if(sender != null)
        {
            sender.SendMessage(MESSAGE_NAME, input.text);
            gameObject.SetActive(false);
            parent.SetActive(false);
            sender = null;
        }
    }

    public void Cancel_Click() 
    {
        if (sender != null)
        {
            sender.SendMessage(MESSAGE_NAME, "");
            gameObject.SetActive(false);
            parent.SetActive(false);
            sender = null;
        }
    }
}
