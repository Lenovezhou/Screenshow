using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
public class WriteXml : MonoBehaviour {

    string path;
    AssetManager AssetManager;
    MsgCenter MsgCenter;
    public static WriteXml _Instand;
    public int Allamt;
    public List<float> temps;
    bool isGooon;
    public string xmlStr;
    List<CurtainManager> curtain;
    float startTime;
	// Use this for initialization
	void Start () 
    {
        _Instand = this;
        AssetManager = Camera.main.GetComponent<AssetManager>();
        MsgCenter = Camera.main.GetComponent<MsgCenter>();
        path = Application.dataPath + "/Ok.xml";
        temps = new List<float>();
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    IEnumerator Count(string number, float fWidth, float fHeight, float fDrape)
    {
        //if (Time.time - startTime >= 3.0f)
        //{
        //    startTime = Time.time;
        //    yield return null;
        //}
        //Camera.main.GetComponent<AssetManager>().textshow.text += " temps.Counttemps.Count：：  " + temps.Count;
        //Camera.main.GetComponent<AssetManager>().textshow.text += " curtain.Countcurtain.Count：：  " + curtain.Count;
        yield return new WaitUntil(() => temps.Count == curtain.Count);
        XmlDocument xmlDoc = new XmlDocument();

        XmlDeclaration declare = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        xmlDoc.AppendChild(declare);
        //创建根节点
        XmlElement Parent = xmlDoc.CreateElement("program");
        xmlDoc.AppendChild(Parent);
        //动作号和功能好（固定）
        XmlElement fubc = xmlDoc.CreateElement("func_id");
        XmlElement action = xmlDoc.CreateElement("action_id");
        fubc.InnerText = "MM404642";
        action.InnerText = "save";
        Parent.AppendChild(fubc);
        Parent.AppendChild(action);
        //条件
        XmlElement parameter = xmlDoc.CreateElement("parameter");
        Parent.AppendChild(parameter);

        XmlAttribute em = xmlDoc.CreateAttribute("empower__");
        em.Value = "1";

        //XmlAttribute ukey = xmlDoc.CreateAttribute("uKey");
        //ukey.Value = "gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM";
        XmlAttribute user = xmlDoc.CreateAttribute("user_id");
        user.Value = MsgCenter.userID;
        XmlAttribute corp_id = xmlDoc.CreateAttribute("corp_id");
        corp_id.Value = MsgCenter.corpID;
        XmlAttribute cart_code = xmlDoc.CreateAttribute("cart_code");
        cart_code.Value = Guid.NewGuid().ToString();
        //-90001  没登陆
        parameter.Attributes.Append(user);
        parameter.Attributes.Append(corp_id);
        parameter.Attributes.Append(cart_code);
        //parameter.Attributes.Append(ukey);
        parameter.Attributes.Append(em);

        //商品列表
        XmlElement prod_list = xmlDoc.CreateElement("prod_list");
        Parent.AppendChild(prod_list);
        //组件列表
        XmlElement prod_bom = xmlDoc.CreateElement("prod_bom");
        Parent.AppendChild(prod_bom);
        Camera.main.GetComponent<AssetManager>().textshow.text += " temps.Count：：  " + temps.Count;

        for (int i = 0; i < curtain.Count; i++)
        {
            XmlElement bom_row = xmlDoc.CreateElement("row");

            prod_bom.AppendChild(bom_row);
            XmlAttribute bom_prod = xmlDoc.CreateAttribute("bom_prod");//构成ID
            bom_prod.Value = curtain[i].Bom_prod;
            bom_row.Attributes.Append(bom_prod);
            XmlAttribute bom_qty = xmlDoc.CreateAttribute("bom_qty");//构成数量
            bom_qty.Value = curtain[i].Bom_qty;
            bom_row.Attributes.Append(bom_qty);

            XmlAttribute prod_id = xmlDoc.CreateAttribute("prod_id");//商品ID
            prod_id.Value = MsgCenter.Complete[MsgCenter.nowWidow.name].Prod_id;
            bom_row.Attributes.Append(prod_id);
            XmlAttribute prod_qty = xmlDoc.CreateAttribute("prod_qty");//商品数量
            prod_qty.Value = "1";
            bom_row.Attributes.Append(prod_qty);
            XmlAttribute unit_id = xmlDoc.CreateAttribute("unit_id");//单位id  
            unit_id.Value = curtain[i].Unit_id;
            bom_row.Attributes.Append(unit_id);
            XmlAttribute price_std = xmlDoc.CreateAttribute("price_std");//标准单价
            price_std.Value = curtain[i].Price_std;
            bom_row.Attributes.Append(price_std);
            XmlAttribute price_cost = xmlDoc.CreateAttribute("price_cost");//成本单价 
            price_cost.Value = curtain[i].Price_cost;
            bom_row.Attributes.Append(price_cost);
            XmlAttribute price = xmlDoc.CreateAttribute("price");//单价
            price.Value = curtain[i].Price;
            bom_row.Attributes.Append(price);
            XmlAttribute prod_amt = xmlDoc.CreateAttribute("prod_amt");//金额
            prod_amt.Value = curtain[i].Prod_amt;
            bom_row.Attributes.Append(prod_amt);
            XmlAttribute prod_long = xmlDoc.CreateAttribute("prod_long");//所需米数
            foreach (float temp in temps)
            {
                Camera.main.GetComponent<AssetManager>().textshow.text += i + " 组件米数为：：  " + temp + " 米   ";
            }
            if (temps.Count != 0)
            {

                prod_long.Value = temps[i].ToString();
            }
            bom_row.Attributes.Append(prod_long);
            Allamt += int.Parse(curtain[i].Prod_amt);
        }
        for (int i = 0; i < 1; i++)
        {
            XmlElement list_row = xmlDoc.CreateElement("row");
            prod_list.AppendChild(list_row);
            XmlAttribute prod_id = xmlDoc.CreateAttribute("prod_id");//商品ID
            prod_id.Value = MsgCenter.Complete[MsgCenter.nowWidow.name].Prod_id;
            list_row.Attributes.Append(prod_id);
            XmlAttribute prod_qty = xmlDoc.CreateAttribute("prod_qty");//商品数量
            //UI控制 套的量
            prod_qty.Value = number;
            list_row.Attributes.Append(prod_qty);
            XmlAttribute unit_id = xmlDoc.CreateAttribute("unit_id");//单位id  
            unit_id.Value = MsgCenter.Complete[MsgCenter.nowWidow.name].Unit_id;
            list_row.Attributes.Append(unit_id);
            XmlAttribute price_std = xmlDoc.CreateAttribute("price_std");//标准单价
            price_std.Value = MsgCenter.Complete[MsgCenter.nowWidow.name].Price_std;
            list_row.Attributes.Append(price_std);
            XmlAttribute price_cost = xmlDoc.CreateAttribute("price_cost");//成本单价 
            price_cost.Value = MsgCenter.Complete[MsgCenter.nowWidow.name].Price_cost;
            list_row.Attributes.Append(price_cost);
            XmlAttribute price = xmlDoc.CreateAttribute("price");//单价
            price.Value = Allamt.ToString();
            list_row.Attributes.Append(price);
            XmlAttribute prod_amt = xmlDoc.CreateAttribute("prod_amt");//金额
            prod_amt.Value = (Allamt * int.Parse(number)).ToString();
            list_row.Attributes.Append(prod_amt);
        }
        xmlStr= xmlDoc.InnerXml;

    }

    //点购物车时，写入数据;
    public void CreateAlterData(string number, float fWidth, float fHeight, float fDrape)
    {
        if (MsgCenter.nowWidow==null)
        {

            return ;
        }
        curtain = new List<CurtainManager>();
        //List<CurtainManager> curtain1 = new List<CurtainManager>();
        if (MsgCenter.isModle)
        {
            for (int j = 0; j < AssetManager.WindowList.First().Value.GetComponent<WindoManager>().Middle.childCount; j++)
            {
                Transform obj=AssetManager.WindowList.First().Value.GetComponent<WindoManager>().Middle.GetChild(j);
                if (obj.gameObject.activeInHierarchy)
                {
                    CurtainManager temp = obj.GetComponent<CurtainManager>();
                    curtain.Add(temp);
                    //htmlJS(temp.ModuleType, fWidth, fHeight, fDrape);
                }
            }
            for (int j = 0; j < AssetManager.WindowList.First().Value.GetComponent<WindoManager>().Up.childCount; j++)
            {
                Transform obj=AssetManager.WindowList.First().Value.GetComponent<WindoManager>().Up.GetChild(j);
                if (obj.gameObject.activeInHierarchy)
                {
                    CurtainManager temp = obj.GetComponent<CurtainManager>();
                    curtain.Add(temp);
                    //htmlJS(temp.ModuleType, fWidth, fHeight, fDrape);
                }
            }
        }
        else
        {
            for (int j = 0; j < AssetManager.WindowList.First().Value.GetComponent<WindoManager>().TwoD.childCount; j++)
            {
                Transform obj=AssetManager.WindowList.First().Value.GetComponent<WindoManager>().TwoD.GetChild(j);
                if (obj.gameObject.activeInHierarchy)
                {
                    CurtainManager temp = obj.GetComponent<CurtainManager>();
                    curtain.Add(temp);
                    //htmlJS(temp.ModuleType, fWidth, fHeight, fDrape);
                }
            }
        }

        //curtain1 = ExceptList(curtain,MsgCenter.OldData);

        foreach (CurtainManager t in curtain)
        { 
            //while(true)
            {
                htmlJS(t.ModuleType, fWidth, fHeight, fDrape);
                //if (isGooon)
                //{
                //    isGooon = false;
                //    continue;
                //    break;
                //}
            }
        }
        //startTime = Time.time;
        StartCoroutine(Count(number, fWidth, fHeight, fDrape));
    }

    public void OnLengMessage(float temp)
    {
        temps.Add(temp);
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 组件米数为：：  " + temp + " 米   ";
        //isGooon = true;
    }
    /// <summary>
    /// 访问html里的函数
    /// </summary>
    /// <param name="sKind"></param>
    /// <param name="fWidth"></param>
    /// <param name="fHeight"></param>
    /// <param name="fDrape"></param>
    public void htmlJS(string sKind, float fWidth, float fHeight, float fDrape)
    {
        Application.ExternalCall("jzCalculateSize", sKind, fWidth, fHeight, fDrape);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private List<CurtainManager> ExceptList(List<CurtainManager> New, List<CurtainManager> Old)
    {
        List<int> index = new List<int>();
        for (int i = 0; i < New.Count; i++)
        {
            for (int j = 0; j < Old.Count; j++)
            {
                if (New[i].Prod_id == Old[j].Prod_id)
                {
                    index.Add(j);
                    //New.RemoveAt(i);
                    //
                }
            }
        }
        for (int m = 0; m < index.Count;m++ )
        {
            New.Remove(New.Find(delegate(CurtainManager p) { return p.Prod_id == Old[index[m]].Prod_id; }));
        }
        for (int n = 0; n < New.Count; n++)
        {
            for (int j = 0; j < Old.Count; j++)
            {
                if (New[n].ModuleType == Old[j].ModuleType)
                {
                    CurtainManager temp = New[n];//New.Find(delegate(CurtainManager p) { return p.Prod_id == Old[j].Prod_id; });
                    Debug.Log(temp==null);
                    if (temp != null)
                    {
                        temp.Bom_prod = Old[j].Prod_id;
                        temp.Bom_qty = Old[j].Prod_qty;
                    }
                }
            }

        }

        return New;
        //Debug.Log(New[2].ID == Old[0].ID);
        //Debug.Log(New[2] == Old[0]);
        //List<CurtainManager> c = new List<CurtainManager>();
        //List<int> A = new List<int>() { 1, 2, 3, 5, 9 };
        //List<int> B = new List<int>() { 1,5, 9 };
        //c = New.Except(Old).ToList<CurtainManager>();
        //New.AddRange(Old);
        //c.Add( (CurtainManager)New.Except(Old));
        //foreach (CurtainManager plate in New)
        //{
        //    CurtainManager existPlate = New.FirstOrDefault(r => r.ID.Equals(plate.ID));
        //    Debug.Log(existPlate.ID);
        //}
        //c = New.FindAll(delegate(CurtainManager p) { return p.ID == "20160608000086"; });
        //List<CurtainManager> c = New.Except(Old).ToList();

        //List<CurtainManager> c = Enumerable.Except(New.Union(Old), New.Intersect(Old)).ToList();

        //foreach (CurtainManager i in New)
        //{
        //    if (!Old.Contains(i))
        //    {
        //        c.Add(i);
        //    }
        //}
        //return c;
    }

}
