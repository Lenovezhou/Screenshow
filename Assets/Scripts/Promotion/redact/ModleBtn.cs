using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum ModelType 
{
    Cover,
    Page,
    Catalogue,
    None
}

public class ModleBtn : MonoBehaviour {
    public ModelType modelType;
    public int ModelNum;
    public Toggle selfToggle;
    public PageControl pageControllSc;
    public CoverModelControll CoverModelSc;
    public virtual void Click() 
    {
        //Debug.Log("OnValueChange:isOn = " + selfToggle.isOn);
        if (selfToggle.isOn)
        {
            switch (modelType)
            {
                case ModelType.Cover:
                    UpService.UpInstance.UpdataCoverModel(ModelNum);
                    RedactControll.Instance.CurrCover.ModelNumber = ModelNum;
                    CoverModelSc.RefreshCoverModel(RedactControll.Instance.CurrCover, false);
                    break;
                case ModelType.Page:
                    UpService.UpInstance.UpdataPageModle(ModelNum);
                    RedactControll.Instance.CurrPage.ModelNumber = ModelNum;
                    pageControllSc.SetCurrentModel(RedactControll.Instance.CurrPage);
                    Debug.Log(RedactControll.Instance.CurrPage.Name + ":" + RedactControll.Instance.CurrPage.ModelNumber);
                    break;
                case ModelType.Catalogue:

                    break;
                case ModelType.None:
                    break;
                default:
                    break;
            }
            
            
        }
    }
}
