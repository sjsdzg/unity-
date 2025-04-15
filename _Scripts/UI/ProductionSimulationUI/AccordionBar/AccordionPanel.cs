using UnityEngine;
using System.Collections;
using UIWidgets;
using XFramework.UI;
using System.Collections.Generic;
using UnityEngine.UI;
using XFramework.Module;
using System;

namespace XFramework.UI
{
    public class AccordionPanel : Accordion
    {
        [SerializeField]
        public GameObject TogglePrefab;

        [SerializeField]
        public GameObject ContentPrefab;

        /// <summary>
        /// 动作试图列表
        /// </summary>
        public List<ActionContent> actionListViews;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ActionItem> ActionItemDict = new Dictionary<string, ActionItem>();

        void Awake()
        {
            actionListViews = new List<ActionContent>();
            if (TogglePrefab == null)
            {
                Debug.LogError("TogglePrefab is null");
            }

            if (ContentPrefab == null)
            {
                Debug.LogError("ContentPrefab is null");
            }
        }

        protected override void Start()
        {
            base.Start();
            TogglePrefab.SetActive(false);
            ContentPrefab.SetActive(false);
            this.OnToggleItem.AddListener(OnToggleItemCall);
        }

        private void OnToggleItemCall(AccordionItem item)
        {
            SequenceToggle toggle = item.ToggleObject.GetComponent<SequenceToggle>();

            if (toggle != null)
            {
                toggle.IsOpen = item.Open;
            }
        }

        public void SetValue(List<Sequence> sequences)
        {
            StartCoroutine(LoadData(sequences));
        }

        IEnumerator LoadData(List<Sequence> sequences)
        {
            foreach (var seq in sequences)
            {
                GameObject toggleObj = Instantiate(TogglePrefab);
                GameObject contentObj = Instantiate(ContentPrefab);
                toggleObj.SetActive(true);
                contentObj.SetActive(true);
                AccordionItem item = new AccordionItem();
                yield return new WaitForEndOfFrame();
                //伸缩组件标题
                SequenceToggle sequenceToggle = toggleObj.GetComponent<SequenceToggle>();
                if (sequenceToggle != null)
                {
                    Transform t = sequenceToggle.transform;
                    t.SetParent(transform, false);
                    toggleObj.layer = gameObject.layer;

                    //string text = seq.ID + "." + seq.Desc;
                    sequenceToggle.SetValue(seq.ID.ToString(), seq.Desc);
                }
                yield return new WaitForEndOfFrame();
                //伸缩组件内容
                ActionContent actionContent = contentObj.GetComponent<ActionContent>();
                if (actionContent != null)
                {
                    Transform t = actionContent.transform;
                    t.SetParent(transform, false);
                    contentObj.layer = gameObject.layer;

                    foreach (var _action in seq.Actions)
                    {
                        actionContent.AddActionItem(seq,_action);
                    }
                    //actionView.SetSizeDelta();
                }

                //设置组件内容尺寸
                actionListViews.Add(actionContent);
                yield return new WaitForEndOfFrame();
                //组织伸缩部分
                item.ToggleObject = toggleObj;
                item.ContentObject = contentObj;
                AddCallback(item);
            }
        }

        public ActionItem GetActionItem(int seqID, int actionID)
        {
            ActionContent actionContent = actionListViews[seqID - 1];
            ActionItem accordionItem = actionContent.GetActionItem(actionID);
            return accordionItem;
        }
    }
}

