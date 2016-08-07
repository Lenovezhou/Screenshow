using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BookProfebSc : MonoBehaviour {
    public static int LoadedCount = 0;
    public int Index;   //  所属下表索引
    public Color SelectColor;   //  选中的颜色
    public Color DefaultColor;   //  默认的颜色
    public CoverModelControll coverModelControllSc;    //  封面模板管理脚本
    public BookCover selfCover;
    public Content CurrentModelSc;   //  当前使用的模板的脚本

    public string ID;  //  对应书的ID

    public RenderTexture renderTexture;
    public RawImage img;

    public string PicUrl;
    public Texture DefaultTexture;

    void OnEnable() 
    {
        if (!string.IsNullOrEmpty(PicUrl))
        {
            StartCoroutine(LoadPic());
        }
        else 
        {
            img.texture = DefaultTexture;
            LoadedCount++;
        }
    }

    public void Click() 
    {
        SetCurrnetColor();
        Bookrack.Instance.CurrentBookIndex = Index;
        coverModelControllSc.CurrentCover = selfCover;
        //img.texture = renderTexture.t
    }

    public void SetCurrnetColor()
    {
        for (int i = 0; i < Bookrack.Instance.AllBookImgs.Count; i++)
        {
            Bookrack.Instance.AllBookImgs[i].color = DefaultColor;
        }
        if (Index >= 0 && Index < Bookrack.Instance.AllBookImgs.Count)
        {
            Bookrack.Instance.AllBookImgs[Index].color = SelectColor;
        }
    }

    IEnumerator LoadPic()
    {
        WWW www = new WWW(GetXml.Instances.LoadUrl + PicUrl);
        yield return www;
        if (www.isDone && www.error == null)
        {
            img.texture = www.texture;
        }
        else 
        {
            Debug.Log("www is not done or err!");
        }
        LoadedCount++;
    }
}
