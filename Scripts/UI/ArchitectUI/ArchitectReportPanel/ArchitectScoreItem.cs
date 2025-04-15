using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.Common;

namespace XFramework.UI
{
    public class ArchitectScoreItem : ListViewItemBase<ArchitectScoreItemData>
    {
        /// <summary>
        /// 标准
        /// </summary>
        [SerializeField]
        private Text m_TextName;

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

        public override void SetData(ArchitectScoreItemData data)
        {
            base.SetData(data);
            m_TextName.text = data.Name;
            m_TextUser.text = data.User;
            m_TextScore.text = data.Score;
        }
    }
}

