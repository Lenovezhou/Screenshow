using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UseCamareController : MonoBehaviour
{
    public Transform BasePositton;
    public float BaseAngles = 30;
    public float BaseLookAt = 0;
    public bool IOS = false;
    public bool IsStatic = false;
    public float MinFOV = 30;
    public float MaxFOV = 60;
    public float MinVerticalAngle = -30;
    public float MaxVerticalAngle = 30;
    public Transform Target1, Target2;
    public float TargetFOV;
    public bool IsCut = false;
    public bool IsCanMove = true;
    public GameObject oWindow;
    //Mask
    //public GameObject Mask;

    Transform Parent1, Parent2, ThisCamera;
    float BaseX, BaseY, BasePosZ;
    float UpDown;
    Vector3 BasePos, BaseMousePos, BaseMousePos2, MousePosLast, MousePosNow, LastPosition;
    float BaseTouchDistance;

    //Mask
    bool Flg1 = false, Flg2 = false;
    float ColorA = 0;

    // Use this for initialization
    void Awake()
    {
        ThisCamera = Camera.main.transform;
        ThisCamera.GetComponent<Camera>().fieldOfView = MaxFOV;
        TargetFOV = MaxFOV;
        MousePosLast = Input.mousePosition;


        Target1 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Target2 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;

        //鍒涘缓鍙傝冪墿浣搝z
        Target1.position = Vector3.zero;
        Target2.position = Vector3.zero;
        Target1.GetComponent<Renderer>().enabled = false;
        Target2.GetComponent<Renderer>().enabled = false;
        Target1.GetComponent<Collider>().enabled = false;
        Target2.GetComponent<Collider>().enabled = false;
        Target2.parent = Target1;

        Target1.localEulerAngles = new Vector3(0, BaseLookAt, 0);
        Target2.localEulerAngles = new Vector3(BaseAngles, 0, 0);

        //鍒涘缓鎽勫奖鏈鸿繍鍔ㄧ粨鏋剒z
        Parent1 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Parent2 = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
        Parent1.position = Vector3.zero;
        Parent2.position = Vector3.zero;
        Parent1.GetComponent<Renderer>().enabled = false;
        Parent2.GetComponent<Renderer>().enabled = false;
        Parent1.GetComponent<Collider>().enabled = false;
        Parent2.GetComponent<Collider>().enabled = false;

        Parent2.parent = Parent1;
        ThisCamera.parent = Parent2;

        //Parent1.localEulerAngles=new Vector3(0,BaseLookAt,0);
        Parent2.localEulerAngles = new Vector3(BaseAngles, 0, 0);
        ThisCamera.localPosition = new Vector3(0, 0, 0);
        ThisCamera.localEulerAngles = Vector3.zero;

        //
        Target1.position = BasePositton.position;
        Parent1.position = BasePositton.position;
        LastPosition = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        ////PC绔?搷浣渮z
        if (!IOS && IsCanMove && !EventSystem.current.IsPointerOverGameObject())
        {

            bool IsMouse = false;
            MousePosNow = Input.mousePosition;
            //鏃嬭浆zz
            if (Input.GetMouseButtonDown(0) && !IsStatic)
            {
                IsMouse = true;
                BaseY = Target1.localEulerAngles.y;
                BaseX = Target2.localEulerAngles.x;
                BaseMousePos = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0) && !IsMouse && !IsStatic)
            {
                float X = (Input.mousePosition.y - BaseMousePos.y) / 10 + BaseX;
                if (X > 180)
                {
                    if (X < 360 - MaxVerticalAngle)
                    {
                        X = 360 - MaxVerticalAngle;
                    }
                }
                if (X < 180)
                {
                    if (X > -MinVerticalAngle)
                    {
                        X = -MinVerticalAngle;
                    }
                }
                Target1.localEulerAngles = new Vector3(0, (BaseMousePos.x - Input.mousePosition.x) / 10 + BaseY, 0);
                Target2.localEulerAngles = new Vector3(X, 0, 0);
            }
            else
            {
                IsMouse = false;
                BaseMousePos = Input.mousePosition;
            }

            //榧犳爣鎺ㄦ媺zz
            if (Input.GetMouseButtonDown(1))
            {
                BaseMousePos2 = Input.mousePosition;
                BasePosZ = TargetFOV;//闀滃ご鐒﹁窛zz

            }
            // Debug.Log("ssss+sss:" + Input.GetAxis("Mouse ScrollWheel"));
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {

                TargetFOV -= Input.GetAxis("Mouse ScrollWheel") * 20f;
                //TargetFOV = (Input.mousePosition.y - BaseMousePos2.y) / 10 + BasePosZ;
                //闄愬埗zz
                if (TargetFOV > MaxFOV)
                {
                    TargetFOV = MaxFOV;
                }
                if (TargetFOV < MinFOV)
                {
                    TargetFOV = MinFOV;
                }
            }
            /*
            //婊氳疆鎺ㄦ媺zz
            MousePosLast=Input.mousePosition;
            ThisCamera.Translate(Vector3.forward*Input.GetAxis("Mouse ScrollWheel")*1,Space.Self);
			
            if(ThisCamera.position.y<Target1.position.y)
            {
                Target2.localEulerAngles=new Vector3 (0,Target2.localEulerAngles.y,Target2.localEulerAngles.z);
            }*/


            ///
            if (Input.GetMouseButton(2))
            {
                Target2.Translate((MousePosNow.x - MousePosLast.x) / 1000 * ThisCamera.localPosition.z, 0, 0);
                Target1.transform.position = Target2.position;
                Target2.localPosition = Vector3.zero;
                Target1.Translate(0, (MousePosNow.y - MousePosLast.y) / 1000 * ThisCamera.localPosition.z, 0);
            }
            //
            //if(Input.GetKey(KeyCode.F))//褰掗浂zz
            //{
            //    Target1.position=Vector3.zero;
            //    ThisCamera.localPosition=new Vector3 (0,0,-5);
            //}
        }

        //闀滃ご璺熼殢鐩?爣zz
        Target1.position = Parent1.position;
        ThisCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(ThisCamera.GetComponent<Camera>().fieldOfView, TargetFOV, 0.1f);
        if (transform.position == LastPosition)
        {
            Parent2.transform.rotation = Quaternion.Lerp(Parent2.transform.rotation, Target2.transform.rotation, 0.1f);
            //Parent2.transform.localEulerAngles = Vector3.Slerp(Parent2.transform.localEulerAngles, Target2.transform.localEulerAngles, 0.1f);
        }
        else
        {
            Parent2.transform.rotation = Target2.transform.rotation;
        }


        LastPosition = transform.position;

    }

}
