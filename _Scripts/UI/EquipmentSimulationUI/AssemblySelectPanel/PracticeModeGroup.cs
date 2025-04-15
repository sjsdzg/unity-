using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Core;

namespace XFramework.UI
{
    public class PracticeModeGroup : MonoBehaviour
    {
        private UniEvent<PracticeMode> m_OnSelected = new UniEvent<PracticeMode>();
        /// <summary>
        /// 选中事件
        /// </summary>
        public UniEvent<PracticeMode> OnSelected
        {
            get { return m_OnSelected; }
            set { m_OnSelected = value; }
        }

        /// <summary>
        /// 简单模式Toggle
        /// </summary>
        private Toggle ToggleEasy;

        /// <summary>
        /// 困难模式Toggle
        /// </summary>
        private Toggle ToggleHard;

        private void Awake()
        {
            ToggleEasy = transform.Find("Content/ToggleEasy").GetComponent<Toggle>();
            ToggleHard = transform.Find("Content/ToggleHard").GetComponent<Toggle>();
            //Event
            ToggleEasy.onValueChanged.AddListener(toggleEasy_onValueChanged);
            ToggleHard.onValueChanged.AddListener(toggleHard_onValueChanged);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleEasy_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(PracticeMode.Easy);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg0"></param>
        private void toggleHard_onValueChanged(bool arg0)
        {
            if (arg0)
            {
                OnSelected.Invoke(PracticeMode.Hard);
            }
        }
    }
}

