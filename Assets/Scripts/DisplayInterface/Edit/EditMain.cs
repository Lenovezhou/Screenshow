using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EditMain : MonoBehaviour
{
    public List<GameObject> WindowList = new List<GameObject>();
    public GameObject Window;//窗户预制体
    public Transform WParent;//窗户的父物体
    public GameObject TipObj;//提示框
    float WindowRotation;//修改的窗户旋转值
    float WindowScale;//修改的窗户缩放
    float CurtainScale; // 窗帘的缩放

    public GameObject CaiZuo;//滑条和保存按钮的UI
    public Slider s_rotation;//滑动条
    public Slider s_scale_Window;//滑动条
    public Slider s_scale_Curtain;//滑动条
    public InputField t_rotation;//显示数值UI
    public InputField t_scale_Window;//显示数值UI
    public InputField t_scale_Curtain;//显示数值UI

    public Transform Movetarget;//移动的目标
    public Transform Rotetarget;//旋转的目标

    public Transform ScaleWindowTarget;
    public Transform ScaleCurtainTarget;

    public Transform Temptarget;//临时目标
    public GameObject AllCull;
    int i = 0;

    bool isTip;
    bool isRemove;
    bool isSave;
    bool isAddWindow;

    MsgCenter MsgCenter;
    RaycastHit hit;
    AssetManager Asset;
    UseCamareController Controller;
    // Use this for initialization
    void Start()
    {
        Asset = Camera.main.GetComponent<AssetManager>();
        MsgCenter = Camera.main.GetComponent<MsgCenter>();
        Controller = Camera.main.GetComponent<UseCamareController>();

        Debug.Log("添加");
        MsgCenter._instance.start(MsgCenter._instance.strXML(EnumToolV2.GetDescription(FuncID.FengGe), "dict"
            , ""));

        StartCoroutine(LoadFengGeXML());
    }

    IEnumerator LoadFengGeXML()
    {
        yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        NewReadXml.ReadFengGe(MsgCenter._instance.xml);

        MsgCenter._instance.start(MsgCenter._instance.strXML(EnumToolV2.GetDescription(FuncID.SceneStyle), "dict"
            , " dict_type=\"scene_kind\" "));
        StartCoroutine(LoadStyleXML());
    }
    IEnumerator LoadStyleXML()
    {
        yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        NewReadXml.ReadSceneStyle(MsgCenter._instance.xml);
    }

    // Update is called once per frame
    void Update()
    {
        if (MsgCenter.isEdit)
        {
            /*显示编辑界面需要的UI*/

            //Text显示滑条的值
            t_rotation.text = s_rotation.value.ToString();
            t_scale_Window.text = s_scale_Window.value.ToString();
            t_scale_Curtain.text = s_scale_Curtain.value.ToString();

            //添加窗户
            if (isAddWindow)
            {
                AddWindow();
            }
            //删除窗户
            if (isRemove)
            {
                isRemove = false;
                RemoveWindow();
            }
            //进入单独展示时操作隐藏
            if (SingleShow._instance.getTarget())
            {
                CaiZuo.SetActive(false);
            }

            if (SingleShow._instance.getTarget() == null && Rotetarget)
            {
                CaiZuo.SetActive(true);
            }
            //点击保存按钮时保存
            if (isSave)
            {
                Save();
            }

            //窗户的操作
            if (Input.GetMouseButtonDown(0) && !Movetarget && !EventSystem.current.IsPointerOverGameObject())
            {
                ClickEven();
            }
            //当有了一个移动的目标并拖动鼠标时
            else if (Input.GetMouseButton(0) && Movetarget && !isTip)
            {

                Movetarget.RotateAround(new Vector3(0, 0, 0), Vector3.up, 3 * Input.GetAxis("Mouse X"));
                //+ -7
                float temp = Movetarget.localPosition.z;
                Movetarget.localPosition = new Vector3(Movetarget.localPosition.x, Movetarget.localPosition.y, temp + Input.GetAxis("Mouse Y"));
            }

            //获得滑条的值
            if (Rotetarget)
            {
                WindowRotation = s_rotation.value;
                WindowScale = s_scale_Window.value;
                CurtainScale = s_scale_Curtain.value;
            }
            //设置目标的旋转缩放，WindowRotation，WindowScale变量由划条控制
            if (Rotetarget)
            {
                Rotetarget.localEulerAngles = new Vector3(0, 0, WindowRotation);
                if (ScaleWindowTarget) ScaleWindowTarget.localScale = new Vector3(WindowScale, WindowScale, WindowScale);
                if (ScaleCurtainTarget) ScaleCurtainTarget.localScale = new Vector3(CurtainScale, CurtainScale, CurtainScale);
            }
            //鼠标弹起 设置移动目标为空，并设置摄像机可旋转
            if (Input.GetMouseButtonUp(0) && Movetarget && !isTip)
            {
                Movetarget = null;
                Camera.main.GetComponent<UseCamareController>().IsCanMove = true;
            }
        }
    }
    /// <summary>
    /// 添加窗户
    /// </summary>
    void AddWindow()
    {
        i = Asset.WindowList.Count;
        CaiZuo.SetActive(true);
        if (Rotetarget)
            Save();
        GameObject WindowObj = Instantiate(Window);
        WindowObj.transform.parent = WParent;

        //WindowObj.transform.localPosition = Vector3.zero;
        //WindowObj.transform.localPosition = new Vector3(0, 7, 0);
        WindowObj.transform.position = Controller.Target1.transform.TransformPoint(0, 0, 7);
        float Zvalue = Controller.Target1.transform.localEulerAngles.y;
        WindowObj.transform.localEulerAngles = new Vector3(0, 0, Zvalue + 180);

        Rotetarget = WindowObj.transform;

        ReAssigned(Rotetarget); //赋值

        WindowObj.name = i.ToString();

        Save();
        //Asset.WindowList.Add(WindowObj.transform);
        MsgCenter._instance.AddWindowList(WindowObj.name, new Dictionary<string, GameObject>());

        MsgCenter._instance.nowWidow = WindowObj.transform;

        s_rotation.value = Rotetarget.localEulerAngles.z;


        isAddWindow = false;
        WindoManager temp = WindowObj.GetComponent<WindoManager>();
        temp.ID = i;
        isAddWindow = false;
    }

    /// <summary>
    /// 删除窗户
    /// </summary>
    void RemoveWindow()
    {

        MsgCenter._instance.WindowList.Remove(Rotetarget.name);
        //Asset.WindowList.RemoveAt(int.Parse(Rotetarget.name));
        //DestroyImmediate(Asset.WindowList[int.Parse(Rotetarget.name)].gameObject);
        CaiZuo.SetActive(false);
        Rotetarget = null;
    }

    /// <summary>
    /// 保存这个窗户的信息
    /// </summary>
    void Save()
    {
        WindoManager temp;
        if (Rotetarget.GetComponent<WindoManager>())
        {
            temp = Rotetarget.GetComponent<WindoManager>();
            temp.Position = RoundV3(Rotetarget.localPosition, 2);
            temp.Rotation = RoundV3(Rotetarget.localEulerAngles, 2);
            temp.Scale = RoundV3(Rotetarget.localScale, 2);
        }
        isSave = false;
    }


    /// <summary>
    /// 鼠标按下
    /// 鼠标点击后，检测是否点到窗户；
    /// </summary>
    private void ClickEven()
    {
        if (Physics.Raycast(MsgCenter._instance.windowCamera.ScreenPointToRay(Input.mousePosition), out hit, 10000, (1 << 10 | 1 << 11)))
        {
            if (hit.collider.name == "chaunghu")
            {
                CaiZuo.SetActive(true);
                Camera.main.GetComponent<UseCamareController>().IsCanMove = false;
                //if (Rotetarget.GetComponent<WindoManager>())
                {
                    //if (Rotetarget.GetComponent<WindoManager>().ID == hit.collider.transform.parent.GetComponent<WindoManager>().ID)
                    {
                        Movetarget = hit.collider.transform.parent;
                        if (Rotetarget == null)
                        {
                            Rotetarget = Movetarget;

                            ReAssigned(Rotetarget); //赋值
                        }
                        else
                        {
                            Temptarget = Movetarget;
                        }

                        //UIToWindow._instance.ChangeWindow(int.Parse(Rotetarget.name));
                        //更换时，确保滑条的值为点击的窗户的旋转缩放

                        if (Temptarget && Temptarget != Rotetarget)
                        {
                            /* 如果数据不一样提示保存   
                             *      点击保存后保存这个窗户并且把目标变为Temptarget  
                             *      点取消还是这个目标
                             *一样的话直接改变
                             */
                            if (ContrastManager(Rotetarget.GetComponent<WindoManager>()))
                            {
                                Rotetarget = Temptarget;
                                ReAssigned(Rotetarget); //赋值
                            }
                            else
                            {
                                AllCull.SetActive(true);
                                Camera.main.GetComponent<UseCamareController>().IsCanMove = false;
                                TipObj.SetActive(true);
                                isTip = true;
                            }
                        }
                        s_rotation.value = Rotetarget.localEulerAngles.z;
                        // s_scale_Window.value = Rotetarget.localScale.z;
                    }
                }
            }
        }
    }


    /// <summary>
    /// 比较前后两个窗户信息  true：一样 false：不一样
    /// </summary>
    /// <param name="befor"></param>
    /// <param name="after"></param>
    /// <returns> 是否一样 </returns>
    bool ContrastManager(WindoManager befor)
    {
        bool temp = true;
        if (befor.Position != RoundV3(Rotetarget.localPosition, 2))
        {
            temp = false;
        }
        else if (befor.Scale != RoundV3(Rotetarget.localScale, 2))
        {
            temp = false;
        }
        else if (befor.Rotation != RoundV3(Rotetarget.localEulerAngles, 2))
        {
            temp = false;
        }
        return temp;
    }

    /// <summary>
    /// 重新赋值
    /// </summary>
    /// <param name="Parent"></param>
    void ReAssigned(Transform Parent)
    {
        ScaleWindowTarget = Parent.transform.FindChild("chaunghu");
        ScaleCurtainTarget = Parent.transform.FindChild("guadian");
        s_scale_Curtain.value = ScaleCurtainTarget.localScale.z;
        s_scale_Window.value = ScaleWindowTarget.localScale.z;
    }


    #region //UI事件
    public void OnSaveClick()
    {
        isSave = true;
    }
    public void OnRemove()
    {
        isRemove = true;
    }
    public void OnAdd()
    {
        isAddWindow = true;
    }
    public void OnTipSave()
    {
        Save();
        Rotetarget = Temptarget;
        ReAssigned(Rotetarget);  //赋值
        ScaleWindowTarget = Rotetarget.transform.FindChild("chaunghu");
        ScaleCurtainTarget = Rotetarget.transform.FindChild("guadian");
        s_rotation.value = Rotetarget.localEulerAngles.z;
        if (ScaleWindowTarget) s_scale_Window.value = ScaleWindowTarget.localScale.z;
        if (ScaleCurtainTarget) s_scale_Window.value = ScaleCurtainTarget.localScale.z;
        TipObj.SetActive(false);
        AllCull.SetActive(false);
        Camera.main.GetComponent<UseCamareController>().IsCanMove = true;
        isTip = false;
    }

    public void OnTipClose()
    {
        TipObj.SetActive(false);
        AllCull.SetActive(false);
        Camera.main.GetComponent<UseCamareController>().IsCanMove = true;
        isTip = false;
    }
    public void OnTipNoSave()
    {
        Rotetarget = Temptarget;
        ReAssigned(Rotetarget);  //赋值
        s_rotation.value = Rotetarget.localEulerAngles.z;
        if (ScaleWindowTarget) s_scale_Window.value = ScaleWindowTarget.localScale.z;
        if (ScaleCurtainTarget) s_scale_Curtain.value = ScaleCurtainTarget.localScale.z;
        TipObj.SetActive(false);
        AllCull.SetActive(false);
        Camera.main.GetComponent<UseCamareController>().IsCanMove = true;
        isTip = false;
    }
    #endregion


    /// <summary>
    /// 四舍五入
    /// </summary>
    /// <param name="value">double</param>
    /// <param name="digit">int</param>
    /// <returns></returns>
    public Vector3 RoundV3(Vector3 v3, int digit)
    {
        return new Vector3(Round(v3.x, digit), Round(v3.y, digit), Round(v3.z, digit));
    }

    float Round(float value, int digit)
    {
        float vt = Mathf.Pow(10, digit);
        //1.乘以倍数 + 0.5
        float vx = (float)(value * vt + 0.5f);
        //2.向下取整
        float temp = Mathf.Floor(vx);
        //3.再除以倍数
        return (temp / vt);
    }

    IEnumerator OnMove(Transform temp)
    {
        var camera = Camera.main;
        if (camera)
        {
            //转换对象到当前屏幕位置 
            Vector3 screenPosition = camera.WorldToScreenPoint(temp.position);
            //鼠标屏幕坐标 
            Vector3 mScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
            //获得鼠标和对象之间的偏移量,拖拽时相机应该保持不动 
            Vector3 offset = temp.position - camera.ScreenToWorldPoint(mScreenPosition);
            print("drag starting:" + temp.name);
            //若鼠标左键一直按着则循环继续 
            while (Input.GetMouseButton(0))
            {
                //鼠标屏幕上新位置
                mScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
                // 对象新坐标  
                temp.position = offset + camera.ScreenToWorldPoint(mScreenPosition);
                //协同，等待下一帧继续 
                yield return new WaitForFixedUpdate();
            }
            print("drag compeleted");
        }
    }
}
