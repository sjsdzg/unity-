using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 日志Item
    /// </summary>
    public class LogItem : MonoBehaviour
    {
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
        public LogItemData data { get; private set; }

        /// <summary>
        /// 正常颜色
        /// </summary>
        public Color normalColor = new Color32(255, 255, 255, 255);

        /// <summary>
        /// 错误颜色
        /// </summary>
        public Color errorColor = new Color32(255, 100, 100, 255);

        void Awake()
        {
            bg = transform.GetComponent<Image>();
            text = transform.GetComponentInChildren<Text>();
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parameter"></param>
        public void SetValue(LogItemData _data)
        {
            data = _data;

            if (_data.type == LogType.Log)
            {
                bg.color = normalColor;
            }
            else
            {
                bg.color = errorColor;
            }
            text.text = _data.Log;
        }
    }
}
