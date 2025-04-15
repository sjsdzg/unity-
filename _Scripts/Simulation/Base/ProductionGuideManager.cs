using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Module;

namespace XFramework.Simulation
{
    public class ProductionGuideManager : Singleton<ProductionGuideManager>
    {
        private readonly string path = "Icons/Guide";

        /// <summary>
        /// 图标列表
        /// </summary>
        private Dictionary<string, Sprite> m_Textures = new Dictionary<string, Sprite>();

        private bool enabled = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 当前SeqID
        /// </summary>
        public int CurrentSeqId { get; set; }

        /// <summary>
        /// 当前ActionId
        /// </summary>
        public int CurrentActionId { get; set; }

        /// <summary>
        /// 引导列表
        /// </summary>
        private Dictionary<string, IndicatorComponent> m_Indicators = new Dictionary<string, IndicatorComponent>();

        /// <summary>
        /// 引导集合
        /// </summary>
        [HideInInspector]
        public GuideCollection m_Collection;

        /// <summary>
        /// uniqueNodes
        /// </summary>
        private List<GuideNode> uniqueNodes;

        protected override void Init()
        {
            base.Init();
            m_Indicators = new Dictionary<string, IndicatorComponent>();
            Sprite[] array = Resources.LoadAll<Sprite>(path);
            for (int i = 0; i < array.Length; i++)
            {
                Sprite texture = array[i];
                m_Textures.Add(texture.name, texture);
            }
        }

        public void Init(string configPath, Transform parent)
        {
            m_Collection = GuideCollection.Parser.ParseXmlFromResources(configPath);
            ReplaceGuideDrugName(m_Collection);
            uniqueNodes = new List<GuideNode>();
            if (m_Collection != null)
            {
                foreach (GuideNode node in m_Collection.GuideNodes)
                {
                    GuideNode uniqueNode = uniqueNodes.Find(x => x.Target == node.Name);
                    if (uniqueNode == null)
                    {
                        uniqueNodes.Add(node);
                    }
                }

                foreach (Transform child in parent)
                {
                    IndicatorComponent indicator = child.GetOrAddComponent<IndicatorComponent>();
                    GuideNode node = uniqueNodes.Find(x => x.Target == indicator.name);
                    if (node == null)
                        continue;

                    indicator.hudInfo = new IndicatorInfo();
                    indicator.hudInfo.onScreenArgs = new IndicatorOnScreenArgs();
                    indicator.hudInfo.offScreenArgs = new IndicatorOffScreenArgs();
                    switch (node.Type)
                    {
                        case GuideType.None:
                            break;
                        case GuideType.Indicator:
                            indicator.hudInfo.HUDType = HUDType.Indicator;
                            indicator.hudInfo.onScreenArgs.m_Sprite = m_Textures["DownArrow"];
                            indicator.hudInfo.offScreenArgs.m_Sprite = m_Textures["Arrow"];
                            indicator.hudInfo.offScreenArgs.m_Color = Color.red;
                            break;
                        default:
                            break;
                    }
                    indicator.hudInfo.m_Target = child;
                    m_Indicators.Add(indicator.name, indicator);
                }
                CloseAllGuide();
            }
        }

        private void ReplaceGuideDrugName(GuideCollection collection)
        {
            switch (App.Instance.VersionTag)
            {
                case VersionTag.Default:
                    break;
                case VersionTag.FZDX:
                    break;
                case VersionTag.TJCU:
                    break;
                case VersionTag.SNTCM:
                case VersionTag.WHGCDX:
                    for (int i = 0; i < collection.GuideNodes.Count; i++)
                    {                      
                        if (collection.GuideNodes[i].Description.IndexOf("缬沙坦") != -1)
                        {
                            collection.GuideNodes[i].Description.Replace("缬沙坦", GlobalManager.DrugName);                         
                        }
                    }
                        break;
                default:
                    break;
            }
        }

        IEnumerator Loading(Transform parent)
        {
            yield return new WaitForEndOfFrame();
            foreach (Transform child in parent)
            {
                IndicatorComponent indicator = child.GetOrAddComponent<IndicatorComponent>();
                GuideNode guide = uniqueNodes.Find(x => x.Target == indicator.name);
                switch (guide.Type)
                {
                    case GuideType.None:
                        break;
                    case GuideType.Indicator:
                        indicator.hudInfo.HUDType = HUDType.Indicator;
                        break;
                    default:
                        break;
                }
                indicator.hudInfo.m_Target = child;
                m_Indicators.Add(guide.Target, indicator);
            }
        }

        /// <summary>
        /// 显示当前步骤下的引导
        /// </summary>
        /// <param name="closeOthers"></param>
        public virtual void ShowCurrentGuide(bool closeOthers = true)
        {
            string name = string.Format("{0}-{1}", CurrentSeqId, CurrentActionId);
            ShowGuide(name, closeOthers);
        }

        /// <summary>
        /// 显示当前步骤下的引导
        /// </summary>
        /// <param name="number"></param>
        /// <param name="closeOthers"></param>
        public virtual void ShowCurrentGuide(int number, bool closeOthers = true)
        {
            string name = string.Format("{0}-{1}-{2}", CurrentSeqId, CurrentActionId, number);
            ShowGuide(name, closeOthers);
        }

        /// <summary>
        /// 显示指示器
        /// </summary>
        /// <param name="name"></param>
        /// <param name="closeOthers">是否关闭其他</param>
        public void ShowGuide(string name, bool closeOthers = true)
        {
            if (!Enabled)
                return;
           
            GuideNode guide = m_Collection.GuideNodes.Find(x => x.Name == name);            
            if (guide == null)
                return;

            bool isLast = (guide == m_Collection.GuideNodes[m_Collection.GuideNodes.Count - 1]);
            CustomWorkshop customWorkshop = GameObject.FindObjectOfType<CustomWorkshop>();
            customWorkshop.ShowCheckQuestion(name, false);

            //关闭其他
            if (closeOthers)
            {
                foreach (string item in m_Indicators.Keys)
                {
                    if (!guide.Target.Equals(item))
                    {
                        m_Indicators[item].hide();
                    }
                }
            }

            IndicatorComponent indicator = null;
            if (m_Indicators.TryGetValue(guide.Target, out indicator))
            {
                LookPointManager.Instance.CurrentName = guide.Target;
                indicator.hudInfo.onScreenArgs.m_Text = guide.Content;
                if (GlobalManager.DefaultMode == ProductionMode.Examine)
                    return;
                indicator.show();
               
            }          
            if (isFirstTimeShowGuide)
            {
                isFirstTimeShowGuide = false;
            }
            else//第一个引导语音应在ScreenFader完毕后播放,但跳步骤情况下可播放
            {
                ProductionAudioController.Instance.Play(name);
            }
        }
        bool isFirstTimeShowGuide = true;

        /// <summary>
        /// 关闭指示器
        /// </summary>
        /// <param name="name"></param>
        public void CloseGuide(string name)
        {
            GuideNode guide = m_Collection.GuideNodes.Find(x => x.Name == name);
            if (guide == null)
                return;
            foreach (string item in m_Indicators.Keys)
            {
                if (guide.Target.Equals(item))
                {
                    m_Indicators[item].hide();
                }
            }
        }

        /// <summary>
        /// 关闭指示器
        /// </summary>
        /// <param name="name"></param>
        public void CloseGuideByName(string name)
        {
            foreach (string item in m_Indicators.Keys)
            {
                if (item.Equals(name))
                {
                    m_Indicators[item].hide();
                }
            }
        }

        /// <summary>
        /// 关闭所有指示器
        /// </summary>
        public void CloseAllGuide()
        {
            foreach (string item in m_Indicators.Keys)
            {
                m_Indicators[item].hide();
            }
        }
    }
}
