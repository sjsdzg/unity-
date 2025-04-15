using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

namespace XFramework.Common
{
    public class ObservationPointView : HUDView
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
        /// 提示标志
        /// </summary>
        private bool tip = false;
      
        /// <summary>
        /// 发现按钮
        /// </summary>
        private Button buttonFind;

        private LayerMask layerMask= 1 << 8;


        private UnityEvent m_OnClicked = new UnityEvent();
        /// <summary>
        /// 点击事件
        /// </summary>
        public UnityEvent OnClicked
        {
            get { return m_OnClicked; }
            set { m_OnClicked = value; }
        }
        /// <summary>
        /// 观察点指示器信息
        /// </summary>
        [SerializeField]
        private ObservationPointInfo ObservationPointInfo ;
        public override HUDInfo HUDInfo
        {
            get
            {
                return ObservationPointInfo;
            }
        }
        protected override void OnAwake()
        {
            base.OnAwake();
            //m_Sequence = DOTween.Sequence();
            //m_Sequence.Append(m_Icon.rectTransform.DOScale(1.2f, 1).SetLoops(-1, LoopType.Yoyo));
            //m_Sequence.Pause();

           // layerMask = LayerMask.NameToLayer("Player");
            buttonFind = transform.Find("Content/ButtonFind").GetComponent<Button>();
            m_Icon.rectTransform.DOScale(1.2f, 1).SetLoops(-1, LoopType.Yoyo);
            buttonFind.onClick.AddListener(buttonFind_onClick);
        }

        private void buttonFind_onClick()
        {
            OnClicked.Invoke();
           // buttonFind.gameObject.SetActive(false);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="hudInfo"></param>
        public override void Initialize(HUDInfo hudInfo)
        {
            base.Initialize(hudInfo);
            ObservationPointInfo = hudInfo as ObservationPointInfo;
           
        }
        private void Start()
        {
       
        }
        /// <summary>
        /// 在屏幕内显示
        /// </summary>
        protected override void OnScreenHandler()
        {
            if (ObservationPointInfo.onScreenArgs.visible)
            {
                Ray ray = new Ray(Camera.main.transform.position, ObservationPointInfo.m_Target.transform.position - Camera.main.transform.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10,~layerMask))
                {
                    if (hit.collider.tag == "Hit")
                    {
                        gameObject.SetActive(true);
                        //Icon
                       // m_Icon.sprite = ObservationPointInfo.onScreenArgs.m_Sprite;
                       // m_Icon.color = ObservationPointInfo.onScreenArgs.m_Color;
                        //Text
                        m_Text.transform.parent.parent.gameObject.SetActive(true);
                        m_Text.text = ObservationPointInfo.onScreenArgs.m_Text;
                    }

                }
                else
                {
                    gameObject.SetActive(false);
                }
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
            if (ObservationPointInfo.offScreenArgs.visible)
            {
                gameObject.SetActive(true);
                m_Icon.sprite = ObservationPointInfo.offScreenArgs.m_Sprite;
                //设置颜色
                m_Icon.color = ObservationPointInfo.offScreenArgs.m_Color;
                //隐藏文本和距离文本
                m_Text.transform.parent.parent.gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
