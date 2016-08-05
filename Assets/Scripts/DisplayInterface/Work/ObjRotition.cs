using UnityEngine;
using System.Collections;

public class ObjRotition : MonoBehaviour {

    public Transform SingleCamera;//单独展示的相机（初始化设置：  Clear Flags：Depth only   Culling Mask：Nothing）
    public GameObject CullCollider;

    public int SingleLayer;//单独展示的物体的层级  例如：8;
    public int OriginLayer;//原来物体的层级 如上;

    public float CameraTo3D=5;//3D物体展示时到摄像机的距离
    public float CameraTo2D=10;//2D物体展示时到摄像机的距离

    public float MoveSpeed=0.02f;//单独展示时物体的移动速度
    public float RotitongSpeed=0.5f;//三维模型的旋转速度

    GameObject ObjDown, ObjUp;

    bool isOnes;
    bool isDown, isUp;
    float m_time;
    RaycastHit hit; 
    Transform Target;
    Vector3 StartPos, StartEurla;

    //获取点击的目标物体
    public Transform getTarget()
    {
        return Target;
    }

    public bool IsPlane, IsStart, GoBack;
    int MainLayer = (1<<0)|(1<<1)|(1<<2)|(1<<4)|(1<<5)|(1<<9);//除单独展示的物体层级外的层级;
	// Use this for initialization
	void Start () 
    {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            isDown = true;
            ObjDown = SingleShow();
            m_time = Time.time;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(Time.time - m_time);
            if (isDown && Time.time - m_time < 0.25f)
            {
                isUp = true;
                ObjUp = SingleShow();
            }
        }

        
        if (isDown && Time.time - m_time > 0.26f)
        {
            isDown = false;
            isUp = false;
            ObjUp = null;
            ObjDown = null;
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            SingleShowOver();
        }

        //判断鼠标按下和抬起时的对象是不是一个，不是的话清空，是的话继续执行
        if (isDown && isUp && ObjUp != null && ObjDown != null)
        {
            if (ObjDown == ObjUp)
            {
                CullCollider.SetActive(true);
                Target = ObjUp.transform; 
                Target.gameObject.layer = SingleLayer;
                if (!IsStart)
                {
                    StartPos = Target.position;
                    StartEurla = Target.eulerAngles;
                    
                }
                SingleCamera.GetComponent<Camera>().cullingMask = (1 << SingleLayer);
                Camera.main.cullingMask = MainLayer;
                IsStart = true;
                isDown = false;
                isUp = false;
                ObjUp = null;
                ObjDown = null;
            }
        }

        if (IsPlane&&IsStart)
        {
            SingleShow(IsPlane, Target);
        }
        else if(!IsPlane&&IsStart)
        {
            SingleShow(IsPlane, Target);
        }

        if (GoBack)
        {
            Target.localScale = (Vector3.Lerp(Target.localScale, new Vector3(1.0001f, 1.0001f, 1.0001f), 0.08f));
            Target.position = (Vector3.Lerp(Target.position, StartPos, 0.08f));
            Target.eulerAngles = StartEurla;
            if (Target.localScale.z>=1)
                GoBack = false;
        }
	}
    /// <summary>
    /// 单独展示函数
    /// </summary>
    /// <param name="isToD">是否为2D物体， true=2D图片,false=3D模型</param>
    /// <param name="Target">点击的物体（Transfer）</param>
    void SingleShow(bool isToD,Transform target)
    {
        if (isToD)
        {
            if (!isOnes)
            {
                isOnes = true;
                target.position = new Vector3(target.position.x, target.position.y + target.GetComponent<MeshFilter>().mesh.bounds.size.y * 3, target.position.z);
            }
            //LookAt目标的中心点位置
            SingleCamera.LookAt(new Vector3(target.position.x, target.position.y - target.GetComponent<MeshFilter>().mesh.bounds.size.y * 3, target.position.z));
            if (Mathf.Abs(target.position.z - SingleCamera.position.z) > CameraTo2D)
            {
                target.Translate((SingleCamera.position - target.position) * MoveSpeed, Space.World);
            }
        }
        else
        {
            if (!isOnes)
            {
                isOnes = true;
                target.position = new Vector3(target.position.x, target.position.y + target.GetComponent<MeshFilter>().mesh.bounds.size.y , target.position.z);
            }
            //LookAt目标的中心点位置
            target.localScale = (Vector3.Lerp(target.localScale, new Vector3(0.3f, 0.3f, 0.3f), 0.1f));
            SingleCamera.LookAt(new Vector3(target.position.x, target.position.y - target.GetComponent<MeshFilter>().mesh.bounds.size.y, target.position.z));

            if (Mathf.Abs(target.position.z - SingleCamera.position.z) > CameraTo3D)
            {
                target.Translate((SingleCamera.position - target.position) * MoveSpeed, Space.World);
            }
            if (Mathf.Abs(target.position.z - SingleCamera.position.z) < CameraTo3D)
            {
                float temp = target.localEulerAngles.y;
                target.localEulerAngles = new Vector3(target.localEulerAngles.x, temp += RotitongSpeed, target.localEulerAngles.z);
            }
        }
    }
    /// <summary>
    /// 开始判断点击，
    /// </summary>
    public GameObject SingleShow()
    {
        GameObject temp=null;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 10000))
        {
            if (hit.collider.gameObject.layer == 0)
            {
                if (hit.transform.name == "chaunghu")
                {
                    IsPlane = true;
                    temp=hit.collider.gameObject;
                }
                else
                {
                    IsPlane = false;
                    temp=hit.collider.gameObject;
                }
            }
        }
        return temp;
    }
    /// <summary>
    /// 结束单独展示，一切还原
    /// </summary>
    public void SingleShowOver()
    {
        IsStart = false;
        GoBack = true;
        isOnes = false;
        CullCollider.SetActive(false);
        SingleCamera.GetComponent<Camera>().cullingMask = 0;
        Camera.main.cullingMask = -1;
        Target.gameObject.layer = OriginLayer;
    }

   
}
