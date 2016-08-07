using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;
using System;
using System.IO;
public class LoadTypeXML
{
    private static LoadTypeXML _instance;
    List<typeID> typeList = new List<typeID>();
    public static int nowIndex = 0;
    public static string LayoutID;
    public static bool IsSame;
    public int nowType = 0;
    private LoadTypeXML()
    {

    }
    public static LoadTypeXML GetInstance()
    {
        if (_instance == null)
        {
            _instance = new LoadTypeXML();
        }
        return _instance;
    }

    /// <summary>
    /// 这个函数主要是从本地读取Xml文件，然后保存到list列表中；
    /// </summary>
    public void ReadXml()
    {

        //获取xml文件所在的路径
        string filePath = Application.dataPath + "/XML/config.xml";//后期要改地址
       // Debug.Log("XML文件的路径为" + filePath);
        //如果没有这个路径，就返回
        if (!File.Exists(filePath))
        {
            return;
        }

        XmlDocument doc = new XmlDocument();
        doc.Load(filePath);

        XmlElement Xmlroot = doc.DocumentElement;

        //XmlNode node = myXML.FirstChild;
        XmlNodeList nodelist = Xmlroot.ChildNodes;
        foreach (XmlNode nd in nodelist)
        {
            typeID tt = new typeID();
            //Debug.Log("nd 的名字为" + nd.Name);
            XmlAttributeCollection col = nd.Attributes;//获取该节点属性的集合
            XmlAttribute type11 = col["type"];//通过一个字符串索引，获取一个属性对象
            tt.type = Int32.Parse(type11.Value);
           // Debug.Log("Itype 的键值为：" + tt.type);

            XmlAttribute layoutID = col["layoutID"];
            tt.id = layoutID.Value;
            //Debug.Log("SlayoutID 的值为：" + tt.id);
            typeList.Add(tt);

        }
        //Debug.Log("typelist 的数量为" + typeList.Count);
        //foreach (typeID ii in typeList)
        //{
        //    Debug.Log("list 文件的id是多少：" + ii.id);
        //}
        //foreach (typeID ii in typeList)
        //{
        //    Debug.Log("list 文件的type是多少：" + ii.type);
        //}


        //读取场景，开始切换
        nowType = typeList[0].type;
        //Debug.Log("type : " +nowType);
        Application.LoadLevel(nowType);
    }

    /// <summary>
    /// 从list列表中根据下标读取到type和id，然后进行场景切换
    /// </summary>
    public void LoadScene()
    {
        if (nowIndex >= typeList.Count)
        {
            nowIndex = 0;
        }
        int aaaa = typeList[nowIndex].type;
        LayoutID = typeList[nowIndex].id;
        IsSame = true;
        nowIndex++;
        if (aaaa != nowType)
        {
            nowType = aaaa;
            IsSame = false;
            Application.LoadLevel(aaaa);
        }
       
    }
}

public class typeID
{
    public int type { get; set; }
    public string id { get; set; }
}
