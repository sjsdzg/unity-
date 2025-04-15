using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace XFramework.Common
{
    public class SetTextColorAction : ActionBase
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        public Text text;

        /// <summary>
        /// color
        /// </summary>
        public Color color;

        public SetTextColorAction(Text _text, Color _color)
        {
            text = _text;
            color = _color;
        }

        public override void Execute()
        {
            text.color = color;
            Completed();
        }
    }
}

