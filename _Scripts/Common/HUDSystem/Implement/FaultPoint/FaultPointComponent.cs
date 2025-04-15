using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace XFramework.Common
{
    public class FaultPointComponent : HUDComponent<FaultPointInfo>
    {
        public class OnFindEvent : UnityEvent<string> { }

        private OnFindEvent m_OnFind = new OnFindEvent();
        /// <summary>
        /// 发现故障事件
        /// </summary>
        public OnFindEvent OnFind
        {
            get { return m_OnFind; }
            set { m_OnFind = value; }
        }

        /// <summary>
        /// 故障点视图
        /// </summary>
        private FaultPointView m_FaultPointView;

        public override void show()
        {
            if (HasCreated)
            {
                hudInfo.onScreenArgs.visible = true;
                hudInfo.offScreenArgs.visible = true;
            }
            else
            {
                CreateHUD();
                m_FaultPointView = View as FaultPointView;
                m_FaultPointView.OnClicked.AddListener(m_FaultPointView_OnClicked);
            }

        }

        public override void hide()
        {
            if (HasCreated)
            {
                hudInfo.onScreenArgs.visible = false;
                hudInfo.offScreenArgs.visible = false;
            }
        }

        /// <summary>
        /// 故障点发现事件
        /// </summary>
        private void m_FaultPointView_OnClicked()
        {
            Debug.Log(name);
            OnFind.Invoke(name);
        }
    }
}
