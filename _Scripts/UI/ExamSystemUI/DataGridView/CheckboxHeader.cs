using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// IResizableItem.
    /// </summary>
    public interface ICheckboxItem
    {
        /// <summary>
        /// Gets the objects to Checkbox.
        /// </summary>
        /// <value>The objects to Checkbox.</value>
        GameObject ObjectToCheckbox
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class CheckboxHeader : MonoBehaviour
    {
        /// <summary>
        /// 选中框单元
        /// </summary>
        private CheckboxHeaderCell checkboxHeaderCell;
        /// <summary>
        /// ListView instance.
        /// </summary>
        [SerializeField]
        public ListViewBase listViewBase;

        private void Awake()
        {
            Initialize();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialize()
        {
            checkboxHeaderCell = transform.GetComponentInChildren<CheckboxHeaderCell>();
            checkboxHeaderCell.CheckedChanged.AddListener(checkboxHeaderCell_CheckedChanged);
        }

        private void checkboxHeaderCell_CheckedChanged(bool arg0)
        {
            ChangeCheckedState(arg0);
        }

        /// <summary>
        /// Check items in ListView.
        /// </summary>
        public void ChangeCheckedState(bool _checked)
        {
            checkboxHeaderCell.Checked = _checked;
            if (listViewBase == null)
            {
                return;
            }
            listViewBase.ForEachComponent(x => CheckComponent(x, _checked));
        }

        public void CheckComponent(ListViewItem component, bool _checked)
        {
            if (component.IsActive())
            {
                var checkbox_item = component as ICheckboxItem;
                if (checkbox_item != null && checkbox_item.ObjectToCheckbox != null)
                {
                    DataGridViewCheckBoxCell checkBoxCell = checkbox_item.ObjectToCheckbox.GetComponent<DataGridViewCheckBoxCell>();

                    if (checkBoxCell != null)
                    {
                        checkBoxCell.SetValue(_checked);
                    }
                }
            }
        }

        /// <summary>
        /// 更改选中列头的选中状态, 不触发事件
        /// </summary>
        public void ChangeCheckedStateNotCause(bool _checked)
        {
            checkboxHeaderCell.IsTrigger = false;
            checkboxHeaderCell.Checked = _checked;

            checkboxHeaderCell.IsTrigger = true;
        }
    }
}
