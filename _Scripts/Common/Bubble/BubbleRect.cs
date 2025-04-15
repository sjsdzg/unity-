using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 气泡
    /// </summary>
    [ExecuteInEditMode]
    public class BubbleRect : MonoBehaviour
    {
        /// <summary>
        /// 左RectTransform
        /// </summary>
        private RectTransform left;

        /// <summary>
        /// 中RectTransform
        /// </summary>
        private RectTransform middle;

        /// <summary>
        /// 右RectTransform
        /// </summary>
        private RectTransform right;

        /// <summary>
        /// 内容大小控制扩展
        /// </summary>
        private ContentSizeFitterExtension extension;

        /// <summary>
        /// 偏移
        /// </summary>
        public float offset;

        void Start()
        {
            left = transform.Find("Background/Left").GetComponent<RectTransform>();
            middle = transform.Find("Background/Middle").GetComponent<RectTransform>();
            right = transform.Find("Background/Right").GetComponent<RectTransform>();
            extension = transform.Find("Content").GetComponent<ContentSizeFitterExtension>();
            extension.OnSizeChanged.AddListener(extension_OnSizeChanged);
        }

        /// <summary>
        /// 内容大小控制扩展更改时，触发
        /// </summary>
        /// <param name="arg0"></param>
        private void extension_OnSizeChanged(Vector2 vector)
        {
            transform.GetComponent<RectTransform>().sizeDelta = vector;
            middle.sizeDelta = new Vector2(middle.sizeDelta.x, vector.y);
            left.sizeDelta = new Vector2((vector.x - middle.sizeDelta.x) / 2, vector.y - offset);
            right.sizeDelta = new Vector2((vector.x - middle.sizeDelta.x) / 2, vector.y - offset);
        }
    }
}
