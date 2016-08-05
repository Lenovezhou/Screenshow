using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

public class NewInfomation
{
    public int Id;
    public string Name;
    public string URL;
    public string Icon;
    public string L;
    public string W;
    public string H;
    public string description;
}

public class NewReadXml
{
    public static List<NewInfomation> ReadInfo(string fileInfo)
    {
        List<NewInfomation> LoadList = new List<NewInfomation>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        if (MsgCenter._instance.StyleTarget == TargetStyle.chuanghu)
        {
            foreach (XmlNode item in Xmlroot["prod_list"].ChildNodes)
            {
                NewInfomation newExcel = new NewInfomation();

                if (item.Attributes["prod_pic_url"] != null)
                    newExcel.URL = item.Attributes["prod_pic_url"].Value;
                if (item.Attributes["prod_spic_url"] != null)
                    newExcel.Icon = item.Attributes["prod_spic_url"].Value;
                if (item.Attributes["prod_name"] != null)
                    newExcel.Name = item.Attributes["prod_name"].Value;
                if (item.Attributes["measure_l"] != null)
                    newExcel.L = item.Attributes["measure_l"].Value;
                if (item.Attributes["measure_w"] != null)
                    newExcel.W = item.Attributes["measure_w"].Value;
                if (item.Attributes["measure_h"] != null)
                    newExcel.H = item.Attributes["measure_h"].Value;
                //newExcel.description = item["Description"].InnerText;
                LoadList.Add(newExcel);
            }
        }
        else
        {
            foreach (XmlNode item in Xmlroot["panorama_list"].ChildNodes)
            {
                NewInfomation newExcel = new NewInfomation();

                if (item.Attributes["panorama_file_url"] != null)
                    newExcel.URL = item.Attributes["panorama_file_url"].Value;
                if (item.Attributes["panorama_spic_url"] != null)
                    newExcel.Icon = item.Attributes["panorama_spic_url"].Value;
                newExcel.Name = item.Attributes["panorama_name"].Value;
                //newExcel.description = item["Description"].InnerText;
                LoadList.Add(newExcel);
            }
        }
        return LoadList;
    }


    /// <summary>
    /// 房子户型XMl
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static List<HouseManager> ReadHouseXml(string fileInfo)
    {
        List<HouseManager> House = new List<HouseManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        if (Xmlroot["scene_list"] != null)
        {
            foreach (XmlNode items in Xmlroot["scene_list"].ChildNodes)
            {
                XmlElement item = items as XmlElement;
                HouseManager temp = new HouseManager();
                if (item.HasAttribute("scene_id"))
                    temp.ID = long.Parse(item.Attributes["scene_id"].Value);
                if (item.Attributes["scene_name"] != null)
                    temp.Name = item.Attributes["scene_name"].Value;
                else if (item.Attributes["scene_templet_name"] != null)
                    temp.Name = item.Attributes["scene_templet_name"].Value;
                else
                    temp.Name = "Null";
                if (item.HasAttribute("scene_templet_id"))
                    temp.Temp_ID = item.GetAttribute("scene_templet_id");
                if (item.HasAttribute("corp_id"))
                    temp.Corp_ID = item.GetAttribute("corp_id");
                if (item.HasAttribute("scene_spic_url"))
                    temp.Icon = item.GetAttribute("scene_spic_url");
                if (item.HasAttribute("idx__"))
                    temp.Idx = int.Parse(item.GetAttribute("idx__"));
                if (item.HasAttribute("scene_pic_url"))
                    temp.Map = item.GetAttribute("scene_pic_url");
                House.Add(temp);
            }
            return House;
        }
        else
            return null;
    }

    public static List<HouseManager> ReadHouseTempXml(string fileInfo)
    {
        List<HouseManager> House = new List<HouseManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        if (Xmlroot["scene_list"] != null)
        {
            foreach (XmlNode items in Xmlroot["scene_list"].ChildNodes)
            {
                XmlElement item = items as XmlElement;
                HouseManager temp = new HouseManager();
                if (item.HasAttribute("scene_id"))
                    temp.ID = long.Parse(item.Attributes["scene_id"].Value);
                if (item.Attributes["scene_name"] != null)
                    temp.Name = item.Attributes["scene_name"].Value;
                else if (item.Attributes["scene_templet_name"] != null)
                    temp.Name = item.Attributes["scene_templet_name"].Value;
                else
                    temp.Name = "Null";
                if (item.HasAttribute("scene_id"))
                    temp.Temp_ID = item.GetAttribute("scene_id");
                if (item.HasAttribute("corp_id"))
                    temp.Corp_ID = item.GetAttribute("corp_id");
                if (item.HasAttribute("scene_spic_url"))
                    temp.Icon = item.GetAttribute("scene_spic_url");
                if (item.HasAttribute("idx__"))
                    temp.Idx = int.Parse(item.GetAttribute("idx__"));
                if (item.HasAttribute("scene_pic_url"))
                    temp.Map = item.GetAttribute("scene_pic_url");
                House.Add(temp);
            }
            return House;
        }
        else
            return null;
    }
    /// <summary>
    /// 场景Xml
    /// </summary>
    /// <param name="windowID"></param>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static List<SceneManager> ReadSceneXml(string windowID, string fileInfo)
    {
        //Camera.main.GetComponent<AssetManager>().textshow.text += " 555555555555555 " + fileInfo;
        List<SceneManager> scene = new List<SceneManager>();

        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        if (Xmlroot["room_list"] != null)
        {
            foreach (XmlNode item in Xmlroot["room_list"].ChildNodes)
            {
                if (item.Attributes["scene_id"].Value == windowID)
                {
                    SceneManager manager = new SceneManager();
                    manager.ID = long.Parse(item.Attributes["room_id"].Value);
                    //manager.Type = item.Attributes["SceneType"].Value;
                    manager.HouseID = long.Parse(item.Attributes["scene_id"].Value);
                    manager.Idx = int.Parse(item.Attributes["room_code"].Value);
                    //manager.ScenePos = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));

                    float X = float.Parse(item.Attributes["room_x"].Value);
                    float Y = float.Parse(item.Attributes["room_y"].Value);
                    manager.ScenePos = new Vector2(X, Y);
                    //string[] temp;
                    //temp = item.Attributes["WindowID"].Value.Split('_');
                    //manager.WindowID = temp;

                    //temp = new string[3];
                    //temp[0] = item.Attributes["DiaoDing"].Value;
                    //temp[1] = item.Attributes["DiMian"].Value;
                    //temp[2] = item.Attributes["QiangMian"].Value;
                    //manager.QiuURL = temp;
                    scene.Add(manager);
                }
            }
            return scene;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 解析布局
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<WindoManager> ReadLoyoutXml(string path)
    {

        MsgCenter._instance.CleanList();
        int index = -1;
        List<WindoManager> window = new List<WindoManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(path);
        //stringReader.Read(); // 跳过 BOM 
        //System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        MsgCenter._instance.QiuURL = new string[3];
        if (Xmlroot["panorama_list"] != null)
        {
            foreach (XmlNode item in Xmlroot["panorama_list"].ChildNodes)
            {
                //Debug.Log(item.SelectSingleNode("//row").Attributes);
                string Type = item.Attributes["panorama_kind"] != null ? item.Attributes["panorama_kind"].Value : "";
                string idx = item.Attributes["sequ"] != null ? item.Attributes["sequ"].Value : "";
                string URL = item.Attributes["panorama_file_url"] != null ? item.Attributes["panorama_file_url"].Value : "";
                string Icon = item.Attributes["panorama_spic_url"] != null ? item.Attributes["panorama_spic_url"].Value : "";
                //Debug.Log(URL + "    " + item.Attributes["panorama_kind"].Value);
                if (idx == "0")
                {
                    if (Type == "103")
                    {//吊顶
                        MsgCenter._instance.QiuURL[0] = URL;
                        MsgCenter._instance.QiuIcon[0] = Icon;
                    }
                    if (Type == "102")
                    {//墙面
                        MsgCenter._instance.QiuURL[2] = URL;
                        MsgCenter._instance.QiuIcon[2] = Icon;
                    }
                    if (Type == "101")
                    {//地面
                        MsgCenter._instance.QiuURL[1] = URL;
                        MsgCenter._instance.QiuIcon[1] = Icon;
                    }
                }
            }
        }
        if (Xmlroot["layout_res_list"] != null)
        {
            foreach (XmlNode item in Xmlroot["layout_res_list"].ChildNodes)
            {
                WindoManager temp = new WindoManager();
                index++;
                temp.ID = index;
                temp.WindowPictureUrl = item.Attributes["def_model_url"] != null ? item.Attributes["def_model_url"].Value : "";
                temp.ModleType = item.Attributes["prod_kind"] != null ? int.Parse(item.Attributes["prod_kind"].Value) : 0;
                if (item.Attributes["unit_group"] != null)
                    temp.GroupID = item.Attributes["unit_group"].Value;
                if (item.Attributes["prod_id"]!=null )
                    temp.Prod_ID = item.Attributes["prod_id"].Value;
                if (item.Attributes["sequ"] != null)
                    temp.Sequ = item.Attributes["sequ"].Value;
                
                float Positionx = item.Attributes["pos_x"] != null ? float.Parse(item.Attributes["pos_x"].Value) : 0;
                float Positiony = item.Attributes["pos_y"] != null ? float.Parse(item.Attributes["pos_y"].Value) : 0;
                float Positionz = item.Attributes["pos_z"] != null ? float.Parse(item.Attributes["pos_z"].Value) : 0;
                temp.Position = new Vector3(Positionx, Positiony, Positionz);

                float scale = item.Attributes["scaling_per"] != null ? float.Parse(item.Attributes["scaling_per"].Value) : 0;
                temp.OfferScale = new Vector3(scale, scale, scale);

                temp.Scale = new Vector3(float.Parse(item.Attributes["size_l"].Value),float.Parse(item.Attributes["size_h"].Value), float.Parse(item.Attributes["size_w"].Value));

                float Rotation = item.Attributes["rotation_angle"] != null ? float.Parse(item.Attributes["rotation_angle"].Value) : 0;
                temp.Rotation = new Vector3(0, 0, Rotation);

                window.Add(temp);
            }
        }

        return window;
    }

    /// <summary>
    /// 加载整体窗帘
    /// </summary>
    /// <param name="group"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static List<CurtainManager> ReadCurtainXml(string group, string path)
    {
        MsgCenter._instance.Complete.Clear();
        MsgCenter._instance.ModuleCount = 0;
        List<CurtainManager> curtain = new List<CurtainManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(path);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        MsgCenter._instance.AddWindowList(group, new Dictionary<string, GameObject>());
        MsgCenter._instance.Complete.Add(group,new CompleteManager ());
        foreach (XmlNode items in Xmlroot["prod_info"].ChildNodes)
        {
            XmlElement item = items as XmlElement;
            if (item.HasAttribute("unit_id"))
                MsgCenter._instance.Complete[group].Unit_id = item.GetAttribute("unit_id");
            if (item.HasAttribute("price_std"))
                MsgCenter._instance.Complete[group].Price_std = item.GetAttribute("price_std");
            if (item.HasAttribute("price_cost"))
                MsgCenter._instance.Complete[group].Price_cost = item.GetAttribute("price_cost");
            if (item.HasAttribute("price"))//item.Attributes["price"] != null)
                MsgCenter._instance.Complete[group].Price = item.GetAttribute("price");
            MsgCenter._instance.Complete[group].Prod_id = item.GetAttribute("prod_id");
        }
        foreach (XmlNode items in Xmlroot["bom_list"].ChildNodes)
        {
            XmlElement item = items as XmlElement;
            CurtainManager temp = new CurtainManager();
            temp.Group_ID = group;
            if (item.Attributes["prod_id"] != null)
                temp.Prod_id = item.Attributes["prod_id"].Value;
            if (item.Attributes["model_kind"] != null)
            {
                if (item.Attributes["model_kind"].Value == "1001")
                    temp.IsModel = false;
                else
                    temp.IsModel = true;
                MsgCenter._instance.isModle = temp.IsModel;
            }
            //temp.ModleType = item.Attributes["prod_kind_code"].Value;
            if (item.Attributes["model_file_name"] != null)
                temp.ModuleName = Path.GetFileNameWithoutExtension(item.Attributes["model_file_name"].Value);
            if (item.Attributes["prod_kind_code"] != null)
                temp.ModuleType = item.Attributes["prod_kind_code"].Value;
            if (item.Attributes["model_file_url"] != null)
            {
                temp.ModleURL = item.Attributes["model_file_url"].Value;
            }
            //材质贴图
            if (item.Attributes["stylor_file_url"] != null)
                temp.TextureURL = item.Attributes["stylor_file_url"].Value;
            if (item.Attributes["prod_spic_url"] != null)
                temp.Icon = item.Attributes["prod_spic_url"].Value;
            temp.Material = new Material(Shader.Find("Standard"));

            if (item.HasAttribute("unit_id"))
                temp.Unit_id = item.Attributes["unit_id"].Value;
            if (item.HasAttribute("bom_prod"))
                temp.Bom_prod = item.GetAttribute("bom_prod");
            if (item.HasAttribute("bom_qty"))
                temp.Bom_qty = item.GetAttribute("bom_qty");
            if (item.HasAttribute("price_std"))
                temp.Price_std = item.Attributes["price_std"].Value;
            if (item.HasAttribute("price_cost"))
                temp.Price_cost = item.Attributes["price_cost"].Value;
            if (item.HasAttribute("price"))//item.Attributes["price"] != null)
                temp.Price = item.GetAttribute("price");

            string Stemp = temp.Group_ID + temp.ModleURL.Split('.')[0];
            if (!MsgCenter._instance.TempDisctionary.ContainsKey(Stemp))
            {
                MsgCenter._instance.ModuleCount += 1;
                MsgCenter._instance.TempDisctionary.Add(Stemp, false);
            }
            //temp.Type = item.Attributes["unit_group"].Value;
            curtain.Add(temp);
        }
        return curtain;
    }
    /// <summary>
    /// 替换整体窗帘
    /// </summary>
    /// <param name="root"></param>
    /// <param name="group"></param>
    /// <param name="path"></param>
    /// <param name="ismodle"></param>
    /// <returns></returns>
    public static List<CurtainManager> ReplaceCurtainXml(string root, string group, string path,bool ismodle)
    {
        string whatD = ismodle ? "1002" : "1001";
        MsgCenter._instance.TempDisctionary.Clear();
        //MsgCenter._instance.ModuleCount = 0;
        List<CurtainManager> curtain = new List<CurtainManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(path);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        MsgCenter._instance.ModuleCount = 0;
        if (root == "prod_list")
        {
            if (Xmlroot[root] != null)
            {
                //Debug.Log("dsdsdsdsdsdsdsdsdsd");
                foreach (XmlNode items in Xmlroot[root].ChildNodes)
                {
                    XmlElement item = items as XmlElement;
                    if (item.Attributes["model_kind"].Value == whatD)
                    {
                        MsgCenter._instance.Complete[group] = new CompleteManager();
                        if (item.HasAttribute("unit_id"))
                            MsgCenter._instance.Complete[group].Unit_id = item.Attributes["unit_id"].Value;

                        if (item.HasAttribute("price_std"))
                            MsgCenter._instance.Complete[group].Price_std = item.Attributes["price_std"].Value;
                        if (item.HasAttribute("price_cost"))
                            MsgCenter._instance.Complete[group].Price_cost = item.Attributes["price_cost"].Value;
                        if (item.HasAttribute("price"))//item.Attributes["price"] != null)
                            MsgCenter._instance.Complete[group].Price = item.GetAttribute("price");
                        CurtainManager temp = new CurtainManager();
                        temp.Group_ID = group;
                        temp.Prod_id = item.Attributes["prod_id"].Value;
                        temp.ModuleType = item.Attributes["prod_kind_code"].Value;
                        if (item.Attributes["model_file_name"] != null)
                            temp.ModuleName = Path.GetFileNameWithoutExtension(item.Attributes["model_file_name"].Value);
                        //temp.ModleType = item.Attributes["prod_kind_code"].Value;
                        temp.Name = item.Attributes["prod_name"].Value;
                        if (item.Attributes["model_kind"] != null)
                        {
                            if (item.Attributes["model_kind"].Value == "1001")
                                temp.IsModel = false;
                            else
                                temp.IsModel = true;
                        }
                        if (item.Attributes["model_file_url"] != null)
                        {
                            temp.ModleURL = item.Attributes["model_file_url"].Value;
                        }
                        //材质贴图
                        if (item.Attributes["stylor_file_url"] != null)
                            temp.TextureURL = item.Attributes["stylor_file_url"].Value;
                        if (item.Attributes["prod_spic_url"] != null)
                            temp.Icon = item.Attributes["prod_spic_url"].Value;
                        else if (item.Attributes["prod_pic_url"] != null)
                            temp.Icon = item.Attributes["prod_pic_url"].Value;
                        temp.Material = new Material(Shader.Find("Standard"));

                        if (item.Attributes["unit_id"] != null)
                            temp.Unit_id = item.Attributes["unit_id"].Value;
                        if (item.Attributes["bom_prod"] != null)
                        {
                            temp.Bom_prod = item.Attributes["bom_prod"].Value;
                        }
                        if (item.Attributes["bom_qty"] != null)
                        {
                            temp.Bom_qty = item.Attributes["bom_qty"].Value;
                        }
                        if (item.Attributes["price_std"] != null)
                            temp.Price_std = item.Attributes["price_std"].Value;
                        if (item.Attributes["price_cost"] != null)
                            temp.Price_cost = item.Attributes["price_cost"].Value;
                        if (item.Attributes["price"] != null)
                            temp.Price = item.Attributes["price"].Value;

                        string Stemp = temp.Group_ID + temp.Icon.Split('.')[0];
                        if (!MsgCenter._instance.TempDisctionary.ContainsKey(Stemp))
                        {
                            MsgCenter._instance.ModuleCount += 1;
                            MsgCenter._instance.TempDisctionary.Add(Stemp, false);
                        }
                        //temp.Type = item.Attributes["unit_group"].Value;
                        curtain.Add(temp);
                    }
                }
            }
        }
        else if (root == "bom_list")
        {
            //Debug.Log("2222222222222222222222222222222222" + path);
            if (Xmlroot[root] != null)
            {
                MsgCenter._instance.isChuangLian = true;
                MsgCenter._instance.Complete[group] = new CompleteManager();
                foreach (XmlNode items in Xmlroot["prod_info"].ChildNodes)
                {
                    XmlElement item = items as XmlElement;
                    if (item.HasAttribute("unit_id"))
                        MsgCenter._instance.Complete[group].Unit_id = item.GetAttribute("unit_id");
                    if (item.HasAttribute("price_std"))
                        MsgCenter._instance.Complete[group].Price_std = item.GetAttribute("price_std");
                    if (item.HasAttribute("price_cost"))
                        MsgCenter._instance.Complete[group].Price_cost = item.GetAttribute("price_cost");
                    if (item.HasAttribute("price"))//item.Attributes["price"] != null)
                        MsgCenter._instance.Complete[group].Price = item.GetAttribute("price");
                    MsgCenter._instance.Complete[group].Prod_id = item.GetAttribute("prod_id");
                }
                foreach (XmlNode item in Xmlroot[root].ChildNodes)
                {
                    CurtainManager temp = new CurtainManager();
                    temp.Group_ID = group;
                    temp.Prod_id = item.Attributes["prod_id"].Value;
                    temp.ModuleType = item.Attributes["prod_kind_code"].Value;
                    if (item.Attributes["model_file_name"] != null)
                        temp.ModuleName = Path.GetFileNameWithoutExtension(item.Attributes["model_file_name"].Value);
                    //temp.ModleType = item.Attributes["prod_kind_code"].Value;
                    temp.Name = item.Attributes["prod_name"].Value;
                    if (item.Attributes["model_kind"] != null)
                    {
                        if (item.Attributes["model_kind"].Value == "1001")
                            temp.IsModel = false;
                        else
                            temp.IsModel = true;
                        MsgCenter._instance.isModle = temp.IsModel;
                    }
                    if (item.Attributes["model_file_url"] != null)
                    {
                        temp.ModleURL = item.Attributes["model_file_url"].Value;
                    }
                    //材质贴图
                    if (item.Attributes["stylor_file_url"] != null)
                        temp.TextureURL = item.Attributes["stylor_file_url"].Value;

                    if (item.Attributes["prod_spic_url"] != null)
                        temp.Icon = item.Attributes["prod_spic_url"].Value;
                    else if (item.Attributes["prod_pic_url"] != null)
                        temp.Icon = item.Attributes["prod_pic_url"].Value;

                    temp.Material = new Material(Shader.Find("Standard"));

                    if (item.Attributes["unit_id"] != null)
                        temp.Unit_id = item.Attributes["unit_id"].Value;
                    if (item.Attributes["bom_prod"] != null)
                    {
                        temp.Bom_prod = item.Attributes["bom_prod"].Value;
                    }
                    if (item.Attributes["bom_qty"] != null)
                    {
                        temp.Bom_qty = item.Attributes["bom_qty"].Value;
                    }
                    if (item.Attributes["price_std"] != null)
                        temp.Price_std = item.Attributes["price_std"].Value;
                    if (item.Attributes["price_cost"] != null)
                        temp.Price_cost = item.Attributes["price_cost"].Value;
                    if (item.Attributes["price"] != null)
                        temp.Price = item.Attributes["price"].Value;
                    //Debug.Log(temp.ModuleName + "           5151515151515515 ");
                    string Stemp = temp.Group_ID + temp.Icon.Split('.')[0];
                    if (!MsgCenter._instance.TempDisctionary.ContainsKey(Stemp))
                    {
                        MsgCenter._instance.ModuleCount += 1;
                        MsgCenter._instance.TempDisctionary.Add(Stemp, false);
                    }
                    //temp.Type = item.Attributes["unit_group"].Value;
                    curtain.Add(temp);
                }
            }
            else
            {
                if (Xmlroot["prod_info"] != null)
                {
                    //Debug.Log("121111111111111111212121212121");
                    MsgCenter._instance.isChuangLian = false;
                    //MsgCenter._instance.Complete[group] = new CompleteManager();
                    foreach (XmlNode items in Xmlroot["prod_info"].ChildNodes)
                    {
                        XmlElement item = items as XmlElement;
                        //if (item.HasAttribute("unit_id"))
                        //    MsgCenter._instance.Complete[group].Unit_id = item.GetAttribute("unit_id");
                        //if (item.HasAttribute("price_std"))
                        //    MsgCenter._instance.Complete[group].Price_std = item.GetAttribute("price_std");
                        //if (item.HasAttribute("price_cost"))
                        //    MsgCenter._instance.Complete[group].Price_cost = item.GetAttribute("price_cost");
                        //if (item.HasAttribute("price"))//item.Attributes["price"] != null)
                        //    MsgCenter._instance.Complete[group].Price = item.GetAttribute("price");
                        //MsgCenter._instance.Complete[group].Prod_id = item.GetAttribute("prod_id");

                        CurtainManager temp = new CurtainManager();
                        temp.Group_ID = group;
                        temp.Prod_id = item.Attributes["prod_id"].Value;
                        temp.ModuleType = item.Attributes["prod_kind_code"].Value;
                        if (item.Attributes["model_file_name"] != null)
                            temp.ModuleName = Path.GetFileNameWithoutExtension(item.Attributes["model_file_name"].Value);
                        //temp.ModleType = item.Attributes["prod_kind_code"].Value;
                        temp.Name = item.Attributes["prod_name"].Value;
                        if (item.Attributes["model_kind"] != null)
                        {
                            if (item.Attributes["model_kind"].Value == "1001")
                                temp.IsModel = false;
                            else
                                temp.IsModel = true;
                            MsgCenter._instance.isModle = temp.IsModel;
                        }
                        if (item.Attributes["model_file_url"] != null)
                        {
                            temp.ModleURL = item.Attributes["model_file_url"].Value;
                        }
                        //材质贴图
                        if (item.Attributes["stylor_file_url"] != null)
                            temp.TextureURL = item.Attributes["stylor_file_url"].Value;

                        if (item.Attributes["prod_spic_url"] != null)
                            temp.Icon = item.Attributes["prod_spic_url"].Value;
                        else if (item.Attributes["prod_pic_url"] != null)
                            temp.Icon = item.Attributes["prod_pic_url"].Value;
                        else if (item.Attributes["prod_pic"] != null)
                            temp.Icon = item.Attributes["prod_pic"].Value;
                        else
                            temp.Icon = "";

                        temp.Material = new Material(Shader.Find("Standard"));

                        if (item.Attributes["unit_id"] != null)
                            temp.Unit_id = item.Attributes["unit_id"].Value;
                        if (item.Attributes["prod_id"] != null)
                        {
                            temp.Bom_prod = item.Attributes["prod_id"].Value;
                        }
                        if (item.Attributes["bom_qty"] != null)
                        {
                            temp.Bom_qty = item.Attributes["bom_qty"].Value;
                        }
                        if (item.Attributes["price_std"] != null)
                            temp.Price_std = item.Attributes["price_std"].Value;
                        if (item.Attributes["price_cost"] != null)
                            temp.Price_cost = item.Attributes["price_cost"].Value;
                        if (item.Attributes["price"] != null)
                            temp.Price = item.Attributes["price"].Value;
                        //Debug.Log(temp.ModuleName + "           5151515151515515 ");
                        string Stemp = temp.Group_ID + temp.Icon.Split('.')[0];
                        if (!MsgCenter._instance.TempDisctionary.ContainsKey(Stemp))
                        {
                            MsgCenter._instance.ModuleCount += 1;
                            MsgCenter._instance.TempDisctionary.Add(Stemp, false);
                        }
                        //temp.Type = item.Attributes["unit_group"].Value;
                        curtain.Add(temp);
                    }
                }
            }
        }
        //Debug.Log("2222222222222222222222222222222222"+MsgCenter._instance.isChuangLian);
        return curtain;
    }

    /// <summary>
    /// 解析整体XML
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static List<AssetInfo> ReadAllAsset(string fileInfo)
    {
        List<AssetInfo> LoadList = new List<AssetInfo>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["prod_list"].ChildNodes)
        {
            AssetInfo myasset = new AssetInfo();
            myasset.Icon = item.Attributes["prod_pic_url"].Value; 
            myasset.isModle = true;
            myasset.Name = item.Attributes["prod_name"].Value;
            myasset.ProdId = item.Attributes["prod_id"].Value;
            LoadList.Add(myasset);
        }
        //AssetInfo my = new AssetInfo();
        //my.Icon = "quanbu2_2";
        //my.DefaultTexture = "Texture";
        //my.isModle = false;
        //LoadList.Add(my);
        return LoadList;
    }
    /// <summary>
    /// 单独展示时解析XML
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static List<SingleCurtain> SingleReadInfo(string fileInfo)
    {
        List<SingleCurtain> LoadList = new List<SingleCurtain>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["stylor_list"].ChildNodes)
        {
            SingleCurtain newExcel = new SingleCurtain();
            //newExcel.IsModel = bool.Parse(item["IsModel"].InnerText);
            //newExcel.Description = item["Description"].InnerText;
            newExcel.TextureUrl = item.Attributes["stylor_file_url"].Value;
            newExcel.Stylor_code = item.Attributes["stylor_code"].Value;
            newExcel.Name = item.Attributes["stylor_name"].Value;
            //newExcel.Curtain_kind = item.Attributes["prod_kind"].Value;
            LoadList.Add(newExcel);
        }
        return LoadList;
    }
    /// <summary>
    /// 错误检测
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static string Result(string fileInfo)
    {
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        string temp = "";
        if (Xmlroot["err_msg"]!=null)
            temp = Xmlroot["err_msg"].InnerText;

        return temp;
    }
    /// <summary>
    /// 读取地址信息
    /// </summary>
    /// <param name="fileInfo"></param>
    /// <returns></returns>
    public static fileData ReadFile(string fileInfo)
    {
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        XmlElement item = Xmlroot["file"] as XmlElement;
        fileData file = new fileData();
        file.file = item.GetAttribute("real_file");
        file.fileName = item.GetAttribute("file_name");
        return file;
    }

    public static void ReadFengGe(string fileInfo)
    {
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["design_style_list"].ChildNodes)
        {
            if (!MsgCenter._instance.FengGe.ContainsKey(item.Attributes["design_style_name"].Value))
                MsgCenter._instance.FengGe.Add(item.Attributes["design_style_name"].Value, item.Attributes["design_style"].Value);
        }
    }
    public static void ReadSceneStyle(string fileInfo)
    {
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        //stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["dict_list"].ChildNodes)
        {
            if (!MsgCenter._instance.FengGe.ContainsKey(item.Attributes["dict_name"].Value))
                MsgCenter._instance.SceneStyle.Add(item.Attributes["dict_name"].Value, item.Attributes["dict_id"].Value);
        }
    }
}

