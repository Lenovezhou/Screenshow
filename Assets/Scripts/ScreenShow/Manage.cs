using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Manage : MonoBehaviour {
    public BagScreenShow bagScreenShow;
    public GameObject Loading;
    public GameObject Login;  //  登录界面
    public GameObject ChooseContent;
    public Text LoadingInfo;
    
    public static Manage Instance;
	// Use this for initialization
	void Awake () {
        Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void Logo() 
    {
        if(bagScreenShow.User != null)
        {
            bagScreenShow.User.LogoFinishFunc = LogoFinish;
            StartCoroutine(bagScreenShow.LoginOn());
            LoadingInfo.text = "登录中...";
            Loading.SetActive(true);
        }
    }

    public void LogoFinish(UserData user) 
    {
        if (user.LogoSucceed)    //  成功登录
        {
            LoadingInfo.text = "更新数据中...";
            bagScreenShow.User.UpdataFinishFunc = UpdateFinish;
            StartCoroutine(bagScreenShow.Update());
        }
        else     //  登录失败
        {
            Loading.SetActive(false);
            if (user.Error != "")
            {
                Debug.Log("输入的密码或用户名不正确！");
            }
            else
            {
                Debug.Log("服务器连接失败！");
            }
        }
        
    }

    public void UpdateFinish(UserData user) 
    {
        Loading.SetActive(false);
        if (bagScreenShow.User.Error != null)   //  更新失败
        {
            Debug.Log(bagScreenShow.User.Error);
            bagScreenShow.User.Error = null;
        }
        else //  显示用户选择展示资源界面
        {
            Login.SetActive(false);
            ChooseContent.SetActive(true);
        }
    }
}
