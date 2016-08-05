using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour
{
    public void InitScene(SceneManager scene)
    {
        ID = scene.ID;
        Type = scene.Type;
        WindowID = scene.WindowID;
        ScenePos = scene.ScenePos;
        QiuURL = scene.QiuURL;
        Idx = scene.Idx;
        HouseID = scene.HouseID;
    }
    //场景ID
    private long iD;

    public long ID
    {
        get { return iD; }
        set { iD = value; }
    }

    private long houseID;

    public long HouseID
    {
        get { return houseID; }
        set { houseID = value; }
    }


    private int idx;

    public int Idx
    {
        get { return idx; }
        set { idx = value; }
    }
    //场景的类型
    private string type;

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    //场景中应该有的窗户的ID
    private string[] windowID;

    public string[] WindowID
    {
        get { return windowID; }
        set { windowID = value; }
    }
    //小地图上的图标位置
    private Vector2 scenePos;

    public Vector2 ScenePos
    {
        get { return scenePos; }
        set { scenePos = value; }
    }
    //场景的全景图URL地址
    private string[] qiuURL;

    public string[] QiuURL
    {
        get { return qiuURL; }
        set { qiuURL = value; }
    }
}
