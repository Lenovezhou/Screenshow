using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject[] panelArr;
    public GameObject MenuContent;
    //public GameObject OperationPanel;
    //public GameObject CurtainGameObject;
    //public GameObject WindowGameObject;
    //public GameObject RepairStyleGameObject;
    //public float[] Position;//UI
    //public float[] Speed;

    private UIStyleEnum[] TempStyle;
    private bool IsShow = false;    //是否展示UI//
    private bool IsRunning = false;
    private bool IsSelf = false;
    private bool IsDone = false;
    private bool IsOver = false;
    public static MainMenu _instand;
    public enum UIStyleEnum
    { 
        None,
        Curtain,
        Window,
        RepairStyle,
    }
    public void Awake()
    {
        _instand = this;
    }
    void Start()
    {
        MsgCenter._instance.m_MainMenu = this;
        TempStyle = new UIStyleEnum[2];
        IsOver = true;

    }
    void Update()
    {
        //StartRunning();
        
    }
    /*//void StartRunning()
    //{
    //    if (IsRunning)
    //    {
    //      //  Debug.Log("IsRunning");
    //        if (!IsShow)
    //        {
    //            ShowUI(TempStyle[0]);
    //           // Debug.Log("ShowUI");
    //        }
    //        else// if(IsShow)
    //        {
    //            HiddenUI(TempStyle[1]);
    //           // Debug.Log("HiddenUI");
    //        }
    //    }
    //}*/

    //点击户型按钮
    public void ButtonType()
    {
        PanelShow(0, 1, 2);
        MsgCenter._instance.lookTarget = LookTarget.huxing;
        //MsgCenter._instance.StyleTarget = TargetStyle.Null;
        if (IsOver)
            ButtonClickInit(UIStyleEnum.Window);

    }
    //点击装饰风格
    public void ButtonStyle()
    {
        PanelShow(1, 0, 2);
        MsgCenter._instance.lookTarget = LookTarget.zhuangxiu;
        if (IsOver)
            ButtonClickInit(UIStyleEnum.RepairStyle);
        panelArr[1].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().isOn = false;
        panelArr[1].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Toggle>().isOn = true;
    }
    //点击窗帘按钮
    public void ButtonCurtains(bool isinit)
    {
        PanelShow(2, 1, 0);
        IniMemu(isinit);
        MsgCenter._instance.lookTarget = LookTarget.chuanglian;
        MsgCenter._instance.StyleTarget = TargetStyle.Null;
        if (IsOver)
            ButtonClickInit(UIStyleEnum.Curtain);
    }

    public void IniMemu(bool isinit) 
    {
        if (MsgCenter._instance.isEdit) return;

        for (int i = 1; i < MenuContent.transform.childCount; i++)
        {
            MenuContent.transform.GetChild(i).gameObject.SetActive(false);
        }
        //Debug.Log(MsgCenter._instance.GameManageList.Count+"    gfffffffffffffffffff");
        foreach (var obj in MsgCenter._instance.GameManageList)
        {
            //Debug.Log(int.Parse(obj.Value.name));
            if (obj.Value != null) 
            {
                MenuContent.transform.GetChild(int.Parse(obj.Value.name)).gameObject.SetActive(true);
            }
        }
        if (isinit)
        {
            SingleShow._instance.UIButton.GetChild(0).GetComponent<ShowPictureList>().ClickButton.isOn = false;
            SingleShow._instance.UIButton.GetChild(0).GetComponent<ShowPictureList>().ClickButton.isOn = true;
        }
    }

    void PanelShow(int show,int hide1,int hide2)
    {
        panelArr[show].SetActive(true);
        panelArr[hide1].SetActive(false);
        panelArr[hide2].SetActive(false);
    }
    public void ButtonCurtain(bool IsTrue)
    {
        if (IsOver && (!IsShow || TempStyle[0] != UIStyleEnum.Curtain))
            ButtonClickInit(UIStyleEnum.Curtain);
    }


    void ButtonClickInit(UIStyleEnum Style)
    {
        IsRunning = true;
        TempStyle[0] = Style;
        IsSelf = true;
        IsOver = false;
       
        if (IsDone && TempStyle[0] != TempStyle[1])
        {
            IsSelf = false;
        }

        if (MsgCenter._instance.isDisplayList && !IsShow)
        {
            MsgCenter._instance.PictureListState(true);
        }
        else
        {
            MsgCenter._instance.PictureListState(false);
        }
    }
    

}
