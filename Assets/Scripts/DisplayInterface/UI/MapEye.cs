using UnityEngine;
using System.Collections;

public class MapEye : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localEulerAngles = new Vector3(0, 0, -Camera.main.transform.parent.localEulerAngles.y);
	}
}
