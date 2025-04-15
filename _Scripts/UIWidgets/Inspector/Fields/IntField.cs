using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace XFramework.UIWidgets
{
    public class IntField : FieldBase<int>
    {
        [SerializeField]
        protected InputField m_InputField;

        protected override void Awake()
        {
            base.Awake();
            m_InputField.onValueChanged.AddListener(m_InputField_onValueChanged);
            m_InputField.onEndEdit.AddListener(m_InputField_onEndEdit);
        }

        protected override void UpdateField(int value)
        {
            base.UpdateField(value);
            m_InputField.text = value.ToString();
        }

        private void m_InputField_onValueChanged(string value)
        {
            if (int.TryParse(value, out int val))
            {
                PropertyWrapper.Value = val;
            }
        }

        private void m_InputField_onEndEdit(string arg0)
        {
            m_InputField.text = PropertyWrapper.Value.ToString();
        }

    }
}
