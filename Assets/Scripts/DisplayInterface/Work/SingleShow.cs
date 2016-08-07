using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using DataBase;
using UnityEngine.UI;

public class SingleShow : MonoBehaviour
{
    public static SingleShow _instance;

    public GameObject OpenWind;
    public Transform SingleCamera;//单独展示的相机（初始化设置：  Clear Flags：Depth only   Culling Mask：Nothing）
    public GameObject CullCollider;

    public GameObject ToSingle;
    public Text OpenWindow; 

    public GameObject PicturePrefab;
    public GameObject PrefabParent;
    private List<GameObject> GameObjectPool;
    private List<string> server = new List<string>();
    public List<SingleCurtain> mydata = new List<SingleCurtain>();

    public int SingleLayer;//单独展示的物体的层级  例如：8;
    public int OriginLayer;//原来物体的层级 如上;

    public float CameraTo3D = 3;//3D物体展示时到摄像机的距离
    public float CameraTo2D = 5;//2D物体展示时到摄像机的距离

    public float MoveSpeed=0.02f;//单独展示时物体的移动速度
    public float RotitongSpeed=0.5f;//三维模型的旋转速度
    public Transform UIButton;
    public bool IsBack=false;
    public GameObject oCurCompoment;   //  当前选中的组件
    GameObject ObjDown, ObjUp;

    public GameObject[] windowCompoments ;
    public Mesh[] windowMeshs;
    public Mesh[] windowSubMeshs;

    bool isOnes;
    bool isDown, isUp;
    public bool IsL=false,IsR=true;
    bool isAuto=true;
    public bool _isClose;
    public bool isClickSingle;//是否点击了单独展示按钮

    float m_time;
    RaycastHit hit;
    public Transform Target;
    public Transform ShowTarget;
    public Transform SingleTarget;
    Vector3 StartPos, StartEurla;
    Mesh _lastMesh;
    Vector2 _lastMousePosition;
    //获取点击的目标物体
    public Transform getTarget()
    {
        return SingleTarget;
    }

    void Awake()
    {
        _instance = this;
        oCurCompoment = null;
        _isClose = false;

        GameObjectPool = new List<GameObject>();
        windowMeshs = new Mesh[3];
        windowCompoments = new GameObject[3];
        windowSubMeshs = new Mesh[3];
    }
    public bool IsPlane, IsStart, GoBack;
    int MainLayer = (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5) | (1 << 9) | (1 << 10) | (1 << 11);//除单独展示的物体层级外的层级;
    private bool hasCloseShadow;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
    //void Update () {
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        _lastMousePosition = Input.mousePosition;
    //    }

    //    //按钮显示
    //    if (MsgCenter._instance.isDisplayList)
    //    {
    //        SingleAddButton._instance.Changetarget(Target);
    //        if (MsgCenter._instance.isModle && SingleAddButton._instance.ShuRu != null)
    //            SingleAddButton._instance.ShuRu.SetActive(true);
    //        ToSingle.SetActive(true);
    //    }
    //    if (!MsgCenter._instance.isDisplayList)
    //    {
    //        if (MsgCenter._instance.isModle && SingleAddButton._instance.ShuRu != null)
    //            SingleAddButton._instance.ShuRu.SetActive(false);
    //        ToSingle.SetActive(false);
    //    }
    //    //自动展示开关
    //    if (MsgCenter._instance.isSingleAuto)
    //    {
    //        isAuto = true;
    //    }
    //    else
    //        isAuto = false;
    //    //点击
    //    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        isDown = true;
    //        ObjDown = Singleshow();
    //        m_time = Time.time;
    //    }
    //    if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        if (isDown) //&& Time.time - m_time < 0.25f)
    //        {
    //            isUp = true;
    //            ObjUp = Singleshow();
    //        }
    //    }
    //    //点击时，判断鼠标是否有移动
    //    if (isDown && Input.GetAxis("Mouse X") != 0 && Input.GetAxis("Mouse Y") != 0)
    //    {
    //        isDown = false;
    //        isUp = false;
    //        ObjUp = null;
    //        ObjDown = null;
    //    }

    //    //返回主界面
    //    if (IsBack)
    //    {
    //        SingleshowOver();
    //        IsBack = false;
    //    }
    //    //Debug.Log(Input.GetAxis("Mouse X") + "            " + Input.GetAxis("Mouse Y"));
    //    //判断鼠标按下和抬起时的对象是不是一个，不是的话清空，是的话继续执行
    //    if (isDown && isUp && ObjUp != null && ObjDown != null&& Input.GetAxis("Mouse X") == 0&&Input.GetAxis("Mouse Y")==0)
    //    {
            
    //        if (ObjDown == ObjUp)
    //        {
    //            //if(MsgCenter._instance.lookTarget!=LookTarget.chuanglian)
    //                //MainMenu._instand.ButtonCurtains(false);
    //            Target = ObjUp.transform;
    //            oCurCompoment = ObjUp;
    //            if (!MsgCenter._instance.mask.activeInHierarchy)
    //            {
    //                if (MsgCenter._instance.isModle)
    //                {
    //                    if (ObjUp.GetComponent<CurtainManager>() != null)
    //                    {
    //                        MainMenu._instand.ButtonCurtains(false);
    //                        int type = (int)EnumToolV2.GetEnumName<ProdKind>(ObjUp.GetComponent<CurtainManager>().ModuleType);
    //                        /*传过去点击的这个物体的对应的模块的ID：type
    //                         */
    //                        //if (ObjUp.transform.parent.parent.parent.name == MsgCenter._instance.nowWidow.name)
    //                        //{
    //                        //    ShowTarget = ObjUp.transform;
    //                        //}
    //                        //MsgCenter._instance.GameManageList = MsgCenter._instance.WindowList[ObjUp.transform.parent.parent.parent.name];
    //                        //if (ObjUp.tag == "Sub")

    //                        UIToWindow._instance.ChangeWindow(ObjUp.GetComponent<CurtainManager>().Group_ID);
    //                        //Debug.Log(type);
    //                        UIButton.GetChild(type).GetComponent<ShowPictureList>().ChangeUIButton();
    //                    }
    //                    //MsgCenter._instance.m_MainMenu.ButtonCurtain(true);
    //                }
    //            }
    //            isDown = false;
    //            isUp = false;
    //            ObjUp = null;
    //            ObjDown = null;
    //        }
    //    }


    //    /* 点击单独显示按钮进入单独显示*/
    //    if (isClickSingle && ShowTarget != null)
    //    {
    //        this.GetComponent<UseCamareController>().IsCanMove = false;
    //        CullCollider.SetActive(true);
    //        SingleTarget = ShowTarget;
    //        SingleTarget.gameObject.layer = SingleLayer;
    //        if (SingleTarget.childCount > 0)
    //        {
    //            for (int i = 0; i < SingleTarget.childCount; i++)
    //            {
    //                SingleTarget.GetChild(i).gameObject.layer = SingleLayer;
    //            }
    //        }
    //        if (!IsStart)
    //        {
    //            StartPos = SingleTarget.position;
    //            StartEurla = SingleTarget.eulerAngles;
    //        }
    //        SingleCamera.GetComponent<Camera>().cullingMask = (1 << SingleLayer);
    //        Camera.main.cullingMask = MainLayer;
    //        isClickSingle = false;
    //        IsStart = true;
    //    }

    //    //开始
    //    if (IsPlane&&IsStart)
    //    {
    //        Singleshow(IsPlane, SingleTarget);
    //    }
    //    else if(!IsPlane&&IsStart)
    //    {
    //        Singleshow(IsPlane, SingleTarget);
    //    }
    //    //结束
    //    if (GoBack)
    //    {
    //        if(!IsPlane)
    //            SingleTarget.localScale = new Vector3(1.0001f, 1.0001f, 1.0001f);
    //        SingleTarget.position =StartPos;
    //        SingleTarget.eulerAngles = StartEurla;
    //            GoBack = false;
    //            SingleTarget = null;
    //        this.GetComponent<UseCamareController>().IsCanMove = true;
    //        SingleCamera.localPosition = Vector3.zero;
    //        SingleCamera.localEulerAngles = Vector3.zero;
    //    }
    //}
    /// <summary>
    /// 单独展示函数
    /// </summary>
    /// <param name="isPlane">是否为2D物体， true=2D图片,false=3D模型</param>
    /// <param name="Target">点击的物体（Transfer）</param>
    void Singleshow(bool isPlane, Transform target)
    {
        float ParentScale = target.transform.parent.parent.localScale.z;
        if (isPlane)
        {
            if (!isOnes)
            {
                isOnes = true;
                target.position = new Vector3(target.position.x, 0 + target.GetComponent<MeshFilter>().mesh.bounds.size.y, target.position.z);
            }
            //LookAt目标的中心点位置
            SingleCamera.LookAt(new Vector3(target.position.x, 0, target.position.z));
            if (Mathf.Abs(target.position.z - SingleCamera.position.z) > CameraTo2D)
            {
                target.Translate((SingleCamera.position - target.position) * 0.3f, Space.World);
                //target.position = new Vector3(0, target.GetComponent<MeshFilter>().mesh.bounds.size.y, target.position.z);
            }
            //target.transform.localScale = new Vector3(1 / ParentScale, 1 / ParentScale, 1 / ParentScale);
        }
        else
        {

            if (!isOnes)
            {
                isOnes = true;
                target.position = new Vector3(target.position.x, 0 + target.GetComponent<MeshFilter>().mesh.bounds.size.y, target.position.z);
            }

            if (SingleTarget != null)
            {
                if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    float temp = target.localEulerAngles.y;
                    ////target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp += RotitongSpeed, target.localEulerAngles.z);

                    ////Debug.Log(target.localEulerAngles.y);
                    //if (IsR && !IsL)
                    //{
                    //    if (temp < 40 || (temp == 0 || temp == 360) || temp > 319)
                    //        target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp += RotitongSpeed, target.localEulerAngles.z);
                    //    else if (temp >= 40)
                    //    {
                    //        IsR = false; IsL = true;
                    //    }
                    //}
                    //else if (IsL && !IsR)
                    //{
                    //    if (temp < 41 || (temp == 0 || temp == 360) || temp > 320)
                    //    {
                    //        target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp -= RotitongSpeed, target.localEulerAngles.z);
                    //    }
                    //    else if (temp <= 320)
                    //    {
                    //        IsR = true; IsL = false;
                    //    }
                    //}
                    if (temp < 40 || (temp == 0 || temp == 360) || temp > 320)
                    {
                        target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp += 5 * Input.GetAxis("Mouse X"), target.localEulerAngles.z);
                    }
                    if (temp > 40 && temp < 180)
                    {
                        target.localEulerAngles = new Vector3(target.localEulerAngles.x, 39, target.localEulerAngles.z);
                    }
                    if (temp < 320 && temp > 180)
                    {
                        target.localEulerAngles = new Vector3(target.localEulerAngles.x, 321, target.localEulerAngles.z);
                    }

                    Debug.Log(isPlane);
                    //SingleCamera.RotateAround(SingleTarget.position, Vector3.up, 10 * Input.GetAxis("Mouse X"));
                }
            }
            //LookAt目标的中心点位置
            SingleCamera.LookAt(new Vector3(target.position.x, 0, target.position.z));
            //SingleCamera.LookAt(new Vector3(0, 0 + target.GetComponent<MeshFilter>().mesh.bounds.size.y * 6, target.position.z));
            target.localScale = new Vector3(1f, 1f, 1f);
            //Debug.Log("           " + (target.position - SingleCamera.position).magnitude + "                    " + CameraTo3D);
            if ((target.position - SingleCamera.position).magnitude > CameraTo3D)
            {
                target.Translate((SingleCamera.position - target.position) * 0.3f, Space.World);
            }
            if ((target.position - SingleCamera.position).magnitude < CameraTo3D && isAuto)
            {
                float temp = target.localEulerAngles.y;

                //target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp += RotitongSpeed, target.localEulerAngles.z);

                if (IsR && !IsL)
                {
                    if (temp < 40 || (temp == 0 || temp == 360) || temp > 319)
                        target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp += RotitongSpeed, target.localEulerAngles.z);
                    else if (temp >= 40)
                    {
                        IsR = false; IsL = true;
                    }
                }
                else if (IsL && !IsR)
                {
                    if (temp < 41 || (temp == 0 || temp == 360) || temp > 320)
                    {
                        target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp -= RotitongSpeed, target.localEulerAngles.z);
                    }
                    else if (temp <= 320)
                    {
                        IsR = true; IsL = false;
                    }
                }
            }
            target.transform.localScale = new Vector3(1 / ParentScale, 1 / ParentScale, 1 / ParentScale);
        }
    }
    /// <summary>
    /// 开始判断点击，
    /// </summary>
    public GameObject Singleshow()
    {
        GameObject temp=null;
        if (Physics.Raycast(MsgCenter._instance.windowCamera.ScreenPointToRay(Input.mousePosition), out hit, 10000))
        {
            if (hit.collider.gameObject.layer ==11)
            {
                if (hit.collider.GetComponent<CurtainManager>().IsModel)
                {
                    IsPlane = false;
                    temp=hit.collider.gameObject;
                }
                else
                {
                    IsPlane = true;
                    temp=hit.collider.gameObject;
                }
            }
        }
        return temp;
    }
    /// <summary>
    /// 结束单独展示，一切还原
    /// </summary>
    public void SingleshowOver()
    {
        IsStart = false;
        GoBack = true;
        isOnes = false;
        CullCollider.SetActive(false);
        SingleCamera.GetComponent<Camera>().cullingMask = 0;
        Camera.main.cullingMask = MainLayer;
        SingleTarget.gameObject.layer = OriginLayer;
        if (SingleTarget.childCount > 0)
        {
            for (int i = 0; i < SingleTarget.childCount; i++)
            {
                SingleTarget.GetChild(i).gameObject.layer = OriginLayer;
            }
        }

    }

    //按钮事件
    public void OnClickTo()
    {
        if (MsgCenter._instance.Go != null)
        {
            ShowTarget = MsgCenter._instance.Go.transform;
            if (ShowTarget.transform.FindChild("shadow") != null)
            {
                if (ShowTarget.transform.FindChild("shadow").gameObject.activeSelf)
                {
                    hasCloseShadow = false;
                    ShowTarget.transform.FindChild("shadow").gameObject.SetActive(false);
                }
                else
                {
                    hasCloseShadow = true;
                    if (ShowTarget.transform.FindChild("Close_shadow"))
                        ShowTarget.transform.FindChild("Close_shadow").gameObject.SetActive(false);
                }
            }

            IsPlane = !ShowTarget.GetComponent<CurtainManager>().IsModel;
        }

        isClickSingle = true;
    }

    public void CloseWindow_OnCllick()
    {
        if (MsgCenter._instance.nowWidow != null)
        {
            _isClose = !_isClose;
            SetCurComAcitev(_isClose);
        }
    }

    public void SetButtonState(bool ison)
    {
        if (ison)
        {
            OpenWind.SetActive(true);
        }
        else
        {
            OpenWind.SetActive(false);
        }
    }

    public void SetCurComAcitev(bool subActive)
    {
        GameObject obj;
        if (subActive)
        {
            if (!MsgCenter._instance.isModle) return;
            OpenWindow.text = "开窗户";
            for (int i = 0; i < windowSubMeshs.Length; i++)
            {
                obj = windowCompoments[i];
                if (obj != null)
                {
                    if (obj.GetComponent<CurtainManager>().IsModel == false) continue;
                    if (obj.name == "2")
                    {
                        if (obj.transform.FindChild("Close_shadow") != null)
                            obj.transform.FindChild("Close_shadow").gameObject.SetActive(true);
                        if (obj.transform.FindChild("shadow") != null)
                            obj.transform.FindChild("shadow").gameObject.SetActive(false);
                    }
                    obj.GetComponent<MeshFilter>().mesh = windowSubMeshs[i];
                    obj.GetComponent<MeshCollider>().sharedMesh = windowSubMeshs[i];
                }
            }
            if (windowCompoments[windowCompoments.Length - 1]!=null)
                windowCompoments[windowCompoments.Length - 1].SetActive(false);
        }
        else
        {
            for (int i = 0; i < windowSubMeshs.Length; i++)
            {
                OpenWindow.text = "关窗户";
                obj = windowCompoments[i];
                if (obj != null)
                {
                    if (obj.GetComponent<CurtainManager>().IsModel == false) continue;
                    if (obj.name == "2")
                    {
                        if (obj.transform.FindChild("Close_shadow") != null)
                            obj.transform.FindChild("Close_shadow").gameObject.SetActive(false);
                        if (obj.transform.FindChild("shadow") != null)
                            obj.transform.FindChild("shadow").gameObject.SetActive(true);
                    }
                    obj.GetComponent<MeshFilter>().mesh = windowMeshs[i];
                    obj.GetComponent<MeshCollider>().sharedMesh = windowMeshs[i];
                }
            }
            windowCompoments[windowCompoments.Length - 1].SetActive(true);
        }
    }

    public void InitCurtainTexture(string qingqiu)
    {

        string temp = MsgCenter._instance.start(MsgCenter._instance.strXML("MM404439", "page", qingqiu));
        Debug.Log(temp);
        if(MsgCenter._instance.isModle)
            StartCoroutine(LoadXMLAsset());
    }

    IEnumerator LoadXMLAsset()
    {
        yield return new WaitWhile(() => MsgCenter._instance.xml == "");
        SendMessage(NewReadXml.SingleReadInfo(MsgCenter._instance.xml));
    }

    public void SendMessage(List<SingleCurtain> Data)
    {
        Debug.Log("传List！！");
        ReceiveMessage(Data);
    }

    public void ReceiveMessage(List<SingleCurtain> _Data)
    {
        mydata = _Data;
        InitNewdata();
    }

    void InitNewdata()
    {
        ClearList();
        if (GameObjectPool.Count == 0)
        {
            for (int i = 0; i < mydata.Count; i++)
            {
                Debug.Log("开始克隆！！" + mydata.Count);
                GameObject Go = (GameObject)Instantiate(PicturePrefab);
                //Debug.Log(Go.name);
                Go.transform.parent = PrefabParent.transform;
                Go.transform.localScale = Vector3.one;
                GameObjectPool.Add(Go);
                string path = mydata[i].TextureUrl;
                server = InitServerConfig.Instance.m_servers;
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
            }
        }
        else
        {
            for (int i = 0; i < mydata.Count; i++)
            {
                string path = mydata[i].TextureUrl;
                server = InitServerConfig.Instance.m_servers;
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
            }
        }

    }
    public void ClearList()
    {
        foreach (GameObject obj in GameObjectPool)
        {
            Destroy(obj);
        }
        GameObjectPool.Clear();
    }

    private void OnLoadUpdateZipComplete(object data, string item)
    {
        Debug.Log("成功！！");
        int i = int.Parse(item);
        Texture t = data as Texture;
        GameObjectPool[i].SetActive(true);
        GameObjectPool[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t;
        GameObjectPool[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = mydata[i].Name;
    }

    private void OnLoadFaile(object data, string item)
    {
        Debug.Log("失败了？？");
    }
}
