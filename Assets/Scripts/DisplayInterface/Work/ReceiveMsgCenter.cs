using UnityEngine;
using System.Collections;

public class ReceiveMsgCenter : MonoBehaviour
{
    public delegate void ReCallBack(object data);//回调

    public ReCallBack raCallBack;
    private static ReceiveMsgCenter _instance;

    void Awake()
    {
        _instance = this;
    }

    

    /// <summary>
    /// 外部调用 
    /// </summary>
    /// <param name="data">外部传递参数</param>
    public void ReceiveMessage(object data)
    {
        //TODO：处理接收到的消息，并处理
        Application.ExternalCall("ReqGetControlInfo"); //调用Web函数，获取
     
        if (raCallBack != null)
        {
            raCallBack(data);
        }

    }
}
