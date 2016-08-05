using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class CoverContent : Content {
    public RawImage ConverPicture;
    public RawImage LogoPicture;

    public Material[] CoverMats;

    public override void Awake() 
    {
        codeIndex = 1;
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Items == null);
	}

}
