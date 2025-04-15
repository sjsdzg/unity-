using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HighlightingSystem;
using UnityEngine;

namespace XFramework.Common
{
    public class HighlighterAction : ActionBase
    {
        /// <summary>
        /// gameObject
        /// </summary>
        public GameObject gameObject;
        /// <summary>
        /// 是否高亮
        /// </summary>
        public bool isHighlighted = true;
        /// <summary>
        /// 颜色
        /// </summary>
        public Color color = Color.cyan;

        public HighlighterAction(GameObject _gameObject, bool _isHighlighted = true)
        {
            gameObject = _gameObject;
            isHighlighted = _isHighlighted;
        }

        public HighlighterAction(GameObject _gameObject, bool _isHighlighted, Color _color)
        {
            gameObject = _gameObject;
            isHighlighted = _isHighlighted;
            color = _color;
        }

        public override void Execute()
        {
            if (gameObject != null)
            {
                Highlighter h = gameObject.GetOrAddComponent<Highlighter>();
                if (isHighlighted)
                {
                    h.ConstantOn(color);
                }
                else
                {
                    h.ConstantOff();
                }
            }
            //完成
            Completed();
        }
    }
}
