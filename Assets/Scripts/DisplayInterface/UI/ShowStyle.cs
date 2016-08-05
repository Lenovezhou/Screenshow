using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Data;
using System.Collections.Generic;

public class ShowStyle : MonoBehaviour
{

    private Toggle ClickButton;
    private List<NewInfomation> _Data;
    public TargetStyle ChooseTarget;
    public string filePath;

    void Awake()
    {
        _Data = new List<NewInfomation>();
        ClickButton = this.GetComponent<Toggle>();
    }
    void Start()
    {
    }

    #region Editor测试
    public void ClickButtonEvent()
    {
        if (ClickButton.isOn)
        {
            MsgCenter._instance.ChangeTarget(ChooseTarget);
            if (ChooseTarget == TargetStyle.chuanghu)
            {
                string qingqiu = "prod_kind=" + "\"" + filePath + "\"";
                string temp = MsgCenter._instance.start(MsgCenter._instance.strXML
                    (EnumToolV2.GetDescription(FuncID.SingleCurtain), EnumToolV2.GetDescription(ActionID.SingleCurtain), qingqiu));
                StartCoroutine(LoadXML());
                //Debug.Log(temp);
            }
            else if (MsgCenter._instance.nowHouse != null)
            {
                //StartCoroutine(LoadXML(filePath));
                string qingqiu = "scene_id=" + "\"" + MsgCenter._instance.nowHouse.Temp_ID + "\"" + " room_id=" + "\"" + MsgCenter._instance.nowScene.ID + "\"" + " panorama_kind=" + "\"" + filePath + "\"";
                string temp = (MsgCenter._instance.start(MsgCenter._instance.strXML("3D404635", "page", qingqiu)));

                Debug.Log(temp);
                MsgCenter._instance.Target = ProdKind.Null;
                StartCoroutine(LoadXML());
            }
        }
        else
        {
            MsgCenter._instance.PictureListState(false);
        }
    }
    IEnumerator LoadXML()
    {

        yield return new WaitWhile(() => MsgCenter._instance.xml =="");
        //yield return new WaitForSeconds(1);
        _Data = NewReadXml.ReadInfo(MsgCenter._instance.xml);
        SendMessage(_Data);
    }
    public void SendMessage(List<NewInfomation> Data)
    {
        MsgCenter._instance.ChangeTarget(ChooseTarget);
        MsgCenter._instance.ReceiveMessage(Data);
    }
    #endregion


    #region Web版
    /// <summary>
    /// 与js通信，传递数据
    /// </summary>
    /// <param name="message">点击button的ID</param>
    void JsConmunication(string message)
    {
        Application.ExternalCall("ShowList", message);
    }
    #endregion

}
