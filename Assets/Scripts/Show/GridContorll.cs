using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Sprites;
using System.IO;

public class GridContorll : MonoBehaviour
{
    public GameObject itemsprefab, Ucellprefab;
    public GameObject[] tparents;
    public List<GameObject> handsitems;
    GameObject item, lastparent;
    public int sceneindex;
    private bool isinstance = false;
    private GameObject tempgame;
    private int templink;
    private int tempa = 0;
    private int tempindex = 0;
    //  private bool isucell = false;
    void Start()
    {

    }
    public void ChangeLR(string state)
    {
        GameObject temp = null;
        int parentindex = 0;
        if (state == "right")//从右向左
        {
            parentindex = 0;
            for (int i = 0; i < tparents[parentindex].transform.childCount; i++)
            {
                if (tparents[parentindex].transform.GetChild(i).GetComponent<Image>().color == Color.gray)
                {
                    temp = tparents[parentindex].transform.GetChild(i).gameObject;
                }
            }
        }
        else
        {
            parentindex = 1;
            for (int i = 0; i < tparents[parentindex].transform.childCount; i++)
            {
                if (tparents[parentindex].transform.GetChild(i).childCount != 0)
                {
                    if (tparents[parentindex].transform.GetChild(i).GetChild(0).GetComponent<Image>().color == Color.gray)
                    {
                        temp = tparents[parentindex].transform.GetChild(i).GetChild(0).gameObject;
                    }
                }

            }
        }

        if (temp != null)
        {
            CloneInit(state, temp);
        }

    }
    public void TestInstanceButton()
    {
      
        Imagesort(Global.roomname, sceneindex);
    }
    public void DicMSG()
    {
        Global.Loadinformation(tparents[1]);
        string path =Application.dataPath + "/"+"000.xml";
        if (File.Exists(path))
        {
            File.Delete(path);      // 删除文件
           
        }  
        Global.CreatXML();
    }

    public void Imagesort(List<string> namelist, int sceneindex)
    {
        if (isinstance)
        {
            return;
        }
        for (int i = 0; i < namelist.Count; i++)
        {
            Debug.Log(namelist.Count+"namelist.Count");
            templink = i;
            GameObject temp = Instantiate(itemsprefab);
            // GameObject Ucell = Instantiate(Ucellprefab);
            InitePre(temp, namelist[i]);
            // InitePre(Ucell,namelist[i]);
        }
        isinstance = true;
    }
    public void InitePre(GameObject pre, string Name)
    {
        pre.transform.SetParent(tparents[0].transform);
        DestroyImmediate(pre.GetComponent<Button>());
        pre.transform.localPosition = Vector3.zero;
        pre.transform.rotation = Quaternion.identity;
        pre.transform.localScale = Vector3.one;
        if (pre.tag == "UU")    //如果是初始化Ucells，从这里结束；
        {
            pre.transform.SetParent(tparents[1].transform);
            return;
        }
        pre.transform.GetChild(0).GetComponent<Text>().text =(templink+1).ToString()+".     "+ Global.childtextsscene[templink] + "/" + Global.childtexts[templink];
       
        SortImage sortimage = pre.AddComponent<SortImage>();
        sortimage.gridcontroll = this;
        sortimage.id = Name;
        sortimage.scenetype = Global.typeidscene[templink];
    }
    public void Select(GameObject castedobj, GameObject movedobj, GameObject oldparent)
    {
        Debug.Log(castedobj.name);
        if (castedobj.tag == "Ucell")
        {
            movedobj.transform.SetParent(castedobj.transform.parent.transform);
            // isucell = true;
        }
        else if (castedobj.tag == "UU")
        {
            movedobj.transform.SetParent(castedobj.transform);
        }
        else if (castedobj.tag == "Viewport")
        {
            movedobj.transform.SetParent(castedobj.transform.GetChild(0).transform);
        }
        else
        {
            movedobj.transform.SetParent(oldparent.transform);
        }

        movedobj.transform.localPosition = Vector3.zero;
        movedobj.transform.rotation = Quaternion.identity;
        movedobj.transform.localScale = Vector3.one;
    }


    //  点击左右向按键时初始化克隆的物体
    public void CloneInit(string state, GameObject revert)
    {
        GameObject cloneself = null;
        bool isfind = true;
        tempindex = tparents[1].transform.childCount;
        tempindex += 1;
        if (state == "right")
        {
            cloneself = Instantiate(itemsprefab);//按下按键时克隆一个自己
            for (int i = 0; i < tparents[1].transform.childCount; i++)
            {
                // Debug.Log(tparents[1].transform.GetChild(i).childCount + "tparents[1].transform.GetChild(i).childCount");
                if (tparents[1].transform.GetChild(i).childCount == 0)
                {
                    cloneself.transform.SetParent(tparents[1].transform.GetChild(i));
                    isfind = true;
                    break;      //执行这行代码时，
                }
                else
                {
                    isfind = false;
                }


            }
			//先将for循环执行完成后再执行此if语句
            if (!isfind || tparents[1].transform.childCount == 0)
            {
                GameObject Ucell = Instantiate(Ucellprefab);
                InitePre(Ucell, "None");
                cloneself.transform.SetParent(Ucell.transform);
            }
			string[] typescene = revert.transform.GetChild (0).GetComponent<Text> ().text.Split ('.');
            //  cloneself.transform.SetParent(tparents[1].transform);
            cloneself.transform.GetChild(0).GetComponent<Text>().text =tempindex+"."+typescene[1];
            cloneself.transform.localPosition = Vector3.zero;
            cloneself.transform.localRotation = Quaternion.identity;
            cloneself.transform.localScale = Vector3.one;
            SortImage cloneimage = cloneself.AddComponent<SortImage>();
            cloneimage.gridcontroll = this;
            cloneimage.id = revert.GetComponent<SortImage>().id;
            cloneimage.scenetype = revert.GetComponent<SortImage>().scenetype;
        }
        else
        {
            Destroy(cloneself);
            //左侧不要删除完：
            if (revert.transform.parent.parent.childCount > 1)
            {
                DestroyImmediate(revert.transform.parent.gameObject);
            }
            //Debug.Log(revert.transform.parent + "revert.transform.parent");
            //handsitems.Remove(revert);
            RefreshIndex();          
        }
    }

    //  刷新parents[1]内的items的显示顺序：
    public void RefreshIndex() 
    {
        for (int j = 0; j < tparents[1].transform.childCount; j++)
        {
			if (tparents [1].transform.GetChild (j).GetComponentInChildren<Text> ()) {
				string[] typescene = tparents [1].transform.GetChild (j).GetComponentInChildren<Text> ().text.Split ('.');
				//Debug.Log (typescene[0]+"typescene.length");
				tparents [1].transform.GetChild (j).GetComponentInChildren<Text> ().text = (j + 1).ToString () +"."+ typescene [1];
			}
        }
    }

    //用于点击button时重新排序,遍历tparent[1]的子物体存入List<gameobject>handitems
    public void Handsort(string state)
    {
        // GameObject tempgame = new GameObject();
        int temp = 0;
        handsitems.Clear();

        //添加到  handsitems  非空的Ucell内的元素：
        for (int i = 0; i < tparents[1].transform.childCount; i++)
        {
            if (tparents[1].transform.GetChild(i).childCount > 0)
            {
                handsitems.Add(tparents[1].transform.GetChild(i).GetChild(0).gameObject);
                Image imagetemp = tparents[1].transform.GetChild(i).GetChild(0).GetComponent<Image>();
                //temp = i;
                //tempgame = imagetemp.gameObject;				
                //Debug.Log(handsitems.Count+"beforRemove");
                //handsitems.Remove(tempgame);
                //Debug.Log(handsitems.Count+"afterRemove");
            }

        }

        //  跳过空的Ucell找到image为灰色的index

        for (int j = 0; j < handsitems.Count; j++)
        {
            if (handsitems[j].GetComponent<Image>().color == Color.gray)
            {
                temp = j;
                tempgame = handsitems[j];
                handsitems.Remove(tempgame);
            }
        }

        if (state == "up")
        {
            if (temp == 0)
            {
                return;
            }
            handsitems.Insert(temp - 1, tempgame);
        }
        else
        {
            if (temp == handsitems.Count)
            {
                return;
            }
            handsitems.Insert(temp + 1, tempgame);
        }
        for (int k = 0; k < handsitems.Count; k++)
        {
            handsitems[k].transform.SetParent(tparents[1].transform.GetChild(k));
            handsitems[k].transform.localPosition = Vector3.zero;
            handsitems[k].transform.localScale = Vector3.one;
            handsitems[k].transform.localRotation = Quaternion.identity;
            //	Debug.Log (tparents [1].transform.GetChild (k).transform);
        }
        RefreshIndex();
        // DestroyImmediate(tempgame);
    }
    void LateUpdate()
    {

    }

    void Update()
    {

    }
}
