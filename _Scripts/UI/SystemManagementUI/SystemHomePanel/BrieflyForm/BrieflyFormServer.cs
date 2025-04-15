using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class BrieflyFormServer : MonoBehaviour
    {
        private Text m_TextName;
        private Text m_TextTime;
        private Text m_TextEnvironment;
        private Text m_TextOperationSystem;
        private Text m_TextDatabase;

        private void Awake()
        {
            m_TextName = transform.Find("Panel/List/Name/Value/Text").GetComponent<Text>();
            m_TextTime = transform.Find("Panel/List/Time/Value/Text").GetComponent<Text>();
            m_TextEnvironment = transform.Find("Panel/List/Environment/Value/Text").GetComponent<Text>();
            m_TextOperationSystem = transform.Find("Panel/List/OperationSystem/Value/Text").GetComponent<Text>();
            m_TextDatabase = transform.Find("Panel/List/Database/Value/Text").GetComponent<Text>();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="profile"></param>
        public void SetData(ServerProfile profile)
        {
            m_TextName.text = profile.Name;
            m_TextTime.text = profile.Time;
            m_TextEnvironment.text = profile.Environment;
            m_TextOperationSystem.text = profile.OperationSystem;
            m_TextDatabase.text = profile.Database;
        }
    }
}

