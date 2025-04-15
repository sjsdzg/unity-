using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace XFramework.UIWidgets
{
    public class UIButton : Button
    {
        /// <summary>
        /// 图标 Graphic
        /// </summary>
        [SerializeField]
        private Graphic m_IconGraphic;

        /// <summary>
        /// 图标 ColorBlock
        /// </summary>
        [SerializeField]
        private ColorBlock m_IconColors = ColorBlock.defaultColorBlock;

        /// <summary>
        /// 文本 Graphic
        /// </summary>
        [SerializeField]
        private Graphic m_TextGraphic;

        /// <summary>
        /// 文本 ColorBlock
        /// </summary>
        [SerializeField]
        private ColorBlock m_TextColors = ColorBlock.defaultColorBlock;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);

            Color iconColor;
            Color textColor;

            switch (state)
            {
                case SelectionState.Normal:
                    iconColor = m_IconColors.normalColor;
                    textColor = m_TextColors.normalColor;
                    break;
                case SelectionState.Highlighted:
                    iconColor = m_IconColors.highlightedColor;
                    textColor = m_TextColors.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    iconColor = m_IconColors.pressedColor;
                    textColor = m_TextColors.pressedColor;
                    break;
                case SelectionState.Disabled:
                    iconColor = m_IconColors.disabledColor;
                    textColor = m_TextColors.disabledColor;
                    break;
                default:
                    iconColor = Color.black;
                    textColor = Color.black;
                    break;
            }

            if (gameObject.activeInHierarchy)
            {
                StartColorTween(m_IconGraphic, iconColor, m_IconColors.fadeDuration, instant);
                StartColorTween(m_TextGraphic, textColor, m_TextColors.fadeDuration, instant);
            }
        }

        private void StartColorTween(Graphic graphic, Color targetColor, float fadeDuration, bool instant)
        {
            if (graphic == null)
                return;

            graphic.CrossFadeColor(targetColor, instant ? 0f : fadeDuration, true, true);
        }
    }
}
