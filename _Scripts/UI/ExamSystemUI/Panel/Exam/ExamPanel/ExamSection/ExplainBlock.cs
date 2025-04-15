using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 名词解释块
    /// </summary>
    public class ExplainBlock : QuestionBlock
    {
        public override int QType
        {
            get { return 5; }
        }

        private string key;
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                key = inputField.text;
                return key;
            }
            set
            {
                key = value;
                inputField.text = key;
            }
        }

        /// <summary>
        /// 答案InputField
        /// </summary>
        private InputField inputField;

        public override void OnAwake()
        {
            base.OnAwake();
            inputField = transform.Find("InputField").GetComponent<InputField>();
            inputField.onEndEdit.AddListener(inputField_onEndEdit);
        }

        private void inputField_onEndEdit(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                OnCompleted.Invoke(true);
            }
            else
            {
                OnCompleted.Invoke(false);
            }
        }

        public override void SetParams(object value)
        {

        }

        public override string GetKey()
        {
            return Key;
        }

        public override void SetKey(string _key)
        {
            Key = _key;
        }
    }
}
