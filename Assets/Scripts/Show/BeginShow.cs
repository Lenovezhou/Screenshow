using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BeginShow : MonoBehaviour
{

    private Button myButton;
    void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(LoadTypeXML.GetInstance().ReadXml);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
