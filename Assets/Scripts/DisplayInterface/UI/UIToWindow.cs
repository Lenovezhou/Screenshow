using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIToWindow : MonoBehaviour
{
    private Button button;

    AssetManager assetManager;
    MsgCenter MsgCenter;
    public static UIToWindow _instance;
    // Use this for initialization
    void Start()
    {
        _instance = this;
        MsgCenter = MsgCenter._instance;
        //button = this.GetComponent<Button>();
        assetManager = Camera.main.GetComponent<AssetManager>();
        //button.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    //void OnClick()
    //{
    //    MsgCenter.isChange = true;
    //    Camera.main.GetComponent<UseCamareController>().Target1.LookAt(assetManager.WindowList[int.Parse( this.name)]);
    //    ChangeWindow(int.Parse(this.name));
    //}

    public void ChangeWindow(string Id)
    {
        //Debug.Log(Id+"  aaaaaaaaaaaaaaaaaaaaaa");
        assetManager.ParentUp = assetManager.WindowList[Id].GetComponent<WindoManager>().Up;
        assetManager.ParentMiddle = assetManager.WindowList[Id].GetComponent<WindoManager>().Middle;
        assetManager.Parent2D = assetManager.WindowList[Id].GetComponent<WindoManager>().TwoD;
        foreach (string temp in MsgCenter.WindowList[Id].Keys)
        {
            //Debug.Log(temp+"  sdeeeeeeeeeeeeeeeeeeeeeeeeeeeee");
        }
        Debug.Log(MsgCenter.WindowList[Id].Count);
        MsgCenter.GameManageList = MsgCenter.WindowList[Id];
        //if (MsgCenter.nowWidow != null)
        {
            //if (MsgCenter.nowWidow.name != assetManager.WindowList[Id].name)
                MsgCenter.nowWidow = assetManager.WindowList[Id].transform;
        }
        //Camera.main.GetComponent<UseCamareController>().Target1.LookAt(assetManager.WindowList[Id]);
        MsgCenter.chuanghu = assetManager.WindowList[Id].transform.FindChild("chaunghu").gameObject;
    }
}
