using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 导航菜单
    /// </summary>
    public class NavigationMenu : MonoBehaviour
    {
        /// <summary>
        /// 图标
        /// </summary>
        public Image image;
        /// <summary>
        /// 文本
        /// </summary>
        public Text text;

        public void SetValue(Sprite sprite, string _text)
        {
            if (image != null && sprite != null)
            {
                image.sprite = sprite;
            }

            if (text != null)
            {
                text.text = _text;
            }
        }
    }
}
