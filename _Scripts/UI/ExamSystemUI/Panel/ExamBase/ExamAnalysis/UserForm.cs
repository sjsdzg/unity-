using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class UserForm : MonoBehaviour
    {
        /// <summary>
        /// 及格人数
        /// </summary>
        private Text textPassNumber;
        /// <summary>
        /// 及格比率
        /// </summary>
        private Text textPassRate;
        /// <summary>
        /// 应参加数
        /// </summary>
        private Text textShouldNumber;
        /// <summary>
        /// 实际人数
        /// </summary>
        private Text textAttendNumber;
        /// <summary>
        /// 参加比率
        /// </summary>
        private Text textAttendRate;
        /// <summary>
        /// 缺考人数
        /// </summary>
        private Text textAbsentNumber;

        private void Awake()
        {
            textPassNumber = transform.Find("List/PassNumber/Value/Text").GetComponent<Text>();
            textPassRate = transform.Find("List/PassRate/Value/Text").GetComponent<Text>();
            textShouldNumber = transform.Find("List/ShouldNumber/Value/Text").GetComponent<Text>();
            textAttendNumber = transform.Find("List/AttendNumber/Value/Text").GetComponent<Text>();
            textAttendRate = transform.Find("List/AttendRate/Value/Text").GetComponent<Text>();
            textAbsentNumber = transform.Find("List/AbsentNumber/Value/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="passNumber"></param>
        /// <param name="passRate"></param>
        /// <param name="shouldNumber"></param>
        /// <param name="attendNumber"></param>
        /// <param name="attendRate"></param>
        /// <param name="absentNumber"></param>
        public void SetValues(int passNumber, float passRate, int shouldNumber, int attendNumber, float attendRate, int absentNumber)
        {
            textPassNumber.text = passNumber.ToString();
            textPassRate.text = (passRate * 100).ToString("F2") + "%";
            textShouldNumber.text = shouldNumber.ToString();
            textAttendNumber.text = attendNumber.ToString();
            textAttendRate.text = (attendRate * 100).ToString("F2") + "%";
            textAbsentNumber.text = absentNumber.ToString();
        }
    }
}

