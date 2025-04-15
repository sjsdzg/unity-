using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.UI
{
    /// <summary>
    /// 文本面板
    /// </summary>
    public class TextPanel : MonoBehaviour
    {
        /// <summary>
        /// 标题
        /// </summary>
        private Text title;

        /// <summary>
        /// 内容
        /// </summary>
        private Text content;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button buttonClose;

        void Awake()
        {
            title = transform.Find("TitleBar/Text").GetComponent<Text>();
            content = transform.Find("ScrollView/Viewport/Content/Text").GetComponent<Text>();
            buttonClose = transform.Find("TitleBar/ButtonClose").GetComponent<Button>();

            buttonClose.onClick.AddListener(() => { gameObject.SetActive(false); });
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="_title"></param>
        /// <param name="_content"></param>
        public void Show(string _title, string _content)
        {
            gameObject.SetActive(true);
            transform.GetComponent<RectTransform>().anchoredPosition = Input.mousePosition;
            title.text = _title;
            content.text = _content;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
