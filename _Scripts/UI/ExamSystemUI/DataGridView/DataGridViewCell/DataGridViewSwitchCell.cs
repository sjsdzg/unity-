using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class DataGridViewSwitchCell : DataGridViewCell
    {
        public override CellType GetCellType()
        {
            return CellType.Switch;
        }

        private SwitchEvent m_OnValueChanged = new SwitchEvent();
        /// <summary>
        /// 选中状态改变时，触发
        /// </summary>
        public SwitchEvent OnValueChanged
        {
            get { return m_OnValueChanged; }
            set { m_OnValueChanged = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 开启文本
        /// </summary>
        public string onText;

        /// <summary>
        /// 关闭文本
        /// </summary>
        public string offText;

        /// <summary>
        /// 复选框
        /// </summary>
        private Switch m_Switch;

        protected override void OnAwake()
        {
            base.OnAwake();
            m_Text = transform.GetComponentInChildren<Text>();
            m_Switch = transform.GetComponentInChildren<Switch>();
            m_Switch.OnValueChanged.AddListener(m_Switch_OnValueChanged);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object data)
        {
            if (m_Switch != null)
            {
                m_Switch.IsOn = (bool)data;
                Value = data;

                if (m_Switch.IsOn)
                {
                    m_Text.text = onText;
                }
                else
                {
                    m_Text.text = offText;
                }
            }
        }

        /// <summary>
        /// 更改时触发
        /// </summary>
        /// <param name="arg0"></param>
        private void m_Switch_OnValueChanged(bool arg0)
        {
            Value = arg0;
            OnValueChanged.Invoke(arg0);

            if (m_Switch.IsOn)
            {
                m_Text.text = onText;
            }
            else
            {
                m_Text.text = offText;
            }
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            if (m_Switch != null)
            {
                return m_Switch.IsOn;
            }
            return false;
        }
    }
}
