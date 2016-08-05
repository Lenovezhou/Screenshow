using UnityEngine;
using System.Collections;

public enum BtnType 
{
    Video,
    Audio,
    Prod
}

public class ItemButton : MonoBehaviour {

    public string prodID;   //  物品ID

    public AudioSource audioSource;

    public string Url;

    public BtnType type;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public virtual void Click() 
    {
        switch (type)
        {
            case BtnType.Video:
                break;
            case BtnType.Audio:
                break;
            case BtnType.Prod:
                Application.ExternalEval("window.open(" + "\'" + MsgCenter_h.Instance.btnUrl + "?scense_id=" + MsgCenter_h.Instance.scense_id + "&prod_id=" +
                    prodID + "\')");
                break;
            default:
                break;
        }
            //Debug.Log(MsgCenter_h.Instance == null);
            //Application.OpenURL(MsgCenter_h.Instance.btnUrl);
     }
}


