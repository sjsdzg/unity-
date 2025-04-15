using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace XFramework.Core
{
    public class AppSettings
    {
        /// <summary>
        /// 默认配置文件
        /// </summary>
        private const string path = "AppSettings";

        private static AppSettings settings;
        /// <summary>
        /// 程序设置信息单例
        /// </summary>
        public static AppSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = new AppSettings();
                }
                return settings;
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                if (null == m_KeyValuePairs || !m_KeyValuePairs.ContainsKey(key))
                    return null;
                return m_KeyValuePairs[key];
            }
            set
            {
                if (null == m_KeyValuePairs)
                    m_KeyValuePairs = new Dictionary<string, string>();
                if (m_KeyValuePairs.ContainsKey(key))
                    m_KeyValuePairs[key] = value;
                else
                    m_KeyValuePairs.Add(key, value);
            }
        }

        #region 程序设置信息
        /// <summary>
        /// 服务器IP
        /// </summary>
        public string ServerIP { get; private set; }

        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// WebSocket Url
        /// </summary>
        public string WebSocketUrl { get; set; }

        /// <summary>
        /// 资源服务器url
        /// </summary>
        public string AssetServerUrl { get; set; }

        /// <summary>
        /// 存储服务器url
        /// </summary>
        public string WebServerUrl { get; set; }
        /// <summary>
        /// CAD路径
        /// </summary>
        //public string CAD { get; set; }
        #endregion

        /// <summary>
        /// 配置信息
        /// </summary>
        private Dictionary<string, string> m_KeyValuePairs = new Dictionary<string, string>();

        private AppSettings()
        {
            ParseInfo();
        }

        /// <summary>
        /// 获取配置文件记录
        /// </summary>
        private void ParseInfo()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return;
#endif

            string path = Application.streamingAssetsPath + "/AppSettings.json";
            string json = File.ReadAllText(path);
            ParseFromJson(json);
        }

        public void ParseFromJson(string json)
        {
            JObject jObject = JObject.Parse(json);
            foreach (var item in jObject)
            {
                m_KeyValuePairs.Add(item.Key, item.Value.ToString());
            }

            //程序设置信息
            ServerIP = m_KeyValuePairs["ServerIP"];
            Port = int.Parse(m_KeyValuePairs["Port"]);
            WebSocketUrl = m_KeyValuePairs["WebSocketUrl"];
            AssetServerUrl = m_KeyValuePairs["AssetServerUrl"];
            WebServerUrl = m_KeyValuePairs["WebServerUrl"];
        }
    }
}