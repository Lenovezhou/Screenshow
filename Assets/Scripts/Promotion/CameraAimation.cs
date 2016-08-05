
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

public class CameraAimation : MonoBehaviour {
    public Animation cameraAnimation;
    public float openField;
    public float closeField;
    public Transform openTrans;
    public Transform closeTrans;

    public GameObject BottomMenu;
    public InputField input;

    private bool _isOpen;
    private bool _isPlaying;

    public static CameraAimation Instance;
	// Use this for initialization
	void Awake () 
    {
        _isOpen = false;
        Instance = this;
        _isPlaying = false;
	}
//End Start	
	// Update is called once per frame
	void FixedUpdate () {
        if (_isPlaying && !cameraAnimation.isPlaying && _isOpen)
        {
            _isPlaying = false;
            transform.position = openTrans.position;
            transform.rotation = openTrans.rotation;
            Camera.main.fieldOfView = openField;
        }
        else if (_isPlaying && !cameraAnimation.isPlaying && !_isOpen)
        {
            _isPlaying = false;
            Debug.Log("adsffasgdfsgsd");
        }
	}//End Update

    public void OpenBook() 
    {
        cameraAnimation["cameraAnimation"].speed = 1;
        cameraAnimation.Play("cameraAnimation");
        _isOpen = true;
        _isPlaying = true;
        BottomMenu.SetActive(true);
        input.text = "1";
    }

    public void CloseBook() 
    {
        cameraAnimation["cameraAnimation"].time = cameraAnimation["cameraAnimation"].length;
        cameraAnimation["cameraAnimation"].speed = -1;
        cameraAnimation.Play("cameraAnimation");
        Debug.Log(cameraAnimation.isPlaying);
        _isOpen = false;
        _isPlaying = true;
        BottomMenu.SetActive(false);
    }
}
