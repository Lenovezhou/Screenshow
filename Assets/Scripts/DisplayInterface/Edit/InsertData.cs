using UnityEngine;
using System.Collections;
using System.Xml;

public class InsertData{

    string _name, _code, _group, _pic, _spic, _audio, _designStyle, _sceneKind;
    /// <summary>
    /// 添加模版的场景
    /// </summary>
    /// <param name="name">场景名字</param>
    /// <param name="code">场景编号</param>
    /// <param name="Sequ">场景序号</param>
    /// <param name="corpID">企业ID</param>
    /// <param name="designStyle">类型ID</param>
    /// <param name="sceneKind">场景类型ID</param>
    /// <param name="pic">图片路径</param>
    /// <param name="spic">缩略图路径</param>
    /// <param name="audio">音乐路径</param>
    /// <returns></returns>
    public static string AddScene(string name, string code, string Sequ, string corpID, string designStyle, string sceneKind, string pic, string spic, string audio)
    {
        //场景名称
        //公司ID
        //场景类型
        //编码
        //序号
        //音乐
        //缩略图
        //大图
        //<?xml version="1.0" encoding="utf-8"?>
        //<program>
        //<func_id>3D404632</func_id>
        //<action_id>insert</action_id>
        //<parameter 
        //scene_name="scene_kind" 
        //corp_id="2015001"
        //scene_code="222"
        //design_style="20160602000002"
        //scene_kind="2"
        //sequ="1"
        //scene_music=""
        //scene_url="/res/d3/demo/scene/scene02.prefab"
        //scene_pic_url="/res/d3/demo/scene/scene02.jpg"
        //scene_pic_name="scene02.jpg"
        //scene_spic_url=""
        //scene_spic_name=""
        //scene_desc="desc"
        //uKey="gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM"
        ///>
        //</program>
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration declare = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        xmlDoc.AppendChild(declare);
        //创建根节点
        XmlElement Parent = xmlDoc.CreateElement("program");
        xmlDoc.AppendChild(Parent);
        //动作号和功能好（固定）
        XmlElement fubc = xmlDoc.CreateElement("func_id");
        XmlElement action = xmlDoc.CreateElement("action_id");
        fubc.InnerText = "3D404632";
        action.InnerText = "insert";
        Parent.AppendChild(fubc);
        Parent.AppendChild(action);
        //条件
        XmlElement parameter = xmlDoc.CreateElement("parameter");
        Parent.AppendChild(parameter);

        XmlAttribute ukey = xmlDoc.CreateAttribute("uKey");
        ukey.Value = "gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM";

        XmlAttribute scene_name = xmlDoc.CreateAttribute("scene_name");
        scene_name.Value = name;

        XmlAttribute corp_id = xmlDoc.CreateAttribute("corp_id");
        corp_id.Value = corpID;

        XmlAttribute scene_code = xmlDoc.CreateAttribute("scene_code");
        scene_code.Value =code;

        XmlAttribute design_style = xmlDoc.CreateAttribute("design_style");
        design_style.Value = designStyle;

        XmlAttribute scene_kind = xmlDoc.CreateAttribute("scene_kind");
        scene_kind.Value = sceneKind;
        XmlAttribute sequ = xmlDoc.CreateAttribute("sequ");
        sequ.Value = Sequ;
        XmlAttribute scene_music = xmlDoc.CreateAttribute("scene_music");
        scene_music.Value = audio;
        XmlAttribute scene_pic_url = xmlDoc.CreateAttribute("scene_pic_url");
        scene_kind.Value = pic;
        XmlAttribute scene_spic_url = xmlDoc.CreateAttribute("scene_spic_url");
        scene_spic_url.Value = spic;

        parameter.Attributes.Append(ukey);
        parameter.Attributes.Append(scene_name);
        parameter.Attributes.Append(corp_id);
        parameter.Attributes.Append(scene_code);
        parameter.Attributes.Append(design_style);
        parameter.Attributes.Append(scene_kind);
        parameter.Attributes.Append(sequ);
        parameter.Attributes.Append(scene_music);
        parameter.Attributes.Append(scene_pic_url);
        parameter.Attributes.Append(scene_spic_url);
        Debug.Log(xmlDoc.InnerXml);
        return xmlDoc.InnerXml;
    }


    /// <summary>
    /// 添加房间（空间）
    /// </summary>
    /// <param name="name">空间名字</param>
    /// <param name="code">空间编码（唯一）</param>
    /// <param name="sceneID">户型的ID</param>
    /// <param name="corpID">企业ID</param>
    /// <param name="roomX">空间点X</param>
    /// <param name="roomY">空间点Y</param>
    /// <param name="pic">大图</param>
    /// <param name="spic">小图</param>
    /// <returns></returns>
    public static string AddRoom(string name, string code, string sceneID, string corpID, string roomX, string roomY, string pic, string spic)
    {
        //<?xml version="1.0" encoding="utf-8"?>
        //<program>
        //<func_id>3D404638</func_id>
        //<action_id> insert </action_id>
        //<parameter 
        //scene_id=”1001”-----------------
        //room_z=””-----------------------
        //room_y=”80”---------------------
        //room_x=”10”---------------------
        //room_spic_url	=””---------------
        //room_spic_name=””---------------
        //room_pic_url=”/upload/2ef2dede-0f7f-473e-aa71-3a8744f27ca9.jpg”----
        //room_pic_name=”dikuang.jpg”-----
        //room_name=”客厅”----------------
        //room_desc=“desc”
        //room_code=“02”------------------
        //uKey="gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM"/>
        //</program>
        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration declare = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        xmlDoc.AppendChild(declare);
        //创建根节点
        XmlElement Parent = xmlDoc.CreateElement("program");
        xmlDoc.AppendChild(Parent);
        //动作号和功能好（固定）
        XmlElement fubc = xmlDoc.CreateElement("func_id");
        XmlElement action = xmlDoc.CreateElement("action_id");
        fubc.InnerText = "3D404638";
        action.InnerText = "insert";
        Parent.AppendChild(fubc);
        Parent.AppendChild(action);
        //条件
        XmlElement parameter = xmlDoc.CreateElement("parameter");
        Parent.AppendChild(parameter);

        XmlAttribute ukey = xmlDoc.CreateAttribute("uKey");
        ukey.Value = "gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM";

        XmlAttribute room_x = xmlDoc.CreateAttribute("room_x");
        room_x.Value = roomX;

        XmlAttribute corp_id = xmlDoc.CreateAttribute("corp_id");
        corp_id.Value = corpID;

        XmlAttribute room_y = xmlDoc.CreateAttribute("room_y");
        room_y.Value =roomY;

        //XmlAttribute room_z = xmlDoc.CreateAttribute("room_z");
        //room_z.Value = roomZ;

        XmlAttribute scene_id = xmlDoc.CreateAttribute("scene_id");
        scene_id.Value = sceneID;
        XmlAttribute room_spic_url = xmlDoc.CreateAttribute("room_spic_url");
        room_spic_url.Value = spic;
        XmlAttribute room_pic_url = xmlDoc.CreateAttribute("room_pic_url");
        room_pic_url.Value = pic;
        XmlAttribute room_code = xmlDoc.CreateAttribute("room_code");
        room_code.Value = code;
        XmlAttribute Name = xmlDoc.CreateAttribute("room_name");
        Name.Value = name;

        parameter.Attributes.Append(ukey);
        parameter.Attributes.Append(Name);
        parameter.Attributes.Append(room_code);
        parameter.Attributes.Append(corp_id);
        parameter.Attributes.Append(scene_id);
        parameter.Attributes.Append(room_x);
        parameter.Attributes.Append(room_y);
        //parameter.Attributes.Append(room_z);
        parameter.Attributes.Append(room_spic_url);
        parameter.Attributes.Append(room_pic_url);
        return xmlDoc.InnerXml;
    }


    /// <summary>
    /// 添加全景图
    /// </summary>
    /// <param name="name">全景图名字</param>
    /// <param name="code">全景图编码</param>
    /// <param name="Sequ">序号</param>
    /// <param name="group">组号</param>
    /// <param name="designStyle">风格</param>
    /// <param name="panoramaKind">全景图类型</param>
    /// <param name="sceneID">户型ID</param>
    /// <param name="roomID">空间ID</param>
    /// <param name="corpID">企业ID</param>
    /// <param name="pic">全景图文件</param>
    /// <param name="spic">全景图的缩略图</param>
    /// <param name="bPic">大图</param>
    /// <returns></returns>
    public static string AddPanorama(string name, string code, string Sequ, string group, string designStyle, string panoramaKind, string sceneID, string roomID, string corpID, string pic, string spic, string bPic)
    {
//scene_id			    场景ID
//room_id			    空间ID
//corp_id			    企业ID
//sequ			        序号
//panorama_code			编码
//panorama_name			名称
//panorama_kind			类型
//design_style			风格
//group_code			组编码
//group_level			组层次	
//panorama_file_name	文件名称
//panorama_file_url		文件URL
//panorama_pic_url		大图名称
//panorama_pic_name		大图URL
//panorama_spic_url		小图URL
//panorama_spic_name	小图名称

        XmlDocument xmlDoc = new XmlDocument();
        XmlDeclaration declare = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
        xmlDoc.AppendChild(declare);
        //创建根节点
        XmlElement Parent = xmlDoc.CreateElement("program");
        xmlDoc.AppendChild(Parent);
        //动作号和功能好（固定）
        XmlElement fubc = xmlDoc.CreateElement("func_id");
        XmlElement action = xmlDoc.CreateElement("action_id");
        fubc.InnerText = "3D404636";
        action.InnerText = "insert";
        Parent.AppendChild(fubc);
        Parent.AppendChild(action);
        //条件
        XmlElement parameter = xmlDoc.CreateElement("parameter");
        Parent.AppendChild(parameter);

        XmlAttribute ukey = xmlDoc.CreateAttribute("uKey");
        ukey.Value = "gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM";

        XmlAttribute panorama_name = xmlDoc.CreateAttribute("panorama_name");
        panorama_name.Value = name;
        XmlAttribute panorama_code = xmlDoc.CreateAttribute("panorama_code");
        panorama_code.Value = code;
        XmlAttribute sequ = xmlDoc.CreateAttribute("sequ");
        sequ.Value = Sequ;
        XmlAttribute group_code = xmlDoc.CreateAttribute("group_code");
        group_code.Value = group;
        XmlAttribute design_style = xmlDoc.CreateAttribute("design_style");
        design_style.Value = designStyle;
        XmlAttribute panorama_kind = xmlDoc.CreateAttribute("panorama_kind");
        panorama_kind.Value = panoramaKind;
        XmlAttribute scene_id = xmlDoc.CreateAttribute("scene_id");
        scene_id.Value = sceneID;
        XmlAttribute room_id = xmlDoc.CreateAttribute("room_id");
        room_id.Value = roomID;
        XmlAttribute corp_id = xmlDoc.CreateAttribute("corp_id");
        corp_id.Value = corpID;
        XmlAttribute panorama_file_url = xmlDoc.CreateAttribute("panorama_file_url");
        panorama_file_url.Value = pic;
        XmlAttribute panorama_pic_url = xmlDoc.CreateAttribute("panorama_pic_url");
        panorama_pic_url.Value = bPic;
        XmlAttribute panorama_spic_url = xmlDoc.CreateAttribute("panorama_spic_url");
        panorama_spic_url.Value = spic;

        parameter.Attributes.Append(ukey);
        parameter.Attributes.Append(panorama_name);
        parameter.Attributes.Append(panorama_code);
        parameter.Attributes.Append(sequ);
        parameter.Attributes.Append(group_code);
        parameter.Attributes.Append(panorama_kind);
        parameter.Attributes.Append(design_style);
        parameter.Attributes.Append(scene_id);
        parameter.Attributes.Append(room_id);
        parameter.Attributes.Append(corp_id);
        parameter.Attributes.Append(panorama_file_url);
        parameter.Attributes.Append(panorama_pic_url);
        parameter.Attributes.Append(panorama_spic_url);

        Debug.Log(xmlDoc.InnerXml);
        return xmlDoc.InnerXml;
    }










	// Use this for initialization
	void Start () 
    {
	
	} 
	
	// Update is called once per frame
	void Update () 
    {
	
	}
}
