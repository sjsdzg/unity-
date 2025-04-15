using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Runtime
{
    /// <summary>
    /// Ref ColorBlockRef
    /// </summary>
    public class ColorBlockRef
    {
        private Color m_NormalColor;

        private Color m_HighlightedColor;

        private Color m_PressedColor;

        private Color m_SelectedColor;

        private Color m_DisabledColor;

        private float m_ColorMultiplier;

        private float m_FadeDuration;

        public Color normalColor { get { return m_NormalColor; } set { m_NormalColor = value; } }
        public Color highlightedColor { get { return m_HighlightedColor; } set { m_HighlightedColor = value; } }
        public Color pressedColor { get { return m_PressedColor; } set { m_PressedColor = value; } }
        public Color selectedColor { get { return m_SelectedColor; } set { m_SelectedColor = value; } }
        public Color disabledColor { get { return m_DisabledColor; } set { m_DisabledColor = value; } }
        public float colorMultiplier { get { return m_ColorMultiplier; } set { m_ColorMultiplier = value; } }
        public float fadeDuration { get { return m_FadeDuration; } set { m_FadeDuration = value; } }

        public static ColorBlockRef defaultColorBlockRef
        {
            get
            {
                var c = new ColorBlockRef
                {
                    m_NormalColor = new Color32(255, 255, 255, 255),
                    m_HighlightedColor = new Color32(245, 245, 245, 255),
                    m_PressedColor = new Color32(200, 200, 200, 255),
                    m_SelectedColor = new Color32(245, 245, 245, 255),
                    m_DisabledColor = new Color32(200, 200, 200, 128),
                    colorMultiplier = 1.0f,
                    fadeDuration = 0.1f
                };
                return c;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ColorBlockRef))
                return false;

            return Equals((ColorBlockRef)obj);
        }

        public bool Equals(ColorBlockRef other)
        {
            return normalColor == other.normalColor &&
                highlightedColor == other.highlightedColor &&
                pressedColor == other.pressedColor &&
                selectedColor == other.selectedColor &&
                disabledColor == other.disabledColor &&
                colorMultiplier == other.colorMultiplier &&
                fadeDuration == other.fadeDuration;
        }

        public static bool operator ==(ColorBlockRef point1, ColorBlockRef point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(ColorBlockRef point1, ColorBlockRef point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
