using UnityEngine;
using System.Collections;

public class UIInput : MonoBehaviour {
    public GameObject ModelPlan;  // 模板显示面板
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ModelBtn_Click() 
    {
        ModelPlan.SetActive(!ModelPlan.activeSelf);
    }
}
