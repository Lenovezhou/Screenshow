using UnityEngine;
using System.Collections;

public class Butterfly : MonoBehaviour {
    public Animation Anima;
    public float speed = 0.4f;

    private float _waitTime;
    private float _currTime;
    private bool _isFlying;
    private Vector3 _localPosition;
    private float _animaLen;
    private float _accumulateTime;
    private float _tmp;
	// Use this for initialization
	void Start () {
        _waitTime = Random.Range(5,10.0f);
        _isFlying = false;
        _localPosition = transform.localPosition;
        _animaLen = Anima["Fly"].length;
        _tmp = _animaLen / 2.0f;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Anima.isPlaying && !_isFlying)
        {
            _currTime += Time.deltaTime;
        }

        if (_currTime >= _waitTime && !_isFlying)
        {
            _currTime = 0;
            _waitTime = Random.Range(5, 10.0f);
            _isFlying = true;
            Anima["Fly"].speed = 0.5f;
            StartCoroutine(PlayAnimation(Random.Range(4,8)));
        }
	}

    public IEnumerator PlayAnimation(int n) 
    {
        int i = 0;
        while(i < n)
        {
            
            Anima.Play("Fly");
            while (_accumulateTime < _tmp)
            {
                yield return new WaitForSeconds(0.01f);
                transform.localPosition = new Vector3(transform.localPosition.x,transform.localPosition.y + 0.05f, transform.localPosition.z);
                _accumulateTime += 0.01f;
            }

            while (_accumulateTime >= _tmp && _accumulateTime < _animaLen)
            {
                yield return new WaitForSeconds(0.01f);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y - 0.03f, transform.localPosition.z);
                _accumulateTime += 0.01f;
            }
            
            _accumulateTime = 0;
            i++;
        }
        while (transform.localPosition.y - _localPosition.y > 0.0001f)
        {
            //Debug.Log("transform.localPosition.y = " + transform.localPosition.y + ";  _y = " + _y);
            transform.localPosition = new Vector3(transform.localPosition.x,
                Mathf.Lerp(transform.localPosition.y, _localPosition.y, 0.33f), transform.localPosition.z);
            yield return new WaitForSeconds(0);
        }
        transform.localPosition = new Vector3(transform.localPosition.x, _localPosition.y, transform.localPosition.z);
        _isFlying = false;
    }
}
