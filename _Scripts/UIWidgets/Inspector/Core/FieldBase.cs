using UnityEngine;
using System.Collections;
using XFramework.Core;
using UnityEngine.UI;
using System;

namespace XFramework.UIWidgets
{
    public abstract class FieldBase : InspectorBehaviour
    {
        /// <summary>
        /// 标签
        /// </summary>
        [SerializeField]
        protected Text m_Label;

        public abstract void Bind(string name, Func<object> getter, Action<object> setter);
    }

    public class FieldBase<T> : FieldBase
    {
        public override string GetKey()
        {
            return typeof(T).ToString();
        }

        private PropertyWrapper m_PropertyWrapper;
        /// <summary>
        /// PropertyWrapper
        /// </summary>
        public PropertyWrapper PropertyWrapper
        {
            get { return m_PropertyWrapper; }
            set { m_PropertyWrapper = value; }
        }

        public override void Bind(string name, Func<object> getter, Action<object> setter)
        {
            if (m_Label != null)
            {
                m_Label.text = name;
            }

            m_PropertyWrapper = new PropertyWrapper(getter, setter);
            UpdateField((T)m_PropertyWrapper.Value);
        }


        /// <summary>
        /// 更新 Field
        /// </summary>
        /// <param name="value"></param>
        protected virtual void UpdateField(T value)
        {
            
        }

        public override void Unbind()
        {
            base.Unbind();
            m_PropertyWrapper = null;
        }
    }
}

