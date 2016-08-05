using UnityEngine;
using System.Collections;

public class SendXML
{
    /// <summary>
    /// 发送企业ID
    /// </summary>
    /// <param name="func_id"></param>
    /// <param name="action_id"></param>
    /// <param name="companyId">企业iD</param>
    public static string SendCompanyId(string companyId, string func_id = "3D404641", string action_id = "page")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" corp_id=\"" +
           companyId + "\"/></program>";
        return strXMLModel;
    }


    /// <summary>
    /// 发送户型ID
    /// </summary>
    /// <param name="func_id"></param>
    /// <param name="action_id"></param>
    /// <param name="houseID">户型ID</param>
    public static string  SendHouseId(string houseID, string func_id = "3D404637", string action_id = "page")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" scene_id=\"" +
           houseID + "\"/></program>";
        return strXMLModel;
    }


    /// <summary>
    /// 发送房间ID
    /// </summary>
    /// <param name="func_id"></param>
    /// <param name="action_id"></param>
    /// <param name="sceneID">户型ID</param>
    /// <param name="roomID">房间ID</param>
    public static string SendRoomId(string sceneID, string roomID, string func_id = "3D404638", string action_id = "roomDetail")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" scene_id=\"" +
           sceneID + "\" room_id=\"" + roomID + "\"/></program>";
        return strXMLModel;

    }

    /// <summary>
    /// 发送产品ID
    /// </summary>
    /// <param name="func_id"></param>
    /// <param name="action_id"></param>
    /// <param name="companyId">企业ID</param>
    /// <param name="produceID">产品ID</param>
    public static string SendProduceID(string companyId, string produceID, string func_id = "MM404442", string action_id = "detail")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" corp_id=\"" +
           companyId + "\" prod_id=\"" + produceID + "\"/></program>";
        return strXMLModel;
    }


    public static string SendCompanyID_bro(string companyId, string func_id = "3D404751", string action_id = "query")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" corp_id=\"" +
           companyId + "\"/></program>";
        return strXMLModel;
    }


    public static string SendLayoutId(string layoutId, string func_id = "3D404767", string action_id = "page")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" brochure_id=\"" +
           layoutId + "\"/></program>";
        return strXMLModel;
    }


    public static string SendBooksId(string booksId, string func_id = "3D404762", string action_id = "detail")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" brochure_id=\"" +
           booksId + "\"/></program>";
        return strXMLModel;
    }


    public static string SendPagesId(string pagesId, string func_id = "3D404767", string action_id = "page")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" page_id=\"" +
           pagesId + "\"/></program>";
        return strXMLModel;
    }


    public static string SendCompanyInfomation(string companyId, string func_id = "3D404767", string action_id = "page")
    {
        string strXMLModel = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><program><func_id>" +
           func_id + "</func_id><action_id>" +
           action_id + "</action_id><parameter  uKey=\"gVWbw92dlJ3Xf1TMgV3clJ3XpRWPxA2c0FmZm9lbh1WZ9U7zz2M352OwxSNYj9mcw9Vak1jMwETNwATM\" " + "empower__=\"1\" page_id=\"" +
           companyId + "\"/></program>";
        return strXMLModel;
    }
}
