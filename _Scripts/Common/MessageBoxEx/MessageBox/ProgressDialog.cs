using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace XFramework.Common
{
    public class ProgressDialog : BaseDialog
    {
        private Text Content;// 操作提示文本
        private Slider Slider;//进度条
        private Action<DialogResult> handler; // 操作回调函数
        private float m_Timer = 0;
        private RectTransform m_Rect;
        private string finishText;

        void Awake()
        {
            Content = transform.Find("Background/Panel/Content").GetComponent<Text>();
            Slider = transform.Find("Background/Panel/Slider").GetComponent<Slider>();
            m_Rect = transform.Find("Background/Panel") as RectTransform;
            m_Rect.localScale = new Vector3(0, 0, 0);
        }

        public override MessageBoxExEnum GetMessageBoxExType()
        {
            return MessageBoxExEnum.ProgressDialog;
        }

        public override void ShowMessageBoxEx(string text, string caption, Action<DialogResult> action, params object[] _params)
        {
            Content.text = text;
            m_Timer = float.Parse(_params[0].ToString());
            finishText = _params[1].ToString();
            handler = action;
            StartCoroutine(Progress());
            m_Rect.DOScale(0.8f, 0.4f);
        }

        public override void OnSubmit()
        {
            throw new NotImplementedException();
        }

        public override void OnCancel()
        {
            throw new NotImplementedException();
        }

        IEnumerator Progress()
        {
            var fadeTime = m_Timer;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                Slider.value = elapsedTime / fadeTime;
                yield return null;
            }

            //设置结束文本
            Content.text = finishText;
            yield return new WaitForSeconds(0.4f);

            if (handler != null)
            {
                DialogResult handleData = new DialogResult(gameObject, true);
                handler(handleData);
            }

            m_Rect.DOScale(0, 0.5f).OnComplete(() => { DestroyImmediate(gameObject); });
        }
    }
}
