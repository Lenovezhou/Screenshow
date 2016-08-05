using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

public class LoadDictionary : MonoBehaviour
{
    public static LoadDictionary _instans;
    public List<string> picURL;
    private List<string> picpath=new List<string>();
    private string localpath = "http://ftp563246.host532.zhujiwu.me/temp.xml";
    private string picurl = "http://101.231.255.158:8780/jzcl/";
   

  

    void Awake()
    {      
        _instans = this;
    }

    public void LoadXML()
    {
      //  StartCoroutine(StartDownLoadXML(localpath));
    }
    public void LoadPic(List<string> picURL)
    {
       
        StartCoroutine(StartDownLoadPic(picURL));//获取的图片连接
       
    }
    /// <summary>
    /// 从服务器下载图片文件并保存
    /// </summary>
    /// <param name="xmltext"></param>
    /// <returns></returns>
    IEnumerator StartDownLoadPic(List<string> picURL)
    {
        Debug.Log(picURL.Count + ">>>>>>>>>>>>");
        for (int i = 0; i < picURL.Count; i++)
        {
          //www(网址)
            WWW mywww = new WWW(picURL[i]);
            yield return mywww;
            Debug.Log(mywww.isDone+"mywww.isdone"+mywww.error);
            //保险条件
            if (mywww.isDone)
            {
                Debug.Log(picURL[i] + "oooowwwwwwwww");
                DirectoryInfo folder = new DirectoryInfo(Application.dataPath + "/Resources/" + picURL[i].Substring(1, 6));
                FileInfo fileinfo = new FileInfo(Application.dataPath + "/Resources" + picURL[i]);
                //如果不存在Resources文件夹则创建
                if (!folder.Exists)
                {
                    Debug.Log("文件夹不存在，创建中");
                    AssetDatabase.CreateFolder("Assets/Resources", picURL[i].Substring(1, 6));
                }
                //如果不存在 name.xml文件则创建
                if (!fileinfo.Exists)
                {
                 
                    Texture2D myTexture = mywww.texture;
                    byte[] data = myTexture.EncodeToPNG();
                    string localpath = Application.dataPath + "/Resources/" + picURL[i];
                    Debug.Log(localpath);
                    FileStream stream = new FileStream(localpath, FileMode.OpenOrCreate);
                    stream.Write(data, 0, data.Length);
                    Debug.Log("下载成功");
                   
                  
                  //  LoadPic("2015001");
                }
                }
               
        }
    }
    
    public void StartDownLoadXML(string filePath,string name)
    {
        
            byte[] model = Encoding.UTF8.GetBytes(filePath);
            int length = model.Length;
            //写入xml到本地
            string localpath = Application.dataPath + name+".xml";
            DirectoryInfo xmlinfo = new DirectoryInfo(Application.dataPath + "/Resources/addXML");
            FileInfo fileinfo = new FileInfo(Application.dataPath + "/Resources/addXML/" + name + ".xml");
            //如果不存在Resources文件夹则创建
            if (!xmlinfo.Exists)
            {
                Debug.Log("文件夹不存在，创建中");
                AssetDatabase.CreateFolder("Assets/Resources", "addXML");
            }
            //如果不存在 name.xml文件则创建
            if (!fileinfo.Exists)
            {
                Debug.Log("文件不存在，创建中");
                CreateModelFile(Application.dataPath + "/Resources/addXML", name + ".xml", model, length);
            }
            
        
    }

    void CreateModelFile(string path, string name, byte[] info, int length)
    {
        // DirectoryInfo xmlinfo = new DirectoryInfo(Application.dataPath + "XML");
        //文件流信息
        //StreamWriter sw;
        Stream sw;
        FileInfo t = new FileInfo(path + "//" + name);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.Create();
        }
        else
        {          
            //如果此文件存在则打开
            return;
        }
        //以行的形式写入信息
        //sw.WriteLine(info);
        sw.Write(info, 0, length);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    
    /// <summary>
    /// "读取xml中的picture_URL"
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
        public void ReadXML(string path)
        {
            XmlDocument myXML = new XmlDocument();
            myXML.Load(path);
            //myXML(path);
            Debug.Log(path);
            XmlElement Xmlroot = myXML.DocumentElement;
            Debug.Log("8888888888888" + Xmlroot.Name);
            //XmlNode node = myXML.FirstChild;
            XmlNodeList nodelist = Xmlroot.ChildNodes;
            Debug.Log("7777777777" + nodelist.Count);
            foreach (XmlNode nd in nodelist)
            {
                if(nd.Name == "brochure_list")
                {
                    XmlElement element = (XmlElement)nd;
                }
               
            }
           
        }


       

 }
