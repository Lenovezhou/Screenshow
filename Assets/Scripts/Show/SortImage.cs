using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SortImage : MonoBehaviour
{
	public Vector2 testvector2;
	public string childtext;
	public string path, id;
	public GameObject background;
	public GameObject _canvas;
	public GameObject grid, panel;
	public GameObject oldparent;
	public GridContorll gridcontroll;
	public int scenetype;
	Vector2 startpos;
	RectTransform rect;
	CanvasGroup canvas;

	void Start ()
	{
		rect = gameObject.GetComponent<RectTransform> ();
		canvas = gameObject.GetComponent<CanvasGroup> ();
		_canvas = GameObject.FindWithTag ("Finish");
		//grid = FindInChild (_canvas,"Grid");
		panel = FindInChild (_canvas, "Panel");
		background = FindInChild (_canvas, "Sortpanel");
		//gridcontroll = grid.GetComponent<GridContorll> ();
		Toggle toggle = this.GetComponent<Toggle> ();
        
	}

	public GameObject FindInChild (GameObject Go, string name)
	{
		foreach (RectTransform obj in Go.GetComponentsInChildren<RectTransform>()) {
			if (obj.name == name) {
				return obj.gameObject;
			}
		}
		return null;
	}
    public void Ontoggle() 
    {
        if (this.GetComponent<Toggle>().isOn)
        {
            Freshcolor(); //刷新所有按键颜色
            this.GetComponent<Image>().color = this.GetComponent<Image>().color == Color.grey ? Color.white : Color.gray;
        }
    }
	public void OnToggleDown ()
	{
      
	}

	public void OnDrag ()
	{		
		//	rect.position = data.position;
	}
	//刷新所有按键颜色
	public void Freshcolor ()
	{
        if (this.transform.parent.tag == "UU")
        {
            for (int j = 0; j < this.transform.parent.parent.childCount; j++)
            {
                this.GetComponent<Toggle>().isOn = false;
                if ( this.transform.parent.parent.GetChild(j).childCount!=0)
                {
                    this.transform.parent.parent.GetChild(j).GetChild(0).GetComponent<Image>().color = Color.white;
                }
                
            }
        }
        else
        {
            for (int i = 0; i < this.transform.parent.childCount; i++)
            {
                this.GetComponent<Toggle>().isOn = false;
                this.transform.parent.GetChild(i).GetComponent<Image>().color = Color.white;
            }
        }
	}
	public void Scroll(bool isScrool)
	{
		if (this.transform.parent.tag == "UU") {
			for (int i = 0; i < gridcontroll.tparents [1].transform.childCount; i++) {
				if (gridcontroll.tparents [1].transform.GetChild (i).childCount > 0) {
					gridcontroll.tparents [1].transform.GetChild (i).GetChild (0).GetComponent<CanvasGroup> ().blocksRaycasts = isScrool;
                
				}
			}

		} else {
			for (int j = 0; j < gridcontroll.tparents[0].transform.childCount; j++) {
				gridcontroll.tparents [0].transform.GetChild (j).GetComponent<CanvasGroup> ().blocksRaycasts = isScrool;
			}
		
		}
		canvas.blocksRaycasts = true;
	}
	void Update ()
	{
        Ontoggle();
	}
}
