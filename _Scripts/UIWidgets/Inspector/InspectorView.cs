using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    public class InspectorView : MonoBehaviour
    {
        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private Text m_Label;

        [SerializeField]
        private RectTransform m_Content;
        /// <summary>
        /// Content
        /// </summary>
        public RectTransform Content
        {
            get { return m_Content; }
            set { m_Content = value; }
        }

        [SerializeField]
        private RectTransform m_Unused;
        /// <summary>
        /// Unused
        /// </summary>
        public RectTransform Unused
        {
            get { return m_Unused; }
            set { m_Unused = value; }
        }

        [SerializeField]
        private InspectorSettings m_InspectorSettings;
        /// <summary>
        /// Inspector 设置
        /// </summary>
        public InspectorSettings Settings 
        {
            get 
            { 
                return m_InspectorSettings; 
            } 
        }

        private object m_Target;
        /// <summary>
        /// 目标
        /// </summary>
        public object Target
        {
            get { return m_Target; }
        }

        private EditorBase m_Editor;

        public void Inspect(object obj, string title = "属性")
        {
            if (obj == m_Target)
                return;

            m_Target = obj;
            m_Label.text = title;

            DestroyEditor();
            CreateEditor();
        }

        /// <summary>
        /// 创建编辑器
        /// </summary>
        private void CreateEditor()
        {
            if (m_Target == null)
                return;

            m_Editor = InspectorManager.Instance.CreateEditor(m_Target.GetType());
            RectTransformUtils.SetParentAndAlign(m_Editor.gameObject, m_Content.gameObject);
            m_Editor.Bind(m_Target);
        }

        /// <summary>
        /// 删除编辑
        /// </summary>
        private void DestroyEditor()
        {
            if (m_Editor != null)
            {
                m_Editor.Unbind();
                m_Editor = null;
            }
        }
    }
}
