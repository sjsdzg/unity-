using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    public class TextControl : ControlBase
    {
        public override string GetKey()
        {
            return typeof(TextControl).ToString();
        }

        /// <summary>
        /// 文本
        /// </summary>
        [SerializeField]
        private Text m_Text;

        protected override void UpdateControl(object value)
        {
            base.UpdateControl(value);
            m_Text.text = value.ToString();
        }
    }
}

