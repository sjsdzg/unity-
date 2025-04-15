using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace XFramework.UI
{
    /// <summary>
    /// 右键菜单参数
    /// </summary>
    public class ContextMenuParameter
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 右键菜单项名称
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 回调
        /// </summary>
        public UnityAction<ContextMenuParameter> call;

        public ContextMenuParameter() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="_isUnderline"></param>
        /// <param name="enabled"></param>
        /// <param name="_call"></param>
        public ContextMenuParameter(int id, string text, string icon, bool enabled, UnityAction<ContextMenuParameter> _call)
        {
            ID = id;
            Content = text;
            Icon = icon;
            Enabled = enabled;
            call = _call;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="text"></param>
        /// <param name="_icon"></param>
        /// <param name="enabled"></param>
        /// <param name="_call"></param>
        public ContextMenuParameter(string text, bool enabled, UnityAction<ContextMenuParameter> _call)
        {
            Content = text;
            Enabled = enabled;
            call = _call;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        /// <param name="_call"></param>
        public ContextMenuParameter(string text, UnityAction<ContextMenuParameter> _call)
        {
            Content = text;
            Enabled = true;
            call = _call;
        }
    }
}
