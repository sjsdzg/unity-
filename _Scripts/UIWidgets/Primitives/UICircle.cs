using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    public class UICircle : UIPrimitiveBase
    {
        /// <summary>
        /// 分割数
        /// </summary>
        [SerializeField]
        private int segmentAmount = 30;

        [SerializeField]
        private float m_StrokeWidth = 1;
        /// <summary>
        /// 轮廓宽度
        /// </summary>
        public float StrokeWidth
        {
            get { return m_StrokeWidth; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref m_StrokeWidth, value))
                {
                    m_StrokeWidth = value;
                    SetVerticesDirty();
                }
            }
        }

        [SerializeField]
        private Color m_StrokeColor = Color.black;
        /// <summary>
        /// 轮廓颜色
        /// </summary>
        public Color StrokeColor
        {
            get { return m_StrokeColor; }
            set
            {
                if (SetPropertyUtils.SetColor(ref m_StrokeColor, value))
                {
                    m_StrokeColor = value;
                    SetVerticesDirty();
                }
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            base.OnPopulateMesh(vh);
            vh.Clear();

            var rect = GetPixelAdjustedRect(); 
            float inner = (rect.width - StrokeWidth) * 0.5f;
            float outer = (rect.width + StrokeWidth) * 0.5f;

            int startIndex = vh.currentVertCount;

            float delta = Mathf.PI * 2 / segmentAmount;
            Vector2 pos = Vector2.zero;
            Vector2 uv0 = new Vector2(0.5f, 0.5f);
            vh.AddVert(pos, color, uv0);

            float angle = 2 * Mathf.PI;
            // inner
            pos = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            uv0 = new Vector2(Mathf.Cos(angle) + 1, Mathf.Sin(angle) + 1) * 0.5f;
            vh.AddVert(pos, color, uv0);

            for (int i = 0; i < segmentAmount; i++)
            {
                angle -= delta;
                // inner
                pos = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                uv0 = new Vector2(Mathf.Cos(angle) + 1, Mathf.Sin(angle) + 1) * 0.5f;
                vh.AddVert(pos, color, uv0);
                vh.AddTriangle(startIndex, startIndex + i + 1, startIndex + i + 2);
            }

            // 轮廓
            angle = 2 * Mathf.PI;
            UIVertex[] UIVerts = new UIVertex[4];
            // 0
            UIVerts[0].position = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            UIVerts[0].color = StrokeColor;
            UIVerts[0].uv0 = new Vector2(0, 0);
            // 1
            UIVerts[1].position = outer * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            UIVerts[1].color = StrokeColor;
            UIVerts[1].uv0 = new Vector2(0, 1);

            for (int i = 0; i < segmentAmount; i++)
            {
                angle -= delta;
                // 2
                UIVerts[2].position = outer * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                UIVerts[2].color = StrokeColor;
                UIVerts[2].uv0 = new Vector2(1, 1);
                // 3
                UIVerts[3].position = inner * new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                UIVerts[3].color = StrokeColor;
                UIVerts[3].uv0 = new Vector2(1, 0);
                vh.AddUIVertexQuad(UIVerts);

                UIVerts[0].position = UIVerts[3].position;
                UIVerts[1].position = UIVerts[2].position;
            }
        }
    }
}

