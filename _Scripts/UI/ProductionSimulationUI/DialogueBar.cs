using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using XFramework.Module;
using XFramework.Core;

namespace XFramework.UI
{
    /// <summary>
    /// 人物对话框栏
    /// </summary>
    public class DialogueBar : MonoBehaviour
    {
        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// 确认按钮
        /// </summary>
        private Button buttonOK;

        /// <summary>
        /// 名称
        /// </summary>
        private Text textName;

        /// <summary>
        /// 对话内容
        /// </summary>
        private Text content;

        /// <summary>
        /// Player
        /// </summary>
        private Transform player;

        /// <summary>
        /// 对话信息
        /// </summary>
        public DialogueInfo DialogueInfo { get; set; }

        private UnityEvent m_OnSubmit = new UnityEvent();
        /// <summary>
        /// 提交事件
        /// </summary>
        public UnityEvent OnSubmit
        {
            get { return m_OnSubmit; }
            set { m_OnSubmit = value; }
        }

        void Awake()
        {
            //获取组件
            m_CanvasGroup = transform.GetComponent<CanvasGroup>();
            textName = transform.Find("Panel/Name").GetComponent<Text>();
            content = transform.Find("Panel/Content").GetComponent<Text>();
            buttonOK = transform.Find("Panel/ButtonOK").GetComponent<Button>();
            //事件
            buttonOK.onClick.AddListener(buttonOK_onClick);
        }

        void Start()
        {
            buttonOK.interactable = false;
            m_CanvasGroup.alpha = 0;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 确认按钮点击时，触发
        /// </summary>
        private void buttonOK_onClick()
        {
            EventDispatcher.ExecuteEvent(Events.Dialogue.Submit, DialogueInfo);
            OnSubmit.Invoke();
            Hide();
        }

        /// <summary>
        /// 显示窗口
        /// </summary>
        public void Show(string name, string text)
        {
            //if (player == null)
            //{
            //    player = GameObject.FindGameObjectWithTag("Player").transform;
            //}

            //if (player != null)
            //{
            //    player.GetComponent<ThirdPersonUserControlEx>().IsEnabled = false;
            //}

            gameObject.SetActive(true);
            buttonOK.interactable = false;
            textName.text = name + "：";
            m_CanvasGroup.DOFade(1, 0.2f).OnComplete(() => 
            {
                content.DOText(text, 2f).OnComplete(() => { buttonOK.interactable = true; });
            });
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="dialogueInfo"></param>
        public void Show(DialogueInfo dialogueInfo)
        {
            DialogueInfo = dialogueInfo;
            gameObject.SetActive(true);
            buttonOK.interactable = false;
            textName.text = dialogueInfo.Name + "：";
            m_CanvasGroup.DOFade(1, 0.2f).OnComplete(() =>
            {
                content.DOText(dialogueInfo.Content, 2f).OnComplete(() => { buttonOK.interactable = true; });
            });
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            m_CanvasGroup.DOFade(0, 0.2f).OnComplete(() => 
            {
                gameObject.SetActive(false);
                textName.text = "";
                content.text = "";
                if (player != null)
                {
                    player.GetComponent<MyselfControl>().disable = true;
                }
            });
        }
    }

    /// <summary>
    /// 对话信息
    /// </summary>
    public class DialogueInfo
    {
        /// <summary>
        /// 图标信息
        /// </summary>
        public Sprite Sprite { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public object Params { get; set; }
    }
}
