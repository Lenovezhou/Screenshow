using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Logo : MonoBehaviour {
    public InputField NameFiled;
    public InputField PassFiled;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LogoBtn_Click() 
    {
        if (NameFiled.text != "" && PassFiled.text.Length >= 6)
        {
            UserData userData = new UserData(NameFiled.text, PassFiled.text);
            Manage.Instance.bagScreenShow = new BagScreenShow(userData);
            Manage.Instance.Logo();
        }
        else 
        {
            Debug.Log("输入的用户名或密码长度不正确！");
        }
    }

    public void ExitBtn_Click()
    {
        Application.Quit();
    }

}
