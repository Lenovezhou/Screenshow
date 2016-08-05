using UnityEngine;
using System.Collections;

public class LoadPictureManager : MonoBehaviour {

    public static LoadPictureManager _instance;
    private Texture2D SendTexture;

    void Awake()
    {
        _instance = this;
    }
	
    public void ReceiveTextureUrl(string path)
    {
        StartCoroutine(LoadTexture(path));
    }

    IEnumerator LoadTexture(string path)
    {
        //MsgCenter._instance.ChangeStyle(SelfImage.mainTexture);
        Debug.Log("加载图片地址：" + path);
        WWW www = new WWW(path);
        yield return www;
        SendTexture = www.texture;
        MsgCenter._instance.ChangeStyle(SendTexture);
    }
}
