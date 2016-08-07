using UnityEngine;
using System.Collections;

public enum CatalogueMsState 
{
    Enter,
    Exit,
    None
}

public class CatalogueAnimationControl : MonoBehaviour
{
    public Animation BtnAnima;
    public Animation CatalogueAnima;
    public float xMax;
  
    public CatalogueMsState msState;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(Input.mousePosition.x);
	}

    public void OnMouseEnter() 
    {
        msState = CatalogueMsState.Enter;
        BtnAnima.Play("CatalogueBtnHide");
        CatalogueAnima.Play("CatalogueDis");
        BookInfomation._isntance.isUI = true;
    }

    public void OnMouseExit() 
    {
        if (Input.mousePosition.x > xMax)
        {
            Debug.Log("Exit.");
            BtnAnima.Play("CatalogueBtnDis");
            CatalogueAnima.Play("CatalogueHide");
            BookInfomation._isntance.isUI = false;
        }
    }
}
