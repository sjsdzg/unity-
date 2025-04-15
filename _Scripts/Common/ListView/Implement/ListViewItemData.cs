using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace XFramework.Common
{
    public class ListViewItemData
    {
        /// <summary>
        /// Sprite
        /// </summary>
        public Sprite Sprite { get; set; }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text { get; set; }

        public ListViewItemData()
        {

        }

        public ListViewItemData(Sprite sprite, string text)
        {
            Sprite = sprite;
            Text = text;
        }
    }
}
