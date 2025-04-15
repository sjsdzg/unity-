using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    public class DescriptionBar : MonoBehaviour
    {
        /// <summary>
        /// 图片
        /// </summary>
        private Image picture;

        /// <summary>
        /// 描述
        /// </summary>
        private Text desc;

        void Awake()
        {
            picture = transform.Find("Picture").GetComponent<Image>();
            desc = transform.Find("Text").GetComponent<Text>();
            desc.gameObject.SetActive(false);
            picture.gameObject.SetActive(false);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sprite"></param>
        public void SetValue(string text, Sprite sprite = null)
        {
            if (text == null)
            {
                desc.gameObject.SetActive(false);
            }
            else
            {
                desc.gameObject.SetActive(true);
                desc.text = text;
            }

            if (sprite == null)
            {
                picture.gameObject.SetActive(false);
            }
            else
            {
                picture.gameObject.SetActive(true);
                picture.sprite = sprite;
            }
        }
    }
}
