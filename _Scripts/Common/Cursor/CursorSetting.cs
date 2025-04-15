using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Common
{
    /// <summary>
    /// 光标设置
    /// </summary>
    public class CursorSetting : DDOLSingleton<CursorSetting>
    {
        /// <summary>
        /// path
        /// </summary>
        private const string path = "Cursor/";

        /// <summary>
        /// The default cursor texture.
        /// </summary>
        [SerializeField]
        public Texture2D ArrowCursorTexture;

        /// The Link cursor texture.
        /// </summary>
        [SerializeField]
        public Texture2D LinkCursorTexture;

        /// The Move cursor texture.
        /// </summary>
        [SerializeField]
        public Texture2D MoveCursorTexture;

        /// The Unavail cursor texture.
        /// </summary>
        [SerializeField]
        public Texture2D UnavailCursorTexture;


        /// <summary>
        /// The default cursor hot spot.
        /// </summary>
        [SerializeField]
        public Vector2 DefaultCursorHotSpot = Vector2.zero;

        void Awake()
        {
            LoadingCursorTexture();
        }

        /// <summary>
        /// Specify a custom cursor that you wish to use as a cursor.
        /// </summary>
        /// <param name="cursorType"></param>
        public void SetCursor(CursorType cursorType)
        {
            Texture2D texture = null;
            switch (cursorType)
            {
                case CursorType.arrow:
                    texture = ArrowCursorTexture;
                    break;
                case CursorType.link:
                    texture = LinkCursorTexture;
                    break;
                case CursorType.move:
                    texture = MoveCursorTexture;
                    break;
                case CursorType.unavail:
                    texture = UnavailCursorTexture;
                    break;
                default:
                    break;
            }
            //设置
            Cursor.SetCursor(texture, DefaultCursorHotSpot, GetCursorMode());
        }

        public CursorMode GetCursorMode()
        {
            #if UNITY_WEBGL
			    return CursorMode.ForceSoftware;
            #else
                return CursorMode.Auto;
            #endif
        }

        /// <summary>
        /// Loading Texture
        /// </summary>
        private void LoadingCursorTexture()
        {
            ArrowCursorTexture = Resources.Load<Texture2D>(path + CursorType.arrow.ToString());
            LinkCursorTexture = Resources.Load<Texture2D>(path + CursorType.link.ToString());
            MoveCursorTexture = Resources.Load<Texture2D>(path + CursorType.move.ToString());
            UnavailCursorTexture = Resources.Load<Texture2D>(path + CursorType.unavail.ToString());
        }
    }
}
