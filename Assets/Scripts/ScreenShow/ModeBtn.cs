using UnityEngine;
using System.Collections;

public class ModeBtn : MonoBehaviour {
    public int Index;

    public void Click() 
    {
        ChooseModeControll.Instance.CurrentIndex = Index;
    }
}
