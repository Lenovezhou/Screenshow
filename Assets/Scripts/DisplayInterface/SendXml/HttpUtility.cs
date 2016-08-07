using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using UnityEngine.Experimental.Networking;

namespace TestUploadData
{
    class HttpUtility:MonoBehaviour
    {
        WWW www = null;
        /// <summary>
        /// 使用Post方法发送XML请求
        /// </summary>
        /// <param name="strUrl">请求URL</param>
        /// <param name="strXML">POST内容(XML)</param>
        /// <returns>响应内容</returns>
        public static string SendXML(string strUrl, string strXML)
        {
            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            //request.Method = "POST";
            byte[] sendBuff = Encoding.UTF8.GetBytes(strXML);
            //using (Stream requestStream = request.GetRequestStream())
            //{
            //    requestStream.Write(sendBuff, 0, sendBuff.Length);
            //    Camera.main.GetComponent<AssetManager>().textshow.text += "    4555555";
            //}
            //WebResponse response = request.GetResponse();
            //using (Stream responseStream = response.GetResponseStream())
            //{
            //    using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            //    {
            //        strResponse = reader.ReadToEnd();
            //    }
            //    Camera.main.GetComponent<AssetManager>().textshow.text += "    5666666";
            //}
            return null;
        }

        //public static string SendXML(string strUrl, string strXML)
        //{

        //    string strResponse = "";
        //    Camera.main.GetComponent<AssetManager>().textshow.text += " strUrl   111111111" + strUrl;
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);

        //    Camera.main.GetComponent<AssetManager>().textshow.text += "    22222222";
        //    request.Method = "POST";
        //    Camera.main.GetComponent<AssetManager>().textshow.text += "    333333";
        //    byte[] sendBuff = Encoding.UTF8.GetBytes(strXML);
        //    Camera.main.GetComponent<AssetManager>().textshow.text += "    4444";
        //    using (Stream requestStream = request.GetRequestStream())
        //    {
        //        requestStream.Write(sendBuff, 0, sendBuff.Length);
        //        Camera.main.GetComponent<AssetManager>().textshow.text += "    4555555";
        //    }
        //    WebResponse response = request.GetResponse();
        //    using (Stream responseStream = response.GetResponseStream())
        //    {
        //        using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
        //        {
        //            strResponse = reader.ReadToEnd();
        //        }
        //        Camera.main.GetComponent<AssetManager>().textshow.text += "    5666666";
        //    }
        //    Camera.main.GetComponent<AssetManager>().textshow.text += "    5667888999";
        //    return strResponse;
        //}
        /// <summary>
        /// 使用Post方法获取字符串结果
        /// </summary>
        /// <param name="strUrl"></param>
        /// <param name="formItems">Post表单内容</param>
        /// <param name="cookieContainer"></param>
        /// <param name="iTimeOut">默认20秒</param>
        /// <param name="encoding">响应内容的编码类型（默认utf-8）</param>
        /// <returns></returns>
        public static string PostForm(string strUrl, List<HttpFormItem> formItems, CookieContainer cookieContainer = null, string strRefererUrl = null, Encoding encoding = null, int iTimeOut = 20000)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(strUrl);
            #region 初始化请求对象
            request.Method = "POST";
            request.Timeout = iTimeOut;
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36";
            if (!string.IsNullOrEmpty(strRefererUrl))
                request.Referer = strRefererUrl;
            if (cookieContainer != null)
                request.CookieContainer = cookieContainer;
            #endregion

            string boundary = "----" + DateTime.Now.Ticks.ToString("x");//分隔符
            request.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            //请求流
            var postStream = new MemoryStream();
            #region 处理Form表单请求内容
            //是否用Form上传文件
            var formUploadFile = formItems != null && formItems.Count > 0;
            if (formUploadFile)
            {
                //文件数据模板
                string fileFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"" +
                    "\r\nContent-Type: application/octet-stream" +
                    "\r\n\r\n";
                //文本数据模板
                string dataFormdataTemplate =
                    "\r\n--" + boundary +
                    "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                    "\r\n\r\n{1}";
                foreach (var item in formItems)
                {
                    string formdata = null;
                    if (item.IsFile)
                    {
                        //上传文件
                        formdata = string.Format(
                            fileFormdataTemplate,
                            item.Key, //表单键
                            item.FileName);
                    }
                    else
                    {
                        //上传文本
                        formdata = string.Format(
                            dataFormdataTemplate,
                            item.Key,
                            item.Value);
                    }

                    //统一处理
                    byte[] formdataBytes = null;
                    //第一行不需要换行
                    if (postStream.Length == 0)
                        formdataBytes = Encoding.UTF8.GetBytes(formdata.Substring(2, formdata.Length - 2));
                    else
                        formdataBytes = Encoding.UTF8.GetBytes(formdata);
                    postStream.Write(formdataBytes, 0, formdataBytes.Length);

                    //写入文件内容
                    if (item.FileContent != null && item.FileContent.Length > 0)
                    {
                        using (var stream = item.FileContent)
                        {
                            byte[] buffer = new byte[1024];
                            int bytesRead = 0;
                            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                postStream.Write(buffer, 0, bytesRead);
                            }
                        }
                    }
                }
                //结尾
                var footer = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
                postStream.Write(footer, 0, footer.Length);

            }
            else
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            #endregion

            request.ContentLength = postStream.Length;

            #region 输入二进制流
            if (postStream != null)
            {
                postStream.Position = 0;
                //直接写入流
                Stream requestStream = request.GetRequestStream();

                byte[] buffer = new byte[1024];
                int bytesRead = 0;
                while ((bytesRead = postStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                }

                ////debug
                //postStream.Seek(0, SeekOrigin.Begin);
                //StreamReader sr = new StreamReader(postStream);
                //var postStr = sr.ReadToEnd();
                postStream.Close();//关闭文件访问
            }
            #endregion

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (cookieContainer != null)
            {
                response.Cookies = cookieContainer.GetCookies(response.ResponseUri);
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader myStreamReader = new StreamReader(responseStream, encoding ?? Encoding.UTF8))
                {
                    string retString = myStreamReader.ReadToEnd();
                    return retString;
                }
            }
        }

     }
}
