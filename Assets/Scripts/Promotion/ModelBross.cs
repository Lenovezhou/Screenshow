using UnityEngine;
using System.Collections;

public class ModelBross : MonoBehaviour {
    public ModelControl ModelControlSc;   //  模板预览的管理脚本
    public int selfModelIndex;   //  自身的索引
    public int selfItemType;   //  自身的项的类型
    public int ID;  // 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click() 
    {
        ModelControlSc.ItemType = selfItemType;
        ModelControlSc.CurrentModel = selfModelIndex;
        ModelControlSc.SetCurrentModel(ID);
    }
}
