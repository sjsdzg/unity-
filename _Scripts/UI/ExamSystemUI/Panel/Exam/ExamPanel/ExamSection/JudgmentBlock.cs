using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 判断题块
    /// </summary>
    public class JudgmentBlock : QuestionBlock
    {
        public override int QType
        {
            get { return 3; }
        }

        private string key = "";
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                if (toggleRight.isOn)
                {
                    key = "Y";
                }

                if (toggleWrong.isOn)
                {
                    key = "N";
                }

                return key;
            }
            set
            {
                key = value;
                if (key.Equals("Y"))
                {
                    toggleRight.isOn = true;
                }
                else if (key.Equals("N"))
                {
                    toggleWrong.isOn = true;
                }
            }
        }

        /// <summary>
        /// 正确
        /// </summary>
        private Toggle toggleRight;

        /// <summary>
        /// 错误
        /// </summary>
        private Toggle toggleWrong;

        public override void OnAwake()
        {
            base.OnAwake();
            toggleRight = transform.Find("Right/Toggle").GetComponent<Toggle>();
            toggleWrong = transform.Find("Wrong/Toggle").GetComponent<Toggle>();
            //事件
            toggleRight.onValueChanged.AddListener(toggle_onValueChanged);
            toggleWrong.onValueChanged.AddListener(toggle_onValueChanged);
        }

        private void toggle_onValueChanged(bool value)
        {
            if (value)
            {
                OnCompleted.Invoke(true);
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
