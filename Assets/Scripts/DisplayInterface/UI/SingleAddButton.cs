using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine.UI;
using DataBase;

public class SingleAddButton : MonoBehaviour
{
    public GameObject PicturePrefab;
    public GameObject PrefabParent;
    public InputField[] inputField;

    public Toggle _Toggle;
    public GameObject ShuRu;
    public GameObject CaiZhi;
    public GameObject TieTu;

    public static SingleAddButton _instance;

    float[] InputNum = new float[2];
    public Transform Target;
    private List<GameObject> GameObjectPool;
    private FurnitureData ReceiveData;
    private List<string> server = new List<string>();
    public List<SingleCurtain> mydata = new List<SingleCurtain>();

    public float[] PointUI;
    bool isCaiZhi;
    bool isTieTu;
    bool isMute;
    bool isChange;

    public 

    SingleShow Show;
    void Awake()
    {
        _instance = this;
        ReceiveData = new FurnitureData();
        GameObjectPool = new List<GameObject>();
        Show = Camera.main.GetComponent<SingleShow>();
    }
	// Use this for initialization
	void Start () 
    {
	    
	}
    void OnEnable()
    {
    }

    public void Changetarget( Transform target)
    {
        if (target != Target || Target == null)
        {
            Target = target;
            if (Target != null)
            {
                inputField[0].text = Target.GetComponent<Renderer>().material.mainTextureScale.x.ToString();
                inputField[1].text = Target.GetComponent<Renderer>().material.mainTextureScale.y.ToString();
                InputNum[0] = float.Parse(inputField[0].text);
                InputNum[1] = float.Parse(inputField[1].text);
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (_Toggle.isOn)
        {
            MsgCenter._instance.isSingleAuto = true;
        }
        else
            MsgCenter._instance.isSingleAuto = false;

        //Target = Show.getTarget();
        if (ShuRu.activeInHierarchy)
        {
            if (Target && isChange)
            {
                Material mater = Target.GetComponent<MeshRenderer>().material;
                mater.mainTextureScale = new Vector2(InputNum[0], InputNum[1]);
                //mater.mainTextureOffset = new Vector2(InputNum[2], InputNum[3]);
                Target.GetComponent<CurtainManager>().Material = mater;
                //    //Debug.Log(mater.mainTextureScale);
                isChange = false;
            }
        }

        //if (isTieTu && !isCaiZhi)
        //{
        //    ShuRu.SetActive(true);
        //    Up(TieTu.transform, 1);
        //}
        //else
        //{
        //    ShuRu.SetActive(false);
        //    Dwon(TieTu.transform, 0);
        //}
        //if (isCaiZhi && !isTieTu)
        //{
        //    ShuRu.SetActive(false);
        //    Up(CaiZhi.transform, 1);
        //}
        //else
        //{
        //    Dwon(CaiZhi.transform, 0);
        //}
        //if (SingleShow._instance.getTarget() == null)
        //{
        //    ShuRu.SetActive(false);
        //    TieTu.transform.localPosition = new Vector3(TieTu.transform.localPosition.x, PointUI[0], TieTu.transform.localPosition.z);
        //    CaiZhi.transform.localPosition = new Vector3(CaiZhi.transform.localPosition.x, PointUI[0], CaiZhi.transform.localPosition.z);
        //}
	}
    public void ChangeX(string value)
    {
        isChange = true;
        InputNum[0] = float.Parse(value);
    }
    public void ChangeY(string value)
    {
        isChange = true;
        InputNum[1] = float.Parse(value);
    }

    public void OnBack()
    {
        Show.IsBack = true;
    }

    public void OnAudio()
    {
        isMute = !isMute;
    }

    public void TieTuClick()
    {
        if (isTieTu)
            isTieTu = false;
        else
            isTieTu = true;
        isCaiZhi = false;
    }
    public void CaiZhiClick()
    {
        isTieTu = false;
        if (isCaiZhi)
            isCaiZhi = false;
        else
            isCaiZhi = true;
    }




    void Up(Transform Obj, int Id)
    {
        float rate = 0;
        rate = Obj.transform.localPosition.y;
        rate = Mathf.Lerp(rate, PointUI[1], 0.08f);
        if (Mathf.Abs(rate - PointUI[1]) > 0.5f)
            Obj.transform.localPosition = new Vector3(Obj.transform.localPosition.x, rate, Obj.transform.localPosition.z);

    }

    void Dwon(Transform Obj, int Id)
    {
        float rate = 0;
        rate = Obj.transform.localPosition.y;
        rate = Mathf.Lerp(rate, PointUI[0], 0.08f);
        if (Mathf.Abs(rate - PointUI[0]) > 0.5f)
            Obj.transform.localPosition = new Vector3(Obj.transform.localPosition.x, rate, Obj.transform.localPosition.z);


    }

    
}
