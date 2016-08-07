
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

public class MessageBox : MonoBehaviour {
    public GameObject InputBox;
    public GameObject Message;

    private Text _messageText;
    private InputField _input;
    
	// Use this for initialization
	void Awake () 
    {
	    _messageText = Message.transform.GetChild(0).GetComponent<Text>();
        _input = InputBox.transform.GetChild(0).GetComponent<InputField>();
	}
//End Start	
	// Update is called once per frame
	void Update () 
    {
	    
	}//End Update

    public void ShowMessage(string message) 
    {
        gameObject.SetActive(true);
        _messageText.text = message;
        Message.SetActive(true);
    }

    public void ShowMessage(GameObject serder,string defaultStr)
    {
        gameObject.SetActive(true);
        _input.text = defaultStr;
        Message.SetActive(true);
    }
}
