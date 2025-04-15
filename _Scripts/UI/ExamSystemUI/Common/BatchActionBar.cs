using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 批量操作栏目
    /// </summary>
    public class BatchActionBar : MonoBehaviour
    {
        public class ButtonCellClickEvent : UnityEvent<BatchActionBar, ButtonCellType> { }

        private ButtonCellClickEvent m_ButtonCellClick = new ButtonCellClickEvent();
        /// <summary>
        /// 按钮单元进行点击
        /// </summary>
        public ButtonCellClickEvent ButtonCellClick
        {
            get { return m_ButtonCellClick; }
            set { m_ButtonCellClick = value; }
        }

        /// <summary>
        /// 一系列按钮
        /// </summary>
        private ButtonCell[] buttonCells;

        void Awake()
        { 
            buttonCells = transform.GetComponentsInChildren<ButtonCell>();
            foreach (ButtonCell buttonCell in buttonCells)
            {
                buttonCell.OnClicked.RemoveAllListeners();
                buttonCell.OnClicked.AddListener(button_onClick);
            }
        }

        /// <summary>
        /// 按钮点击时，触发
        /// </summary>
        private void button_onClick(ButtonCellType type)
        {
            ButtonCellClick.Invoke(this, type);
        }
    }
}
