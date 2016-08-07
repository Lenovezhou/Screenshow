using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using UnityEngine.UI;
using DataBase;

/// <summary>
/// 接收MsgCenter传递过来的消息
/// 动态创建图片并添加属性
/// </summary>
public class LoadDataFromList : MonoBehaviour
{
    public GameObject PicturePrefab;            //实例prefab
    public GameObject HousePrefab;
    public GameObject ScenePrefab;
    public GameObject StylePrefab;

    public GameObject PrefabParent;             //实例对象的父物体
    public GameObject styleParent;
    public GameObject houseParent;
    public GameObject sceneParent;

    public List<GameObject> GameObjectPool;   //创建动态缓冲池存储实例对象
    public List<GameObject> StyleGameObjectPool;   //创建动态缓冲池存储实例对象
    public List<GameObject> HouseGameObjectPool;   //创建动态缓冲池存储实例对象
    public List<GameObject> SceneGameObjectPool;   //创建动态缓冲池存储实例对象


    private List<NewInfomation> ReceiveData;                      //接受到的消息（数据类型待定）
    private List<CurtainManager> mydata = new List<CurtainManager>();     //接受到的消息（数据类型待定）
    private List<AssetInfo> data;
    private List<HouseManager> HouseData;
    private List<SceneManager> SceneData;

    private List<string> server = new List<string>();
    public static LoadDataFromList _Instand;


    void Awake()
    {
        _Instand = this;
        GameObjectPool = new List<GameObject>();
        StyleGameObjectPool = new List<GameObject>();
        HouseGameObjectPool = new List<GameObject>();
    }

    /// <summary>
    /// 加载户型
    /// </summary>
    /// <param name="_Data"></param>
    #region 加载所有户型
    public void ReceiveMessage(List<HouseManager> _Data)
    {
        HouseData = _Data;
        InitHouse();

    }
    //克隆户型的的按钮
    void InitHouse()
    {

        ClearPool(HouseGameObjectPool);
        if (HouseGameObjectPool.Count > HouseData.Count)
        {
            for (int i = 0; i < HouseData.Count; i++)
            {
                //GameObjectPool[i] 表示第几个prefab
                string path = HouseData[i].Icon;
                server = InitServerConfig.Instance.m_servers;
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete2, OnLoadFaile2, true);
            }
            int DeleteNum = HouseGameObjectPool.Count - HouseData.Count;
            for (int i = 0; i < DeleteNum; i++)
            {
                HouseGameObjectPool[HouseData.Count + i].SetActive(false);
            }
        }
        else
        {
            int AddNum = HouseData.Count - HouseGameObjectPool.Count;
            int index = 0;
            if (HouseGameObjectPool.Count != 0)
            {
                Debug.Log("wwwwww");
                for (int i = 0; i < HouseGameObjectPool.Count; i++)
                {
                    //GameObjectPool[i] 表示第几个prefab
                    string path = HouseData[i].Icon;
                    server = InitServerConfig.Instance.m_servers;
                    InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete2, OnLoadFaile2, true);
                    index++;
                }
            }
            for (int i = 0; i < AddNum; i++)
            {
                GameObject Go = (GameObject)Instantiate(HousePrefab);
                Go.transform.parent = houseParent.transform;
                Go.AddComponent<HouseManager>().InitHouse(HouseData[index]);
                Go.transform.localScale = Vector3.one;
                HouseGameObjectPool.Add(Go);
                string path = HouseData[index].Icon;
                server = InitServerConfig.Instance.m_servers;
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, index + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete2, OnLoadFaile2, true);
                index++;
            }
        }
    }

    private void OnLoadFaile2(object data, string item)
    {
        Debug.Log("失败");
    }

    private void OnLoadUpdateZipComplete2(object data, string item)
    {
        Debug.Log("成功");
        int i = int.Parse(item);
        Texture t = data as Texture;
        HouseGameObjectPool[i].SetActive(true);
        HouseGameObjectPool[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t;
        HouseGameObjectPool[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = HouseData[i].Name;
        if (MsgCenter._instance.isInit)
        {
            MsgCenter._instance.isInit = false;
            //Camera.main.GetComponent<AssetManager>().textshow.text += " 1 111111111111111111111111111111111111111111  ";
            HouseGameObjectPool[0].GetComponent<HousPictureClick>().Request();
        }
    }
    #endregion

    /// <summary>
    /// 克隆场景的按钮
    /// </summary>
    /// <param name="_Data"></param>

    #region 克隆场景的按钮
    public void ReceiveMessage(List<SceneManager> _Data)
    {
        SceneData = _Data;
        InitScene();
        //SceneGameObjectPool[0].GetComponent<Toggle>().isOn = true;
    }

    private void InitScene()
    {
        if (SceneGameObjectPool.Count != 0)
        {
            MsgCenter._instance.LookIcon.SetParent(MsgCenter._instance.Map.transform);
            foreach (GameObject temp in SceneGameObjectPool)
            {
                DestroyImmediate(temp);
            }
            SceneGameObjectPool.Clear();
        }
        for (int i = 0; i < SceneData.Count; i++)
        {
            GameObject Go = (GameObject)Instantiate(ScenePrefab);
            Go.name = SceneData[i].ID.ToString();
            Go.AddComponent<SceneManager>().InitScene(SceneData[i]);
            Go.transform.SetParent(sceneParent.transform);
            Go.transform.localScale = Vector3.one;
            Go.GetComponent<RectTransform>().anchoredPosition = SceneData[i].ScenePos;
            SceneGameObjectPool.Add(Go);
        }
        if (SceneGameObjectPool.Count!=0)
            SceneGameObjectPool[0].GetComponent<Toggle>().isOn = true;
    }
    //回调

    #endregion
    /// <summary>
    /// 加载装饰风格
    /// </summary>
    /// <param name="_Data"></param>
    #region 加载装饰风格
    public void ReceiveMessage(List<NewInfomation> _Data)
    {
        //Debug.Log(_Data.PictureList[0]);
        ReceiveData = _Data;
        InitNew();
    }
    void InitNew()
    {
        ClearPool(StyleGameObjectPool);
        if (StyleGameObjectPool.Count > ReceiveData.Count)
        {
            for (int i = 0; i < ReceiveData.Count; i++)
            {
                //GameObjectPool[i] 表示第几个prefab
                string path = ReceiveData[i].Icon;
                server = InitServerConfig.Instance.m_servers;
                StyleGameObjectPool[i].GetComponent<ClickPicture>().setValue(ReceiveData[i]);
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete1, OnLoadFaile1, true);
            }
            int DeleteNum = StyleGameObjectPool.Count - ReceiveData.Count;
            for (int i = 0; i < DeleteNum; i++)
            {
                StyleGameObjectPool[ReceiveData.Count + i].SetActive(false);
            }
        }
        else
        {
            int AddNum = ReceiveData.Count - StyleGameObjectPool.Count;
            int index = 0;
            if (StyleGameObjectPool.Count != 0)
            {
                for (int i = 0; i < StyleGameObjectPool.Count; i++)
                {
                    //GameObjectPool[i] 表示第几个prefab
                    string path = ReceiveData[i].Icon;
                    server = InitServerConfig.Instance.m_servers;
                    StyleGameObjectPool[i].GetComponent<ClickPicture>().setValue(ReceiveData[i]);
                    InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete1, OnLoadFaile1, true);
                    index++;
                }
            }
            for (int i = 0; i < AddNum; i++)
            {
                GameObject Go = (GameObject)Instantiate(StylePrefab);
                Go.transform.parent = styleParent.transform;
                Go.transform.localScale = Vector3.one;
                StyleGameObjectPool.Add(Go);
                string path = ReceiveData[index].Icon;
                server = InitServerConfig.Instance.m_servers;
                StyleGameObjectPool[i].GetComponent<ClickPicture>().setValue(ReceiveData[i]);
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, index + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete1, OnLoadFaile1, true);
                index++;
            }
        }
    }
    private void OnLoadUpdateZipComplete1(object data, string item)
    {
        int i = int.Parse(item);
        Texture t = data as Texture;
        StyleGameObjectPool[i].SetActive(true);
        StyleGameObjectPool[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t;
        StyleGameObjectPool[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = ReceiveData[i].Name;
    }

    private void OnLoadFaile1(object data, string item)
    {
        Debug.Log("失败了？？");
    }
    #endregion

    /// <summary>
    /// 单个替换
    /// </summary>
    /// <param name="_Data"></param>

    #region 单个替换
    public void ReceiveMessage(List<CurtainManager> _Data)
    {
        mydata = _Data;
        data = null;
        InitNewdata();
    }
    //生成下方按钮，并赋值图片
    void InitNewdata()
    {
        ClearPool(GameObjectPool);
        if (GameObjectPool.Count > mydata.Count)
        {
            for (int i = 0; i < mydata.Count; i++)
            {
                //GameObjectPool[i] 表示第几个prefab
                string path = mydata[i].Icon;
                server = InitServerConfig.Instance.m_servers;
                GameObjectPool[i].GetComponent<ClickPicture>().setValue(mydata[i]);
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
            }
            int DeleteNum = GameObjectPool.Count - mydata.Count;
            Debug.Log(DeleteNum);
            for (int i = 0; i < DeleteNum; i++)
            {
                GameObjectPool[mydata.Count + i].SetActive(false);
                //GameObjectPool[mydata.Count + i].GetComponent<ClickPicture>().setValue("");
            }
        }
        else
        {
            int AddNum = mydata.Count - GameObjectPool.Count;
            int index = 0;
            if (GameObjectPool.Count != 0)
            {
                for (int i = 0; i < GameObjectPool.Count; i++)
                {
                    //GameObjectPool[i] 表示第几个prefab
                    string path = mydata[i].Icon;
                    server = InitServerConfig.Instance.m_servers;
                    GameObjectPool[i].GetComponent<ClickPicture>().setValue(mydata[i]);
                    //                                                    [ 服务器地址，,图片地址，                                        成功的回调                 失败回调 ]
                    InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
                    index++;
                }
            }
            for (int i = 0; i < AddNum; i++)
            {
                GameObject Go = (GameObject)Instantiate(PicturePrefab);
                Go.transform.parent = PrefabParent.transform;
                Go.transform.localScale = Vector3.one;
                GameObjectPool.Add(Go);
                GameObjectPool[index].GetComponent<ClickPicture>().setValue(mydata[index]);
                string path = mydata[index].Icon;
                server = InitServerConfig.Instance.m_servers;
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, index + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
                index++;
            }
        }
    }
    #endregion

    /// <summary>
    /// 整体替换
    /// </summary>
    /// <param name="Data"></param>
    #region 整体替换
    public void ReceiveMessage(List<AssetInfo> Data)
    {
        data = Data;
        mydata = null;
        InitNewdata1(data);
    }
    //
    void InitNewdata1(List<AssetInfo> Data)
    {
        ClearPool(GameObjectPool);
        if (GameObjectPool.Count > Data.Count)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                //GameObjectPool[i] 表示第几个prefab
                string path = Data[i].Icon;
                server = InitServerConfig.Instance.m_servers;
                GameObjectPool[i].GetComponent<ClickPicture>().setValue(Data[i]);
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
            }
            int DeleteNum = GameObjectPool.Count - Data.Count;
            for (int i = 0; i < DeleteNum; i++)
            {
                GameObjectPool[Data.Count + i].SetActive(false);
                //GameObjectPool[Data.Count + i].GetComponent<ClickPicture>().setValue(null);
            }
        }
        else
        {
            int AddNum = Data.Count - GameObjectPool.Count;
            int index = 0;
            if (GameObjectPool.Count != 0)
            {
                for (int i = 0; i < GameObjectPool.Count; i++)
                {
                    //GameObjectPool[i] 表示第几个prefab
                    string path = Data[i].Icon;
                    server = InitServerConfig.Instance.m_servers;
                    GameObjectPool[i].GetComponent<ClickPicture>().setValue(Data[i]);
                    InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, i + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
                    index++;
                }
            }
            for (int i = 0; i < AddNum; i++)
            {
                GameObject Go = (GameObject)Instantiate(PicturePrefab);
                Go.transform.parent = PrefabParent.transform;
                Go.transform.localScale = Vector3.one;
                GameObjectPool.Add(Go);
                GameObjectPool[index].GetComponent<ClickPicture>().setValue(Data[index]);
                string path = Data[index].Icon;
                server = InitServerConfig.Instance.m_servers;
                InitServerConfig._instance.m_iconLoader.StartDownload(server, "", path, index + "", null, eDownloadType.Type_Texture, OnLoadUpdateZipComplete, OnLoadFaile, true);
                index++;
            }
        }
    }

    #endregion


    #region 不使用队列加载图片的方法
    /// <summary>
    /// //解析获得到的数据,生成prefab
    /// </summary>
    //void Init()
    //{

    //    if (GameObjectPool.Count > ReceiveData.PictureList.Count)
    //    {
    //        for (int i = 0; i < ReceiveData.PictureList.Count; i++)
    //        {
    //            //GameObjectPool[i] 表示第几个prefab
    //            string path = ReceiveData.PictureList[i];
    //            WWW www = new WWW(path);
    //            GameObjectPool[i].SetActive(true);
    //            GameObjectPool[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = www.texture;
    //        }
    //        int DeleteNum = GameObjectPool.Count - ReceiveData.PictureList.Count;
    //        for (int i = 0; i < DeleteNum; i++)
    //        {
    //            GameObjectPool[ReceiveData.PictureList.Count + i].SetActive(false);
    //        }
    //    }
    //    else
    //    {
    //        int AddNum = ReceiveData.PictureList.Count - GameObjectPool.Count;
    //        int index = 0;
    //        if (GameObjectPool.Count != 0)
    //        {
    //            for (int i = 0; i < GameObjectPool.Count; i++)
    //            {
    //                //GameObjectPool[i] 表示第几个prefab
    //                string path = ReceiveData.PictureList[i];
    //                WWW www = new WWW(path);
    //                GameObjectPool[i].SetActive(true);
    //                GameObjectPool[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = www.texture;
    //                index++;
    //            }
    //        }
    //        for (int i = 0; i < AddNum; i++)
    //        {
    //            GameObject Go = (GameObject)Instantiate(PicturePrefab);
    //            Go.transform.parent = PrefabParent.transform;
    //            Go.transform.localScale = Vector3.one;
    //            //TODO 加载图片
    //            string path = ReceiveData.PictureList[index];
    //            WWW www = new WWW(path);
    //            Go.transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = www.texture;
    //            index++;
    //            GameObjectPool.Add(Go);
    //        }
    //    }
    //}
    #endregion


    #region 回调函数
    private void OnLoadUpdateZipComplete(object data, string item)
    {
        int i = int.Parse(item);
        Texture t = data as Texture;
        GameObjectPool[i].SetActive(true);
        GameObjectPool[i].transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = t;
        if (this.data!= null)
            GameObjectPool[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = this.data[i].Name;
        if(mydata!=null)
            GameObjectPool[i].transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = this.mydata[i].Name;
    }

    private void OnLoadFaile(object data, string item)
    {
        Debug.Log("失败了？？");
    }
    #endregion

    public void ClearPool(List<GameObject> temp)
    {
        foreach (GameObject item in temp)
        {
            Destroy(item);
        }
        temp.Clear();
    }

    void Disable(List<GameObject> gameObjectPool)
    {
        foreach (GameObject item in gameObjectPool)
        {
            item.SetActive(false);
        }
    }

    public void DisableStyle()
    {
        Disable(StyleGameObjectPool);
    }

    public void DisableCurtain()
    {
        Disable(GameObjectPool);
    }
}
