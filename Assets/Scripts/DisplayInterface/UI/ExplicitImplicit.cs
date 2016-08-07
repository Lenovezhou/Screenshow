using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ExplicitImplicit : MonoBehaviour
{
    private ProdKind ChooseTarget;
    //private bool IsActive=false;
    private RawImage SelfRawImage;
    private Color SelfColor;

    private Toggle toggle;
    void Start()
    {
        ChooseTarget = transform.parent.GetComponent<ShowPictureList>().ChangeTarget;
        SelfRawImage = this.GetComponent<RawImage>();
        SelfColor = SelfRawImage.color;
        toggle = GetComponent<Toggle>();
        toggle.isOn = true;
        toggle.onValueChanged.AddListener(OnPointerClick);
    }


    public void OnPointerClick(bool ag0)
    {
        MsgCenter._instance.ChangeTarget(ChooseTarget);

        if (!ag0)
        {
            SelfRawImage.color = new Color(SelfColor.r, SelfColor.g, SelfColor.b, 0.5f);
            SendMessage(ag0);
            //IsActive = true;
        }
        else
        {
            SelfRawImage.color = SelfColor;
            SendMessage(ag0);
            //IsActive = false;
        }
    }

    void SendMessage(bool active)
    {
        if (ChooseTarget != ProdKind.ChuangLian)
        {
            if (MsgCenter._instance._changeTexture != null)
            {
                MsgCenter._instance._changeTexture.ReceiveMessage(active);
            }
        }
        else
        {
            if (active)
            {
                transform.parent.parent.GetComponent<ToggelGroup>().SetActive();
            }
            else
            {
                transform.parent.parent.GetComponent<ToggelGroup>().SetFalse();
            }
            //foreach (GameObject temp in MsgCenter._instance.GameManageList.Values)
            //{
            //    temp.SetActive(active);
            //}
        }
    }
}
