using UnityEngine;
using System.Collections;
using DataBase;
using LitJson;
using UnityEngine.EventSystems;
public class TestJavaScript1 : MonoBehaviour,IPointerClickHandler
{

    public GameObject target;
    private Window window;
    private string message="";


    void Start()
    {
        window = new Window();

        window.WindowPosition = new DataInt3(1,1,1);
        window.ScaleParameters = new DataInt3(1,1,1);
        window.SizeParameters = new DataInt2(1,1);
        window.WindowPictureUrl = "Picture/2.png";

        message = JsonMapper.ToJson(window);
        Debug.Log(message);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        target.GetComponent<ChangeWindow>().Recall = CallBack;
        target.SendMessage("ReceiveMessage", message);
    }

    void CallBack(bool IsRight)
    {
        if (IsRight)
        {
            Debug.Log("执行成功");
        }
        else
        {
            Debug.Log("执行失败");
        }
    }
}
