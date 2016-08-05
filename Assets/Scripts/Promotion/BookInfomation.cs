using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BookInfomation : MonoBehaviour
{
    public struct JumpPageParams 
    {
        public int TargetPage;   //  目标页
        public float AnimaSpeed;   //  动画播放速度
        public float WaitTiem;   //  每页播放完毕后等待的时间
        public bool isLeft;   //  是否向左翻页
        public bool RefreshBtnFlg;  //  是否刷新按钮显示
        public bool RefreshData;   //  是否刷新数据
        public JumpPageParams(int pTargetPage, float pAnimaSpeed, float pWaitTiem) 
        {
            TargetPage = pTargetPage;
            AnimaSpeed = pAnimaSpeed;
            WaitTiem = pWaitTiem;
            isLeft =false;
            RefreshBtnFlg = false;
            RefreshBtnFlg = true;
            RefreshData = true;
        }

        public JumpPageParams(int pTargetPage, float pAnimaSpeed, float pWaitTiem, bool pIsLeft)
        {
            TargetPage = pTargetPage;
            AnimaSpeed = pAnimaSpeed;
            WaitTiem = pWaitTiem;
            isLeft = pIsLeft;
            RefreshBtnFlg = false;
            RefreshData = true;
        }

        public JumpPageParams(bool pRefreshBtnFlg, float pAnimaSpeed,bool pResfreshData)
        {
            TargetPage = 0;
            AnimaSpeed = pAnimaSpeed;
            WaitTiem = 0;
            isLeft = false;
            RefreshBtnFlg = pRefreshBtnFlg;
            RefreshData = pResfreshData;
        }

        public JumpPageParams(bool pRefreshBtnFlg, float pAnimaSpeed)
        {
            TargetPage = 0;
            AnimaSpeed = pAnimaSpeed;
            WaitTiem = 0;
            isLeft = false;
            RefreshBtnFlg = pRefreshBtnFlg;
            RefreshData = true;
        }
    }
    public static BookInfomation _isntance;
    public bool LastIsLeft;   //  上一次翻页是否朝左方
    public float MouseSpiritLightness;   //  鼠标翻页的灵明度
    public GameObject CurrentSeletedBook;
    public GameObject BookCover;
    public GameObject BookPageA;
    public GameObject BookPageB;
    public GameObject BookPageC;
    public GameObject CurrentRender;
    public GameObject LastRender;
    public GameObject NextCurrentRender;
    public GameObject NextRender;
    public GameObject PagePrefabs;   //  翻书的预设

    public AudioSource AudioSce;
    public AudioClip LeftClip;
    public AudioClip RightClip;
    public Coroutine PlayAudioCor;

    public int CurrentPageNum;  //  当前的页码
    public InputField CurrentPageBox;  //  当前页码显示
    public Text TestText;   // 测试的Text

    public PageNControl PageNControl_A;
    public PageNControl PageNControl_B;
    public PageNControl PageNControl_C;
    public PageNControl PageNControl_D;

    public ModelBtnControll AllModelBtnControllSc;  //  所有按钮的管理脚本
    public CatalogueBtnContent CatalogueBtnContentSc;  //  右侧目录UI的按钮容器脚本

    public float lerpSetUp;

    public GameObject OpenShadw;  //  打开时的阴影
    public GameObject CloseShadw;

    public GameObject BookUI;
    public CatalogueAllBtnCtr CatalogueAllBtnCtrSc;  //  目录的模板管理脚本

    public Page LastSeletedPage;
    public Page CurrentSeletedPage;
    public Page NextSeletedPage;
    public Page NextCurrentPage;
    public BookCover CurrentSeletedCover;
    public bool isUI;

    private bool _lastIsLeft;
    private bool _lastIsLeftJump;
    private bool _lastIsRightJump;
    private bool _isFirstPage = false;
    private bool _isPlayingJumpAnima = false;
    private Promotion _promotion;
    public Promotion promotion
    {
        set 
        {
            _promotion = value;
            for (int i = 0; i < _promotion.PageList.Count &&
                _promotion.PageList[i] is CataloguePage; i++)
            {
                CatalogueLen = i + 1;
            }
            _promotion.CatalogueLen = CatalogueLen;
            CurrentSeletedCover = value.SelfCover;
            LastSeletedPage = null;

            _setUpLeft = (leftMax - leftMin) / promotion.PageList.Count;
            _setUpRight = (rightMax - rightMin) / promotion.PageList.Count;
            _isValuetion = true;
            //Debug.Log("setLeftUp" + _setUpLeft);
            //Debug.Log("set: " + promotion.PageList.Count);
        }
        get
        {
            return _promotion;
        }
    }
    
    public bool AutoPlay;    //  自动播放
    public bool AutoFlg = false;    //  是否自动播放
    public float InvTiem;   //  自动翻页的间隔时间

    // 书侧面动态变化数据
    public Transform LeftRifler;
    public Transform RightRifler;
    public float leftMax;
    public float leftMin;
    public float rightMax;
    public float rightMin;
    public double _setUpLeft;
    public double _setUpRight;
    private Vector3 _tmpLeftVec;
    private Vector3 _tmpRightVec;
    private bool _isValuetion;

    private float _accumulateTiem;   //  积累时间

    public Animation CatalogueBtnAnima;
    //  书是否可翻
    private bool _canTurn;  
    public bool CanTurn
    {
        set 
        {
            _canTurn = value;
            BookUI.SetActive(value);
        }
        get 
        {
            return _canTurn;
        }
    }
    public int CatalogueLen;  //  目录的页数

    private Vector2 TempPos;
    private Vector2 EndPos;
    private bool HasTake;
    public int index;
    private Texture BackTexture;
    private Texture FrontTexture;
    public bool CanPage;
    public bool CanCover;
    private bool CanReverse;
    private bool isPlaying;
    private GameObject PageA_B;
    private bool _leftFlg;
    private bool _rightFlg;
    private int _leftPage;
    private int _rightPage;
    private bool _isPlaying;  // 翻页动画是否在播放
    private bool _isTurn;  //  正在响应播放事件
    private CatalogueGroupControl _tempLeftGroupSc;
    private CatalogueGroupControl _tempRightGroupSc;
    private List<Animation> anims;
    public Animation _bAnima;
    private float leftLen, rightLen;
    
    private bool _refreshPageFlg;  //  刷新页数显示的标志
    public JumpPageParams turnParams;

    void Awake() 
    {
        _isValuetion = false;
        breakFlg = false;
        canPlay = true;
        StartCoroutine(startPlayAudio());
    }

    void Start()
    {
        index = 0;
        _lastIsLeft = false;
        _isntance = this;
        //promotion = new Promotion();
        isPlaying = false;
        PageA_B = BookPageA.transform.GetChild(1).gameObject;
        _leftFlg = false;
        _rightFlg = false;
        anims = new List<Animation>();
        //Debug.Log(PagePrefabs.GetComponent<Animation>()["left"] == null);
        leftLen = PagePrefabs.GetComponent<Animation>()["left"].length;
        rightLen = PagePrefabs.GetComponent<Animation>()["right"].length;
        _bAnima = BookPageB.GetComponent<Animation>();
        _refreshPageFlg = false;
        LastIsLeft = false;
        CanTurn = false;
        isUI = false;
        _isTurn = false;
        turnParams = new JumpPageParams(true,1.0f,true);
        
        _tmpLeftVec = LeftRifler.localPosition;
        //Debug.Log(LeftRifler.localPosition);
        _tmpRightVec = RightRifler.localPosition;
    }

    public void MoveBook() 
    {
        if (_leftFlg && !_rightFlg)
        {
            CurrentSeletedBook.transform.position = Vector3.Lerp(CurrentSeletedBook.transform.position, new Vector3(0.1f, CurrentSeletedBook.transform.position.y, CurrentSeletedBook.transform.position.z), lerpSetUp);
            //Debug.Log("left_ x : " + CurrentSeletedBook.transform.position.x);
            if (CurrentSeletedBook.transform.position.x >= 0.1f)
            {
                CurrentSeletedBook.transform.position = new Vector3(0.1f, CurrentSeletedBook.transform.position.y, CurrentSeletedBook.transform.position.z);
                _leftFlg = false;
                _isPlaying = false;
                Debug.Log("_leftFlg");
            }
        }
        if (_rightFlg && !_leftFlg)
        {
            CurrentSeletedBook.transform.position = Vector3.Lerp(CurrentSeletedBook.transform.position, new Vector3(-0.00001f, CurrentSeletedBook.transform.position.y, CurrentSeletedBook.transform.position.z), lerpSetUp);
            //Debug.Log("right_ x : " + CurrentSeletedBook.transform.position.x);
            if (CurrentSeletedBook.transform.position.x <= 0.0001f)
            {
                CurrentSeletedBook.transform.position = new Vector3(0.0f, CurrentSeletedBook.transform.position.y, CurrentSeletedBook.transform.position.z);
                _rightFlg = false;
                _isPlaying = false;
                //Debug.Log("_rightFlg");
            }
        }
    }

    void FixedUpdate()
    {
        if (_isValuetion)
        {
            if (index <= 2)
            {
                LeftRifler.gameObject.SetActive(false);
            }
            else if(index > 2)
            {
                _tmpLeftVec.x = (float)(leftMax -  index * _setUpLeft);
                _tmpLeftVec.x = Mathf.Max(_tmpLeftVec.x,leftMin);
                LeftRifler.localPosition = _tmpLeftVec;
                LeftRifler.gameObject.SetActive(true);
            } 

            if (index == promotion.PageList.Count - 1)
            {
                RightRifler.gameObject.SetActive(false);
            }
            else
            {
                _tmpRightVec.x = (float)(rightMax - index * _setUpRight);
                Debug.Log((float)(rightMax - index * _setUpRight));
                _tmpRightVec.x = Mathf.Max(_tmpRightVec.x,rightMin);
                RightRifler.localPosition = _tmpRightVec;
                RightRifler.gameObject.SetActive(true);
            }
        }
        
        MoveBook();
        //Debug.Log(promotion == null ? "null" : promotion.PageList == null ? "promotion.PageList == null" : "promotion.PageList != null");
        if(CanTurn)
        {
            if(AutoFlg)
            {
                if (CurrentSeletedBook.activeSelf && !AutoPlay)
                {
                    AutoPlay = true;
                    _accumulateTiem = InvTiem / 5.0f;
                }

                if(AutoPlay)
                {
                    _accumulateTiem += Time.deltaTime;
                    if (_accumulateTiem >= InvTiem)
                    {
                        if (!IsPlaying())
                        {
                            TurnLeft(turnParams);
                            LastIsLeft = true;
                        }
                    
                    }
                }
            }
            
            //Debug.Log("CatalogueLen:" + CatalogueLen);
            if (CatalogueLen != 0 && _refreshPageFlg && !IsPlaying())
            {
                int n = index - CatalogueLen + 1;
                if (n >= promotion.PageList.Count - CatalogueLen && promotion.PageList.Count % 2 == 0)
                {
                    n--;
                }
                else if (n <= 0)
                {
                    n = index + 1;
                }
                try
                {
                    if (Mathf.Abs(int.Parse(CurrentPageBox.text) - n) > 1)
                    {
                        CurrentPageBox.text = n.ToString();
                        CatalogueBtnContentSc.SetTargetBtnShine(n - 1);
                    }
                }
                catch (System.Exception exc)
                {
                    CurrentPageBox.text = n.ToString();
                    Debug.Log(exc.Message);
                }

                _refreshPageFlg = false;
            }
            //Debug.Log("left_ :" + _leftFlg + "; _right" + _rightFlg);
            
            if (_rightFlg && _leftFlg)
            {
                _rightFlg = false;
                _leftFlg = false;
            }
            //Debug.Log(isPlaying);
            if (BookPageA.activeSelf && index == 0)
            {
                BookPageA.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                TempPos = Input.mousePosition;
                HasTake = true;
            }
            //Debug.Log("_isTurn[update] = " + _isTurn);
            if (HasTake && !_bAnima.isPlaying && !isPlaying && !isUI)
            {
                EndPos = Input.mousePosition;
                float Distance = EndPos.x - TempPos.x;
                if (Distance > MouseSpiritLightness)
                {
                    HasTake = false;
                    Debug.Log("向右滑动");
                    LastIsLeft = false;
                    _isTurn = true;
                    TurnRight(turnParams);
                }
                else if (Distance < -MouseSpiritLightness)
                {
                    HasTake = false;
                    Debug.Log("向左滑动");
                    LastIsLeft = true;
                    _isTurn = true;
                    TurnLeft(turnParams);
                }
                
            }
            if (Input.GetMouseButtonUp(0))
            {
                //Debug.Log("IsPointerOverGameObject() : " + EventSystem.current.IsPointerOverGameObject());
                HasTake = false;
            }
        }
    }

    public bool IsPlaying() 
    {
        //Debug.Log("_isPlaying:" + _isPlaying + "; isPlaying:" + isPlaying + "; _bAnima.isPlaying:" + _bAnima.isPlaying + " ; _isTurn:" + _isTurn);
        return _isPlaying || isPlaying || _bAnima.isPlaying || _isTurn;
    }
    /// <summary>
    /// 跳转到指定的页
    /// </summary>
    /// <param name="pTargetPage"></param>
    public void JumpPageTab(int pTargetPage) 
    {
        //Debug.Log("JumpPage(isPlaying): " + isPlaying);
        if (!isPlaying && pTargetPage < promotion.PageList.Count && pTargetPage >= 0)
        {
            int target = pTargetPage + CatalogueLen;
            if (index < CatalogueLen)
            {
                target--;
            }
            Debug.Log("CatalogueLen : " + CatalogueLen);
            //target = target % 2 == 0 ? target - 1 : target;
            JumpPageParams jpp = new JumpPageParams(target, 1.0f, 0.15f - Mathf.Abs(target - index) * 0.002f);
            isPlaying = true;
            //_refreshPageFlg = true;
            PageNControl_A.HideAllHeightShine();
            StartCoroutine(JumpPage(jpp));
        }
    }
    /// <summary>
    /// 播放翻页动画
    /// </summary>
    /// <param name="jumpParams"></param>
    /// <returns></returns>
    public IEnumerator JumpPage(JumpPageParams jumpParams)
    {
        AllModelBtnControllSc.HideAllBtn();

        if (jumpParams.TargetPage > index)
        {
            if (jumpParams.TargetPage > index)
            {
                float n;
                if (jumpParams.TargetPage % 2 == 0)
                {
                    n = (jumpParams.TargetPage - index) / 2.0f;
                    Debug.Log("n1 = " + n + "; TargetPage = " + jumpParams.TargetPage + " ; index = " + index);
                }
                else
                {
                    n = (jumpParams.TargetPage - index + 1) / 2.0f - 1;
                    Debug.Log("n2 = " + n + "; TargetPage = " + jumpParams.TargetPage + " ; index = " + index);
                }

                RefreshAnimasData((int)n);

                bool once = false;
                if (n > 0)
                {
                    _isPlaying = true;
                    StartCoroutine(TrunAnimation(new JumpPageParams((int)n, jumpParams.AnimaSpeed, jumpParams.WaitTiem, true)));
                    yield return new WaitForSeconds(anims.Count * leftLen / n / jumpParams.AnimaSpeed);
                }
                else if (n <= 0)
                {
                    _isPlaying = true;
                   // Debug.Log("CurrentSeletedPage1 = " + CurrentSeletedPage.PageTab + " ; index1 = " + index);
                    //Time.timeScale = 0.01f;
                    TurnLeft(new JumpPageParams(true, jumpParams.AnimaSpeed, true));
                    //Debug.Log("CurrentSeletedPage2 = " + CurrentSeletedPage.PageTab + " ; index2 = " + index);
                    once = true;
                }

                while ((_bAnima.isPlaying || _isPlaying || Content.IsLoading || Content.isLoaded) 
                    && ! once)
                {
                    //AllModelBtnControllSc.HideLeftBtn();
                    yield return new WaitForSeconds(0);
                }

                //RefreshToTargetPage(jumpParams.TargetPage);
                Debug.Log("CurrentSeletedPage = " + CurrentSeletedPage.PageTab + " ; index = " + index );

                RefreshCurrentCatalogueBtn();
                AllModelBtnControllSc.Initialize(NextSeletedPage);
                PlayAudio(false);
                _lastIsLeftJump = true;
            }
        }
        else if (jumpParams.TargetPage < index - 1)
        {
            if (jumpParams.TargetPage != index - 1)
            {
                float n;
                if (jumpParams.TargetPage % 2 == 0)
                {
                    //ChangeCurrentRender(promotion.PageList[jumpParams.TargetPage + CatalogueLen]);
                    n = (index - jumpParams.TargetPage) / 2.0f - 1;
                    Debug.Log("n1 = " + n + "; TargetPage = " + jumpParams.TargetPage + " ; index = " + index);
                }
                else
                {
                    n = (index - jumpParams.TargetPage - 1) / 2.0f - 1;
                    Debug.Log("n2 = " + n + "; TargetPage = " + jumpParams.TargetPage + " ; index = " + index);
                }

                RefreshAnimasData((int)n);
                
                bool once = false;
                if (n > 0)
                {
                    _isPlaying = true;
                    StartCoroutine(TrunAnimation(new JumpPageParams((int)n, jumpParams.AnimaSpeed, jumpParams.WaitTiem, false)));
                    yield return new WaitForSeconds(anims.Count * rightLen / n / jumpParams.AnimaSpeed);
                }
                else if (n <= 0)
                {
                    _isPlaying = true;
                    Debug.Log("TurnRight : anims.Count == 1");
                    TurnRight(new JumpPageParams(true, jumpParams.AnimaSpeed, true));
                    once = true;
                }

                while ((_bAnima.isPlaying || _isPlaying || Content.IsLoading || Content.isLoaded) && !once )
                {
                    yield return new WaitForSeconds(0);
                }

                Debug.Log("CurrentSeletedPage = " + CurrentSeletedPage.PageTab + " ; index = " + index);

                //ChangeLastRender(LastSeletedPage);
                //ChangeNextRender(NextSeletedPage);
                //ChangeNextCurrentRender(NextCurrentPage);
                //ChangeCurrentRender(CurrentSeletedPage);

                RefreshCurrentCatalogueBtn();
                AllModelBtnControllSc.Initialize(CurrentSeletedPage);
                PlayAudio(false);
            }
        }
        //while (_isPlaying || _isTurn)
        //{
        //    yield return new WaitForSeconds(0);
        //}
        ChangeLastRender(LastSeletedPage);
        ChangeCurrentRender(CurrentSeletedPage);
        isPlaying = false;
        AllModelBtnControllSc.HideLeftBtn();
        AllModelBtnControllSc.Initialize(LastSeletedPage);
        LastIsLeft = false;
        _lastIsRightJump = true;
        Resources.UnloadUnusedAssets();
        //Debug.Log("idnex:" + index + "; " + jumpParams.TargetPage);
        //Debug.Log("lastIndex:" + lastIndex + "; currentIndex:" + currentIndex);
    }

    public void RefreshToTargetPage(int targetPage) 
    {
        if(targetPage < promotion.PageList.Count && targetPage >= CatalogueLen)
        {
            if (targetPage % 2 != 0)
            {
                targetPage += 1;
            }

            if (targetPage - 1 >= 0)
            {
                LastSeletedPage = promotion.PageList[targetPage - 1];
            }
            else 
            {
                LastSeletedPage = null;
            }
            if (targetPage + 1 < promotion.PageList.Count)
            {
                NextSeletedPage = promotion.PageList[targetPage + 1];
            }
            else
            {
                NextSeletedPage = null;
            }

            if (targetPage + 2 < promotion.PageList.Count)
            {
                NextCurrentPage = promotion.PageList[targetPage + 2];
            }
            else
            {
                NextCurrentPage = null;
            }

            CurrentSeletedPage = promotion.PageList[targetPage];

            if (targetPage < promotion.PageList.Count && targetPage >= CatalogueLen)
            {
                index = targetPage;
            }
            else if (targetPage < promotion.PageList.Count)
            {
                index = targetPage;
            }
        }
    }

    public void RefreshAnimasData(int pPages)
    {
        anims.Clear();
        GameObject obj;
        for (int i = 0; i < pPages
            && i < 10
            ;i++)
        {
            obj = Instantiate(PagePrefabs, PagePrefabs.transform.position, PagePrefabs.transform.rotation) as GameObject;
            anims.Add(obj.GetComponent<Animation>());
        }
    }
    /// <summary>
    /// 释放书页
    /// </summary>
    /// <returns></returns>
    public IEnumerator DestroyPages() 
    {
        int i = 0;
        while(i < anims.Count)
        {
            if (anims[i].isPlaying)
            {
                yield return new WaitForSeconds(0);
            }
            else 
            {
                Destroy(anims[i].gameObject);
                i++;
            }
        }
        _isPlayingJumpAnima = false;
    }

    public IEnumerator TrunAnimation(JumpPageParams pageParams) 
    {
        int x = -1;
        while (anims.Count > 0)
        {
            x++;
            
            if (x < anims.Count)
            {
                anims[x].gameObject.SetActive(true);
                if (pageParams.isLeft)
                {
                    RefreshToLeftData();
                    ChangeNextRender(CurrentSeletedPage);
                    ChangeCurrentRender(LastSeletedPage);
                    anims[x]["left"].speed = pageParams.AnimaSpeed;
                    //Debug.Log("speed = " + anims[x]["left"].speed);
                    anims[x].CrossFade("left");
                    ChangeNextCurrentRender(NextSeletedPage);
                    yield return new WaitForSeconds(pageParams.WaitTiem);
                }
                else
                {
                    RefreshToRightData();
                    ChangeCurrentRender(NextCurrentPage);
                    anims[x]["right"].speed = pageParams.AnimaSpeed;
                    Debug.Log("speed = " + anims[x]["left"].speed);
                    anims[x].CrossFade("right");
                    ChangeLastRender(NextSeletedPage);
                    //Debug.Log("_index:" + index);
                    yield return new WaitForSeconds(pageParams.WaitTiem);
                    //yield return new WaitForSeconds(5);
                }
                if (x == 0)
                {
                    StartCoroutine(DestroyPages());
                    _isPlayingJumpAnima = true;
                }
            }
            else
            {
                break;
            }
        }

        //Debug.Log("pageParams.TargetPage = " + pageParams.TargetPage);
        if (pageParams.isLeft)
        {
            if (pageParams.TargetPage > anims.Count - 1)
            {
                //Debug.Log("left:" + (pageParams.TargetPage - anims.Count));
                for (int i = 0; i < pageParams.TargetPage - anims.Count; i++)
                {
                    RefreshToLeftData();
                }
            }
            Debug.Log("AnimaSpeed = " + pageParams.AnimaSpeed);
            
            TurnLeft(new JumpPageParams(true, pageParams.AnimaSpeed * 2.0f, true));
        }
        else
        {
            JumpPageParams param = new JumpPageParams(true, pageParams.AnimaSpeed, true);
            if (pageParams.TargetPage > anims.Count - 1)
            {
                //Debug.Log("pageParams.TargetPage:" + pageParams.TargetPage + " ; anims.Count:" + anims.Count);
                for (int i = 0; i < pageParams.TargetPage - anims.Count; i++)
                {
                    RefreshToRightData();
                }
            }
            
            TurnRight(param);
        }
        //Debug.Log("index0:" + index);
        _isPlaying = false;
    }

    /// <summary>
    /// 获取指定目录模板的按钮管理脚本
    /// </summary>
    /// <param name="pModelNum"></param>
    /// <returns></returns>
    public CatalogueGroupControl GetCatalogueControl(int pModelNum) 
    {
        CatalogueGroupControl resSc = null;
        if (pModelNum < CatalogueAllBtnCtrSc.CataGroupControlScs.Length && pModelNum >= 0)
        {
            resSc = CatalogueAllBtnCtrSc.CataGroupControlScs[pModelNum];
        }
        return resSc;
    }

    public void SeletedCurrentBook(GameObject go)
    {
        CurrentSeletedBook = go;
        //CurrentSeletedCover = promotion.B_bookCover;
        //CurrentSeletedCover = promotion.SelfCover;
        
    }


    public void TurnLeft(JumpPageParams param)
    {
        //Debug.Log("TurnLeft");
        _accumulateTiem = 0;
        #region 播放翻封面动画
        if (CurrentSeletedCover != null && CanCover == false)
        {
            //播放翻封面动画
            BookCover.GetComponent<Animation>()["Take"].speed = param.AnimaSpeed;
            BookCover.GetComponent<Animation>().Play("Take");
            if(_leftFlg)
            {
                _leftFlg = false;
            }
            _rightFlg = true;
            CloseShadw.SetActive(false);
            StartCoroutine(CoverOpen());
            FirstChoose();

            ChangeRender();
            CatalogueBtnAnima.Play("CatalogueBtnDis");
            //Debug.Log("CurrentSeletedPage:" + CurrentSeletedPage.PageTab);
            CurrentSeletedCover = null;
            CanCover = true;
            CameraAimation.Instance.OpenBook();
            AllModelBtnControllSc.Initialize(CurrentSeletedPage);
            _isFirstPage = true;
        }
        #endregion

        #region 播放B动画
        else
        {
            if(param.RefreshData)
            {
                RefreshToLeftData();
            }
    
            //Debug.Log("CurrentSeletedPage[left] = " + CurrentSeletedPage.PageTab + " ; index = " + index);
            if (CanPage)
            {
               // Time.timeScale = 0.1f;
                Debug.Log("播放B动画");
                turnParams.AnimaSpeed = param.AnimaSpeed;
                if (_lastIsLeft && !_isFirstPage)
                {
                    if (CurrentSeletedPage != null)
                    {
                        if (CurrentSeletedPage is CataloguePage)
                        {
                            Catalogue.isLoaded = true;

                            CallBack.turnPlay = TurnLeftPlay;
                            CallBack.CurrentState = PageState.Current;
                        }
                        else
                        {
                            Content.isLoaded = true;

                            CallBack.turnPlay = TurnLeftPlay;
                            CallBack.CurrentState = PageState.Current;
                        }
                        
                        ChangeCurrentRender(CurrentSeletedPage);
                    }
                    else 
                    {
                        TurnLeftPlay(turnParams);
                    }
                }
                else 
                {
                    TurnLeftPlay(param);
                }
                if (_isFirstPage)
                {
                    _isFirstPage = false;
                }
            }
            else
            {
                //Debug.Log("不可翻:" + index + ";  PageList.Count:" + promotion.PageList.Count);
                if (AutoPlay)
                {
                    if ((promotion.PageList.Count % 2 == 0 && index >= promotion.PageList.Count) ||
                        (promotion.PageList.Count % 2 != 0 && index >= promotion.PageList.Count - 1))
                    {
                        Debug.Log("PageList.Count: " + promotion.PageList.Count + ";  index: " + index);
                        OperationBook.Instance.ReturnBookrack();
                        AutoPlay = false;
                    }
                }
                else 
                {
                    _isPlaying = false;
                    isPlaying = false;
                    _isTurn = false;
                }
            }
        }
        #endregion
    }
    /// <summary>
    /// 播放向左翻页动画
    /// </summary>
    /// <param name="param"></param>
    public void TurnLeftPlay(JumpPageParams param)
    {
        AllModelBtnControllSc.HideAllBtn();
        PageNControl_A.HideAllHeightShine();
        ClearCatalogueBtn();
        StartCoroutine(BeginToPageLeft(param));
        _bAnima["left"].speed = param.AnimaSpeed;
        _bAnima.CrossFade("left");
    }

    /// <summary>
    /// 刷新向左侧翻页数据
    /// </summary>
    public void RefreshToLeftData() 
    {
        if (promotion.PageList.Count == 0)
        {
            return;
        }
        if (index >= promotion.PageList.Count)
        {
            CurrentSeletedPage = null;
            CanPage = false;
        }
        else
        {
            CurrentSeletedPage = promotion.PageList[index];
            if (index == 0)
            {
                LastSeletedPage = null;
                CurrentSeletedPage = promotion.PageList[0];
            }
            else
            {
                LastSeletedPage = promotion.PageList[index - 1];
            }
            //Debug.Log("CurrentSeletedPage = " + CurrentSeletedPage.PageTab);
            if (promotion.PageList.Count > index + 2)
            {
                NextSeletedPage = promotion.PageList[index + 1];
                NextCurrentPage = promotion.PageList[index + 2];
                CanPage = true;
            }
            else if (promotion.PageList.Count > index + 1)
            {
                NextCurrentPage = null;
                NextSeletedPage = promotion.PageList[index + 1];
                CanPage = true;
            }
            else
            {
                NextCurrentPage = null;
                NextSeletedPage = null;
                CanPage = false;
            }
            
            if (promotion.PageList.Count % 2 == 0)
            {
                index += 2;
            }
            else 
            {
                index = index + 2 <= promotion.PageList.Count? index + 2 : index;
            }
            //Debug.Log("Index  :=: " + index + "; Count = " + promotion.PageList.Count);
        }

        
    }
    /// <summary>
    /// 刷新向右侧翻页数据
    /// </summary>
    public void RefreshToRightData() 
    {
        if(index >= 2)
        {
            index -= 2;
            CanReverse = true;

            if(index >= 0)
            {
                CurrentSeletedPage = promotion.PageList[index];
                NextCurrentPage = promotion.PageList[index];
            }
            
            if (index - 1 >= 0)
            {
                LastSeletedPage = promotion.PageList[index - 1];
                NextSeletedPage = promotion.PageList[index - 1];
            }
            else
            {
                LastSeletedPage = null;
            }
            try
            {
                NextCurrentPage = promotion.PageList[index];
            }
            catch (System.Exception exc)
            {
                Debug.Log("ModelNummber Covert err:" + exc.Message);
                NextCurrentPage = null;
            }
            
        }
    }

    IEnumerator BeginToPageLeft(JumpPageParams param)
    {
        while(!_bAnima.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }

        _bAnima["left"].speed = 0;

        if (NextCurrentPage != null)
        {
            if (NextCurrentPage is CataloguePage)
            {
                Catalogue.isLoaded = true;

                CallBack.turnPlay = GoOnBeginLeft;
                CallBack.CurrentState = PageState.NextCurrent;
            }
            else
            {
                Content.isLoaded = true;

                CallBack.turnPlay = GoOnBeginLeft;
                CallBack.CurrentState = PageState.NextCurrent;
            }
            //Debug.Log("BeginToPageLeft : NextCurrentPage != null ;  last = " + promotion.PageList[promotion.PageList.Count - 1].PageTab);
            ChangeNextCurrentRender(NextCurrentPage);
        }
        else 
        {
            //Debug.Log("BeginToPageLeft : NextCurrentPage == null");
            Content.isLoaded = false;
            NextCurrentPage = null;
            //Debug.Log("index = " + index + ";   count = " + promotion.PageList.Count + ";  pageCount = " + (promotion.PageList.Count - CatalogueLen));
            ChangeNextCurrentRender(NextCurrentPage);
            GoOnBeginLeft(param);
        }
        
    }

    public void GoOnBeginLeft(JumpPageParams param)
    {
        if (NextSeletedPage != null)
        {
            //Debug.Log("if");
            if (NextSeletedPage is CataloguePage)
            {
                //Debug.Log("if_if");
                Catalogue.isLoaded = true;

                CallBack.CurrentState = PageState.Next;
                CallBack.turnPlay = GoOnBeginLeft1;
            }
            else
            {
                //Debug.Log("if_else");
                Content.isLoaded = true;

                CallBack.CurrentState = PageState.Next;
                CallBack.turnPlay = GoOnBeginLeft1;
            }
            ChangeNextRender(NextSeletedPage);
        }
        else 
        {
            Content.isLoaded = false;
            //Debug.Log("else");
            ChangeNextRender(NextSeletedPage);
            GoOnBeginLeft1(param);
        }
        Debug.Log("GoOnBeginLeft");
    }

    public void GoOnBeginLeft1(JumpPageParams param)
    {
        Debug.Log("GoOnBeginLeft1 : " + param.AnimaSpeed);
        _bAnima["left"].speed = param.AnimaSpeed;
        //Debug.Log("Count = " + promotion.PageList.Count + " ; index = " + index);

        LastSeletedPage = promotion.PageList[index - 1];
        StartCoroutine(PageLeftSuccess(param.RefreshBtnFlg));
        //Debug.Log("GoOnBeginLeft1");
    }

    public IEnumerator PageLeftSuccess(bool pRefreshBtnFlg)
    { 
        Content.isLoaded = false;
        Catalogue.isLoaded = false;
        //BookPageB.transform.position = new Vector3(-0.0005f, BookPageB.transform.position.y, BookPageB.transform.position.z);
        float wait = BookPageB.GetComponent<Animation>()["left"].length;
        yield return new WaitForSeconds(wait * 2 / 5);
        //BookPageA.transform.localPosition = new Vector3(0, 0.017f, 0);
        yield return new WaitForSeconds(wait * 1 / 5);
        _bAnima["left"].speed = 20.0f;
        ChangeLastRender(LastSeletedPage);
        yield return new WaitForSeconds(wait * 1 / 5);
        if (!BookPageA.activeSelf) BookPageA.SetActive(true);
        while(_bAnima.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }

        RefreshCurrentCatalogueBtn();
        _refreshPageFlg = true;
        if (pRefreshBtnFlg && index >= CatalogueLen) 
        {
            AllModelBtnControllSc.Initialize(LastSeletedPage);
            //Debug.Log("NextCurrentPage == NULL ? " + (NextCurrentPage == null));
            AllModelBtnControllSc.Initialize(NextCurrentPage);
            PlayAudio(true);
        }
        ChangeCurrentRender(CurrentSeletedPage);
        turnParams.AnimaSpeed = 1.0f;
        _lastIsLeftJump = false;
        _lastIsLeft = true;
        _isTurn = false;
        _isPlaying = false;
        HasTake = false;
PagePool.Instance.SetCurrentIndex(index);
StartCoroutine(PlayAudio(true));
        //Debug.Log("HasTake = " + HasTake);

    }

    /// <summary>
    /// 刷新翻页后的目录按钮
    /// </summary>
    public void RefreshCurrentCatalogueBtn()
    {
       // Debug.Log("LastSeletedPage:" + LastSeletedPage.PageTab);
        //Debug.Log("CurrentSeletedPage:" + CurrentSeletedPage.PageTab);
        //Debug.Log("Next:" + NextSeletedPage.PageTab);
        //Debug.Log("CurrentNext:" + NextCurrentPage.PageTab);
        CatalogueGroupControl currCata = null;
        if (LastSeletedPage is CataloguePage)
        {
            //Debug.Log("LastSeletedPage is CataloguePage.");
            currCata = GetCatalogueControl(promotion.ModelNumber);
            if (currCata != null)
            {
                _tempLeftGroupSc = currCata;
                currCata.LeftCatalogueBtnControlSc.Refresh((CataloguePage)LastSeletedPage);
            }
            else
            {
                Debug.Log("null");
            }
        }
        if (NextCurrentPage is CataloguePage)
        {
            currCata = GetCatalogueControl(NextCurrentPage.ModelNumber);
            if (currCata != null)
            {
                _tempRightGroupSc = currCata;
                currCata.RightCatalogueBtnControlSc.Refresh((CataloguePage)NextCurrentPage);
            }
        }
    }
    /// <summary>
    /// 清空所有按钮
    /// </summary>
    public void ClearCatalogueBtn() 
    {
        for (int i = 0; i < CatalogueAllBtnCtrSc.CataGroupControlScs.Length; i++)
        {
            CatalogueAllBtnCtrSc.CataGroupControlScs[i].LeftCatalogueBtnControlSc.Refresh(null);
            CatalogueAllBtnCtrSc.CataGroupControlScs[i].RightCatalogueBtnControlSc.Refresh(null);
        }
    }

    public void FirstRefreshCurrentCatalogueBtn()
    {
        CatalogueGroupControl currCata = null;
        if (CurrentSeletedPage is CataloguePage)
        {
            currCata = GetCatalogueControl(promotion.ModelNumber);
            if (currCata != null)
            {
                currCata.RightCatalogueBtnControlSc.Refresh((CataloguePage)CurrentSeletedPage);
            }
            else
            {
                Debug.Log("null");
            }
        }
        else 
        {
            Debug.Log("CurrentSeletedPage == null ? " + (CurrentSeletedPage == null));
        }
    }

    private void FirstChoose()
    {
        if (promotion.PageList.Count != 0)
        {
            LastSeletedPage = null;
            index = 0;
            CurrentSeletedPage = promotion.PageList[0];
            if (promotion.PageList.Count > index + 2)
            {
                NextCurrentPage = promotion.PageList[index + 2];
                NextSeletedPage = promotion.PageList[index + 1];
                CanPage = true;
            }
            else if (promotion.PageList.Count > index + 1)
            {
                NextCurrentPage = null;
                NextSeletedPage = promotion.PageList[index + 1];
                CanPage = true;
            }
            else
            {
                NextSeletedPage = null;
                NextCurrentPage = null;
                CanPage = false;
            }
        }
        else
        {
            CurrentSeletedPage = null;
            LastSeletedPage = null;
            NextSeletedPage = null;
            NextCurrentPage = null;
        }
    }

    public void ChangeRender()
    {
        ChangeLastRender(LastSeletedPage);
        ChangeNextRender(NextSeletedPage);
        ChangeNextCurrentRender(NextCurrentPage);
        ChangeCurrentRender(CurrentSeletedPage);
    }

    public void TurnRight(JumpPageParams param)
    {
        _isTurn = true;
        _accumulateTiem = 0;
        if (index == 0)
        {
            //CurrentSeletedCover = promotion.B_bookCover;
            CurrentSeletedCover = promotion.SelfCover;
        }
        if (CurrentSeletedCover != null)
        {
            if (CanCover)
            {
                Debug.Log("播放翻封面动画");
                BookPageA.SetActive(false);
                BookCover.GetComponent<Animation>()["Take"].time = BookCover.GetComponent<Animation>()["Take"].length; 
                BookCover.GetComponent<Animation>()["Take"].speed = -1;
                BookCover.GetComponent<Animation>().Play("Take");
                if(_rightFlg)
                {
                    _rightFlg = false;
                }
                _leftFlg = true;
                //Debug.Log("l:" + _leftFlg + " ; r:" + _rightFlg);
                _isPlaying = true;
                AllModelBtnControllSc.HideAllBtn();
                OpenShadw.SetActive(false);
                StartCoroutine(CoverClose());
                CanCover = false;
                CatalogueBtnAnima.Play("CatalogueBtnHide");
                ClearCatalogueBtn();
                CameraAimation.Instance.CloseBook();
            }
        }
        else
        {
            if (index >= 2)
            {
                turnParams.AnimaSpeed = param.AnimaSpeed;
                if (NextSeletedPage != null && !_lastIsLeft)
                {
                    if (NextSeletedPage is CataloguePage)
                    {
                        Catalogue.isLoaded = true;
                        CallBack.turnPlay = TurnRightPlay;
                        CallBack.CurrentState = PageState.Next;
                    }
                    else 
                    {
                        Content.isLoaded = true;

                        CallBack.turnPlay = TurnRightPlay;
                        CallBack.CurrentState = PageState.Next;
                    }
                    //Debug.Log("Catalogue.isLoaded = " + Catalogue.isLoaded);
                    param.RefreshData = true;
                    ChangeNextRender(NextSeletedPage);
                }
                else
                {
                    TurnRightPlay(param);
                }
            }
            else
            {
                Debug.Log("不可翻");
            }
        }
    }
    /// <summary>
    /// 播放向右翻页动画
    /// </summary>
    /// <param name="param"></param>
    public void TurnRightPlay(JumpPageParams param) 
    {
        //  Debug.Log("index0 : " + index);
        if (param.RefreshData)
        {
            RefreshToRightData();
        }
        AllModelBtnControllSc.HideAllBtn();
        Debug.Log("index1 : " + index);
        PageNControl_A.HideAllHeightShine();
        ClearCatalogueBtn();
        StartCoroutine(BeginToPageRight());
        _bAnima["right"].speed = param.AnimaSpeed;
        _bAnima.Play("right");
    }

    IEnumerator CoverOpen()
    {
        float wait = BookCover.GetComponent<Animation>()["Take"].length;
        yield return new WaitForSeconds(wait * 3 / 10);
        BookPageB.SetActive(true);
        BookPageC.SetActive(true);
        FirstRefreshCurrentCatalogueBtn();
        yield return new WaitForSeconds(wait * 7 / 10);
        OpenShadw.SetActive(true);
        
    }

    IEnumerator CoverClose()
    {
        
        ClearCatalogueBtn();
        float wait = BookCover.GetComponent<Animation>()["Take"].length;
        yield return new WaitForSeconds(wait * 7 / 10);
        BookPageB.SetActive(false);
        BookPageC.SetActive(false);
        yield return new WaitForSeconds(wait * 3 / 10);
        ClearCatalogueBtn();
        _isTurn = false;
        CloseShadw.SetActive(true);
    }

    IEnumerator BeginToPageRight()
    {
        while (!_bAnima.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }
        _bAnima["right"].speed = 0;
        if (LastSeletedPage != null)
        {
            if (LastSeletedPage is CataloguePage)
            {
                Catalogue.isLoaded = true;

                CallBack.turnPlay = GoOnTrunRight;
                CallBack.CurrentState = PageState.Last;
            }
            else
            {
                Content.isLoaded = true;

                CallBack.turnPlay = GoOnTrunRight;
                CallBack.CurrentState = PageState.Last;
            }
            ChangeLastRender(LastSeletedPage);
        }
        else 
        {
            ChangeLastRender(LastSeletedPage);
            GoOnTrunRight(turnParams);
        }
        //BookPageC.transform.localPosition = new Vector3(0, 0.0188f, 0);
    }

    public void GoOnTrunRight(JumpPageParams param)
    {
        //ChangeCurrentRender(CurrentSeletedPage);
        //StartCoroutine(PageToRightSuccess(param.RefreshBtnFlg));
        //Debug.Log("GoOnTrunRight: " + LastSeletedPage.PageTab + " ;  LastSeletedPage is CataloguePage:" + (LastSeletedPage is CataloguePage));
        if (!_lastIsLeft || _lastIsRightJump)
        {
            if (CurrentSeletedPage is CataloguePage)
            {
                Catalogue.isLoaded = true;

                CallBack.turnPlay = GoOnTrunRight1;
                CallBack.CurrentState = PageState.Current;
                ChangeCurrentRender(CurrentSeletedPage);
            }
            else
            {
                Content.isLoaded = true;
                CallBack.turnPlay = GoOnTrunRight1;
                //Debug.Log("GoOnTrunRight : content");
                ChangeCurrentRender(CurrentSeletedPage);
            }
        }
        else
        {
            StartCoroutine(PageToRightSuccess(param.RefreshBtnFlg));
        }
    }

    public void GoOnTrunRight1(JumpPageParams param)
    {
        Debug.Log("GoOnTrunRight1");
        StartCoroutine(PageToRightSuccess(param.RefreshBtnFlg));
    }

    public IEnumerator PageToRightSuccess(bool pRefreshBtnFlg)
    {
        Catalogue.isLoaded = false;
        Content.isLoaded = false;
        _bAnima["right"].speed = turnParams.AnimaSpeed;
        //BookPageB.transform.position = new Vector3(0, BookPageB.transform.position.y, BookPageB.transform.position.z);
        //Debug.Log("PageToRightSuccess");
        yield return new WaitForSeconds(_bAnima["right"].length * 0.6f);
        _bAnima["right"].speed = 20.0f;
        while (_bAnima.isPlaying)
        {
            yield return new WaitForSeconds(0);
        }

        ChangeNextCurrentRender(NextCurrentPage);  
        RefreshCurrentCatalogueBtn();
        _refreshPageFlg = true;
        if (pRefreshBtnFlg && index >= CatalogueLen)
        {
            AllModelBtnControllSc.Initialize(LastSeletedPage);
            AllModelBtnControllSc.Initialize(CurrentSeletedPage);
            PlayAudio(false);
        }
        turnParams.AnimaSpeed = 1.0f;
        _lastIsRightJump = false;
        _lastIsLeft = false;
        _isTurn = false;
        _isPlaying = false;
        HasTake = false;
        PagePool.Instance.SetCurrentIndex(index);
        StartCoroutine(PlayAudio(true));
    }


    public void ChangeCurrentRender(Page bookPage)
    {
        Debug.Log("ChangeCurrentRender");
        if (bookPage == null)
        {
            SetFalse(CurrentRender);
        }
        else
        {
            SetFalse(CurrentRender);
            ChooseSetActive(bookPage, CurrentRender);
            //PageNControl_B.Initialize(bookPage);
            //Debug.Log("ChangeCurrentRender:" + bookPage.PageTab);
        }
        //Debug.Log("ChangeCurrentRender");
    }

    public void ChangeNextRender(Page bookPage)
    {
        if (bookPage == null)
        {
            Debug.Log("ChangeNextRender --> bookPage == null");
            SetFalse(NextRender);
        }
        else
        {
            Debug.Log("ChangeNextRender");
            SetFalse(NextRender);
            ChooseSetActive(bookPage, NextRender);
        }
    }

    public void ChangeNextCurrentRender(Page bookPage)
    {
        if (bookPage == null)
        {
            SetFalse(NextCurrentRender);
        }
        else
        {
            SetFalse(NextCurrentRender);
            ChooseSetActive(bookPage, NextCurrentRender);
            //PageNControl_C.Initialize(bookPage);
        }
        //Debug.Log("ChangeNextCurrentRender");
    }

    void ChooseSetActive(Page bookPage, GameObject go)
    {
        SetActive(go, bookPage);
    }

    public void ChangeLastRender(Page bookPage)
    {
        if (bookPage == null)
        {
            SetFalse(LastRender);
        }
        else
        {
            SetFalse(LastRender);
            ChooseSetActive(bookPage, LastRender);
            //Debug.Log("Last : " + bookPage.PageTab);
            //PageNControl_A.Initialize(bookPage);
        }
        //Debug.Log("ChangeLastRender");
    }

    void SetFalse(GameObject go)
    {
        foreach (Transform item in go.transform)
        {
            item.gameObject.SetActive(false);
        }
    }

    void SetActive(GameObject go, Page pBookPage)
    {
        
        PageControl pnc = go.GetComponent<PageControl>();
    
        if (pnc != null)
        {
            pnc.SetCurrentModel(pBookPage);
     //Debug.Log(pBookPage.PageTab);
        }
    }

    public IEnumerator PlayAudio(bool lastIsLeft) 
    {
        Debug.Log("isLoadingClip = " + PagePool.Instance.isLoadingClip);
        while(PagePool.Instance.isLoadingClip)
        {
            Debug.Log("isLoading...");
            yield return new WaitForSeconds(0);
        }

        if (LastSeletedPage != null)
        {
            LeftClip = PagePool.Instance.GetAudioClip(new Content.UpParam(GetIndex(LastSeletedPage), LastSeletedPage is CataloguePage));
        }
        else
        {
            LeftClip = null;
        }
        if (CurrentSeletedPage != null)
        {

            RightClip = PagePool.Instance.GetAudioClip(new Content.UpParam(GetIndex(CurrentSeletedPage), CurrentSeletedPage is CataloguePage));
        }
        else
        {
            RightClip = null;
        }
        breakFlg = true;
        while (breakFlg)
        {
            //Debug.Log("sb");
            yield return new WaitForSeconds(0);
        }
        StartCoroutine(startPlayAudio());
    }

    public int GetIndex(Page page) 
    {
        if (page is CataloguePage)
        {
            return page.PageTab;
        }
        else 
        {
            return page.PageTab + CatalogueLen;
        }
    }

    public bool canPlay = true;
    public bool breakFlg = false;
    IEnumerator startPlayAudio() 
    {
        //Debug.Log("startPlayAudio?");
        while (!breakFlg && canPlay)
        {
            //Debug.Log("1");
            if (LeftClip != null && canPlay)
            {
                AudioSce.clip = LeftClip;
                AudioSce.Play();
            }
            else if (LeftClip == null)
            {
                //Debug.Log("LeftClip == null");
            }
            //Debug.Log("2");
            if (!canPlay)
            {
                yield return new WaitForSeconds(0.01f);
            }
            //Debug.Log("3");
            while (AudioSce.isPlaying && !breakFlg && canPlay)
            {
                //Debug.Log("left is playing.");
                yield return new WaitForSeconds(0);
            }
            //Debug.Log("4");
            if (RightClip != null && canPlay)
            {
                Debug.Log("play right Audio.");
                AudioSce.clip = RightClip;
                AudioSce.Play();
            }
            else if (RightClip == null)
            {
                //Debug.Log("RightClip == null");
            }
            //Debug.Log("5");
            if (!canPlay)
            {
                //Debug.Log("canPlay is false!");
                yield return new WaitForSeconds(0.01f);
            }
            while (AudioSce.isPlaying && !breakFlg && canPlay)
            {
                //Debug.Log("right is playing.");
                yield return new WaitForSeconds(0);
            }
            yield return null;
            //Debug.Log("6");
        }
        AudioSce.Stop();
        breakFlg = false;
    }
}
