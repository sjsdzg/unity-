using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    public class UIInputField : InputField
    {
        private UniEvent<PointerEventData> m_OnClicked = new UniEvent<PointerEventData>();
        /// <summary>
        /// 点击事件
        /// </summary>
        public UniEvent<PointerEventData> OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            Debug.Log("click");
            OnClicked.Invoke(eventData);
        }
    }
}
