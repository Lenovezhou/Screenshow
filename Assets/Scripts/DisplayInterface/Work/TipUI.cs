using UnityEngine;
using System.Collections;

public class TipUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClickButton()
    {
        MsgCenter._instance.shopingTip.parent.gameObject.SetActive(false);
    }
}
