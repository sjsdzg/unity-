using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.UIWidgets
{
    public static class RectTransformUtils
    {
        /// <summary>
        /// 根据屏幕上点调整轴
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="screenPoint"></param>
        /// <param name="cam"></param>
        public static void ScreenPointToAdjustPivot(RectTransform content, RectTransform viewport, Vector2 screenPoint, Camera cam)
        {
            if (viewport.rect.width < content.rect.width * content.localScale.x && viewport.rect.height < content.rect.height * content.localScale.y)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(content, Input.mousePosition, cam, out Vector2 localPoint);
                //var pivot = content.pivot + localPoint / new Vector2(content.rect.width, content.rect.height);
                var pivot = (localPoint - content.rect.position) / content.rect.size;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(viewport, Input.mousePosition, cam, out localPoint);
                content.pivot = pivot;
                content.localPosition = localPoint;
            }
            else
            {
                content.pivot = new Vector2(0.5f, 0.5f);
                content.anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        /// 扩展边框
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public static Rect Expand(Rect rect, Vector2 delta)
        {
            return new Rect(rect.position - delta * 0.5f, rect.size + delta);
        }

        public static void SetParentAndAlign(GameObject child, GameObject parent)
        {
            if (parent == null)
                return;

            child.transform.SetParent(parent.transform, false);
            SetLayerRecursively(child, parent.layer);
        }

        public static void SetLayerRecursively(GameObject go, int layer)
        {
            go.layer = layer;
            Transform t = go.transform;
            for (int i = 0; i < t.childCount; i++)
            {
                SetLayerRecursively(t.GetChild(i).gameObject, layer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="anchor"></param>
        /// <param name="localPoint"></param>
        public static void AnchorToLocalPoint(RectTransform target, Vector2 anchor, out Vector3 localPoint)
        {
            var offset = new Vector3((anchor.x - target.pivot.x) * target.rect.width, (anchor.y - target.pivot.y) * target.rect.height, 0);
            localPoint = target.localPosition + offset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="anchor"></param>
        /// <param name="worldPoint"></param>
        public static void AnchorToWorldPoint(RectTransform target, Vector2 anchor, out Vector3 worldPoint)
        {
            AnchorToLocalPoint(target, anchor, out Vector3 localPoint);
            worldPoint = target.parent.TransformPoint(localPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="anchor"></param>
        /// <param name="cam"></param>
        /// <returns></returns>
        public static Vector2 AnchorToScreenPoint(Camera cam, RectTransform target, Vector2 anchor)
        {
            AnchorToWorldPoint(target, anchor, out Vector3 worldPoint);
            return RectTransformUtility.WorldToScreenPoint(cam, worldPoint);
        }
    }
}
