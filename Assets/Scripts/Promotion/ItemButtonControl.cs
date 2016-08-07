using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemButtonControl : MonoBehaviour {
    public int ItemIndex;  // 自身的项的编号
    public PageNControl ParentControl;   //  父级管理脚本
    public Content SelfContentSc;   //  自身的另一个管理脚本
    public UiItem[] Items;  //  

    public List<string> CurtainImgs { set; get; }
    public bool isRigth = false;
    public float moveSetUp;
    public string Introduce { set; get; }


    private Vector3 position;

    void Awake()
    {
        position = transform.position;
    }

    void OnEnable() 
    {
        if (isRigth)
        {
            if (BookInfomation._isntance.LastIsLeft)
            {
                transform.position = position;
            }
            else
            {
                transform.position = new Vector3(position.x + moveSetUp, position.y, position.z);
            }
            //Debug.Log(BookInfomation._isntance.LastIsLeft);
        }
        else
        {

        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click() 
    {
        
    }
}
