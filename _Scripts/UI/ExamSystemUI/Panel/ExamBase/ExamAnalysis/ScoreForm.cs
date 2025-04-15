using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ScoreForm : MonoBehaviour
    {
        /// <summary>
        /// 最高分
        /// </summary>
        private Text textMax;
        /// <summary>
        /// 最低分
        /// </summary>
        private Text textMin;
        /// <summary>
        /// 平均分
        /// </summary>
        private Text textAverage;

        private void Awake()
        {
            textMax = transform.Find("List/Max/Value/Text").GetComponent<Text>();
            textMin = transform.Find("List/Min/Value/Text").GetComponent<Text>();
            textAverage = transform.Find("List/Average/Value/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="max"></param>
        /// <param name="min"></param>
        /// <param name="average"></param>
        public void SetValues(string max, string min, string average)
        {
            textMax.text = max;
            textMin.text = min;
            textAverage.text = average;
        }
    }
}

