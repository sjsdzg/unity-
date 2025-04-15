using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace XFramework.UIWidgets
{
    public class BoolField : FieldBase<bool>
    {
        [SerializeField]
        private Toggle m_Toggle;

        protected override void Awake()
        {
            base.Awake();
            m_Toggle.onValueChanged.AddListener(m_Toggle_onValueChanged);
        }

        protected override void UpdateField(bool value)
        {
            base.UpdateField(value);
            m_Toggle.isOn = value;
        }

        private void m_Toggle_onValueChanged(bool value)
        {
            PropertyWrapper.Value = value;
        }
    }
}
