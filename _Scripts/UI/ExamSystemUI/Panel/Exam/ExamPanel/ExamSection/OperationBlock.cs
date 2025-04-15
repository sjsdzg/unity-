using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 简答题块
    /// </summary>
    public class OperationBlock : QuestionBlock
    {
        public override int QType
        {
            get { return 7; }
        }

        private string key = "";
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        /// <summary>
        /// 进入操作场景按钮
        /// </summary>
        private Button buttonEnter;

        public override void OnAwake()
        {
            base.OnAwake();
            buttonEnter = transform.Find("ButtonEnter").GetComponent<Button>();
            buttonEnter.onClick.AddListener(buttonEnter_onClick);
        }

        /// <summary>
        /// 进入场景按钮
        /// </summary>
        private void buttonEnter_onClick()
        {
            OnEnter.Invoke();
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
            if (string.IsNullOrEmpty(_key))
            {
                OnCompleted.Invoke(false);
            }            
            else
            {
                OnCompleted.Invoke(true);
            }
        }
    }
}
