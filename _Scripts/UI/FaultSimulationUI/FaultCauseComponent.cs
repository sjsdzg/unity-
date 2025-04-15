using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class FaultCauseComponent : MonoBehaviour
    {
        /// <summary>
        /// 序号文本
        /// </summary>
        private Text textNumber;

        /// <summary>
        /// 信息文本
        /// </summary>
        private Text textInfo;

        /// <summary>
        /// 绑定数据
        /// </summary>
        public FaultCause Data { get; private set; }

        private void Awake()
        {
            textNumber = transform.Find("Number/Text").GetComponent<Text>();
            textInfo = transform.Find("Text").GetComponent<Text>();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="data"></param>
        public void SetValue(FaultCause data)
        {
            Data = data;
            textInfo.text = Data.Cause.ToString();
        }

        public void SetNumber(int number)
        {
            textNumber.text = number.ToString();
        }
    }
}

