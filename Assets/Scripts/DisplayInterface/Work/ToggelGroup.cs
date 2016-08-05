using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToggelGroup : MonoBehaviour
{
    ExplicitImplicit[] groups;
    void Start()
    {
        groups=GetComponentsInChildren<ExplicitImplicit>();
    }

   public void SetActive()
    {
        foreach (var item in groups)
        {
            item.GetComponent<Toggle>().isOn = true;
        }
    }


   public void SetFalse()
    {
        foreach (var item in groups)
        {
            item.GetComponent<Toggle>().isOn = false;
        }
    }
}
