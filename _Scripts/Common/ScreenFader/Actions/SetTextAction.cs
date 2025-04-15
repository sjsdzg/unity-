using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace XFramework.Common
{
    public class SetTextAction : ActionBase
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        public Text text;

        /// <summary>
        /// content
        /// </summary>
        public string content = "";

        /// <summary>
        /// Alpha
        /// </summary>
        public float alpha = 1;

        public SetTextAction(Text _text, string _content, float _alpha = 1)
        {
            text = _text;
            content = _content;
            alpha = _alpha;
        }

        public override void Execute()
        {
            text.text = content;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            Completed();
        }
    }
}

