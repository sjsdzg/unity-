using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using XFramework.UIWidgets;
using XFramework.Core;
using UnityEngine.EventSystems;

namespace XFramework.Diagram
{
    public class GraphViewer : BaseViewer, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private RectTransform m_Container;
        /// <summary>
        /// Container
        /// </summary>
        public RectTransform Container
        { 
            get { return m_Container; } 
            set { m_Container = value; } 
        }

        [SerializeField]
        private RectTransform m_Assist;
        /// <summary>
        /// Assist
        /// </summary>
        public RectTransform Assist
        {
            get { return m_Assist; }
            set { m_Assist = value; }
        }

        [SerializeField]
        private RectTransform m_Page;
        /// <summary>
        /// 绘图页面
        /// </summary>
        public RectTransform Page
        {
            get { return m_Page; }
            set { m_Page = value; }
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

        private Canvas m_Canvas;
        /// <summary>
        /// 缓存 Canvas
        /// </summary>
        public Canvas Canvas
        {
            get 
            {
                if (m_Canvas == null)
                    CacheCanvas();

                return m_Canvas;
            }
        }

        /// <summary>
        /// 指针是否在视图内
        /// </summary>
        public bool IsPointerInside { get; set; }

        private void CacheCanvas()
        {
            var list = ListPool<Canvas>.Get();
            gameObject.GetComponentsInParent<Canvas>(false, list);
            if (list.Count > 0)
            {
                // Find the first active and enabled canvas.
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].isActiveAndEnabled)
                    {
                        m_Canvas = list[i];
                        break;
                    }
                }
            }
            else
                m_Canvas = null;
        }

        [SerializeField]
        private Font m_Font;
        /// <summary>
        /// 字体
        /// </summary>
        public Font Font
        {
            get { return m_Font; }
            set { m_Font = value; }
        }

        protected override void Awake()
        {
            base.Awake();

        }

        public override void SetScale(float value)
        {
            base.SetScale(value);
            GraphMaster.Instance.Scale = value;
        }

        protected override void Start()
        {
            base.Start();
            AdjustPagePosition();
        }

        /// <summary>
        /// 调整 Page 位置
        /// </summary>
        public void AdjustPagePosition()
        {
            RectTransformUtils.AnchorToLocalPoint(Page, new Vector2(0.5f, 1f), out Vector3 pageLocalPoint);
            RectTransformUtils.AnchorToWorldPoint(viewport, new Vector2(0.5f, 1f), out Vector3 viewportWorldPoint);
            Vector3 viewportLocalPoint = content.InverseTransformPoint(viewportWorldPoint);
            Vector3 offset = viewportLocalPoint - pageLocalPoint;
            content.localPosition += offset;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsPointerInside = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPointerInside = false;
        }

    }
}
