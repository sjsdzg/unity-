using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class AssessmentItem : MonoBehaviour
    {
        /// <summary>
        /// 考核点描述
        /// </summary>
        private Text Desc;

        /// <summary>
        /// 考核点分值
        /// </summary>
        private Text Value;

        /// <summary>
        /// 考核点得分
        /// </summary>
        private Text Score;

        void Awake()
        {
            Desc = transform.Find("Point/Text").GetComponent<Text>();
            Value = transform.Find("Value/Text").GetComponent<Text>();
            Score = transform.Find("Score/Text").GetComponent<Text>();
        }

        public void SetValue(AssessmentPoint point)
        {
            Desc.text = point.Id + "." + point.Desc;
            Value.text = point.Value.ToString();
            Score.text = point.Score.ToString();
        }

        public void SetValue(string desc, string value, string score)
        {
            Desc.text = desc;
            Value.text = value;
            Score.text = score;
        }
    }
}
