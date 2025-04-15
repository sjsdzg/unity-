using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Core;
using UnityEngine.Events;

namespace XFramework.Common
{
    public class ProgressPanel : MonoSingleton<ProgressPanel>
    {
        private UnityEvent m_OnCompleted = new UnityEvent();
        /// <summary>
        /// 完成时触发
        /// </summary>
        public UnityEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }
        /// <summary>
        /// 操作提示文本
        /// </summary>
        private Text content;

        /// <summary>
        /// 进度Image
        /// </summary>
        private Image progressImage;

        /// <summary>
        /// 进度Text
        /// </summary>
        private Text progressText;

        /// <summary>
        /// 持续时间
        /// </summary>
        private float duration;

        protected override void Init()
        {
            base.Init();

            content = transform.Find("Background/Content/Text").GetComponent<Text>();
            progressImage = transform.Find("Background/Progress").GetComponent<Image>();
            progressText = transform.Find("Background/Progress/Text").GetComponent<Text>();
            transform.localPosition = new Vector3(0, 0, 0);
            Hide();
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="text"></param>
        /// <param name="duration"></param>
        public void Show(string text, float duration)
        {
            StopAllCoroutines();
            gameObject.SetActive(true);
            content.text = text;
            progressText.text = "0%";
            this.duration = duration;
            StartCoroutine(Progress());
        }

        IEnumerator Progress()
        {
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                progressImage.fillAmount = elapsedTime / duration;
                progressText.text = Mathf.Ceil(progressImage.fillAmount * 100).ToString() + "%";
                yield return new WaitForEndOfFrame();
            }
            progressImage.fillAmount = 1;
            progressText.text = "100%";
            OnCompleted.Invoke();
            yield return new WaitForSeconds(0.2f);
            Hide();
        }

        public void ResetProgressPanel()
        {
            progressImage.fillAmount = 1;
            progressText.text = "0%";
            OnCompleted.RemoveAllListeners();
            Hide();
        }
    }
}

