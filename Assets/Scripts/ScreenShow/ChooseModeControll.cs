using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChooseModeControll : MonoBehaviour {
    public Text[] Texts;
    public int CurrentIndex = -1;

    public static ChooseModeControll Instance;
    void Awake() 
    {
        Instance = this;
    }

	// Use this for initialization
	void OnEnable ()
    {
        Dictionary<int, PlayState>.Enumerator em = Manage.Instance.bagScreenShow.User.PlayMode.GetEnumerator();
        while (em.MoveNext())
        {
            switch (em.Current.Value)
            {
                case PlayState.Introduce:
                    Texts[em.Current.Key].text = "企业介绍";
                    break;
                case PlayState.Publicity:
                    Texts[em.Current.Key].text = "宣传册";
                    break;
                case PlayState.Show3D:
                    Texts[em.Current.Key].text = "三维展示";
                    break;
                case PlayState.Moive:
                    Texts[em.Current.Key].text = "视屏";
                    break;
            }
        }
	}

    public void UpChanged() 
    {
        if(CurrentIndex != -1)
        {
            if (CurrentIndex == 0)
            {
                Manage.Instance.bagScreenShow.User.ExchangePlayModel(CurrentIndex, Texts.Length - 1);
            }
            else
            {
                Manage.Instance.bagScreenShow.User.ExchangePlayModel(CurrentIndex, CurrentIndex - 1);
            }
            OnEnable();
        }

    }

    public void DownChanged()
    {
        if (CurrentIndex != -1)
        {
            if (CurrentIndex == Texts.Length - 1)
            {
                Manage.Instance.bagScreenShow.User.ExchangePlayModel(CurrentIndex,0);
            }
            else 
            {
                Manage.Instance.bagScreenShow.User.ExchangePlayModel(CurrentIndex, CurrentIndex + 1);
            }
            //Debug.Log(CurrentIndex);
            OnEnable();
        }
    }
}
