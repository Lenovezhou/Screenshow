
/*************************
 * Title: 工程名
 * Function:方法作用
 *      - 
 * Used By: 
 * Author: 001
 * Date:    2015.10
 * Version: 1.0
 * Record:  
 *      
 *************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PagePool : MonoBehaviour
{
    public static PagePool Instance;
    public class PageData
    {
        public Dictionary<string, Texture2D> ImagesUrl;
        public string AudioUrl;
        public AudioClip Audio;
        public bool needLoad;
        public PageData()
        {
            ImagesUrl = new Dictionary<string, Texture2D>();
            Audio = null;
            AudioUrl = "";
        }
    }

    public Texture ClgTx1;
    public Texture ClgTx2;

    public int PageSize;
    public Coroutine UploadImgCor;
    public Coroutine UploadAudioCor;

    public Dictionary<int, PageData> PageDataBuffer1;   //  图片

    public bool isLoadingPicture;
    public bool isLoadingClip;

    private int _index;
    public int index
    {
        set
        {
            _index = value;
            if (_index > _pageBuffer1.Count - 1)
            {
                _index = _pageBuffer1.Count - 1;
            }
        }
        get
        {
            return _index;
        }
    }
    private Promotion book;
    public Promotion Book
    {
        set
        {
            book = value;
            Init();
            index = 0;
        }
        get
        {
            return book;
        }
    }
    private List<PageData> _pageBuffer1;

    int max, min;
    void Awake()
    {
        max = 0;
        min = 0;

        _pageBuffer1 = new List<PageData>();
        PageDataBuffer1 = new Dictionary<int, PageData>();
        
        Instance = this;
        UploadImgCor = null;
        UploadAudioCor = null;
        isLoadingPicture = false;
        isLoadingClip = false;
    }
    /// <summary>
    /// 初始化加载列表
    /// </summary>
    public void Init()
    {
        if (book.PageList.Count > 2)
        {
            PageData pd = new PageData();
            string clg1 = (book.PageList[0] as CataloguePage).PictureUrl, clg2 = (book.PageList[1] as CataloguePage).PictureUrl;
            for (int i = 0; i < BookInfomation._isntance.CatalogueLen; i++)
            {
                pd = new PageData();
                if (i >= 2)
                {
                    //pd.needLoad = false;
                    if (i % 2 == 0)
                    {
                        pd.ImagesUrl.Add(clg1, null);
                    }
                    else 
                    {
                        pd.ImagesUrl.Add(clg2, null);
                    }
                }
                else 
                {
                    pd.ImagesUrl.Add((book.PageList[i] as CataloguePage).PictureUrl, null);
                }
                pd.AudioUrl = book.PageList[i].AudioUrl;
                _pageBuffer1.Add(pd);
            }
            for (int i = BookInfomation._isntance.CatalogueLen; i < book.PageList.Count; i++)
            {
                //Debug.Log(i + ":" + (book.PageList[i].Items == null));
                if (book.PageList[i].Items != null)
                {
                    pd = new PageData();
                    foreach (string item in book.PageList[i].Items.Keys)
                    {
                        pd.AudioUrl = book.PageList[i].AudioUrl;
                        if (book.PageList[i].Items[item].type == ItemType.RawImage)
                        {
                            pd.ImagesUrl.Add(book.PageList[i].Items[item].target as string, null);
                        }
                    }
                    _pageBuffer1.Add(pd);
                }
                
            }
            //Debug.Log("_pageBuffer1.count = " + _pageBuffer1.Count);
        }
    }
    public void SetCurrentIndex(int pIndex)
    {
        index = pIndex;
        //Debug.Log(index + ";");
        #region 设置最大最小区间
        if (index >= PageSize / 2.0f || index >= max / 2.0f && max != 0)
        {
            if (index + PageSize / 2 < _pageBuffer1.Count)
            {
                max = index + PageSize / 2;
                if (max - PageSize >= 0)
                {
                    min = max - PageSize;
                }
                else
                {
                    min = 0;
                }
            }
            else
            {
                max = _pageBuffer1.Count - 1;
                if (max - PageSize >= 0)
                {
                    min = max - PageSize;
                }
                else
                {
                    min = 0;
                }
            }
        }
        else
        {
            min = 0;
            if (PageSize < _pageBuffer1.Count)
            {
                max = PageSize;
            }
            else
            {
                max = _pageBuffer1.Count - 1;
            }
        }
        #endregion
        //Debug.Log("max = " + max + ";  min = " + min);
        //Debug.Log("min = " + min + ";  max = " + max);
        // 移除不需要的缓冲
        RemovedNotNeed(min, max);
        // 填充加载数据所需配置参数
        FullPageBuffer(min, max);

        if (UploadImgCor != null)
        {
            try
            {
                StopCoroutine(UploadImgCor);
            }
            catch(System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally
            {
                UploadImgCor = null;
            }
        }

        if (UploadAudioCor != null)
        {
            try
            {
                StopCoroutine(UploadAudioCor);
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message);
            }
            finally
            {
                UploadAudioCor = null;
            }  
        }

        // 加载图片
        UploadImgCor = StartCoroutine(UploadPicture());
        // 加载音频协成
        UploadAudioCor = StartCoroutine(UploadAudioClip());
    }
    /// <summary>
    /// 移除不需要的缓冲
    /// </summary>
    /// <param name="max"></param>
    /// <param name="min"></param>
    public void RemovedNotNeed(int min, int max)
    {
        Dictionary<int, PageData>.Enumerator em = PageDataBuffer1.GetEnumerator();
        List<int> removedId = new List<int>();
        while (em.MoveNext())
        {
            if (em.Current.Key > max || em.Current.Key < min)
            {
                removedId.Add(em.Current.Key);
            }
        }
        for (int i = 0; i < removedId.Count; i++)
        {
            Dictionary<string, Texture2D>.Enumerator em1 = PageDataBuffer1[removedId[i]].ImagesUrl.GetEnumerator();
            while (em1.MoveNext())
            {
                if (em1.Current.Value != null)
                {
                    GameObject.Destroy(em1.Current.Value);
                }
            }
            if (PageDataBuffer1[removedId[i]].Audio != null)
            {
                GameObject.Destroy(PageDataBuffer1[removedId[i]].Audio);
            }
            PageDataBuffer1.Remove(removedId[i]);
        }
        Resources.UnloadUnusedAssets();
    }
    /// <summary>
    /// 填充加载数据所需配置参数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void FullPageBuffer(int min, int max)
    {
        for (int i = min; i <= max; i++)
        {
            if (!PageDataBuffer1.ContainsKey(i))
            {
                PageDataBuffer1.Add(i, _pageBuffer1[i]);
            }
        }
    }
    /// <summary>
    /// 加载图片协成
    /// </summary>
    /// <returns></returns>
    public IEnumerator UploadPicture()
    {
        //Debug.Log("UploadPicture");
        isLoadingPicture = true;
        if (PageDataBuffer1.Count > 0)
        {
            WWW www;
            Dictionary<int, PageData>.KeyCollection piKeys = PageDataBuffer1.Keys;
            List<PageData> pds = new List<PageData>();
            List<string> urls = new List<string>();
            foreach (int piKey in piKeys)
            {
                pds.Add(PageDataBuffer1[piKey]);
            }
            for (int i = 0; i < pds.Count; i++)
            {
                urls.Clear();
                foreach (string k in pds[i].ImagesUrl.Keys)
                {
                    urls.Add(k);
                }
                
                for (int j = 0; j < urls.Count; j++)
                {
                    www = new WWW(GetXml.Instances.LoadUrl +  urls[j]);
                    yield return www;
                    if (www.isDone && www.error == null)
                    {
                        pds[i].ImagesUrl[urls[j]] = www.texture;
                    }
                    else
                    {
                        Debug.Log(www.error == null ? "www of picture is not done!" : www.error);
                    }
                }
            }
        }
        isLoadingPicture = false;
        UploadImgCor = null;
    }
    /// <summary>
    /// 加载音频协成
    /// </summary>
    /// <returns></returns>
    public IEnumerator UploadAudioClip()
    {
        //yield return new WaitForSeconds(0);
        isLoadingClip = true;
        if (PageDataBuffer1.Count > 0)
        {
            WWW www;

            foreach (int index in PageDataBuffer1.Keys)
            {
                if (!string.IsNullOrEmpty(PageDataBuffer1[index].AudioUrl) && PageDataBuffer1[index].Audio == null)
                {
                    //Debug.Log("www:" + PageDataBuffer1[index].AudioUrl);
                    www = new WWW(GetXml.Instances.LoadUrl + PageDataBuffer1[index].AudioUrl);
                    yield return new WaitForSeconds(0.01f);
                    //yield return www;
                    if (
                        //www.isDone && 
                        www.error == null)
                    {
                        PageDataBuffer1[index].Audio = www.audioClip;
                    }
                    else
                    {
                        Debug.Log(www.error == null ? "www of audio is not done!" : www.error);
                    }
                }
                
            }
        }
        isLoadingClip = false;
        UploadAudioCor = null;
    }
    /// <summary>
    /// 从缓冲区取音频
    /// </summary>
    /// <param name="upParam"></param>
    /// <returns></returns>
    public AudioClip GetAudioClip(Content.UpParam upParam) 
    {
        AudioClip ac = null;
        if (PageDataBuffer1.ContainsKey(upParam.PageTab))
        {
            if (PageDataBuffer1[upParam.PageTab].Audio != null)
            {
                ac = PageDataBuffer1[upParam.PageTab].Audio;
            }
            else 
            {
                Debug.Log(":-> PageDataBuffer1[" + upParam.PageTab + "].Audio == null");
                //foreach (int index in PageDataBuffer1.Keys)
                //{
                //    Debug.Log(":" + index + "; ; " + PageDataBuffer1[index].AudioUrl + "; ; index[audio] == null ? " + (PageDataBuffer1[index].Audio == null));
                //}
            }
        }
        else 
        {
            Debug.Log("PageDataBuffer1 not ContainsKey:" + upParam.PageTab);
            foreach (int index in PageDataBuffer1.Keys)
            {
                Debug.Log(":" + index + "; ; " + PageDataBuffer1[index].AudioUrl + "; ; index[audio] == null ? " + (PageDataBuffer1[index].Audio == null));
            }
        }
        
        return ac;
    }
    /// <summary>
    /// 从缓冲区获取图片
    /// </summary>
    /// <param name="upParam"></param>
    public void GetPictureData(Content.UpParam upParam) 
    {
        if (PageDataBuffer1.ContainsKey(upParam.PageTab))
        {
            //Debug.Log("PageTab = " + upParam.PageTab);
            Dictionary<string, RawImage>.Enumerator em = upParam.UploadData.GetEnumerator();
            while (em.MoveNext())
            {
                if (PageDataBuffer1[upParam.PageTab].ImagesUrl.ContainsKey(em.Current.Key) &&
                    PageDataBuffer1[upParam.PageTab].ImagesUrl[em.Current.Key] != null)
                {
                    em.Current.Value.texture = PageDataBuffer1[upParam.PageTab].ImagesUrl[em.Current.Key];
                    Debug.Log("buffer[get:Picture]:" + upParam.PageTab);
                }
                else
                {
                    Debug.Log("ContainsUrl ? " + PageDataBuffer1[upParam.PageTab].ImagesUrl.ContainsKey(em.Current.Key) + ";  texture != null ? " +
                        (PageDataBuffer1[upParam.PageTab].ImagesUrl.ContainsKey(em.Current.Key) ? PageDataBuffer1[upParam.PageTab].ImagesUrl[em.Current.Key] != null : true));
                    Debug.Log("book.PageTab = " + upParam.PageTab);
                }
            }
        }
        else
        {
            Debug.Log("PageImgBuffer not ContainsKey:" + (upParam.PageTab));
        }
    }
}


