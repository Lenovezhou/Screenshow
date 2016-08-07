using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AnimationPlay : MonoBehaviour {

    //public Animation animation;
    public Toggle toggle;
    private Image BackToggle;
    public Animator animator;
    void Start()
    {
        toggle.onValueChanged.AddListener(ChangeState);
        BackToggle = toggle.transform.GetChild(0).GetComponent<Image>();
    }

    private void ChangeState(bool arg0)
    {
        #region MyRegion
        //if (arg0)
        //{
        //    animation["MovePlane"].time = 0;
        //    animation["MovePlane"].speed = 1f; 
        //    animation.CrossFade("MovePlane");
        //    BackToggle.color = new Color(1,1,1,0);
        //}
        //else
        //{
        //    animation["MovePlane"].time = animation["MovePlane"].length;
        //    animation["MovePlane"].speed = -1f;
        //    BackToggle.color = new Color(1, 1, 1, 1);
        //    animation.CrossFade("MovePlane");
        //}
        #endregion

        if (arg0)
        {
            animator.Play("Moveright");
            //animator.SetBool("MoveRight", true);
            BackToggle.color = new Color(1, 1, 1, 0);
            
        }
        else
        {
            animator.Play("MoveLeft");
            //animator.SetBool("MoveRight", false);
            BackToggle.color = new Color(1, 1, 1, 1);
            
        }
       
    }

    //IEnumerator PlayAnimation(string ani)
    //{
    //    animation["MovePlane"].time = 0;
    //    animation["MovePlane"].speed = 1f;
    //    animation.CrossFade("MovePlane");
    //    yield return animation;

    //}
}
