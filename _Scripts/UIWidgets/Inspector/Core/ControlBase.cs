using UnityEngine;
using System.Collections;
using XFramework.Core;
using System;

namespace XFramework.UIWidgets
{
    public abstract class ControlBase : InspectorBehaviour
    {
        private PropertyWrapper m_PropertyWrapper;
        /// <summary>
        /// PropertyWrapper
        /// </summary>
        public PropertyWrapper PropertyWrapper
        {
            get { return m_PropertyWrapper; }
            set { m_PropertyWrapper = value; }
        }

        public virtual void Bind(Func<object> getter, Action<object> setter)
        {
            m_PropertyWrapper = new PropertyWrapper(getter, setter);
            UpdateControl(m_PropertyWrapper.Value);
        }

        /// <summary>
        /// 更新 Field
        /// </summary>
        /// <param name="value"></param>
        protected virtual void UpdateControl(object value)
        {

        }

        public override void Unbind()
        {
            base.Unbind();
            m_PropertyWrapper = null;
        }
    }
}

