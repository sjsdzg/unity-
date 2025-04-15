using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ContentSizeFitterExtension : ContentSizeFitter
    {
        public class SizeChangedEvent : UnityEvent<Vector2> { }

        private SizeChangedEvent m_OnSizeChanged = new SizeChangedEvent();
        /// <summary>
        /// 尺寸更改时触发
        /// </summary>
        public SizeChangedEvent OnSizeChanged
        {
            get { return m_OnSizeChanged; }
            set { m_OnSizeChanged = value; }
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            OnSizeChanged.Invoke(transform.GetComponent<RectTransform>().sizeDelta);
        }
    }
}
