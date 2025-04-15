using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 规范文件列表
    /// 
    /// </summary>
	public class AuthorityFileBar : MonoBehaviour {

        /// <summary>
        /// 组件列表
        /// </summary>
        private List<AuthortyItem> m_ItemComponents;


        /// <summary>
        /// 默认项
        /// </summary>
        public GameObject DefaultItem;


        /// <summary>
        /// 内容
        /// </summary>
        private RectTransform Content;
        void Awake()
        {
            m_ItemComponents = new List<AuthortyItem>();


            Content = transform.Find("Scroll View/Viewport/Content").GetComponent<RectTransform>();

            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type LogItem to DefaultItem.");
            }

            DefaultItem.gameObject.SetActive(false);

        }
        /// <summary>
        /// 清空
        /// </summary>
        private void Clear()
        {
            for (int i = 0; i < m_ItemComponents.Count; i++)
            {
                AuthortyItem item = m_ItemComponents[i];
                Destroy(item.gameObject);
            }

            m_ItemComponents.Clear();
            //m_LogDatas.Clear();
        }
    }
}