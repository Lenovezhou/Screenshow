using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DataBase;
using LitJson;
public class Testjavascript : MonoBehaviour,IPointerClickHandler
{
    public GameObject target;
    private Curtain curtain;
    private string message;


    void Start()
    {
        curtain = new Curtain();
        curtain.ComponentType = 1;
        curtain.IsModel = true;
        curtain.ModelUrl = "";
        curtain.TextureUrl = "Picture/3.png";
        curtain.UVrepeatParameters = new DataInt2(1, 1);
        curtain.UVParameters = new DataInt3(1, 1, 1);
        //curtain.ScaleParameters = new DataInt3(1, 1, 1);
        curtain.GoodUrl = "";
        curtain.Description = "";
        curtain.AudioUrl = "";
        curtain.MovieUrl = "";
        message = JsonMapper.ToJson(curtain);
        Debug.Log(message);

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        target.GetComponent<changeCurtain>().Recall = CallBack;
        target.SendMessage("ReceiveMessage", message);
    }

    void CallBack(bool IsRight)
    {
        if (IsRight)
        {
            Application.ExternalCall("showTip", "成功");
        }
        else
        {
            Debug.Log("执行失败");
            Application.ExternalCall("showTip", "失败");
        }
    }
}
