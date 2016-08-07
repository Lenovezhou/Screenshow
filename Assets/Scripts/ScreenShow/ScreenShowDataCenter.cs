using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PlayState 
{
    Introduce,
    Publicity,
    Show3D,
    Moive,
    None
}

public class UserData 
{
    public string UserName;
    public string Password;
    public string DataUrl;
    public bool LogoSucceed;

    public BackFunction LogoFinishFunc;
    public BackFunction UpdataFinishFunc;

    public UserChoosedData UserLoadedDatas;     //  存储用户选择的要展览的数据
    public Dictionary<int, PlayState> PlayMode;

    // 用户操作的错误信息
    private string _error;
    public string Error 
    {
        set 
        {
            _error = value;
        }
        get 
        {
            return _error;
        }
    }

    public UserData(string pUserName, string pPassword)
    {
        UserName = pUserName;
        Password = pPassword;
        LogoSucceed = false;
        Error = null;
        PlayMode = new Dictionary<int, PlayState>();
        PlayMode.Add(0, PlayState.Introduce);
        PlayMode.Add(1, PlayState.Moive);
        PlayMode.Add(2, PlayState.Publicity);
        PlayMode.Add(3, PlayState.Show3D);
    }
    /// <summary>
    /// 交换两个模式数据
    /// </summary>
    /// <param name="key1"></param>
    /// <param name="key2"></param>
    public void ExchangePlayModel(int key1,int key2) 
    {
        if (PlayMode.ContainsKey(key1) && PlayMode.ContainsKey(key2))
        {
            PlayState sta = PlayMode[key1];
            PlayMode[key1] = PlayMode[key2];
            PlayMode[key2] = sta;
        }
    }
}

public class ShowUserDate 
{
    Dictionary<string, string> Introduce;
    Dictionary<string, string> Publicity;
    Dictionary<string, string> Show3D;
    public ShowUserDate() 
    {
        Introduce = new Dictionary<string, string>();
        Publicity = new Dictionary<string, string>();
        Show3D = new Dictionary<string, string>();
    }
}

public class ChooseContent 
{
    public Dictionary<int, string> EnterpriseIntroduce;   //  企业介绍
    public Dictionary<int, string> PublicityBooks;   //  宣传册
    public Dictionary<int, string> Show3D;   //  3维展示
    public ChooseContent(Dictionary<int, string> pEnterpriseIntroduce,
        Dictionary<int, string> pPublicityBooks,
        Dictionary<int, string> pShow3D)
    {
        EnterpriseIntroduce = pEnterpriseIntroduce;
        PublicityBooks = pPublicityBooks;
        Show3D = pShow3D;
    }

    public ChooseContent() 
    {
        EnterpriseIntroduce = new Dictionary<int, string>();
        PublicityBooks = new Dictionary<int,string>();
        Show3D = new Dictionary<int,string>();
    }

}

public class UserChoosedData : ChooseContent
{
    public PlayState UserChoose;

    public UserChoosedData(Dictionary<int, string> pEnterpriseIntroduce,
        Dictionary<int, string> pPublicityBooks,
        Dictionary<int, string> pShow3D)
        : base(pEnterpriseIntroduce, pPublicityBooks, pShow3D)
    {     
    }

    public UserChoosedData() 
        :base()
    {
    }

    /// <summary>
    /// 添加或者删除已知键
    /// </summary>
    /// <param name="pKey"></param>
    /// <param name="pContent"></param>
    public void AddToCurrentData(int pKey,string pContent,ref bool pChecked)
    {
        switch (UserChoose)
        {
            case PlayState.Introduce:
                if (EnterpriseIntroduce.ContainsKey(pKey))
                {
                    EnterpriseIntroduce.Remove(pKey);
                    pChecked = false;
                }
                else 
                {
                    EnterpriseIntroduce.Add(pKey,pContent);
                    pChecked = true;
                }
                break;
            case PlayState.Publicity:
                if (PublicityBooks.ContainsKey(pKey))
                {
                    PublicityBooks.Remove(pKey);
                    pChecked = false;
                }
                else
                {
                    PublicityBooks.Add(pKey, pContent);
                    pChecked = true;
                }
                break;
            case PlayState.Show3D:
                if (Show3D.ContainsKey(pKey))
                {
                    Show3D.Remove(pKey);
                    pChecked = false;
                }
                else
                {
                    Show3D.Add(pKey, pContent);
                    pChecked = true;
                }
                break;
            default:
                break;
        }
    }
}

public delegate void PlayFunction(ChooseContent pChooseContentParam, Dictionary<int, string> pPlayMode);
public delegate void BackFunction(UserData user);

/// <summary>
/// 接口
/// </summary>
public interface IScreenShow
{
    IEnumerator LoginOn();
    IEnumerator Update();
    void StartPlay(PlayFunction pPlay);
}

public abstract class ScreenShow : IScreenShow 
{
    public UserData User;
    public ShowUserDate UserShowDate;
    public ChooseContent ChooseContent;
    public Dictionary<int, string> PlayMode;
    //  从服务器验证用户名和密码 并获取反馈 存储到 User 中
    public abstract IEnumerator LoginOn();
    public abstract IEnumerator Update();
    public abstract void StartPlay(PlayFunction pPlay);

}

public class BagScreenShow : ScreenShow 
{
    public string DefaultFilePath;

    public BagScreenShow(UserData pUser) 
    {
        User = pUser;
    }
    /// <summary>
    /// 从服务器更新数据到本地
    /// </summary>
    /// <param name="pUserData"></param>
    public override IEnumerator Update() 
    {
        //  把错误置空
        User.Error = null;

        yield return new WaitForSeconds(0);

        //  初始化用户选择展览容器类
        User.UserLoadedDatas = new UserChoosedData();

        //  初始化选择展览内容模块所需加载的资源列表
        UserShowDate = new ShowUserDate();

        //  调用回调，告知调用者已经加载完成
        User.UpdataFinishFunc(User);
    }
    /// <summary>
    /// 开始播放
    /// </summary>
    /// <param name="pPlay"></param>
    public override void StartPlay(PlayFunction pPlay)
    {
        pPlay(ChooseContent,PlayMode);
    }
    /// <summary>
    /// 登录
    /// </summary>
    /// <returns></returns>
    public override IEnumerator LoginOn()
    {
        yield return new WaitForSeconds(0);
        ///  服务器反馈一个加载数据的 Url
        User.DataUrl = "///";
        User.LogoSucceed = true;
        User.LogoFinishFunc(User);
    }

    /// <summary>
    /// 创建用户目录
    /// </summary>
    public void CreateFile()
    {
        
    }
}
