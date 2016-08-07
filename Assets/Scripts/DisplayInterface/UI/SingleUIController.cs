using UnityEngine;
using System.Collections;

public class SingleUIController : MonoBehaviour {
    public GameObject workMenu;
    public GameObject singleMenu;
    SingleShow single;
	// Use this for initialization
	void Start () 
    {
        single = Camera.main.GetComponent<SingleShow>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        //if (!MsgCenter._instance.isEdit)
        {
            if (single.getTarget())
            {
                workMenu.SetActive(false);
                singleMenu.SetActive(true);
            }
            else if (single.getTarget() == null)
            {
                singleMenu.SetActive(false);
                workMenu.SetActive(true);
            }
        }
	}
}
