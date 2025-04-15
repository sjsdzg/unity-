using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class JudgmentOptionBar : MonoBehaviour
    {
        private string key = "Y";
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

        /// <summary>
        /// 警告提示文本
        /// </summary>
        private Text warning;

        void Awake()
        {
            toggleRight = transform.Find("Data/ToggleGroup/ToggleRight").GetComponent<Toggle>();
            toggleWrong = transform.Find("Data/ToggleGroup/ToggleWrong").GetComponent<Toggle>();
        }
    }
}
