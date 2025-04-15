using UnityEngine;
using System.Collections;
using XFramework.Module;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using XFramework.Core;

namespace XFramework.UI
{
    public class ScoreRangeForm : ItemContainer<ScoreRangeElement, ScoreRange>, IValidate
    {
        private UniEvent<ScoreRangeForm> m_OnAnalysisEvent = new UniEvent<ScoreRangeForm>();
        /// <summary>
        /// 分析事件
        /// </summary>
        public UniEvent<ScoreRangeForm> OnAnalysisEvent
        {
            get { return m_OnAnalysisEvent; }
            set { m_OnAnalysisEvent = value; }
        }

        /// <summary>
        /// 警告文本
        /// </summary>
        private Text warning;

        /// <summary>
        /// 添加区间按钮
        /// </summary>
        private Button buttonAddRange;

        /// <summary>
        /// 统计分析
        /// </summary>
        private Button buttonAnalysis;

        protected override void OnAwake()
        {
            base.OnAwake();
            warning = transform.Find("Value/Panel/Warning").GetComponent<Text>();
            buttonAddRange = transform.Find("Value/Panel/Handle/ButtonAddRange").GetComponent<Button>();
            buttonAnalysis = transform.Find("Value/Panel/Handle/ButtonAnalysis").GetComponent<Button>();
            // Event
            buttonAddRange.onClick.AddListener(buttonAddRange_onClick);
            buttonAnalysis.onClick.AddListener(buttonAnalysis_onClick);
        }

        /// <summary>
        /// 获取分数区间
        /// </summary>
        public List<ScoreRange> ScoreRanges {
            get
            {
                List<ScoreRange> scoreRanges = new List<ScoreRange>();
                foreach (var element in m_Elements)
                {
                    ScoreRange item = new ScoreRange();
                    item.Min = element.ScoreRange.Min;
                    item.Max = element.ScoreRange.Max;
                    scoreRanges.Add(item);
                }
                return scoreRanges;
            }
        }

        public void InitData(List<ScoreRange> scoreRanges)
        {
            for (int i = 0; i < scoreRanges.Count; i++)
            {
                ScoreRange item = scoreRanges[i];
                AddItem(item);
            }
        }

        public bool Validate()
        {
            bool result = true;
            foreach (var element in m_Elements)
            {
                if (!element.Validate())
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        protected override void SetData(ScoreRangeElement element, ScoreRange item)
        {
            base.SetData(element, item);
            element.SetData(m_Elements.Count, item);
            element.OnValidated.AddListener(element_OnValidated);
            element.OnRemove.AddListener(element_OnRemove);
        }

        private void element_OnValidated(bool arg0, string arg1)
        {
            if (!arg0)
            {
                warning.text = arg1;
            }
        }

        private void element_OnRemove(ScoreRangeElement arg0)
        {
            ScoreRange item = arg0.Data as ScoreRange;
            RemoveItem(item);
            m_Elements.ForEach(x =>
            {
                if (x.Number > arg0.Number)
                {
                    x.Number = x.Number - 1;
                }
            });
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetWarning()
        {
            warning.text = "";
        }

        /// <summary>
        /// 添加区间
        /// </summary>
        private void buttonAddRange_onClick()
        {
            if (m_Elements.Count >= 6)
                return;

            ScoreRange item = new ScoreRange(0, 0);
            AddItem(item);
        }

        /// <summary>
        /// 统计
        /// </summary>
        private void buttonAnalysis_onClick()
        {
            if (Validate())
            {
                ResetWarning();
                OnAnalysisEvent.Invoke(this);
            }
        }
    }
}
