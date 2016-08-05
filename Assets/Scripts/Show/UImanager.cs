using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UImanager : MonoBehaviour {
   
  
    public GameObject canvasclone;
    public GameObject showpanel,secondpanel, loadingpanel, editpanel,sortpanel,uploadpanel;
    private bool issecondpanel=false;
    private Animator secondpanelin;
    private static int oo= -1;
    private bool isrotate = false;
    private GameObject lastpanel;    //切换的panel名称
    private string lastname;         //切换的animator名称
    void Awake() 
    {
       // DontDestroyOnLoad(showpanel);
    }
	void Start () {
        ChangePanel(showpanel);
        showpanel = this.transform.GetChild(0).gameObject;
        secondpanelin = this.transform.GetChild(1).GetComponent<Animator>();  
	}

    // isrotate属性
    public bool Isrotate 
    {
        set { isrotate = value; }
        get { return isrotate; }
    }

    //资源更新button调用：控制登录界面的动画：
    public void SecondPanel(string name)
    {
        if (name == "in")
        {
            ChangePanel(secondpanel);

        }
       // SortScene(false);
        SelectAnim(name);
    }
   


    //编辑Button调用：
    public void EditpanelOpen() 
    {
        ChangePanel(editpanel);
    }

    //Editpanel里的排序button调用
    public void SortScene() 
    {
        ChangePanel(sortpanel);
    }


    // sortpanel 里的取消按钮
    public void Exitsortpanel()
    {
        ChangePanel(showpanel);
    }


    // 切换动画被调用的方法：
    public void SelectAnim(string name) 
    {
        
        if (name == lastname)
        {
            secondpanelin.SetBool(name, true);
        }
        else { 
        secondpanelin.SetBool(lastname,false);
        secondpanelin.SetBool(name, true);
        lastname = name;
        }
        //当动画状态为“out”时调用，显示showpanel
        if (name=="out")
        {
            ChangePanel(showpanel);
        }
    }
    //button打开上传资源panel
    public void UploadPanel() 
    {
        ChangePanel(uploadpanel);
    }
    // 切换Panel被调用的方法：
    public void ChangePanel(GameObject newpanel) 
    {
        if (lastpanel)
        {
            lastpanel.SetActive(false);
        }
        newpanel.SetActive(true);
        lastpanel = newpanel;
        //同时关掉sortpanel1，sortpanel2：
          //  SortScene(false);
       
    }
   
    // 测试level跳转:共3个场景
    public void ChangeLevel() 
    {
        Debug.Log("++++++++++++++++++" + oo);
        if (oo < 2)
        {
            Debug.Log("a < 3a < 3a < 3a < 3a < 3a < 3a < 3");
            oo =oo+ 1;
        }
        else
        {
            Debug.Log("a>=3a>=3a>=3a>=3a>=3a>=3a>=3a>=3");
            oo = 0;
        }
        Debug.Log(oo + "aaaaaaaaaaaaa");
        Application.LoadLevel(oo);

    }

    //button调用上下排序
    public void SortItems(string state) 
    {

        switch (state)
        {
            case "UP":


                break;
        }
    }

	void Update () {
        if (isrotate)
        {
            // loading 旋转
            this.transform.GetChild(2).transform.RotateAround(Vector3.back, 10f * Time.deltaTime);
        }
	}
}
