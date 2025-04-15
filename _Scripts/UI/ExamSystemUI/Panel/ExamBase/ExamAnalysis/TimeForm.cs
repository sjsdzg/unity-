using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class TimeForm : MonoBehaviour
    {
        /// <summary>
        /// 最早交卷
        /// </summary>
        private Text textEarliest;
        /// <summary>
        /// 最迟交卷
        /// </summary>
        private Text textLatest;
        /// <summary>
        /// 最长耗时
        /// </summary>
        private Text textLongest;
        /// <summary>
        /// 最短耗时
        /// </summary>
        private Text textShortest;

        private void Awake()
        {
            textEarliest = transform.Find("List/Earliest/Value/Text").GetComponent<Text>();
            textLatest = transform.Find("List/Latest/Value/Text").GetComponent<Text>();
            textLongest = transform.Find("List/Longest/Value/Text").GetComponent<Text>();
            textShortest = transform.Find("List/Shortest/Value/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="earliest"></param>
        /// <param name="latest"></param>
        /// <param name="longest"></param>
        /// <param name="shortest"></param>
        public void SetValues(string earliest, string latest, string longest, string shortest)
        {
            textEarliest.text = earliest;
            textLatest.text = latest;
            textLongest.text = longest;
            textShortest.text = shortest;
        }
    }
}

