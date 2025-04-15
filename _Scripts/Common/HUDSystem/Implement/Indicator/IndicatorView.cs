using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XFramework.Common
{
    /// <summary>
    /// 指示器视图
    /// </summary>
    public class IndicatorView : HUDView
    {
        /// <summary>
        /// Image
        /// </summary>
        public Image m_Icon;
        /// <summary>
        /// 文本
        /// </summary>
        public Text m_Text;
        /// <summary>
        /// 文本距离
        /// </summary>
        public Text m_TextDistance;

        /// <summary>
        /// 提示标志
        /// </summary>
        private bool tip = false;

        /// <summary>
        /// 指示器信息
        /// </summary>
        private IndicatorInfo indicatorInfo;

        /// <summary>
        /// CanvasGroup
        /// </summary>
        private CanvasGroup m_CanvasGroup;

        /// <summary>
        /// HUDInfo属性
        /// </summary>
        public override HUDInfo HUDInfo
        {
            get  { return indicatorInfo; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="hudInfo"></param>
        public override void Initialize(HUDInfo hudInfo)
        {
            base.Initialize(hudInfo);
            indicatorInfo = hudInfo as IndicatorInfo;
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            m_CanvasGroup = transform.GetOrAddComponent<CanvasGroup>();
            m_CanvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// 在屏幕内显示
        /// </summary>
        protected override void OnScreenHandler()
        {
            if (indicatorInfo.onScreenArgs.visible)
            {
                gameObject.SetActive(true);
                m_Icon.sprite = indicatorInfo.onScreenArgs.m_Sprite;
               // m_Icon.color = indicatorInfo.onScreenArgs.m_Color;
                m_Icon.transform.localScale = Vector3.one;
                m_Text.transform.parent.gameObject.SetActive(true);
                m_Text.text = indicatorInfo.onScreenArgs.m_Text;
                m_Text.color = indicatorInfo.onScreenArgs.m_Color;

                //显示距离
                if (indicatorInfo.onScreenArgs.showDistance)
                {
                    m_TextDistance.gameObject.SetActive(true);
                    m_TextDistance.text = string.Format("[{0:N0}m]", indicatorInfo.distance);
                }
                else
                {
                    m_TextDistance.gameObject.SetActive(false);
                }

                //闪烁
                if (indicatorInfo.onScreenArgs.flashing)
                {
                    if (m_CanvasGroup.alpha <= 0)
                    {
                        tip = false;
                    }
                    else if (m_CanvasGroup.alpha >= 1.0f)
                    {
                        tip = true;
                    }

                    if (tip)
                    {
                        m_CanvasGroup.alpha -= Time.deltaTime;
                    }
                    else
                    {
                        m_CanvasGroup.alpha += Time.deltaTime;
                    }
                }
                else
                {
                    m_CanvasGroup.alpha = 1;
                }

               // m_Icon.color = indicatorInfo.onScreenArgs.m_Color;
                m_Text.color = indicatorInfo.onScreenArgs.m_Color;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 在屏幕外显示
        /// </summary>
        protected override void OffScreenHandler()
        {
            if (indicatorInfo.offScreenArgs.visible)
            {
                gameObject.SetActive(true);
                m_Icon.sprite = indicatorInfo.offScreenArgs.m_Sprite;
                m_Icon.transform.localScale = new Vector3(2, 2, 1);
                //隐藏文本和距离文本
                m_Text.transform.parent.gameObject.SetActive(false);
                m_TextDistance.gameObject.SetActive(false);
                //闪烁
                if (indicatorInfo.offScreenArgs.flashing)
                {
                    if (m_CanvasGroup.alpha <= 0)
                    {
                        tip = false;
                    }
                    else if (m_CanvasGroup.alpha >= 1.0f)
                    {
                        tip = true;
                    }

                    if (tip)
                    {
                        //indicatorInfo.onScreenArgs.m_Color.a -= Time.deltaTime;
                        m_CanvasGroup.alpha -= Time.deltaTime;
                    }
                    else
                    {
                        //indicatorInfo.onScreenArgs.m_Color.a += Time.deltaTime;
                        m_CanvasGroup.alpha += Time.deltaTime;
                    }
                }
                else
                {
                    m_CanvasGroup.alpha = 1;
                }
                //设置颜色
                //m_Icon.color = indicatorInfo.offScreenArgs.m_Color;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
