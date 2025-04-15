using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;
using UnityEngine.Events;
using System;

namespace XFramework.UI
{
    public class SectionItemComponent : MonoBehaviour
    {
        /// <summary>
        /// 文本
        /// </summary>
        private Text text;

        /// <summary>
        /// 工具栏
        /// </summary>
        public SectionContentComponent Parent { get; private set; }

        /// <summary>
        /// 工具数据
        /// </summary>
        public Stage data { get; private set; }

        /// <summary>
        /// 点击事件类
        /// </summary>
        public class ClickEvent : UnityEvent<SectionItemComponent> { }

        private ClickEvent m_OnClick = new ClickEvent();
        /// <summary>
        /// 工具点击事件
        /// </summary>
        public ClickEvent OnClick
        {
            get { return m_OnClick; }
            set { m_OnClick = value; }
        }

        private void Awake()
        {
            text = transform.Find("Text").GetComponent<Text>();
            transform.GetComponent<Button>().onClick.AddListener(Button_onClick);
        }

        private void Button_onClick()
        {
            OnClick.Invoke(this);
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="parameter"></param>
        public void SetValue(SectionContentComponent bar, Stage _data)
        {
            Parent = bar;
            data = _data;
            text.text = _data.Name;
            //switch (App.Instance.VersionTag)
            //{
            //    case VersionTag.Default:
            //        break;
            //    case VersionTag.JIANGNAN:
            //        break;
            //    case VersionTag.TJCU:
            //        break;
            //    case VersionTag.ZJXU:
            //        if (text.text.Contains("裂解离心工段") || text.text.Contains("离心破碎工段"))
            //        {
            //            text.text = "菌体破碎工段";
            //        }
            //        break;
            //    case VersionTag.WMU:
            //        break;
            //    default:
            //        break;
            //}
        }
    }
}

