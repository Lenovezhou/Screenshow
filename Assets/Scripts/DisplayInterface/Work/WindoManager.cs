using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataBase;

public class WindoManager : MonoBehaviour
{

    public void InitWindow(WindoManager Window)
    {

        ID = Window.Id;
        Curtain = Window.Curtain;
        WindowPictureUrl = Window.WindowPictureUrl;
        //Up = Window.Up;
        //Middle = Window.Middle;
        Position = Window.Position;
        Rotation = Window.Rotation;
        Scale = Window.Scale;
        GroupID = Window.GroupID;
        Sequ = Window.Sequ;
    }
    private string sequ;

    public string Sequ
    {
        get { return sequ; }
        set { sequ = value; }
    }
    //窗户的ID
    private int Id;

    public int ID
    {
        get { return Id; }
        set { Id = value; }
    }

    //窗户的位置
    private Vector3 position;

    public Vector3 Position
    {
        get { return position; }
        set { position = value; }
    }
    //窗户的旋转
    private Vector3 rotation;

    public Vector3 Rotation
    {
        get { return rotation; }
        set { rotation = value; }
    }
    //窗户的缩放
    private Vector3 scale;
    private Vector2 _tmpVec2;
    public Vector3 Scale
    {
        get { return scale; }
        set { scale = value; }
    }

    private Vector3 offerScale;

    public Vector3 OfferScale
    {
        get { return offerScale; }
        set { offerScale = value; }
    }
    //模型类型(101:窗帘     102    )
    private int modleType;

    public int ModleType
    {
        get { return modleType; }
        set { modleType = value; }
    }

    //该资源的组号
    private string groupID;

    public string GroupID
    {
        get { return groupID; }
        set { groupID = value; }
    }
    //商品ID
    private string prod_ID;

    public string Prod_ID
    {
        get { return prod_ID; }
        set { prod_ID = value; }
    }

    //窗户的图片的路径
    private string windowPictureUrl;

    public string WindowPictureUrl
    {
        get { return windowPictureUrl; }
        set { windowPictureUrl = value; }
    }






    //窗户下的所有窗帘
    private List<Curtain> curtain = new List<Curtain>();

    public List<Curtain> Curtain
    {
        get { return curtain; }
        set { curtain = value; }
    }
    //窗户下所有窗帘的所有贴图
    private List<Material> material = new List<Material>();

    public List<Material> Material
    {
        get { return material; }
        set { material = value; }
    }


    //窗户的上挂点
    private Transform up;

    public Transform Up
    {
        get { return up; }
        set { up = value; }
    }
    //窗户的中间挂点
    private Transform middle;

    public Transform Middle
    {
        get { return middle; }
        set { middle = value; }
    }

    //窗户的中间挂点
    public Transform twoD;

    public Transform TwoD
    {
        get { return twoD; }
        set { twoD = value; }
    }
    // Use this for initialization
    void Awake()
    {
        Up = this.transform.FindChild("guadian").FindChild("GD_UP01");
        Middle = this.transform.FindChild("guadian").FindChild("GD_Middle01");
        TwoD = this.transform.FindChild("guadian").FindChild("2D");
    }

    //// Update is called once per frame
    //void Update () {
    //    _tmpVec2 = Camera.main.WorldToViewportPoint(transform.position);
    //    //Debug.Log(_tmpVec2);
    //    if ((_tmpVec2.x < 0 || _tmpVec2.x > 1 ) ||
    //        (_tmpVec2.y < 0 || _tmpVec2.y > 1))
    //    {
    //        OnBecameVisible();
    //    }
    //}

    //void OnBecameVisible()
    //{
    //    //Debug.Log("1132131");
    //    oSingleMenu.SetActive(false);
    //}
}
