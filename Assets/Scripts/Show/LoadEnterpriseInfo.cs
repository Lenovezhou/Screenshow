using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class LoadEnterpriseInfo : MonoBehaviour
{
    public Transform imageObj;
    public Transform _text;
    public float timers;
    EnterpriseInfo enterInfo = new EnterpriseInfo();
    AudioSource myAudiosoutce;
    private List<string> infoPath = new List<string>();

    public void Start()
    {
        _GetDirectory(Application.dataPath + "/XML/CompanyXML");
        ReadEnterpriseInfo();
        StartCoroutine(wwwLoad());
    }
    public void Update()
    {
        timers -= Time.deltaTime;
        if (timers <= 0.05f)
        {
            LoadTypeXML.GetInstance().LoadScene();

            if (LoadTypeXML.IsSame)
            {
                // Debug.Log("++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                ReadEnterpriseInfo();
                StartCoroutine(wwwLoad());
            }
        }
    }
    /// <summary>
    /// 读取企业信息的XML
    /// </summary>
    public void ReadEnterpriseInfo()
    {
        string fileXml = "";
        foreach (string path in infoPath)
        {
            Debug.Log("路径为：" + path);
            fileXml = path;//后期修改路径

        }

        //Debug.Log("4444444fileXml的文件路径：" + fileXml);
        //如果不存在此文件，就返回
        if (!File.Exists(fileXml))
        {
            return;
        }
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(fileXml);
        XmlNode enterpriseNode = xmlDoc.DocumentElement;
        //Debug.Log("2222根节点的名字：" + enterpriseNode.Name);
        //Debug.Log("######根节点下面子节点的数量为：" + enterpriseNode.ChildNodes.Count);
        foreach (XmlNode node in enterpriseNode.FirstChild.ChildNodes)
        {
            // Debug.Log("node.Attributes[id].Value :" + node.Attributes["id"].Value);
            if (node.Attributes["id"].Value == LoadTypeXML.LayoutID)//“2”这个后期要修改
            {
                XmlAttributeCollection col = node.Attributes;//获取这个节点的属性的集合
                XmlAttribute address = col["address"];//后期要改名字
                enterInfo.Address = address.Value;
                XmlAttribute text = col["text"];//后期要改名字
                enterInfo.Text = text.Value;
                XmlAttribute music = col["music"];//后期要改名字
                enterInfo.Music = music.Value;
                XmlAttribute timer = col["timer"];//后期要改名字
                enterInfo.Timer = float.Parse(timer.Value);
            }

        }
    }

    IEnumerator wwwLoad()
    {
        timers = enterInfo.Timer;//记录场景播放的时间

        string textureAddress = "file://" + Application.dataPath + enterInfo.Address;//后期要改地址
        //Debug.LogError(textureAddress);
        WWW www = new WWW(textureAddress);
        Debug.Log("www加载的图片地址为:" + textureAddress);
        yield return www;
        if (www.isDone && www.error == null)
        {
            Texture tt = www.texture;
            //Debug.Log("www.Texture" + tt);
            imageObj.transform.GetComponent<RawImage>().texture = tt;
            _text.GetComponent<Text>().text = enterInfo.Text;
        }

        string musicAddress = "file://" + Application.dataPath + enterInfo.Music;//后期要改地址
        //Debug.LogError(bbb);
        www = new WWW(musicAddress);
        yield return www;
        if (www.isDone)
        {
            //Debug.Log("www加载的音乐地址为:" + bbb);
            //Debug.Log("wwwwwwwwwwwwwwww:" + www.audioClip);

            myAudiosoutce = this.GetComponent<AudioSource>();
            myAudiosoutce.clip = www.audioClip;
            myAudiosoutce.loop = true;
            myAudiosoutce.Play();
        }

    }


    //获取文件夹下的所有文件路径
    public bool _VerifyPath(string path)
    {
        return System.IO.Directory.Exists(path);
    }
    public void _GetDirectory(string strDirName)
    {

        if (!_VerifyPath(strDirName)) { return; }

        // string[] diArr = System.IO.Directory.GetDirectories(strDirName, "*", System.IO.SearchOption.AllDirectories);
        string[] rootfiArr = System.IO.Directory.GetFiles(strDirName);
        {
            if (rootfiArr != null)
            {
                foreach (string fi in rootfiArr)
                {
                    //Debug.Log("aaaaaaaaaaaaaaaaaaaaa" + fi.ToString());
                    if (!fi.Contains("meta"))
                    {
                        infoPath.Add(fi);
                    }
                }
            }
        }
        //Debug.Log("ccccccccccccc" + diArr.Length);
        //foreach (string dri in diArr)
        //{
        //    Debug.Log( "bbbbbbbbbbbbbb" + dri);
        //    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dri);
        //    {
        //        if (dri != null)
        //        {
        //            string[] fiArr = System.IO.Directory.GetFiles(dri);
        //            if (fiArr != null)
        //            {
        //                foreach (string fi in fiArr) { Debug.Log(fi + "diarrrrrr"); }
        //            }
        //        }
        //    }
        //}
    }

    public void OnExitClick()
    {
        Application.LoadLevel(0);
    }

}

public class EnterpriseInfo
{
    public int Id { get; set; }
    public string Address { get; set; }
    public string Text { get; set; }
    public string Music { get; set; }
    public float Timer { get; set; }
}