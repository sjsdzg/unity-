using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 复选框单元
    /// </summary>
    public class DataGridViewCheckBoxCell : DataGridViewCell
    {
        public override CellType GetCellType()
        {
            return CellType.CheckBox;
        }

        public class CheckedChangedEvent : UnityEvent<bool> { }

        private CheckedChangedEvent checkedChanged = new CheckedChangedEvent();
        /// <summary>
        /// 选中状态改变时，触发
        /// </summary>
        public CheckedChangedEvent CheckedChanged
        {
            get { return checkedChanged; }
            set { checkedChanged = value; }
        }

        /// <summary>
        /// 复选框
        /// </summary>
        private Toggle checkBox;

        protected override void OnAwake()
        {
            base.OnAwake();
            checkBox = transform.GetComponentInChildren<Toggle>();
            checkBox.onValueChanged.RemoveAllListeners();
            checkBox.onValueChanged.AddListener(checkBox_onValueChanged);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="value"></param>
        public override void SetValue(object data)
        {
            if (checkBox != null)
            {
                checkBox.isOn = (bool)data;
                Value = data;
            }
        }

        /// <summary>
        /// 复选框更改时触发
        /// </summary>
        /// <param name="arg0"></param>
        private void checkBox_onValueChanged(bool arg0)
        {
            Value = arg0;
            CheckedChanged.Invoke(arg0);
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public override object GetValue()
        {
            if (checkBox != null)
            {
                return checkBox.isOn;
            }
            return false;
        }
    }
}
