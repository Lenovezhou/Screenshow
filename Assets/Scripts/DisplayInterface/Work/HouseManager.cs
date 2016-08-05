using UnityEngine;
using System.Collections;

public class HouseManager : MonoBehaviour
{
    public void InitHouse(HouseManager House)
    {
        ID = House.ID;
        Icon = House.Icon;
        M_default = House.M_default;
        Map = House.Map;
        Temp_ID = House.Temp_ID;
        Corp_ID = House.Corp_ID;
    }
    //ID
    public long iD;

    public long ID
    {
        get { return iD; }
        set { iD = value; }
    }

    //户型名
    private string name;

    public string Name
    {
        get { return name; }
        set { name = value; }
    }


    private string corp_ID;

    public string Corp_ID
    {
        get { return corp_ID; }
        set { corp_ID = value; }
    }

    public string temp_ID;

    public string Temp_ID
    {
        get { return temp_ID; }
        set { temp_ID = value; }
    }
    //缩略图
    public string icon;
    public string Icon
    {
        get { return icon; }
        set { icon = value; }
    }
    private int idx;

    public int Idx
    {
        get { return idx; }
        set { idx = value; }
    }
    //小地图
    private string map;

    public string Map
    {
        get { return map; }
        set { map = value; }
    }

    //默认的场景
    private string m_default;

    public string M_default
    {
        get { return m_default; }
        set { m_default = value; }
    }

}
