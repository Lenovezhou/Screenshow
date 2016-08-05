using UnityEngine;
using System.Collections;
using DataBase;
public class CurtainManager : MonoBehaviour
{

    public void InitCurtain(CurtainManager Curtain)
    {
        IsModel = Curtain.IsModel;
        TextureURL = Curtain.TextureURL;
        Material = Curtain.Material;
        Icon = Curtain.Icon;
        ModuleType = Curtain.ModuleType;
        Group_ID = Curtain.Group_ID;
        ModleURL = Curtain.ModleURL;
        ModuleName = Curtain.ModuleName;
        if (ModuleType == "10105" || ModuleType == "10109")
        {
            Bom_qty = "2";
        }
        else
            Bom_qty = "1";
        //Bom_qty = Curtain.Bom_qty;
        Bom_prod = Curtain.Bom_prod;
        Prod_id = Curtain.Prod_id;
        Unit_id = Curtain.Unit_id;
        Price_std = Curtain.Price_std;
        Price_cost = Curtain.Price_cost;
        Price = Curtain.Price;
        if (Bom_qty != null && Price != null)
            Prod_amt = (int.Parse(Price) * int.Parse(Bom_qty)).ToString();
        else
            Prod_amt = "0";
        if (Unit_id == null)
        {
            Unit_id = "102";
        }
    }

    //构成ID
    private string bom_prod;

    public string Bom_prod
    {
        get { return bom_prod; }
        set { bom_prod = value; }
    }
    //构成数量
    private string bom_qty;

    public string Bom_qty
    {
        get { return bom_qty; }
        set { bom_qty = value; }
    }
    //商品名字
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }
   
    //商品ID
    private string prod_id;
    public string Prod_id
    {
        get { return prod_id; }
        set { prod_id = value; }
    }
    //商品数量（判断组件类型）
    private string prod_qty;

    public string Prod_qty
    {
        get { return prod_qty; }
        set { prod_qty = value; }
    }

    //单位ID
    private string unit_id;

    public string Unit_id
    {
        get { return unit_id; }
        set { unit_id = value; }
    }
    //标准单价
    private string price_std;

    public string Price_std
    {
        get { return price_std; }
        set { price_std = value; }
    }
    //成本单价
    private string price_cost;

    public string Price_cost
    {
        get { return price_cost; }
        set { price_cost = value; }
    }
    //单价
    private string price;

    public string Price
    {
        get { return price; }
        set { price = value; }
    }
    //金额
    private string prod_amt;

    public string Prod_amt
    {
        get { return prod_amt; }
        set { prod_amt = value; }
    }

    //组件类型
    public string moduleType;

    public string ModuleType
    {
        get { return moduleType; }
        set { moduleType = value; }
    }
    //组件名称
    public string moduleName;

    public string ModuleName
    {
        get { return moduleName; }
        set { moduleName = value; }
    }
    //所处组号
    private string group_ID;

    public string Group_ID
    {
        get { return group_ID; }
        set { group_ID = value; }
    }

    //模型地址
    private string modleURL;

    public string ModleURL
    {
        get { return modleURL; }
        set { modleURL = value; }
    }
    //缩略图
    private string icon;

    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    //该窗帘组件的材质
    public Material material;

    public Material Material
    {
        get { return material; }
        set { material = value; }
    }
    //是否为模型
    public bool isModel;

    public bool IsModel
    {
        get { return isModel; }
        set { isModel = value; }
    }
    //贴图的类型（贴图的URl）
    private string textureURL;

    public string TextureURL
    {
        get { return textureURL; }
        set { textureURL = value; }
    }
    //贴图的重复次数
    private Vector2 textureScale;

    public Vector2 TextureScale
    {
        get { return textureScale; }
        set { material.mainTextureScale = textureScale = value; }
    }
    //贴图的偏移
    private Vector2 textureOffset;

    public Vector2 TextureOffset
    {
        get { return textureOffset; }
        set { material.mainTextureOffset = textureOffset = value; }
    }

    // Use this for initialization

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}
