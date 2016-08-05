using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public Button LoginBtn;
    public InputField username;
    void Start()
    {
        LoginBtn.onClick.AddListener(ClickButton);
    }

    private void ClickButton()
    {
        //登录


        //开始加载数据
        LoadManager.GetInstance().BeginToLoadHouse("2015001");
    }

    IEnumerator LoginToSever(string path)
    {
        WWW www = new WWW(path);
        yield return www;
    }

    void Update()
    {

    }
}
