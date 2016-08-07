using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AddScene : MonoBehaviour {
    public GameObject pointObj;
    public Transform pointParent;
	// Use this for initialization
    RectTransform target;
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    public void ClickAdd()
    {
        MsgCenter._instance.insertType = "2";
        target =Instantiate(pointObj).GetComponent<RectTransform>();
        target.transform.SetParent(pointParent);
        target.localPosition = Vector3.zero;
    }

    public void ClickSave()
    {
        target.GetComponent<UIToScene>().isDrop = false;
        MsgCenter._instance.nowScenePoint = target.anchoredPosition;
        MsgCenter._instance.UIShowStr = "2_3_4_5_8_9";
        MsgCenter._instance.EditUI.SetActive(true);
    }
}
