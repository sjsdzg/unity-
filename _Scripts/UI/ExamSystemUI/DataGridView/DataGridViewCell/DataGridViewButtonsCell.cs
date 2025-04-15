using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 一系列按钮单元
    /// </summary>
    public class DataGridViewButtonsCell : DataGridViewCell
    {
        public class ButtonCellClickEvent : UnityEvent<DataGridViewButtonsCell, ButtonCellType> { }

        private ButtonCellClickEvent m_ButtonCellClick = new ButtonCellClickEvent();
        /// <summary>
        /// 按钮单元进行点击
        /// </summary>
        public ButtonCellClickEvent ButtonCellClick
        {
            get { return m_ButtonCellClick; }
            set { m_ButtonCellClick = value; }
        }

        public override CellType GetCellType()
        {
            return CellType.Buttons;
        }

        /// <summary>
        /// 一系列按钮
        /// </summary>
        private ButtonCell[] buttonCells;

        protected override void OnAwake()
        {
            base.OnAwake();
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
