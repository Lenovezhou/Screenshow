using UnityEngine;
using System.Collections;

public class ChooseContentUI : MonoBehaviour {
    public GameObject Introduce;
    public GameObject Publicity;
    public GameObject Show3D;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 企业介绍按钮
    /// </summary>
    public void EnterpriseIntroduceBtn_Click() 
    {
        Publicity.SetActive(false);
        Show3D.SetActive(false);
        Introduce.SetActive(true);

        Manage.Instance.bagScreenShow.User.UserLoadedDatas.UserChoose = PlayState.Introduce;
    }
    /// <summary>
    /// 宣传册按钮
    /// </summary>
    public void PublicityBtn_Click()
    {
        Show3D.SetActive(false);
        Introduce.SetActive(false);
        Publicity.SetActive(true);
        
        Manage.Instance.bagScreenShow.User.UserLoadedDatas.UserChoose = PlayState.Publicity;
    }
    /// <summary>
    /// 3维展示按钮
    /// </summary>
    public void Show3DBtn_Click()
    {
        Introduce.SetActive(false);
        Publicity.SetActive(false);
        Show3D.SetActive(true);

        Manage.Instance.bagScreenShow.User.UserLoadedDatas.UserChoose = PlayState.Show3D;
    }
}
