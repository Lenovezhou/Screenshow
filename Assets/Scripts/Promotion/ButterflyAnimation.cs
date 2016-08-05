using UnityEngine;
using System.Collections;

public class ButterflyAnimation : MonoBehaviour {
    public Animation anima;
    public float time;

    private float _time;
    private bool _isOder;
	// Use this for initialization
	void Start () {
        _isOder = false;
	}
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;
        if(_time >= time)
        {
            _time = 0;
            if (_isOder)
            {
                anima["Fly"].speed = -1;
                anima.CrossFade("Fly");
                _isOder = false;
            }
            else 
            {
                anima["Fly"].speed = 1;
                anima.CrossFade("Fly");
                _isOder = true;
            }
        }
	}
}
