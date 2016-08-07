using UnityEngine;
using System.Collections;

public class RedactMenu : MonoBehaviour {
    public GameObject[] TargetViews;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnChange_toggle(GameObject obj) 
    {
        for (int i = 0; i < TargetViews.Length; i++)
        {
            if (TargetViews[i] == obj)
            {
                TargetViews[i].SetActive(true);
            }
            else 
            {
                TargetViews[i].SetActive(false);
            }
        }
    }
}
