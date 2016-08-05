using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TabItem : MonoBehaviour {

    public int ID = -1;
    public string Content;

    public Image selfImage;
    public Color DefaultColor;
    public Color SelectColor;

    public bool Checked = false;

    public void Click()
    {
        if (ID != -1)
        {
            Manage.Instance.bagScreenShow.User.UserLoadedDatas.AddToCurrentData(ID, Content, ref Checked);
            if (Checked)
            {
                selfImage.color = DefaultColor;
            }
            else 
            {
                selfImage.color = SelectColor;
            }
        }
    }
}
