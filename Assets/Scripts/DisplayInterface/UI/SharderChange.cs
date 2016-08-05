using UnityEngine;
using System.Collections;

public class SharderChange : MonoBehaviour {
    
	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    public void OnOpaque()
    {
        SingleShow._instance.getTarget().GetComponent<Renderer>().material.shader = Shader.Find("KimoShaders/Diffuse");
    }
    public void OnTransparent()
    {

        SingleShow._instance.getTarget().GetComponent<Renderer>().material.shader = Shader.Find("KimoShaders/Transparent");
    }
    public void OnCutout()
    {

        SingleShow._instance.getTarget().GetComponent<Renderer>().material.shader = Shader.Find("KimoShaders/Cutout");
    }
}
