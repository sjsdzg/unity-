using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    public class ProcessGraphPreview : MonoBehaviour
    {
        private UniEvent<Texture2D> m_OnImageView = new UniEvent<Texture2D>();
        /// <summary>
        /// 查看大图
        /// </summary>
        public UniEvent<Texture2D> OnImageView
        {
            get { return m_OnImageView; }
            set { m_OnImageView = value; }
        }

        /// <summary>
        /// 标准 按钮 
        /// </summary>
        private Button buttonStandard;

        /// <summary>
        /// 用户 按钮 
        /// </summary>
        private Button buttonUser;

        /// <summary>
        /// 标准 Image
        /// </summary>
        private RawImage m_RawImageStandard;

        /// <summary>
        /// 用户 Image
        /// </summary>
        private RawImage m_RawImageUser;

        /// <summary>
        /// 得分
        /// </summary>
        private Text m_TextScore;

        /// <summary>
        /// 获取标准贴图
        /// </summary>
        public Texture2D StandardGraph
        {
            get { return m_RawImageStandard.texture as Texture2D; }
        }

        /// <summary>
        /// 获取标准贴图
        /// </summary>
        public Texture2D UserGraph
        {
            get { return m_RawImageUser.texture as Texture2D; }
        }

        private void Awake()
        {
            buttonStandard = transform.Find("Standard/ButtonStandard").GetComponent<Button>();
            buttonUser = transform.Find("User/ButtonUser").GetComponent<Button>();
            m_RawImageStandard = transform.Find("Standard/ButtonStandard/Mask/RawImage").GetComponent<RawImage>();
            m_RawImageUser = transform.Find("User/ButtonUser/Mask/RawImage").GetComponent<RawImage>();
            m_TextScore = transform.Find("Score/Text").GetComponent<Text>();

            // Event
            buttonStandard.onClick.AddListener(buttonStandard_onClick);
            buttonUser.onClick.AddListener(buttonUser_onClick);
        }


        public void Init(Texture2D userGraph, float score)
        {
            m_RawImageUser.texture = userGraph;
            m_TextScore.text = score.ToString();
        }

        private void buttonStandard_onClick()
        {
            OnImageView.Invoke(StandardGraph);
        }

        private void buttonUser_onClick()
        {
            OnImageView.Invoke(UserGraph);
        }
    }
}

