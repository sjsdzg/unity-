using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace XFramework.UI
{
    public enum ButtonCellType
    {
        /// <summary>
        /// 插入
        /// </summary>
        Insert,
        /// <summary>
        /// 更新
        /// </summary>
        Update,
        /// <summary>
        /// 删除
        /// </summary>
        Delete,
        /// <summary>
        /// 批量删除
        /// </summary>
        BatchDelete,
        /// <summary>
        /// 导出试题
        /// </summary>
        Export,
        /// <summary>
        /// 选择所有
        /// </summary>
        SelectAll,
        /// <summary>
        /// 开始考试
        /// </summary>
        StartExam,
        /// <summary>
        /// 人工评卷
        /// </summary>
        MakePaper,
        /// <summary>
        /// 详情
        /// </summary>
        Detail,
        /// <summary>
        /// 上传
        /// </summary>
        Upload,
        /// <summary>
        /// 上传文件
        /// </summary>
        UploadFile,
        /// <summary>
        /// 上传文件夹
        /// </summary>
        UploadDirectory,
        /// <summary>
        /// 下载
        /// </summary>
        Download,
        /// <summary>
        /// 新建文件夹
        /// </summary>
        CreateDir,
        /// <summary>
        /// 刷新
        /// </summary>
        Refresh,
    }

    [RequireComponent(typeof(Button))]
    public abstract class ButtonCell : MonoBehaviour
    {
        public class OnClickedEvent : UnityEvent<ButtonCellType> { }

        private OnClickedEvent m_OnClicked = new OnClickedEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public OnClickedEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }

        private Button button;

        public abstract ButtonCellType GetButtonCellType();

        void Awake()
        {
            button = transform.GetComponent<Button>();
            button.onClick.AddListener(() => OnClicked.Invoke(GetButtonCellType()));
        }
    }
}
