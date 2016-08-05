using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SingleUIClick : MonoBehaviour {
    GameObject ClickUI;
    SingleShow Show;
    Transform Target;

    public Toggle GameSearch;//更换完成图片或者是bundle之后，出现搜索图片覆盖//
    void Awake()
    {
        ClickUI =this.transform.GetChild(0).GetChild(0).gameObject;
    }
	// Use this for initialization
	void Start () 
    {
        Show = Camera.main.GetComponent<SingleShow>();
        //ClickUI.GetComponent<Button>().onClick.AddListener(OnClickButton);

        GameSearch.group = transform.parent.GetComponent<ToggleGroup>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if (Show.getTarget())
        //{
        //    Target = Show.getTarget();
        //}
        //else if (Show.getTarget()==null)
        //{
        //    Target = null;
        //}
	}

    public void OnClickButton()
    {
        if (GameSearch.isOn)
        {
            MsgCenter._instance.Go.GetComponent<MeshRenderer>().material.mainTexture = ClickUI.GetComponent<RawImage>().mainTexture;
            MsgCenter._instance.Go.GetComponent<CurtainManager>().Material.mainTexture = MsgCenter._instance.Go.GetComponent<MeshRenderer>().material.mainTexture;
        }
        
    }

}
