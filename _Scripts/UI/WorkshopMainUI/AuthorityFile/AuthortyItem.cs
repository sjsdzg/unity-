using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 规范文件列表个体
    /// </summary>
	public class AuthortyItem : MonoBehaviour {
        /// <summary>
        /// 文本
        /// </summary>
        private Text text;

        /// <summary>
        /// 背景图标
        /// </summary>
        private Image bg;

        /// <summary>
        /// 动作数据
        /// </summary>


        void Awake()
        {
            bg = transform.GetComponent<Image>();
            text = transform.GetComponentInChildren<Text>();
        }

        public void SetValue()
        {

        }
	}
}