using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class companyscenemanager : MonoBehaviour {
    public float textSpeed;
    private float textValue;
    public Transform scrollbar;
    
	void Start () {
        //scrollbar = this.transform.Find("Scrollbar").transform;
        Debug.Log("++++++++++++++++++++" + scrollbar.name);
        textValue = scrollbar.transform.GetComponent<Scrollbar>().value;
        scrollbar.transform.GetComponent<Scrollbar>().size = 0.3f;
        
	}
    public void TextMove() 
    {

    }
	// Update is called once per frame
	void Update () {
        if (textValue >= 0.1f)
        {
            textValue -= Time.deltaTime * textSpeed;
        }
        else {
            textValue = 1f;
        }
        scrollbar.GetComponent<Scrollbar>().value = textValue;
	}
}
