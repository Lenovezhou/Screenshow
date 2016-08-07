using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class Global : MonoBehaviour {
    public GridContorll gridcontorll;
    public static List<int> typeidscene = new List<int>();
	public static List<string> childtexts = new List<string> ();
    public static List<string> childtextsscene = new List<string>();
    public static List<int> sceneindex=new List<int>();
    public static Dictionary<string, string> MSGdic = new Dictionary<string, string>();
    public static int scene0=0,scene1=1,scene2=2,scene3=3;
    public static List<string> roomname=new List<string>();
    public static List<string> roomid = new List<string>();
    private static List<string> xmlnames=new List<string>();

    
	void Start () {
     
        //string xmlname = "20160528000143";
      //  string xmlpath="file://" + Application.dataPath + "/XML/RoomXML/" + xmlname + ".xml";    
      //  Debug.Log("file://" + Application.dataPath + "/XML/RoomXML");
        GetDirectory(Application.dataPath + "/XML/RoomXML");
        GetDirectory(Application.dataPath + "/XML/BrochureALL");
        GetDirectory(Application.dataPath + "/XML/CompanyXML");
        for (int i = 0; i < xmlnames.Count; i++)
        {
        //    Debug.Log("_________________________" + i);
            if (xmlnames[i].Contains("RoomXML"))
            {
                ReadXml("file://" + xmlnames[i], xmlnames[i], "RoomXML", "room_list", "room_id", "room_name", "scene_name",scene1);
                gridcontorll.sceneindex = scene1;
               
            }
            if (xmlnames[i].Contains("BrochureALL"))
            {
                ReadXml("file://" + xmlnames[i], xmlnames[i] ,"BrochureALL","brochure_list","brochure_id","brochure_name",null,scene2);
                gridcontorll.sceneindex = scene2;
              
            }
            if (xmlnames[i].Contains("CompanyXML"))
            {
               
                ReadXml("file://" + xmlnames[i], xmlnames[i], "CompanyXML", "companeyinformation", "id", "name", null,scene3);
                gridcontorll.sceneindex = scene3;
              
            }

        }
	}
    

    //获取文件夹下的所有文件路径
    public static bool _VerifyPath(string path)
    {
        return System.IO.Directory.Exists(path);
    }
    public static void GetDirectory(string strDirName)
    {
   
        if (!_VerifyPath(strDirName)) { return; }
       
        string[] diArr = System.IO.Directory.GetDirectories(strDirName, "*", System.IO.SearchOption.AllDirectories);
        string[] rootfiArr = System.IO.Directory.GetFiles(strDirName);
        {
            if (rootfiArr != null)
            {
                foreach (string fi in rootfiArr) 
                {
                  //  Debug.Log(fi.ToString());
                        if (!fi.Contains("meta"))
                        {
                            Debug.Log(fi+"fiiiiiiiiiiiiiis");
                            xmlnames.Add(fi);
                        }
                }
            }
        }
        foreach (string dri in diArr)
        {
          // Debug.Log(dri);
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dri);
            {
                if (dri != null)
                {
                    string[] fiArr = System.IO.Directory.GetFiles(dri);
                    if (fiArr != null)
                    {
                        foreach (string fi in fiArr) { Debug.Log(fi+"diarrrrrr"); }
                    }
                }
            }
        }
    }



    // 获取字符串中的数字；
    public static decimal GetNumber(string str)
    {
        decimal result = 0;
        if (str != null && str != string.Empty)
        {
            // 正则表达式剔除非数字字符（不包含小数点.）
            str = Regex.Replace(str, @"[^\d.\d]", "");
            // 如果是数字，则转换为decimal类型
            if (Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
            {
                result = decimal.Parse(str);
            }
        }
        return result;
    }

	public static void ReadXml(string path,string xmlname,string pathcontains,string childnodes,string IDattribuesname,string name1,string name2,int scene)
    {
       
        XmlDocument xmldoc = new XmlDocument();
        xmldoc.Load(path);
        XmlNodeList topM = xmldoc.DocumentElement.ChildNodes;
      
            foreach (XmlNode nd in topM)
            {
                if (path.Contains(pathcontains))
                {
                    if (nd.Name == childnodes)
                    {
                       // Debug.Log("000000000000000");
                        
                       
                        XmlNodeList nodelist = nd.ChildNodes;
                        if (nodelist.Count > 0)
                        {
                          //  Debug.Log(nodelist.Count + "counttttt");
                            foreach (XmlElement el in nodelist)//读元素值
                            {
                               
                                if (el.Attributes[IDattribuesname].Value != "")
                                {
                                    typeidscene.Add(scene);
                                    Debug.Log(el.Attributes[IDattribuesname].Value);
                                    //  Debug.Log(el.Attributes["room_id"].Value);
                                    roomname.Add(el.Attributes[IDattribuesname].Value);
                                    childtexts.Add(el.Attributes[name1].Value);
                                    if (name2 != null)
                                    {
                                   //     Debug.Log(el.Attributes[name2].Value);
                                        childtextsscene.Add(el.Attributes[name2].Value);
                                    }
                                    else {
                                        childtextsscene.Add(null);
                                    }
                                  
                                    if (!MSGdic.ContainsKey(xmlname))
                                    {
                                        MSGdic.Add(xmlname, el.Attributes[IDattribuesname].Value);

                                    }


                                }
                            }
                        }
                    }
                }
            }      
    }
  

    

    //遍历子物体，保存用于配置信息：
    public static void Loadinformation(GameObject parent) 
    {
        roomid.Clear(); //每次收集roomid信息时清空以前保存的信息；
        for (int i = 0; i < parent.transform.childCount; i++)
        {
			if (parent.transform.GetChild(i).childCount>0)
			{
	            SortImage sortimage = parent.transform.GetChild(i).GetChild(0).GetComponent<SortImage>();
		           // if (!roomid.ContainsValue(sortimage.id))
		            {
		                roomid.Add( sortimage.id);
		                sceneindex.Add(sortimage.scenetype);
		            }
	          //  Debug.Log(sortimage.scenetype);
	          //  Debug.Log(sortimage.id);
			}
        }
    }


    public static void CreatXML() 
    {
      //  Debug.Log("CreatXML()");
        XmlDocument doc = new XmlDocument();
        XmlNode declare = doc.CreateXmlDeclaration("1.0", "utf-8", "");
        doc.AppendChild(declare);
        XmlElement root = doc.CreateElement("Program");
        doc.AppendChild(root);
        Debug.Log(roomid.Count+"................");
        Debug.Log(sceneindex.Count);
        for (int i = 0; i < roomid.Count; i++)
        {
            Debug.Log(i);
            XmlElement id = doc.CreateElement("id");
            //id.SetAttributeNode("layoutID", roomid[i]);
           
            id.SetAttribute("layoutID", roomid[i]);
           
            id.SetAttribute("type", sceneindex[i].ToString());
            root.AppendChild(id);
            
        }
        doc.Save(Application.dataPath + "/XML/config.xml");
    //    Debug.Log(Application.dataPath + "/000.xml");
    }


	void Update () {
	
	}
}
