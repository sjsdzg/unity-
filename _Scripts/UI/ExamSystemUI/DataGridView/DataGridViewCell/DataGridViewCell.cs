using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.UI
{
    /// <summary>
    /// 表示 DataGridView 控件中的列。
    /// </summary>
    public abstract class DataGridViewCell : MonoBehaviour
    {
        /// <summary>
        /// 单元类型
        /// </summary>
        /// <returns></returns>
        public abstract CellType GetCellType();

        /// <summary>
        /// 拥有行
        /// </summary>
        public DataGridViewRow OwningRow { get; private set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; protected set; }

        void Awake()
        {
            OwningRow = transform.parent.GetComponent<DataGridViewRow>();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
           
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="data"></param>
        public virtual void SetValue(object data)
        {

        }

        public virtual object GetValue()
        {
            return null;
        }
    }

    /// <summary>
    /// 列类型
    /// </summary>
    public enum CellType
    {
        /// <summary>
        /// 文本类型
        /// </summary>
        Text,
        /// <summary>
        /// 复选框类型
        /// </summary>
        CheckBox,
        /// <summary>
        /// 一系列按钮类型(包含单个情况)
        /// </summary>
        Buttons,
        /// <summary>
        /// 切换按钮
        /// </summary>
        Switch,
    }
}
