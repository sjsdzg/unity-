using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using XFramework.Module;
using System.Collections.Generic;
using DG.Tweening;
using XFramework.Core;
using System;

namespace XFramework.UI
{
    public class ProcedureBar : MonoBehaviour
    {
        /// <summary>
        /// 伸缩面板
        /// </summary>
        private AccordionPanel m_AccordionPanel;

        /// <summary>
        /// 标题
        /// </summary>
        private Text m_Title;
        private Text m_StageName;

        /// <summary>
        /// 当前流程
        /// </summary>
        private Procedure m_Procedure;

        /// <summary>
        /// 当前操作
        /// </summary>
        private List<XFramework.Module.Sequence> Sequences;

        /// <summary>
        /// lastSeqID
        /// </summary>
        private int lastSeqID = 0;

        /// <summary>
        /// lastActionID
        /// </summary>
        private int lastActionID = 0;

        /// <summary>
        /// 上一个Item
        /// </summary>
        private ActionItem lastItem = null;
        /// <summary>
        /// 存放 上一个Item
        /// </summary>
        private List<ActionItem> lastItemList = new List<ActionItem>();
        /// <summary>
        /// 最小化
        /// </summary>
        private Button buttonMin;

        /// <summary>
        /// 最大化
        /// </summary>
        private Button buttonMax;
        /// <summary>
        /// background
        /// </summary>
        private RectTransform background;

        void Awake()
        {
            m_AccordionPanel = transform.Find("Scroll View/Viewport/Content/AccordionPanel").GetComponent<AccordionPanel>();
            m_Title = transform.Find("ProduceName/Text").GetComponent<Text>();
            m_StageName = transform.Find("StageName/Text").GetComponent<Text>();

            background = gameObject.GetComponent<RectTransform>();

            //buttonMin = transform.Find("TitleBar/ButtonMin").GetComponent<Button>();
           // buttonMax = transform.parent.Find("TitleBar/ButtonMax").GetComponent<Button>();

            //Event
            //buttonMin.onClick.AddListener(buttonMin_onClick);
            //buttonMax.onClick.AddListener(buttonMax_onClick);
            EventDispatcher.RegisterEvent(Events.Procedure.InitActionState, InitActionState_callBack);
        }

  

        private void buttonMin_onClick()
        {
            background.DOScale(0, 0.2f).OnComplete(() => buttonMax.gameObject.SetActive(true));
        }

        private void buttonMax_onClick()
        {
            buttonMax.gameObject.SetActive(false);
            background.DOScale(1, 0.2f);
        }
        public void SetValue(List<XFramework.Module.Sequence> sequences)
        {
            m_AccordionPanel.SetValue(sequences);
        }
        public void SetValue(string name, List<XFramework.Module.Sequence> sequences)
        {
            m_Title.text = name;

            m_AccordionPanel.SetValue(sequences);
        }
        public void SetValue(string name,string stageName, List<XFramework.Module.Sequence> sequences)
        {
            m_Title.text = name;
            m_StageName.text = stageName;
            m_AccordionPanel.SetValue(sequences);
        }
        public void SetCurrentItem(int seqID, int actionID)
        {
            if (lastItem != null)
            {
                lastItem.State = ActionState.Finished;
                lastItemList.Add(lastItem);
            }

            ActionItem currentItem = m_AccordionPanel.GetActionItem(seqID, actionID);
            if (lastItemList.Contains(currentItem))
            {
                currentItem.State = ActionState.Unfinished;
            }
            else
            {
                lastItemList.Add(currentItem);
            }
            currentItem.State = ActionState.Doing;                
            lastItem = currentItem;
        }
        private void InitActionState_callBack()
        {
            for (int i = 0; i < lastItemList.Count; i++)
            {
                lastItemList[i].State = ActionState.Unfinished;
            }
            lastItemList.Clear();
        }
        private void OnDestroy()
        {
            EventDispatcher.UnregisterEvent(Events.Procedure.InitActionState, InitActionState_callBack);
        }
    }
}

