using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace XFramework.UI
{
    public class HandCursorSupport : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private bool allowHandCursor = true;
        /// <summary>
        /// 允许手势指针
        /// </summary>
        public bool AllowHandCursor
        {
            get { return allowHandCursor; }
            set
            {
                allowHandCursor = value;
                if (!allowHandCursor)
                {
                    Cursor.SetCursor(null, DefaultCursorHotSpot, CursorMode.Auto);
                }
            }
        }

        /// <summary>
        /// The cursor texture.
        /// </summary>
        [SerializeField]
        public Texture2D HandCursorTexture;

        /// <summary>
        /// The cursor hot spot.
        /// </summary>
        [SerializeField]
        public Vector2 HandCursorHotSpot = new Vector2(4, 2);

        /// <summary>
        /// The default cursor hot spot.
        /// </summary>
        [SerializeField]
        public Vector2 DefaultCursorHotSpot = Vector2.zero;

        public void OnPointerEnter(PointerEventData eventData)
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            if (AllowHandCursor)
            {
                Cursor.SetCursor(HandCursorTexture, HandCursorHotSpot, CursorMode.Auto);
            }
            else
            {
                Cursor.SetCursor(null, DefaultCursorHotSpot, CursorMode.Auto);
            }
#endif
        }

        public void OnPointerExit(PointerEventData eventData)
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            Cursor.SetCursor(null, DefaultCursorHotSpot, CursorMode.Auto);
#endif
        }
    }
}
