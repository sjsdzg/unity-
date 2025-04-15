using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class TextHeaderCell : MonoBehaviour
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;

        private void Awake()
        {
            m_Text = transform.Find("Text").GetComponent<Text>();
        }

        public void SetValue(string text)
        {
            m_Text.text = text;
        }
    }
}
