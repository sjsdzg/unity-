using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Core;
using System;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class ViewSelectorItem : SelectorItem
    {
        [SerializeField]
        private Text m_Text;

        [SerializeField]
        private Color m_NormalColor;
        [SerializeField]
        private Color m_SelectColor;

        [SerializeField]
        private Color m_TextNormalColor;
        [SerializeField]
        private Color m_TextSelectColor;

        protected override void Awake()
        {
            base.Awake();
            Data = m_Text.text;
        }

        protected override void PlayEffect()
        {
            base.PlayEffect();
            if (IsSelected)
            {
                targetGraphic.color = m_SelectColor;
                m_Text.color = m_TextSelectColor;
            }
            else
            {
                targetGraphic.color = m_NormalColor;
                m_Text.color = m_TextNormalColor;
            }
        }
    }
}
