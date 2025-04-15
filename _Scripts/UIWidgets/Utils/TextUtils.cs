using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.UIWidgets
{
    public static class TextUtils
    {
        public static readonly string no_breaking_space = "\u00A0";

        /// <summary>
        /// 我们平时打出的空格(Breaking Space)都是带这种换行功能的，
        /// 还有一个不换行空格(Non-breaking space)，Unicode编码为/u00A0。
        /// </summary>
        /// <param name="label"></param>
        public static void SetTextWithNoBreakingSpace(this Text label)
        {
            if (label.text.Contains(" "))
            {
                label.text = label.text.Replace(" ", no_breaking_space);
            }
        }

        /// <summary>
        /// 实现文字过长省略号显示
        /// </summary>
        /// <param name="label"></param>
        public static void SetTextWithEllipsis(this Text label)
        {
            var generator = new TextGenerator();
            var rectTransform = label.GetComponent<RectTransform>();
            var settings = label.GetGenerationSettings(rectTransform.rect.size);
            generator.Populate(label.text, settings);

            var characterCountVisible = generator.characterCountVisible;
            var text = label.text;
            if (text.Length > characterCountVisible)
            {
                text = text.Substring(0, characterCountVisible - 1);
                text += "...";
            }
        }

        /// <summary>
        /// \\n 替换成 \n
        /// </summary>
        public static void SetTextReplaceLineBreak(this Text label)
        {
            var text = label.text;
            text.Replace("\\n", "\n");
        }
    }
}

