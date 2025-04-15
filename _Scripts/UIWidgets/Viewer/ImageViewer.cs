using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace XFramework.UIWidgets
{
    public class ImageViewer : BaseViewer
    {
        //protected override void Start()
        //{
        //    base.Start();
        //    Show(content.GetComponent<Image>().sprite);
        //}

        public void Show(Texture2D texture)
        {
            gameObject.SetActive(true);

            content.anchoredPosition = Vector2.zero;
            var rawImage = content.GetComponent<RawImage>();
            rawImage.texture = texture;
            rawImage.SetNativeSize();
            // radio
            float widthRadio = viewport.rect.width / (float)texture.width;
            float heightRadio = viewport.rect.height / (float)texture.height;
            // scale
            Scale = widthRadio <= heightRadio ? widthRadio : heightRadio;
            content.localScale = new Vector3(Scale, Scale, 1);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public override void OnScroll(PointerEventData data)
        {
            if (!IsActive())
                return;

            RectTransformUtils.ScreenPointToAdjustPivot(content, viewport, Input.mousePosition, data.enterEventCamera);
            // 缩放
            Vector2 delta = data.scrollDelta;
            if (delta.y > 0)
                ZoomIn();
            else
                ZoomOut();
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_Velocity = Vector2.zero;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            UpdateBounds();

            m_PointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor);
            m_ContentStartPosition = m_Content.anchoredPosition;
            m_Dragging = true;
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_Dragging = false;
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            if (!IsActive())
                return;

            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            UpdateBounds();

            var pointerDelta = localCursor - m_PointerStartLocalCursor;
            Vector2 position = m_ContentStartPosition + pointerDelta;

            // Offset to get content into place in the view.
            Vector2 offset = CalculateOffset(position - m_Content.anchoredPosition);
            position += offset;
            if (m_MovementType == MovementType.Elastic)
            {
                if (offset.x != 0)
                    position.x = position.x - RubberDelta(offset.x, m_ViewBounds.size.x);
                if (offset.y != 0)
                    position.y = position.y - RubberDelta(offset.y, m_ViewBounds.size.y);
            }

            SetContentAnchoredPosition(position);
        }
    }

}
