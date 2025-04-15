using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIWidgets;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 评分组件
    /// </summary>
    public class MarkScoreComponent : MonoBehaviour
    {
        private int score;
        /// <summary>
        /// 分值
        /// </summary>
        public int Score
        {
            get
            {
                score = spinnerInput.Value;
                return score;
            }
            set
            {
                score = value;
                spinnerInput.Value = score;
            }
        }

        private int status;
        /// <summary>
        /// 状态 0 错误 1 正确 2 未知
        /// </summary>
        public int Status
        {
            get { return status; }
            set
            {
                status = value;
                if (status == 0)
                {
                    imageStatus.sprite = m_ImageList["error"];
                }
                else if (status == 1)
                {
                    imageStatus.sprite = m_ImageList["success"];
                }
                else if (status == 2)
                {
                    imageStatus.sprite = m_ImageList["help"];
                }
            }
        }

        /// <summary>
        /// 图片状态
        /// </summary>
        private Image imageStatus;

        /// <summary>
        /// Spinner
        /// </summary>
        private Spinner spinnerInput;

        /// <summary>
        /// ImageList
        /// </summary>
        private ImageList m_ImageList;


        void Awake()
        {
            imageStatus = transform.Find("Status").GetComponent<Image>();
            spinnerInput = transform.Find("SpinnerInput").GetComponent<Spinner>();
            m_ImageList = transform.Find("ImageList").GetComponent<ImageList>();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void SetSpinner(int max)
        {
            spinnerInput.Min = 0;
            spinnerInput.Max = max;
        }
    }
}
