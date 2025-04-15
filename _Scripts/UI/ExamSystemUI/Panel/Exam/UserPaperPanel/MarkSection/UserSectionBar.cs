using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Module;

namespace XFramework.UI
{
    public class UserSectionBar : MonoBehaviour
    {
        public class OnCompletedEvent : UnityEvent<int, int, bool> { }

        private OnCompletedEvent m_OnCompleted = new OnCompletedEvent();
        /// <summary>
        /// 题目完成时，触发
        /// </summary>
        public OnCompletedEvent OnCompleted
        {
            get { return m_OnCompleted; }
            set { m_OnCompleted = value; }
        }

        private UnityEvent m_OnInitialized = new UnityEvent();
        /// <summary>
        /// 初始化完成事件
        /// </summary>
        public UnityEvent OnInitialized
        {
            get { return m_OnInitialized; }
            set { m_OnInitialized = value; }
        }

        private List<PaperSection> paperSections = new List<PaperSection>();
        /// <summary>
        /// 章节列表
        /// </summary>
        public List<PaperSection> PaperSections
        {
            get { return paperSections; }
            set
            {
                paperSections = value;
                StartCoroutine(BuildPaperSection(paperSections));
            }
        }

        private List<ExamSectionJson> examSectionJsons = new List<ExamSectionJson>();
        /// <summary>
        /// 考试答题内容
        /// </summary>
        public List<ExamSectionJson> ExamSectionJsons
        {
            get
            {
                examSectionJsons.Clear();
                foreach (var item in UserSectionComponents)
                {
                    examSectionJsons.Add(item.ExamSectionJson);
                }
                return examSectionJsons;
            }
            set
            {
                examSectionJsons = value;
                for (int i = 0; i < UserSectionComponents.Count; i++)
                {
                    UserSection markSection = UserSectionComponents[i];
                    markSection.ExamSectionJson = examSectionJsons[i];
                }
            }
        }

        /// <summary>
        /// 包含选项Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认选项
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// scrollRect
        /// </summary>
        private ScrollRect scrollRect;

        /// <summary>
        /// 选项列表
        /// </summary>
        private List<UserSection> UserSectionComponents;

        void Awake()
        {
            UserSectionComponents = new List<UserSection>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
            scrollRect = transform.GetComponent<ScrollRect>();
        }

        /// <summary>
        /// 增加部件Item
        /// </summary>
        /// <param name="info"></param>
        public void AddPaperSection()
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            UserSection component = obj.GetComponent<UserSection>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                UserSectionComponents.Add(component);

                component.OnCompleted.AddListener(component_OnCompleted);
            }
        }


        /// <summary>
        /// 加载试卷章节
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        IEnumerator BuildPaperSection(List<PaperSection> sections)
        {
            for (int i = 0; i < sections.Count; i++)
            {
                AddPaperSection();
            }

            yield return new WaitForEndOfFrame();

            int start = 0;
            for (int i = 0; i < UserSectionComponents.Count; i++)
            {
                PaperSection section = sections[i];
                UserSection component = UserSectionComponents[i];
                component.Id = i + 1;
                component.StartNum = start;
                component.PaperSection = section;
                start += section.Number;
                yield return new WaitForEndOfFrame();
            }

            //初始化完成时，触发
            OnInitialized.Invoke();
        }

        /// <summary>
        /// 试题完成时，触发
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="number"></param>
        /// <param name="value"></param>
        private void component_OnCompleted(int sectionId, int number, bool value)
        {
            OnCompleted.Invoke(sectionId, number, value);
        }

        /// <summary>
        /// 定位
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="number"></param>
        public void Center(int sectionId, int number)
        {
            CenterOnItem(UserSectionComponents[sectionId - 1].GetUserQuestion(number).GetComponent<RectTransform>());
        }

        /// <summary>
        /// 指定一个 item让其定位到ScrollRect中间
        /// </summary>
        /// <param name="target">需要定位到的目标</param>
        public void CenterOnItem(RectTransform target)
        {
            // Item is here
            var itemCenterPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(target));
            Debug.Log("Item Anchor Pos In Scroll: " + itemCenterPositionInScroll);
            // But must be here
            var targetPositionInScroll = GetWorldPointInWidget(scrollRect.GetComponent<RectTransform>(), GetWidgetWorldPoint(scrollRect.viewport));
            Debug.Log("Target Anchor Pos In Scroll: " + targetPositionInScroll);
            // So it has to move this distance
            var difference = targetPositionInScroll - itemCenterPositionInScroll;
            difference.z = 0f;

            var newNormalizedPosition = new Vector2(difference.x / (Content.rect.width - scrollRect.viewport.rect.width),
                difference.y / (Content.rect.height - scrollRect.viewport.rect.height));

            newNormalizedPosition = scrollRect.normalizedPosition - newNormalizedPosition;

            //newNormalizedPosition.x = Mathf.Clamp01(newNormalizedPosition.x);
            newNormalizedPosition.y = Mathf.Clamp01(newNormalizedPosition.y);

            DOTween.To(() => scrollRect.normalizedPosition, x => scrollRect.normalizedPosition = x, newNormalizedPosition, 0.5f);
        }

        private Vector3 GetWidgetWorldPoint(RectTransform target)
        {
            //pivot position + item size has to be included
            var pivotOffset = new Vector3(
                (0.5f - target.pivot.x) * target.rect.size.x,
                (0.5f - target.pivot.y) * target.rect.size.y,
                0f);
            var localPosition = target.localPosition + pivotOffset;
            return target.parent.TransformPoint(localPosition);
        }

        private Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
        {
            return target.InverseTransformPoint(worldPoint);
        }
    }
}
