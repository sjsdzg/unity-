using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace XFramework.Core
{
    public class IlabUtil
    {
        /// <summary>
        /// 实验空间登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static WebRequestOperation OpenLogin(string userName, string password)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(AppSettings.Settings.WebServerUrl);
            builder.Append("ilab_x/validate");

            // 发送校验请求
            string uri = builder.ToString();
            Debug.Log("uri: " + uri);
            WWWForm form = new WWWForm();
            form.AddField("name", userName);
            form.AddField("pwd", password);

            //return WebRequestManager.Instance.Get(uri);
            return WebRequestManager.Instance.Post(uri, form);
        }

        /// <summary>
        /// 实验结果数据回传
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static WebRequestOperation LogUpload(string json)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(AppSettings.Settings.WebServerUrl);
            builder.Append("ilab_x/logupload");

            string uri = builder.ToString();
            Debug.Log("uri: " + uri);

            return WebRequestManager.Instance.Post(uri, json);
        }


        /// <summary>
        /// 实验操作状态回传
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static WebRequestOperation StatusUpload(string userName)
        {
            JObject jObj = new JObject();
            jObj.Add(new JProperty("username", userName));
            string json = jObj.ToString();

            StringBuilder builder = new StringBuilder();
            builder.Append(AppSettings.Settings.WebServerUrl);
            builder.Append("ilab_x/statusupload");

            string uri = builder.ToString();
            Debug.Log("uri: " + uri);

            return WebRequestManager.Instance.Post(uri, json);
        }


        /// <summary>
        /// 安全验证随机数
        /// </summary>
        /// <returns></returns>
        public static string GetValidationNumbers()
        {
            StringBuilder sb = new StringBuilder();
            System.Random random = new System.Random();
            for (int i = 0; i < 16; i++)
            {
                sb.Append(random.Next(0, 16).ToString("X"));
            }
            Debug.Log(sb.ToString());
            return sb.ToString();
        }
    }
}
