using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModelControl : MonoBehaviour {
    public Image[] ModelBrosss;
    public Color DefaultColor;  // 默认的颜色
    public Color SelectColor;   //  选中的颜色
    public OperationBook operationBookSC;
    //  当前选中的模板的下标
    public int CurrentModel 
    {
        set 
        {
            operationBookSC.CurrentPage.ModelNumber = value;
        }
        get
        {
            return operationBookSC.CurrentPage.ModelNumber;
        }
    }

    public int ItemType 
    {
        set
        {
            operationBookSC.CurrentPage.ModelNumber = value;
        }
        get
        {
            return operationBookSC.CurrentPage.ModelNumber;
        }
    }
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 设置当前选中高亮
    /// </summary>
    /// <param name="pIndex"></param>
    public void SetCurrentModel(int pIndex)
    {
        if (pIndex >= 0 && pIndex < ModelBrosss.Length)
        {
            for (int i = 0; i < ModelBrosss.Length; i++)
            {
                ModelBrosss[i].color = DefaultColor;
            }
            ModelBrosss[pIndex].color = SelectColor;
        }
    }
}
