using UnityEngine;
using System.Collections;

namespace XFramework.UI
{
    public class StatusItemComponent<T> : MonoBehaviour
    {
        private T m_Value;
        /// <summary>
        /// 变量
        /// </summary>
        public T Value
        {
            get { return m_Value; }
            set
            {
                m_Value = value;
                OnValueChanged();
            }
        }

        protected virtual void OnValueChanged()
        {

        }
    }
}

