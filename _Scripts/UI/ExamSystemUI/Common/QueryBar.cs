using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 搜索栏目
    /// </summary>
    public abstract class QueryBar : MonoBehaviour
    {
        private UnityEvent m_OnQuery = new UnityEvent();
        /// <summary>
        /// 查询时，触发
        /// </summary>
        public UnityEvent OnQuery
        {
            get { return m_OnQuery; }
            set { m_OnQuery = value; }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        private Button buttonQuery;

        /// <summary>
        /// 获取Sql条件列表
        /// </summary>
        public List<SqlCondition> SqlConditions
        {
            get
            {
                return BuildSqlConditions();
            }
        }

        void Awake()
        {
            buttonQuery = transform.Find("ButtonQuery").GetComponent<Button>();
            buttonQuery.onClick.AddListener(buttonQuery_onClick);
            OnAwake();
        }

        public virtual void OnAwake()
        {

        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <returns></returns>
        protected abstract List<SqlCondition> BuildSqlConditions();

        /// <summary>
        /// 查询按钮点击时，触发
        /// </summary>
        protected void buttonQuery_onClick()
        {
            OnQuery.Invoke();
        }
    }
}
