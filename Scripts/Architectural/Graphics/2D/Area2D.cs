using UnityEngine;
using System.Collections;
using TMPro;
using XFramework.Math;
using XFramework.Runtime;

namespace XFramework.Architectural
{
    public class Area2D : ProceduralGraphic
    {
        private Area area;
        /// <summary>
        /// 区域
        /// </summary>
        public Area Area
        {
            get { return area; }
            set 
            {
                area = value;
            }
        }

        private TextMeshPro m_Text;
        /// <summary>
        /// 文本
        /// </summary>
        public TextMeshPro Text
        {
            get
            {
                if (m_Text == null)
                {
                    GameObject textPrefab = Resources.Load<GameObject>("Prefabs/Text (TMP)");
                    GameObject textGameObject = Instantiate<GameObject>(textPrefab, transform);
                    m_Text = textGameObject.GetComponent<TextMeshPro>();
                }
                return m_Text;
            }
        }

        protected override void UpdateName()
        {
            base.UpdateName();
            Text.color = new Color32(225, 255, 255, 100);
            Text.text = area.Name;
        }

        protected override void UpdateTransform()
        {
            base.UpdateTransform();
            Text.transform.position = area.GetVisualPoint();
        }

        protected override void OnPopulateMesh(MeshBuilder vh)
        {
            base.OnPopulateMesh(vh);
            vh.MeshTopology = MeshTopology.Lines;
            vh.AddArea2D(area, Color);
        }
    }

}
