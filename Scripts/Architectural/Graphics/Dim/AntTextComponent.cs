using UnityEngine;
using System.Collections;
using TMPro;

namespace XFramework.Architectural
{
    public class AntTextComponent : ProceduralGraphic
    {
        private bool m_ContentDirty;

        private AText antText;
        /// <summary>
        /// 文本标注
        /// </summary>
        public AText AntText
        {
            get { return antText; }
            set 
            {
                antText = value;
                antText.TransformChanged += AntText_TransformChanged;
                antText.TextChanged += AntText_ContentChanged;
            }
        }

        private TextMeshPro m_TMP;
        /// <summary>
        /// 显示文本
        /// </summary>
        public TextMeshPro TMP
        {
            get
            {
                if (m_TMP == null)
                {
                    GameObject textPrefab = Resources.Load<GameObject>("Prefabs/Text (TMP)");
                    GameObject textGameObject = Instantiate<GameObject>(textPrefab, transform);
                    m_TMP = textGameObject.GetComponent<TextMeshPro>();
                }
                return m_TMP;
            }
        }

        private void AntText_TransformChanged()
        {
            SetTranformDirty();
        }

        private void AntText_ContentChanged()
        {
            SetContentDirty();
        }

        public virtual void SetContentDirty()
        {
            m_ContentDirty = true;
            GraphicManager.RegisterGraphicForRebuild(this);
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            TMP.transform.position = AntText.Position;
        }

        protected virtual void UpdateContent()
        {
            TMP.text = AntText.Text;
        }

        public override void Rebuild()
        {
            base.Rebuild();
            if (m_ContentDirty)
            {
                UpdateContent();
                m_ContentDirty = false;
            }
        }
    }
}


