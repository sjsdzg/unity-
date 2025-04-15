using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 显示在自定义网格的数据。
    /// </summary>
    public class DataGridView : ListViewCustom<DataGridViewRow, DataGridViewRowData>
    {
        public class ButtonCellClickEvent : UnityEvent<DataGridViewRow, ButtonCellType> { }

        private ButtonCellClickEvent m_ButtonCellClick = new ButtonCellClickEvent();
        /// <summary>
        /// 按钮单元进行点击
        /// </summary>
        public ButtonCellClickEvent ButtonCellClick
        {
            get { return m_ButtonCellClick; }
            set { m_ButtonCellClick = value; }
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

        public class SwitchChangedEvent : UnityEvent<DataGridViewRow, bool> { }

        private SwitchChangedEvent m_OnSwitchChanged = new SwitchChangedEvent();
        /// <summary>
        /// 选中状态改变时，触发
        /// </summary>
        public SwitchChangedEvent OnSwitchChanged
        {
            get { return m_OnSwitchChanged; }
            set { m_OnSwitchChanged = value; }
        }

        /// <summary>
        /// 选中列头
        /// </summary>
        private CheckboxHeader CheckboxHeader;

        /// <summary>
        /// 表格Text表头
        /// </summary>
        private TextHeader m_TextHeader;

        protected override void Awake()
        {
            base.Awake();
            CheckboxHeader = transform.GetComponentInChildren<CheckboxHeader>();
            m_TextHeader = transform.GetComponentInChildren<TextHeader>();
        }

        /// <summary>
        /// 设置所有组件选中状态
        /// </summary>
        /// <param name="_checked"></param>
        public void ChangeCheckedState(bool _checked)
        {
            if (CheckboxHeader != null)
            {
                CheckboxHeader.ChangeCheckedState(_checked);
            }
        }

        /// <summary>
        /// 更改选中列头的选中状态, 不触发事件
        /// </summary>
        public void ChangeCheckedStateNotCause(bool _checked)
        {
            if (CheckboxHeader != null)
            {
                CheckboxHeader.ChangeCheckedStateNotCause(_checked);
            }
        }

        /// <summary>
        /// 设置组件选中状态
        /// </summary>
        /// <param name="component"></param>
        /// <param name="_checked"></param>
        public void CheckedComponent(ListViewItem component, bool _checked)
        {
            var checkbox_item = component as ICheckboxItem;
            if (checkbox_item != null && checkbox_item.ObjectToCheckbox != null)
            {
                checkbox_item.ObjectToCheckbox.GetComponent<DataGridViewCheckBoxCell>().SetValue(_checked);
            }
        }

        protected override void SetData(DataGridViewRow component, DataGridViewRowData data)
        {
            component.SetData(data);
            component.ButtonCellClick.RemoveAllListeners();
            component.ButtonCellClick.AddListener((x, y) => ButtonCellClick.Invoke(x, y));

            component.CheckedChanged.RemoveAllListeners();
            component.CheckedChanged.AddListener(x => CheckedChanged.Invoke(x));

            component.OnSwitchChanged.RemoveAllListeners();
            component.OnSwitchChanged.AddListener((x, y) => OnSwitchChanged.Invoke(x, y));
        }

        protected override void HighlightColoring(DataGridViewRow component)
        {
            component.SetCellsColor(HighlightedColor);
        }

        protected override void SelectColoring(DataGridViewRow component)
        {
            component.SetCellsColor(SelectedColor);
        }

        protected override void DefaultColoring(DataGridViewRow component)
        {
            component.SetCellsColor(DefaultColor);
        }

        /// <summary>
        /// 获取所有选中的行
        /// </summary>
        /// <returns></returns>
        public List<DataGridViewRow> GetRowsByChecked()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            this.ForEachComponent(component =>
            {
                if (component.IsActive())
                {
                    var checkbox_item = component as ICheckboxItem;
                    if (checkbox_item != null && checkbox_item.ObjectToCheckbox != null)
                    {
                        DataGridViewCheckBoxCell checkBoxCell = checkbox_item.ObjectToCheckbox.GetComponent<DataGridViewCheckBoxCell>();

                        if (checkBoxCell != null)
                        {
                            bool _checked = (bool)checkBoxCell.GetValue();

                            if (_checked)
                            {
                                rows.Add(checkBoxCell.OwningRow);
                            }
                        }
                    }
                }

            });

            return rows;
        }

        /// <summary>
        /// 设置表头文本
        /// </summary>
        /// <param name="cellTextDict"></param>
        public void SetHeaderCellText(Dictionary<string, string> cellTextDict)
        {
            m_TextHeader.SetData(cellTextDict);
        }
    }
}
