using System;
using UnityEngine.Events;

namespace XFramework.Common
{
    public class ObservationPointCompontent:HUDComponent<ObservationPointInfo>
    {
        public class OnFindEvent : UnityEvent<string> { }

        private OnFindEvent m_OnFind = new OnFindEvent();
        /// <summary>
        /// 发现观察点事件
        /// </summary>
        private ObservationPointView m_ObservationPointView;
        /// <summary>
        /// 观察点视图
        /// </summary>
        public OnFindEvent OnFind
        {
            get { return m_OnFind; }
            set { m_OnFind = value; }
        }
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
                m_ObservationPointView = View as ObservationPointView;
                m_ObservationPointView.OnClicked.AddListener(m_ObservationPointView_OnClickEvent);
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
        private void m_ObservationPointView_OnClickEvent()
        {
            OnFind.Invoke(name);
        }

    }
}
