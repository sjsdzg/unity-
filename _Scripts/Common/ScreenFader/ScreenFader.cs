using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using XFramework.Core;

namespace XFramework.Common
{
    /// <summary>
    /// ScreenFader
    /// </summary>
    public class ScreenFader : MonoSingleton<ScreenFader>
    {
        /// <summary>
        /// Executor
        /// </summary>
        private Task m_Task;

        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// Text
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 
        /// </summary>
        //private UnityEvent CompletedAction= new UnityEvent();

        protected override void Init()
        {
            base.Init();
            m_Task = new Task();
            transform.GetComponent<Canvas>().enabled = true;
            m_CanvasGroup = transform.Find("Panel").GetComponent<CanvasGroup>();
            m_Text = transform.Find("Panel/Text").GetComponent<Text>();
            m_Text.text = "";
        }

        public ScreenFader FadeIn(float time = 1, float alpha = 1)
        {
            m_Task.Append(new CanvasGroupFadingAction(m_CanvasGroup, alpha, time))
                .Append(new CanvasGroupBlocksAction(m_CanvasGroup, true));
            return this;
        }

        public ScreenFader FadeOut(float time = 1)
        {
            m_Task.Append(new CanvasGroupFadingAction(m_CanvasGroup, 0, time))
                .Append(new CanvasGroupBlocksAction(m_CanvasGroup, false));
            return this;
        }

        public ScreenFader Pause(float time = 1)
        {
            m_Task.Append(new DelayedAction(time));
            return this;
        }

        public ScreenFader FadeTextIn(float time = 1)
        {
            m_Task.Append(new TextFadingAction(m_Text, 1, time));
            return this;
        }

        public ScreenFader SetText(string content, float alpha = 1)
        {
            m_Task.Append(new SetTextAction(m_Text, content, alpha));
            return this;
        }

        public ScreenFader SetTextColor(Color color)
        {
            m_Task.Append(new SetTextColorAction(m_Text, color));
            return this;
        }

        public ScreenFader FadeTextOut(float time = 1)
        {
            m_Task.Append(new TextFadingAction(m_Text, 0, time));
            return this;
        }

        public ScreenFader OnCompleted(UnityAction action)
        {
            m_Task.OnCompleted(action);
            return this;
        }
        public ScreenFader OnRemoveAction()
        {
            //if (CompletedAction != null)
            //{
            //    CompletedAction.RemoveAllListeners();

            //}
            m_Task = new Task();
            return this;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public void Execute()
        {
            //if (CompletedAction != null)
            //{
            //    m_Task.OnCompleted(()=>{ CompletedAction.Invoke(); } );
            //}
            m_Task.OnCompleted(() =>
            {
                m_Task.Clear();
            }).Execute();
        }
    }
}