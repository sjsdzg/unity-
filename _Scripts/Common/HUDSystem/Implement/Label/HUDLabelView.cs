using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Module;
using XFramework.UI;
namespace XFramework.Common
{
    /// <summary>
    /// 标签view
    /// </summary>
	public class HUDLabelView : HUDView
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
        /// 面板内容
        /// </summary>
        public Text m_Content;

        ///// <summary>
        ///// 遮挡
        ///// </summary>
        public Transform m_Mask;
        /// <summary>
        /// 提示标志
        /// </summary>
        private bool tip = false;

        /// <summary>
        /// 标签信息
        /// </summary>
        private HUDLabelInfo lableInfo;

        /// <summary>
        /// HUDInfo属性
        /// </summary>
        public override HUDInfo HUDInfo
        {
            get { return lableInfo; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="hudInfo"></param>
        public override void Initialize(HUDInfo hudInfo)
        {
            base.Initialize(hudInfo);
            lableInfo = hudInfo as HUDLabelInfo;
        }

        /// <summary>
        /// 在屏幕内显示
        /// </summary>
        protected override void OnScreenHandler()
        {
            if (lableInfo.onScreenArgs.visible)
            {
                gameObject.SetActive(true);
                m_Icon.sprite = lableInfo.onScreenArgs.m_Sprite;
                m_Icon.color = lableInfo.onScreenArgs.m_Color;

                m_Text.gameObject.SetActive(true);
                m_Text.text = lableInfo.onScreenArgs.m_Text;
                m_Text.color = lableInfo.onScreenArgs.m_Color;

                m_Content.gameObject.SetActive(true);
                m_Content.text = lableInfo.onScreenArgs.m_Content;
                //显示距离
                if (lableInfo.onScreenArgs.showDistance)
                {
                    m_TextDistance.gameObject.SetActive(true);
                    m_TextDistance.text = string.Format("[{0:N0}m]", lableInfo.distance);
                }
                else
                {
                    m_TextDistance.gameObject.SetActive(false);
                }

                //闪烁
                if (lableInfo.onScreenArgs.flashing)
                {
                    if (lableInfo.onScreenArgs.m_Color.a <= 0)
                    {
                        tip = false;
                    }
                    else if (lableInfo.onScreenArgs.m_Color.a >= 1.0f)
                    {
                        tip = true;
                    }

                    if (tip)
                    {
                        lableInfo.onScreenArgs.m_Color.a -= Time.deltaTime;
                    }
                    else
                    {
                        lableInfo.onScreenArgs.m_Color.a += Time.deltaTime;
                    }
                }
                else
                {
                    lableInfo.onScreenArgs.m_Color.a = 1;
                }

                m_Icon.color = lableInfo.onScreenArgs.m_Color;
                m_Text.color = lableInfo.onScreenArgs.m_Color;

                ///动画
                //PlayAnimaiton();
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
            if (lableInfo.offScreenArgs.visible)
            {
                gameObject.SetActive(true);
                m_Icon.sprite = lableInfo.offScreenArgs.m_Sprite;
                //隐藏文本和距离文本
                m_Text.gameObject.SetActive(false);
                m_TextDistance.gameObject.SetActive(false);
                //闪烁
                if (lableInfo.offScreenArgs.flashing)
                {
                    if (lableInfo.offScreenArgs.m_Color.a <= 0)
                    {
                        tip = false;
                    }
                    else if (lableInfo.offScreenArgs.m_Color.a >= 1.0f)
                    {
                        tip = true;
                    }

                    if (tip)
                    {
                        lableInfo.offScreenArgs.m_Color.a -= Time.deltaTime;
                    }
                    else
                    {
                        lableInfo.offScreenArgs.m_Color.a += Time.deltaTime;
                    }
                }
                else
                {
                    lableInfo.offScreenArgs.m_Color.a = 1;
                }
                //设置颜色
                m_Icon.color = lableInfo.offScreenArgs.m_Color;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayAnimaiton()
        {
            Vector3 temp = m_Mask.localPosition;
            m_Mask.transform.localPosition = new Vector3(0, temp.y, temp.z);
            m_Mask.DOLocalMoveX(216, 3);
        }
    }
}