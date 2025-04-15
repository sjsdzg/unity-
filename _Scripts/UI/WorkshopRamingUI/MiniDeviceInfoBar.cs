using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XFramework.Core;
using XFramework.Common;
using XFramework.Module;

namespace XFramework.UI
{

    /// <summary>
    /// 设备展示面板
    /// </summary>
    public class MiniDeviceInfoBar : BaseRamingUI
    {
        /// <summary>
        /// 内容信息
        /// </summary>
        private Text m_Content;


        /// <summary>
        /// 设备视角
        /// </summary>
        private MouseOrbit3D mouseOrbit;

        /// <summary>
        /// 设备
        /// </summary>
        private GameObject deviceObj; 

        /// <summary>
        /// 标题 
        /// </summary>
        private Text titleText;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private Button closeButton;

        /// <summary>
        /// 进入房间按钮
        /// </summary>
        private Button EnterButton;

        /// <summary>
        /// 背景遮挡
        /// </summary>
        private Image MaskImage;


        //private UnityEvent m_ClosePanel = new UnityEvent();

        //public UnityEvent OnClosePanel
        //{
        //    get { return m_ClosePanel; }
        //    set { m_ClosePanel = value; }
        //}

        private bool isBackGround;
        /// <summary>
        /// 是否有背景
        /// </summary>
        public bool IsBackGround
        {
            get { return isBackGround; }
            set {
                
                isBackGround = value;
                MaskImage.enabled = value;
            }
        }
            

        private int SpeakSpeed = 20;

        /// <summary>
        /// 进入场景
        /// </summary>
        private Action EnterAction;
        /// <summary>
        /// 关闭窗口的动作(设备)
        /// </summary>
        private Action DeCloseAction;

        void Awake()
        {
            m_Content = transform.Find("Panel/ScrollView/Viewport/Content/Principle").GetComponent<Text>();
            titleText = transform.Find("Panel/Title/Text").GetComponent<Text>();
            closeButton = transform.Find("Panel/CloseButton").GetComponent<Button>();
            EnterButton = transform.Find("Panel/EnterButton").GetComponent<Button>() ;

            MaskImage = transform.GetComponent<Image>();

            closeButton.onClick.AddListener(Hide);
            EnterButton.onClick.AddListener(Enter_Click);

            ShowMode = UIWindowShowMode.HideOther;
        }
        private void Start()
        {
            EnterButton.gameObject.SetActive(false);
        }
        private void Update()
        {
        }
        /// <summary>
        /// 进入按钮点击事件
        /// </summary>
        private void Enter_Click()
        {
            if (EnterAction != null)
            {
                EnterButton.gameObject.SetActive(false);
                EnterAction.Invoke();
            }
        }
        /// <summary>
        /// 设置设备知识点
        /// </summary>
        /// <param name="value"></param>
        public void ExhibitionEquipShow(MachineItem value,  Action  _CloseAction)
        {
            IsActive = true;
            if (EnterButton.gameObject.activeSelf)
                EnterButton.gameObject.SetActive(false);
            IsBackGround = true;
            DeCloseAction = _CloseAction;

            m_Content.DOKill();

            m_Content.text = string.Empty;

            string _value = value.Desc.Replace("\\n", "\n");
            int count = _value.Length;

            m_Content.DOText(_value, count / SpeakSpeed);
            titleText.text = value.Name.Trim();


            if (deviceObj != null)
            {
                Destroy(deviceObj);
            }

            string path = "Assets/_Prefabs/Equipments/" + value.DevUrl;
            //ResourceManager.Instance.LoadAsyncInstance(path, x =>
            //{
            //    deviceObj = x as GameObject;
            //});
            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;

                GameObject prefab = loader.GetAsset<GameObject>();
                deviceObj = Instantiate(prefab);

                int layer = LayerMask.NameToLayer("Unit");
                deviceObj.layer = layer;
                //遍历当前物体及其所有子物体 
                foreach (Transform tran in deviceObj.GetComponentsInChildren<Transform>())
                {
                    tran.gameObject.layer = layer;//更改物体的Layer层  
                }

                ///播放音频
                //RamingAudioController.Instance.PlayAudio(value.Mp3Url);
                RamingAudioController.Instance.Play(value.Mp3Url);
            });
        }
        protected override void SetUI(params object[] uiParams)
        {
        }
        protected override void OnLoadData()
        {
            base.OnLoadData();
        }
        /// <summary>
        /// 显示窗口
        /// </summary>
        public  override void Show(BaseUIArgs uiParams)
        {
            StopAllCoroutines();
            base.Show(uiParams);
            print("显示 device");
            DeviceUiArgs arg = uiParams as DeviceUiArgs;
            ExhibitionEquipShow(arg.machineInfo,arg.CloseAction);
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        public override void Hide(BaseUIArgs uiParams)
        {
            base.Show(uiParams);
            ///参数空只是为了 管理器清空字典 所以不做操作，只关闭窗口
            if(uiParams==null)
            {

            }
            else if (DeCloseAction != null)
            {
                DeCloseAction.Invoke();
            }

            HideWindow();

            //print("关闭 device");

        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void HideWindow()
        {
            IsActive = false;
            if (deviceObj != null) ///设备展示
            {
                Destroy(deviceObj);
                //OnClosePanel.Invoke();
            }
            ///停止播放音频
            RamingAudioController.Instance.Stop();
        }
        /// <summary>
        /// 按钮触发关闭事件
        /// </summary>
        public void Hide()
        {
            if (DeCloseAction != null)
            {
                DeCloseAction.Invoke();
            }
            HideWindow();
        }
    }
}
