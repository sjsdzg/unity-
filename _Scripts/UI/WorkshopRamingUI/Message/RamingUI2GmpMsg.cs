using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.UI
{
    /// <summary>
    /// 漫游界面给GMP知识点的消息
    /// </summary>
	public class RamingUI2GmpMsg : MonoBehaviour
    {


        public bool IsShow { get; set; }


        public object Content;

        public RamingUI2GmpMsg(bool _show,object _content)

        {
            IsShow = _show;
            Content = _content;
        }
    }
}