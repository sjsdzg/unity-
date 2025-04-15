using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class CheckboxHeaderCell : MonoBehaviour
    {
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

        private bool isTrigger = true;
        /// <summary>
        /// 是否触发事件
        /// </summary>
        public bool IsTrigger
        {
            get { return isTrigger; }
            set { isTrigger = value; }
        }

        /// <summary>
        /// 选中框头
        /// </summary>
        private CheckboxHeader checkboxHeader;
        /// <summary>
        /// 选中框
        /// </summary>
        private Toggle checkbox;


        private bool _checked = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                checkbox.isOn = _checked;
            }
        }

        private void Awake()
        {
            checkbox = transform.GetComponentInChildren<Toggle>();
            if (checkbox != null)
            {
                checkbox.onValueChanged.AddListener(checkbox_onValueChanged);
            }
        }

        /// <summary>
        /// 选中框值更改时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void checkbox_onValueChanged(bool arg0)
        {
            if (IsTrigger)
            {
                _checked = arg0;
                CheckedChanged.Invoke(arg0);
            }
        }

    }
}
