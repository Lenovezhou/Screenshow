using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class ReadXML
{
    private static Dictionary<string, string> selfDictionary = new Dictionary<string, string>();
    public static List<string> ReadCompanyInfo(string data, InfoType infoTpye)
    {

        List<string> strList = new List<string>();
        XmlDocument myXML = new XmlDocument();
        myXML.LoadXml(data);
        string xpath = "";
        string xpath2 = "";
        switch (infoTpye)
        {
            case InfoType.SendCompanyID:
                xpath = "/program/scene_list/row";
                break;
            case InfoType.SendHouseID:
                xpath = "/program/room_list/row";
                break;
            case InfoType.SendRoomID:
                xpath = "/program/layout_res_list/row";
                xpath2 = "/program/panorama_list/row";
                break;
            case InfoType.SendProduceID:
                xpath = "/program/bom_list/row";
                break;
            case InfoType.Bro_sendcompanyID:
                xpath = "/program/brochure_list/row";
                break;
            case InfoType.Bro_sendLayoutID:
                xpath = "/program/rec_list/row";
                break;
            case InfoType.Bro_sendBooksID:
                xpath = "/program/page_list/row";
                xpath2 = "/program/catalog_list/row";
                break;
            case InfoType.Bro_sendPagesID:
                break;
            default:
                break;
        }
        if (xpath != string.Empty)
        {
            XmlNodeList XmlList = myXML.SelectNodes(xpath);
            //Debug.Log(XmlList.Count);
            foreach (XmlElement item in XmlList)
            {
                switch (infoTpye)
                {
                    case InfoType.SendCompanyID:
                        string info = item.Attributes["scene_templet_id"].Value;
                        string sceneid = item.Attributes["scene_id"].Value;
                        string LoadPath = item.Attributes["scene_spic_url"].Value;
                        //Debug.Log(LoadPath + "           aaa");
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath);
                        selfDictionary.Add(info, sceneid);
                        strList.Add(info);
                        break;
                    case InfoType.SendHouseID:
                        string info1 = item.Attributes["scene_id"].Value;
                        string info5 = selfDictionary[info1];
                        string info2 = item.Attributes["room_id"].Value;
                        string temp = info5 + "|" + info2;
                        strList.Add(temp);
                        break;
                    case InfoType.SendRoomID:
                        //Debug.Log(data);
                        if (item.Attributes["prod_kind"].Value == "101")
                        {
                            //Debug.Log(item.Attributes["prod_id"].Value);
                            string info3 = item.Attributes["corp_id"].Value;
                            string info4 = item.Attributes["prod_id"].Value;
                            string LoadPath1 = item.Attributes["prod_pic_url"].Value;
                            LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath1);
                            string temp1 = info3 + "|" + info4;
                            strList.Add(temp1);
                        }
                        else if (item.Attributes["prod_kind"].Value == "102")
                        {
                            string LoadPath2 = item.Attributes["def_model_url"].Value;
                            LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath2);
                        }
                        XmlNodeList XmlList2 = myXML.SelectNodes(xpath2);
                        foreach (XmlElement xmltemp in XmlList2)
                        {
                            string LoadPath3 = xmltemp.Attributes["panorama_file_url"].Value;
                            LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath3);
                            string LoadPath4 = xmltemp.Attributes["panorama_spic_url"].Value;
                            LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath4);
                        }
                        break;
                    case InfoType.SendProduceID:
                        string LoadPath5 = item.Attributes["prod_pic_url"].Value;
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath5);
                        string LoadPath6 = item.Attributes["model_file_url"].Value;
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath6);
                        break;

                    case InfoType.Bro_sendcompanyID:
                        //Debug.Log(item.Attributes["brochure_id"].Value);
                        string info6 = item.Attributes["brochure_id"].Value;
                        strList.Add(info6);

                        string LoadPath7 = item.Attributes["res_audio_url"].Value;
                        Debug.LogError("LoadPath7::" + LoadPath7);
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath7);
                        string LoadPath8 = item.Attributes["catalog_audio_url"].Value;
                        Debug.LogError("LoadPath8::" + LoadPath8);
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath8);
                        string LoadPath9 = item.Attributes["brochure_file_url"].Value;
                        Debug.LogError("LoadPath9::" + LoadPath9);
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath9);
                        string LoadPath10 = item.Attributes["brochure_spic_url"].Value;
                        Debug.LogError("LoadPath10::"+LoadPath10);
                        LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath10);

                        break;
                    case InfoType.Bro_sendLayoutID:
                        if (item.GetAttribute("res_file_url") != string.Empty)
                        {
                            string LoadPath11 = item.Attributes["res_file_url"].Value;
                            Debug.LogError("LoadPath11::" + LoadPath11);
                            LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath11);
                        }
                        break;

                    case InfoType.Bro_sendBooksID:


                        string info7 = item.Attributes["page_id"].Value;
                        strList.Add(info7);



                        XmlNodeList XmlList3 = myXML.SelectNodes(xpath2);
                        foreach (XmlElement xmltemp in XmlList3)
                        {
                            if (xmltemp.GetAttribute("res_audio_url") != string.Empty)
                            {
                                string LoadPath12 = xmltemp.Attributes["res_audio_url"].Value;
                                Debug.LogError("LoadPath12::" + LoadPath12);
                                LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath12);
                            }
                            if (xmltemp.GetAttribute("res_video_url") != string.Empty)
                            {
                                string LoadPath13 = xmltemp.Attributes["res_video_url"].Value;
                                Debug.LogError("LoadPath13::" + LoadPath13);
                                LoadConfig.GetInstance().m_downLoader.StartDownload(LoadPath13);
                            }

                        }

                        break;
                }
            }
        }
        return strList;
    }
}
