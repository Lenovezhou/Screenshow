using DataBase;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;



public class ReadXml
{
    public static List<StyleInfo> ReadStyleXML(int HouseID, int SceneID, string fileInfo)
    {
        List<StyleInfo> LoadList = new List<StyleInfo>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot.ChildNodes)
        {
            StyleInfo newData = new StyleInfo();
            newData.HouseID = int.Parse(item["House"].InnerText);
            if (newData.HouseID==HouseID)
            {
                newData.SceneID = int.Parse(item["Scene"].InnerText);

                if (newData.SceneID == SceneID)
                {
                    newData.URL = item["URL"].InnerText;
                    newData.IconUrl = item["Icon"].InnerText;
                    newData.description = item["Description"].InnerText;
                    LoadList.Add(newData);
                }
            }
        }
        return LoadList;
    }
    
    #region Delete
    //public static List<string> ReadInfo(string fileInfo)
    //{
    //    List<string> LoadList = new List<string>();
    //    System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
    //    stringReader.Read(); // 跳过 BOM 
    //    System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
    //    XmlDocument myXML = new XmlDocument();
    //    myXML.LoadXml(stringReader.ReadToEnd());
    //    XmlElement Xmlroot = myXML.DocumentElement;
    //    foreach (XmlNode item in Xmlroot.ChildNodes)
    //    {
    //        Infomation newExcel = new Infomation();
    //        newExcel.URL = item["URL"].InnerText;
    //        newExcel.description = item["Description"].InnerText;
    //        LoadList.Add(newExcel.URL);
    //    }
    //    return LoadList;
    //}
    #endregion
    
    //场景Xml
    public static List<SceneManager> ReadSceneXml(int windowID, string fileInfo)
    {
        List<SceneManager> scene = new List<SceneManager>();

        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["program"].ChildNodes)
        {
            if (item.Attributes["HouseID"].Value == windowID.ToString())
            {
                SceneManager manager = new SceneManager();
                manager.ID = int.Parse(item.Attributes["SceneID"].Value);
                manager.Type = item.Attributes["SceneType"].Value;

                string[] temp = item.Attributes["Pos"].Value.Split('_');
                manager.ScenePos = new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));

                temp = item.Attributes["WindowID"].Value.Split('_');
                manager.WindowID = temp;

                temp = new string[3];
                temp[0] = item.Attributes["DiaoDing"].Value;
                temp[1] = item.Attributes["DiMian"].Value;
                temp[2] = item.Attributes["QiangMian"].Value;
                manager.QiuURL = temp;
                scene.Add(manager);
            }
        }
        return scene;
    }

    //房子户型XMl
    public static List<HouseManager> ReadHouseXml(string fileInfo)
    {
        List<HouseManager> House = new List<HouseManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["program"].ChildNodes)
        {
            HouseManager temp = new HouseManager();
            temp.ID = int.Parse(item.Attributes["HouseID"].Value);
            temp.Icon = item.Attributes["HouseIcon"].Value;
            temp.M_default = item.Attributes["DefaultSence"].Value;
            temp.Map = item.Attributes["HouseMap"].Value;
            House.Add(temp);
        }
        return House;
    }

    // 解析窗户的XML
    public static List<WindoManager> ReadWindowXml(string[] WindowsID, string path)
    {
        int index = -1;
        int Inde = 0;
        MsgCenter._instance.CleanList();
        //MsgCenter._instance.CleanAllList(MsgCenter._instance.nowWidow);
        MsgCenter._instance.TempDisctionary.Clear();
        List<WindoManager> AllWindow = new List<WindoManager>();
        System.IO.StringReader stringReader = new System.IO.StringReader(path);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        
        foreach (XmlNode item in Xmlroot.ChildNodes)
        {
            Debug.Log(WindowsID[0]);
            if (((IList)WindowsID).Contains(item.Attributes["ID"].Value))
            {
                index++;
                MsgCenter._instance.AddWindowList(Inde.ToString(), new Dictionary<string, GameObject>());
                WindoManager newExcel = new WindoManager();
                newExcel.WindowPictureUrl = item["WindowPictureUrl"].InnerText;
                newExcel.ID = index;

                float Scalex = float.Parse(item["WindowScale"].Attributes["X"].Value);
                float Scaley = float.Parse(item["WindowScale"].Attributes["Y"].Value);
                float Scalez = float.Parse(item["WindowScale"].Attributes["Z"].Value);

                float Positionx = float.Parse(item["WindowPosition"].Attributes["X"].Value);
                float Positiony = float.Parse(item["WindowPosition"].Attributes["Y"].Value);
                float Positionz = float.Parse(item["WindowPosition"].Attributes["Z"].Value);
                float Rotationx = float.Parse(item["WindowRotation"].Attributes["X"].Value);
                float Rotationy = float.Parse(item["WindowRotation"].Attributes["Y"].Value);
                float Rotationz = float.Parse(item["WindowRotation"].Attributes["Z"].Value);
                
               
                MsgCenter._instance.ModuleCount = (index+1)*item["Model"].ChildNodes.Count;
                
                for (int i = 0; i < item["Model"].ChildNodes.Count; i++)
                {
                    Curtain temp = new Curtain();
                    
                    temp.IsModel = true;
                    temp.ModelUrl = item["Model"].ChildNodes[i].InnerText;
                    temp.ScaleParameters = float.Parse(item["Model"].Attributes["CurtainScale"].Value);
                    Debug.Log(" index   " + index);
                    temp.Id = index;
                    string Stemp = temp.Id + temp.ModelUrl.Split('.')[0];
                    
                    if (!MsgCenter._instance.TempDisctionary.ContainsKey(Stemp))
                        MsgCenter._instance.TempDisctionary.Add(Stemp, false);
                    temp.TextureUrl = item["Model"].ChildNodes[i].Attributes["TextureUrl"].InnerText;
                    temp.Material = new Material(Resources.Load<Shader>("Standard"));
                    string[] rgba = item["Model"].ChildNodes[i].Attributes["RGB"].InnerText.Split('.');
                    
                    if (rgba[0] != "")
                    {
                        float r = float.Parse(rgba[0]);
                        float g = float.Parse(rgba[1]);
                        float b = float.Parse(rgba[2]);
                        float a = float.Parse(rgba[3]); 
                        temp.Material.color = new Color(r / 255, g / 255, b / 255, a / 255);
                    }
                    newExcel.Curtain.Add(temp);

                }
                newExcel.Scale = new Vector3(Scalex, Scaley,Scalez);
                newExcel.Position = new Vector3(Positionx, Positiony, Positionz);
                newExcel.Rotation = new Vector3(Rotationx, Rotationy, Rotationz);
               
                AllWindow.Add(newExcel);
                Inde++;
                
            }
        }
        index = 0;
        Inde = 0;
        return AllWindow;
    }
    
    // 单个窗帘组件解析XML
    public static List<Curtain> ReadInfo(int id, string fileInfo)
    {
        List<Curtain> LoadList = new List<Curtain>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());

        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot["Info"].ChildNodes)
        {
            Curtain newExcel = new Curtain();
            newExcel.IsModel = bool.Parse(item.Attributes["IsModel"].Value);
            newExcel.ModelUrl = item.Attributes["ModelUrl"].Value;
            //newExcel.MatID = int.Parse(item["ModelUrl"].Attributes["ID"].Value);
            newExcel.TextureUrl = item.Attributes["TextureUrl"].Value;
            newExcel.IconUrl = item.Attributes["Icon"].Value;
            Material temp = new Material(Resources.Load<Shader>("Standard"));
            string[] rgba = item.Attributes["RGB"].Value.Split('.');
            if (rgba[0] != "")
            {
                float r = float.Parse(rgba[0]);
                float g = float.Parse(rgba[1]);
                float b = float.Parse(rgba[2]);
                float a = float.Parse(rgba[3]);
                temp.color = new Color(r / 255, g / 255, b / 255, a / 255);
            }
            newExcel.Material = temp;
            LoadList.Add(newExcel);
        }
        return LoadList;
    }

    // 单独展示时解析XML 
    public static List<SingleCurtain> SingleReadInfo(string fileInfo)
    {
        List<SingleCurtain> LoadList = new List<SingleCurtain>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());

       

        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot.ChildNodes)
        {
            SingleCurtain newExcel = new SingleCurtain();
            //newExcel.IsModel = bool.Parse(item["IsModel"].InnerText);
            newExcel.TextureUrl = item["TextureUrl"].InnerText;
            LoadList.Add(newExcel);
        }
        return LoadList;
    }

    // 解析整体XML
    public static List<AssetInfo> ReadAllAsset(string fileInfo)
    {
        List<AssetInfo> LoadList = new List<AssetInfo>();
        System.IO.StringReader stringReader = new System.IO.StringReader(fileInfo);
        stringReader.Read(); // 跳过 BOM 
        System.Xml.XmlReader reader = System.Xml.XmlReader.Create(stringReader);
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(stringReader.ReadToEnd());
        XmlElement Xmlroot = myXML.DocumentElement;
        foreach (XmlNode item in Xmlroot.ChildNodes)
        {
            AssetInfo myasset = new AssetInfo();
            myasset.DefaultTexture = item.Attributes["Icon"].Value;
            foreach (XmlNode data in item.ChildNodes)
            {
                myasset.ModelPath.Add(data.InnerText);

                Material temp = new Material(Resources.Load<Shader>("Standard"));

                string[] rgba = data.Attributes["RGB"].InnerText.Split('.');
                if (rgba[0] != "")
                {
                    float r = float.Parse(rgba[0]);
                    float g = float.Parse(rgba[1]);
                    float b = float.Parse(rgba[2]);
                    float a = float.Parse(rgba[3]);
                    temp.color = new Color(r / 255, g / 255, b / 255, a / 255);
                }
                myasset.material.Add(temp);
                myasset.Texture.Add(data.Attributes["TextureUrl"].Value);
            }
            LoadList.Add(myasset);
        }
        return LoadList;
    }



}

