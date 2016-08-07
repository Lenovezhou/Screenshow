using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlowUpShow : MonoBehaviour {

    public Toggle myToggle;
    public GameObject BlowUp;
    private Button button;
    void Start()
    {
        myToggle.onValueChanged.AddListener(ChangeState);
        button = BlowUp.GetComponent<Button>();
        button.onClick.AddListener(ClickEvent);
    }

    private void ClickEvent()
    {
        Debug.Log("点击事件："+"uwuwuuwuwu");
    }

    private void ChangeState(bool arg0)
    {
        if (arg0)
        {
            BlowUp.SetActive(true);
        }
        else
        {
            BlowUp.SetActive(false);
        }
    }
}
