using UnityEngine;
using System.Collections;

public class LoadingAnima : MonoBehaviour {
    public float RotateSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        transform.Rotate(0,0,-RotateSpeed * Time.deltaTime);
	}
}
