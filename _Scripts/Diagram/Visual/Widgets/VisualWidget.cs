using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.UIWidgets;

namespace XFramework.Diagram
{
    public class VisualWidget : MaskableGraphic
    {
        private float scale = 1;
        /// <summary>
        /// 真实缩放值
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set
            {
                if (SetPropertyUtils.SetStruct(ref scale, value))
                {
                    SetScaleDirty();
                }
            }
        }

        /// <summary>
        /// Parent RectTransform
        /// </summary>
        public RectTransform Parent
        {
            get 
            {
                if (transform.parent == null)
                    return null;

                return transform.parent.GetComponent<RectTransform>();
            }
        }

        public virtual void SetScaleDirty()
        {
            SetVerticesDirty();
        }
    }
}

