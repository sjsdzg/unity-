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
    /// <summary>
    /// 表示 DataGridView 控件中的行。
    /// </summary>
    public class DataGridViewRow : ListViewItem, IResizableItem, ICheckboxItem
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
        /// 填充行的单元格集合。
        /// </summary>
        public DataGridViewCell[] Cells { get; private set; }

        public DataGridViewRowData Data { get; private set; }

        public GameObject[] ObjectsToResize
        {
            get
            {
                GameObject[] objects = new GameObject[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform item = transform.GetChild(i);
                    objects[i] = item.gameObject;
                }
                return objects;
            }
        }

        public GameObject ObjectToCheckbox
        {
            get
            {
                DataGridViewCheckBoxCell cell = transform.GetComponentInChildren<DataGridViewCheckBoxCell>();
                return cell.gameObject;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            Cells = transform.GetComponentsInChildren<DataGridViewCell>();

            foreach (var cell in Cells)
            {
                if (cell.GetCellType() == CellType.Buttons )
                {
                    DataGridViewButtonsCell buttonsCell = cell as DataGridViewButtonsCell;
                    buttonsCell.ButtonCellClick.RemoveAllListeners();
                    buttonsCell.ButtonCellClick.AddListener((x, y) => ButtonCellClick.Invoke(this, y));
                }

                if (cell.GetCellType() == CellType.CheckBox)
                {
                    DataGridViewCheckBoxCell checkBoxCell = cell as DataGridViewCheckBoxCell;
                    checkBoxCell.CheckedChanged.RemoveAllListeners();
                    checkBoxCell.CheckedChanged.AddListener(x => CheckedChanged.Invoke(x));
                }

                if (cell.GetCellType() == CellType.Switch)
                {
                    DataGridViewSwitchCell switchCell = cell as DataGridViewSwitchCell;
                    switchCell.OnValueChanged.RemoveAllListeners();
                    switchCell.OnValueChanged.AddListener(x => OnSwitchChanged.Invoke(this, x));
                }
            }
        }

        /// <summary>
        /// 设置行数据
        /// </summary>
        /// <param name="data"></param>
        public void SetData(DataGridViewRowData data)
        {
            Data = data;

            foreach (var key in data.CellValueDict.Keys)
            {
                DataGridViewCell cell = GetCell(key);

                if (cell != null)
                {
                    cell.SetValue(data.CellValueDict[key]);
                }
            }
        }

        public void SetData(string key, object value)
        {
            DataGridViewCell cell = GetCell(key);

            if (cell == null)
                return;

            cell.SetValue(value);
        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataGridViewCell GetCell(string _name)
        {
            DataGridViewCell cell = null;

            //Cells.ForEach(x =>
            //{
            //    if (x.name == name)
            //    {
            //        cell = x;
            //    }
            //});
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].name == _name)
                {
                    cell = Cells[i];
                    break;
                }
            }

            return cell;
        }

        /// <summary>
        /// 设置单元格颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetCellsColor(Color color)
        {
            Cells.ForEach(x =>
            {
                Image image = x.GetComponent<Image>();
                if (image != null)
                {
                    image.color = color;
                }
            });
        }
    }
}
