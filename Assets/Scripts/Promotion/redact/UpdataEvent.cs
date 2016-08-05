using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdataEvent : MonoBehaviour {
    public string LeyoutID;

    public ItemType selfType;

    public ModelType modelType;

    public string DefaultText;
    public InputField Input;

    void Awake() 
    {
        selfType = ItemType.None;
    }
	// Use this for initialization
	void OnEnable ()
    {
        switch (selfType)
        {
            case ItemType.Text:
                DefaultText = Input.text;
                break;
            case ItemType.RawImage:
                break;
            case ItemType.Link:
                break;
            case ItemType.Audio:
                break;
            case ItemType.Video:
                break;
            case ItemType.None:
                break;
            default:
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click() 
    {
        UpService.UpInstance.UpdataLeyoutPicture(LeyoutID);
    }

    public void End_Edit() 
    {
        if(DefaultText != Input.text)
        {
            UpService.UpInstance.UpdataPageText(LeyoutID);
            DefaultText = Input.text;
        }
    }
}
