using UnityEngine;
using System.Collections;
using XFramework.Common;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class ProcessParamItem : ListViewItemBase<ProcessParamItemData>
    {
        /// <summary>
        /// 标准
        /// </summary>
        [SerializeField]
        private Text m_TextStandard;

        /// <summary>
        /// 用户
        /// </summary>
        [SerializeField]
        private Text m_TextUser;

        /// <summary>
        /// 得分
        /// </summary>
        [SerializeField]
        private Text m_TextScore;

        public override void SetData(ProcessParamItemData data)
        {
            base.SetData(data);
            m_TextStandard.text = data.Standard;
            m_TextUser.text = data.User;
            m_TextScore.text = data.Score;
            // color
            SetItemColor();
        }

        public void SetItemColor()
        {
            if (Data.Score.Equals("0"))
            {
                m_TextStandard.color = new Color32(255, 160, 160, 255);
                m_TextUser.color = new Color32(255, 160, 160, 255);
                m_TextScore.color = new Color32(255, 160, 160, 255);
            }
            else
            {
                m_TextStandard.color = new Color32(255, 255, 255, 255);
                m_TextUser.color = new Color32(255, 255, 255, 255);
                m_TextScore.color = new Color32(255, 255, 255, 255);
            }
        }
    }
}

