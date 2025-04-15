using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using XFramework.Simulation;

namespace XFramework.UI
{
    public class ModeGroup : MonoBehaviour
    {
        public class OnSelectedEvent : UnityEvent<ProductionMode> { }

        private OnSelectedEvent m_OnSelected = new OnSelectedEvent();
        /// <summary>
        /// 选中事件
        /// </summary>
        public OnSelectedEvent OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        private bool m_StudyActive;
        /// <summary>
        /// 学习模式是否显示
        /// </summary>
        public bool StudyActive
        {
            get { return m_StudyActive; }
            set
            {
                m_StudyActive = value;
                toggleStudy.gameObject.SetActive(m_StudyActive);
            }
        }

        private bool m_ExamActive;
        /// <summary>
        /// 考试模式是否显示
        /// </summary>
        public bool ExamActive
        {
            get { return m_ExamActive; }
            set
            {
                m_ExamActive = value;
                toggleExam.gameObject.SetActive(m_ExamActive);
            }
        }

        /// <summary>
        /// 学习模式Toggle
        /// </summary>
        public Toggle toggleStudy;

        /// <summary>
        /// 考试模式Toggle
        /// </summary>
        public Toggle toggleExam;

        private void Awake()
        {
            toggleStudy = transform.Find("Content/ToggleStudy").GetComponent<Toggle>();
            toggleExam = transform.Find("Content/ToggleExam").GetComponent<Toggle>();
            //Event
            toggleStudy.onValueChanged.AddListener(toggleStudy_onValueChanged);
            toggleExam.onValueChanged.AddListener(toggleExam_onValueChanged);

            switch (GlobalManager.DefaultMode)
            {
                case ProductionMode.None:
                    break;
                case ProductionMode.Study:
                    break;
                case ProductionMode.Examine:
                    toggleExam.GetComponentInChildren<Text>().text = "考核模式";
                    break;
                case ProductionMode.Banditos:
                    toggleExam.GetComponentInChildren<Text>().text = "闯关模式";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleStudy_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(ProductionMode.Study);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleExam_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                switch (GlobalManager.DefaultMode)
                {
                    case ProductionMode.Examine:
                        OnSelected.Invoke(ProductionMode.Examine);
                        break;
                    case ProductionMode.Banditos:
                        OnSelected.Invoke(ProductionMode.Banditos);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flag"></param>
        public void SetToggleStatus(bool flag)
        {
            toggleStudy.isOn = flag;
            toggleExam.isOn = !flag;
        }
    }
}

