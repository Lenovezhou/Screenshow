using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CopyText : MonoBehaviour {
    public Text text1;
    public Text TargetText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        TargetText.text = text1.text;
	}
}
