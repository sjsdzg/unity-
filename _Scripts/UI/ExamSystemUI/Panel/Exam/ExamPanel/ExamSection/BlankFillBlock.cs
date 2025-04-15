using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Module;

namespace XFramework.UI
{
    /// <summary>
    /// 填空题块
    /// </summary>
    public class BlankFillBlock : QuestionBlock
    {
        public override int QType
        {
            get { return 4; }
        }

        private string key;
        /// <summary>
        /// 答案
        /// </summary>
        public string Key
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < examBlankConponents.Count; i++)
                {
                    ExamBlankConponent component = examBlankConponents[i];
                    string content = component.BlankContent.Replace(","," ");
                    sb.Append(component.BlankContent);
                    if (i < examBlankConponents.Count - 1)
                    {
                        sb.Append(",");//空格隔开填空
                    }
                }

                key = sb.ToString();
                return key;
            }
            set
            {
                key = value;
                string[] strs = key.Split(',');
                if (strs.Length == examBlankConponents.Count)
                {
                    for (int i = 0; i < examBlankConponents.Count; i++)
                    {
                        ExamBlankConponent component = examBlankConponents[i];
                        component.BlankContent = strs[i];
                    }
                }
            }
        }

        private List<QBlank> blankList = new List<QBlank>();
        /// <summary>
        /// 选项列表
        /// </summary>
        public List<QBlank> BlankList
        {
            get
            {
                blankList = new List<QBlank>();
                for (int i = 0; i < examBlankConponents.Count; i++)
                {
                    ExamBlankConponent component = examBlankConponents[i];
                    QBlank blank = new QBlank();
                    blank.Id = i + 1;
                    blank.Name = "BLANK" + (i + 1);
                    blank.Value = component.BlankContent;
                    blankList.Add(blank);
                }
                return blankList;
            }
            set
            {
                blankList = value;

                for (int i = 0; i < blankList.Count; i++)
                {
                    string text = (i + 1).ToString();
                    AddBlank(text);
                }
            }
        }


        /// <summary>
        /// 包含填空Rect
        /// </summary>
        public RectTransform Content;

        /// <summary>
        /// 默认填空
        /// </summary>
        public GameObject DefaultItem;

        /// <summary>
        /// 填空列表
        /// </summary>
        private List<ExamBlankConponent> examBlankConponents;

        public override void OnAwake()
        {
            base.OnAwake();
            examBlankConponents = new List<ExamBlankConponent>();
            if (DefaultItem == null)
            {
                throw new NullReferenceException("DefaultItem is null. Set component of type ImageAdvanced to DefaultItem.");
            }
            DefaultItem.gameObject.SetActive(false);
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="info"></param>
        public void AddBlank(string text)
        {
            GameObject obj = Instantiate(DefaultItem);
            obj.SetActive(true);

            ExamBlankConponent component = obj.GetComponent<ExamBlankConponent>();

            if (Content != null && component != null)
            {
                component.transform.SetParent(Content, false);
                obj.layer = Content.gameObject.layer;

                component.SetParams(text + ".");
                examBlankConponents.Add(component);
                component.OnEditCompleted.AddListener(component_OnEditCompleted);
            }
        }

        private void component_OnEditCompleted(bool value)
        {
            string[] strs = Key.Split(',');

            int count = 0;
            for (int i = 0; i < strs.Length; i++)
            {
                string str = strs[i];
                if (string.IsNullOrEmpty(str))
                {
                    count++;
                }
            }

            if (count > 0)
            {
                OnCompleted.Invoke(false);
            }
            else
            {
                OnCompleted.Invoke(true);
            }
        }

        public void Clear()
        {
            if (examBlankConponents == null)
                return;

            for (int i = 0; i < examBlankConponents.Count; i++)
            {
                ExamBlankConponent component = examBlankConponents[i];
                Destroy(component.gameObject);
            }
            examBlankConponents.Clear();
        }

        public override void SetParams(object value)
        {
            BlankList = value as List<QBlank>;
        }

        public override string GetKey()
        {
            return Key;
        }

        public override void SetKey(string _key)
        {
            Key = _key;
        }
    }
}
