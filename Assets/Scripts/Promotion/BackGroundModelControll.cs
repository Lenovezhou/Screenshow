using UnityEngine;
using System.Collections;

public class BackGroundModelControll : MonoBehaviour {
    public BackgroundModel[] BackgroundModelScs;   //  所有的模板身上的脚本
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DisplayModel(int pID) 
    {
        if (pID >= 0 && pID < BackgroundModelScs.Length)
        {
            for (int i = 0; i < BackgroundModelScs.Length; i++)
            {
                if (BackgroundModelScs[i].ID == pID)
                {
                    BackgroundModelScs[i].gameObject.SetActive(true);
                }
                else
                {
                    BackgroundModelScs[i].gameObject.SetActive(false);
                }
            }
        }         
    }
}
