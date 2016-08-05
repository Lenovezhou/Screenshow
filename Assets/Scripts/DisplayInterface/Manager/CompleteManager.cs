using UnityEngine;
using System.Collections;

public class CompleteManager : MonoBehaviour {

    public void InitComplete(CompleteManager Curtain)
    {
        ModuleType = Curtain.ModuleType;
        Prod_id = Curtain.Prod_id;
        Unit_id = Curtain.Unit_id;
        Price_std = Curtain.Price_std;
        Price_cost = Curtain.Price_cost;
        Price = Curtain.Price;
        Prod_amt = (int.Parse(price) * int.Parse(Prod_qty)).ToString();
    }

    //组件类型
    public string moduleType;

    public string ModuleType
    {
        get { return moduleType; }
        set { moduleType = value; }
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



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
